using UnityEngine;
using System.Collections.Generic;

public class RoomChildMover : MonoBehaviour
{
    private RoomManager _roomManager;
    // Mapping from room transform -> list of child info that should follow the room
    private class ChildInfo { public Transform child; public Vector3 localPos; public Quaternion localRot; }
    private Dictionary<Transform, List<ChildInfo>> _roomChildren = new Dictionary<Transform, List<ChildInfo>>();
    private Dictionary<Transform, Vector3> _lastRoomPositions = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Quaternion> _lastRoomRotations = new Dictionary<Transform, Quaternion>();
    // [SerializeField] private float searchThreshold = 0.0f; // strict parent-only mode by default

    void Awake()
    {
        // Try to get RoomManager on same GameObject, else find in scene
        _roomManager = this.transform.GetComponent<RoomManager>();
        Debug.Log("RoomChildMover: Found RoomManager on same GameObject: " + (_roomManager != null));
        if (_roomManager == null)
            _roomManager = FindObjectOfType<RoomManager>();
        // Subscribe to room randomization signal so we can reposition children quickly
        // after rooms have been randomized. Full remap can be triggered manually via RescanChildren().
        if (_roomManager != null)
            _roomManager.RoomsRandomized += RepositionChildren;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (_roomManager == null)
        {
            Debug.LogWarning("RoomChildMover: RoomManager not found in scene. Disabling RoomChildMover.");
            enabled = false;
            return;
        }

        // Build initial mapping by scanning the scene once
        MapChildren();
    }

    /// <summary>
    /// Scan the scene and (re)build mappings of objects that should follow rooms.
    /// Call this whenever rooms or their contents change at runtime.
    /// </summary>
    public void RescanChildren()
    {
        _roomChildren.Clear();
        _lastRoomPositions.Clear();
        _lastRoomRotations.Clear();

        // Get all transforms in scene
        Transform[] all = this.transform.parent.GetComponentsInChildren<Transform>();

        // Build mapping of scene objects that should follow rooms.
        // Strategy: find transforms in the scene that are near a room's renderer bounds (within threshold)
        foreach (Transform room in _roomManager.PossibleRooms)
        {
            if (room == null) continue;
            if (!_roomChildren.ContainsKey(room)) _roomChildren[room] = new System.Collections.Generic.List<ChildInfo>();

            Renderer roomRend = room.GetComponent<Renderer>();
            if (roomRend == null) continue;
            Bounds rb = roomRend.bounds;

            foreach (Transform t in all)
            {
                if (t == null) continue;
                // Skip the room itself and any hallway or manager objects
                if (t == room) continue;
                if (System.Array.IndexOf(_roomManager.Hallways, t) >= 0) continue;
                // Only consider objects that have NPCScript or ItemScript components
                bool hasTrackedComponent = t.GetComponent<NPCScript>() != null || t.GetComponent<ItemScript>() != null;
                if (!hasTrackedComponent) continue;

                // Skip obvious primitives we don't want to move or other room/hallway objects
                if (t.name.StartsWith("Door_") || t.name.StartsWith("Hallway") || t.GetComponent<RoomManager>() != null) continue;

                // If transform is itself a possible room, skip
                if (System.Array.IndexOf(_roomManager.PossibleRooms, t) >= 0) continue;

                // Determine belonging: first, if the object shares the same parent as the room
                // (EmptyParentofRoom pattern), treat it as part of the room.
                // Strict parent-only: require the same parent (EmptyParentofRoom pattern)
                bool belongs = false;
                if (t.parent != null && room.parent != null && t.parent == room.parent)
                {
                    belongs = true;
                }

                if (belongs)                                                    
                {
                    // Store child's local offset relative to room transform so we can reapply after room moves
                    ChildInfo info = new ChildInfo();
                    info.child = t;
                    info.localPos = room.InverseTransformPoint(t.position);
                    info.localRot = Quaternion.Inverse(room.rotation) * t.rotation;
                    _roomChildren[room].Add(info);
                    Debug.Log("RoomChildMover: Mapped child " + t.name + " to room " + room.name);
                }
            }

            // cache last known transform state
            _lastRoomPositions[room] = room.position;
            _lastRoomRotations[room] = room.rotation;
        }
    }

    /// <summary>
    /// One-time mapping of children to rooms. If mapping has already been created this is a no-op.
    /// Use RescanChildren() to force a full remap.
    /// </summary>
    private bool _hasMapped = false;
    [UnityEngine.Header("Debug")]
    [UnityEngine.Tooltip("Toggle in the inspector to force a full remap at runtime. Will automatically clear.")]
    public bool forceRemap = false;
    public void MapChildren()
    {
        if (_hasMapped)
        {
            Debug.Log("RoomChildMover: MapChildren called but mapping already exists. Skipping.");
            return;
        }

        // Reuse RescanChildren logic to build mapping
        RescanChildren();
        _hasMapped = true;
        Debug.Log("RoomChildMover: Completed one-time MapChildren mapping.");
    }

    [ContextMenu("Force Rescan Children (Editor)")]
    private void ForceRescanContext()
    {
        RescanChildren();
        _hasMapped = true;
        Debug.Log("RoomChildMover: Forced rescan via context menu.");
    }

    /// <summary>
    /// Fast reposition using the stored mappings. Does not perform a scene scan.
    /// This is intended to be called frequently (e.g. after rooms are randomized).
    /// If no mapping exists yet, it will perform a full RescanChildren first.
    /// </summary>
    public void RepositionChildren()
    {
        if (_roomChildren == null || _roomChildren.Count == 0)
        {
            Debug.Log("RoomChildMover: No existing mapping found, performing full rescan before reposition.");
            RescanChildren();
            _hasMapped = true;
            return;
        }

        foreach (var kv in _roomChildren)
        {
            Transform room = kv.Key;
            if (room == null) continue;

            var list = kv.Value;
            for (int i = 0; i < list.Count; i++)
            {
                var info = list[i];
                if (info == null || info.child == null) continue;
                info.child.position = room.TransformPoint(info.localPos);
                info.child.rotation = room.rotation * info.localRot;
            }

            _lastRoomPositions[room] = room.position;
            _lastRoomRotations[room] = room.rotation;
        }
    }

    void OnDestroy()
    {
        if (_roomManager != null)
            _roomManager.RoomsRandomized -= RepositionChildren;
    }

    // Update is called once per frame
    void Update()
    {
        // Inspector toggle: force a full remap once and clear the toggle.
        if (forceRemap)
        {
            Debug.Log("RoomChildMover: Inspector forceRemap triggered — performing full rescan.");
            RescanChildren();
            _hasMapped = true;
            forceRemap = false;
        }

        // For each tracked room, if it has moved/rotated, update its children to keep relative offsets
        foreach (var kv in _roomChildren)
        {
            Transform room = kv.Key;
            if (room == null) continue;

            Vector3 lastPos = _lastRoomPositions.ContainsKey(room) ? _lastRoomPositions[room] : room.position;
            Quaternion lastRot = _lastRoomRotations.ContainsKey(room) ? _lastRoomRotations[room] : room.rotation;

            if (room.position != lastPos || room.rotation != lastRot)
            {
                var list = kv.Value;
                for (int i = 0; i < list.Count; i++)
                {
                    var info = list[i];
                    if (info == null || info.child == null) continue;
                    // Apply room transform to child's stored local position/rotation
                    info.child.position = room.TransformPoint(info.localPos);
                    info.child.rotation = room.rotation * info.localRot;
                }

                _lastRoomPositions[room] = room.position;
                _lastRoomRotations[room] = room.rotation;
            }
        }
    }
}
