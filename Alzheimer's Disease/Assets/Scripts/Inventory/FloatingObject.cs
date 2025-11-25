using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float _spinDegPerSec = 15.0f;
    [SerializeField] private float _spinAmp= 1.0f;
    [SerializeField] private float _spinFreq = 1.0f;
    [SerializeField] private float _bobSmoothing = 3f;
    [SerializeField] private float _rotationSmooth = 8f;
    [SerializeField, Tooltip("If the object is moved more than this distance from its stored base position, snap/teleport instead of smoothing.")]
    private float _snapThreshold = 0.5f;
    [SerializeField, Tooltip("Number of frames to hold the object at the teleported base position before resuming smoothing.")]
    private int _snapFrames = 3;
    private Transform _camera;
    private Vector3 _tempPos = new Vector3 ();
    private Vector3 _posOffset = new Vector3();
    private float _spinAngle = 0f;
    private int _snapRemaining = 0;

    void Awake()
    {
        _camera = Camera.main.transform;
        _posOffset = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        // detect if external code teleported/moved this object far from our stored base position
        float dist = Vector3.Distance(transform.position, _posOffset);
        if (dist > _snapThreshold)
        {
            // adopt the new base position and hold the object there for a few frames
            _posOffset = transform.position;
            _snapRemaining = Mathf.Max(1, _snapFrames);
            // ensure an immediate teleport to the new base
            transform.position = _posOffset;
        }

        // accumulate spin angle (degrees)
        _spinAngle = (_spinAngle + _spinDegPerSec * Time.deltaTime) % 360f;

        // Float up/down with a Sin() -> compute target then ease toward it using SmoothStep
        _tempPos = _posOffset;
        float targetY = _posOffset.y + Mathf.Sin(Time.time * Mathf.PI * _spinFreq) * _spinAmp;
        _tempPos.y = targetY;

        // If we're in snap-hold frames, keep the object at the base position (teleport),
        // otherwise smoothly lerp toward the bob target for nice motion.
        if (_snapRemaining > 0)
        {
            // Hold at the base position (no bob) during snap frames to avoid smoothing into the move.
            transform.position = _posOffset;
            _snapRemaining--;
        }
        else
        {
            // ease from current position toward target using SmoothStep for nicer motion
            float t = Mathf.Clamp01(_bobSmoothing * Time.deltaTime);
            float ease = Mathf.SmoothStep(0f, 1f, t);
            transform.position = Vector3.Lerp(transform.position, _tempPos, ease);
        }

        // compute look rotation toward camera (ignore vertical tilt)
        Vector3 lookPos = _camera.position - transform.position;
        lookPos.y = 0f; // ignore vertical tilt
        Quaternion lookRot = Quaternion.LookRotation(lookPos);

        // combine look rotation with spin around Y by adding spin angle to yaw
        Vector3 lookEuler = lookRot.eulerAngles;
        Quaternion targetRot = Quaternion.Euler(lookEuler.x, lookEuler.y + _spinAngle, lookEuler.z);

        // smooth the rotation to avoid choppy changes
        float rotT = Mathf.Clamp01(_rotationSmooth * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotT);
    }
}
