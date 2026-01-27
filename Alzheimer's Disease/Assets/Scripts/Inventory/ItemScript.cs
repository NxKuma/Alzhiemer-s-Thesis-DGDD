using NUnit.Framework;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ItemScript : MonoBehaviour
{
    [SerializeField] private Item _itemResource;
    private GameObject _itemShadow;
    private Coroutine _thicknessCoroutine;
    private bool _isSpinning = false;

    private void OnValidate()
    {  
        #if UNITY_EDITOR
        this.name = _itemResource != null ? _itemResource.GetItemName() : "Item_NULL";
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }

    void Awake()
    {
        // this.gameObject.SetActive(false); //This is for future gameplay things
        float size = _itemResource.GetItemSize();
        Mesh mesh = _itemResource.GetItemMesh();

        _itemShadow = this.transform.GetChild(0).gameObject;
        _isSpinning = _itemResource.GetItemtype() == Item.eItemType.JigsawPuzzle;

        if (_itemResource.GetItemMesh() != null)
        {
            GetComponent<MeshFilter>().mesh = mesh;
            _itemShadow.GetComponent<MeshFilter>().mesh = mesh;

            BoxCollider bc = GetComponent<BoxCollider>();
            BoxCollider iSbc = _itemShadow.GetComponent<BoxCollider>();
            if (iSbc == null) iSbc = _itemShadow.AddComponent<BoxCollider>();
            if (bc == null) bc = gameObject.AddComponent<BoxCollider>();
            bc.center = mesh.bounds.center;
            iSbc.center = mesh.bounds.center;
            iSbc.size = mesh.bounds.size;
            bc.size = mesh.bounds.size;
        }

        if (_itemResource.GetItemMaterial() != null)
        {
            Renderer rend = GetComponent<Renderer>();

            if(_itemResource.GetType() == typeof(PuzzlePiece))
            {
                PuzzlePiece _itemResource = (PuzzlePiece)this._itemResource;
                Material[] materials = new Material[2];
                materials[0] = new Material(_itemResource.GetItemMaterial());
                materials[1] = new Material(_itemResource.GetPuzzleShaderMaterial());
                materials[1].SetTexture("_MainTexture", _itemResource.GetPuzzleMeshTexture().texture);
                rend.materials = materials;
            } else {
                int materialCount = _isSpinning ? (rend.materials.Length + 1) : rend.materials.Length;
                Material mats = new Material(_itemResource.GetItemMaterial());
                rend.material = mats;
            }
        }   
        SetShadowThickness(0.0f);
        transform.localScale *= size;
        
        if (_isSpinning){
            Destroy(GetComponent<Rigidbody>()); 
            gameObject.AddComponent<FloatingObject>(); 
        } 
    }

    // void LateUpdate()
    // {
    //     if (_camera == null || !_isSpinning) return;

    //     Vector3 lookPos = _camera.position - transform.position;
    //     lookPos.y = 0; // ignore vertical tilt
    //     transform.rotation = Quaternion.LookRotation(lookPos);
    // }

    public Item GetItemResource() => _itemResource;

    // public void Initialize(Item item)
    // {
    //     _itemResource = item;

    //     if (_itemResource != null)
    //     {
    //         if (_itemResource.GetItemMesh() != null)
    //         {
    //             MeshFilter mf = GetComponent<MeshFilter>();
    //             if (mf != null) mf.mesh = _itemResource.GetItemMesh();
    //         }

    //         Renderer rend = GetComponent<Renderer>();
    //         if (rend != null && _itemResource.GetItemMaterial() != null)
    //             rend.material = _itemResource.GetItemMaterial()[0];
    //     }
    // }

    public void SetShadowThickness(float thickness)
    {
        _itemShadow.GetComponent<Renderer>().material.SetFloat("_Outline_Thickness", thickness);
    }

    public void RemoveCoutine()
    {
        if (_thicknessCoroutine != null)
        {
            StopCoroutine(_thicknessCoroutine);
            _thicknessCoroutine = null;
        }
    }

    // Tween the shadow/outline thickness to a target value at the given speed (units per second-like)
    public void TweenShadowThickness(float target, float speed)
    {
        if (_thicknessCoroutine != null)
            StopCoroutine(_thicknessCoroutine);
        _thicknessCoroutine = StartCoroutine(CoTweenThickness(target, speed));
    }

    private System.Collections.IEnumerator CoTweenThickness(float target, float speed)
    {
        var rend = _itemShadow.GetComponent<Renderer>();
        if (rend == null)
            yield break;

        // Read current value from material (may create instance)
        float current = rend.material.GetFloat("_Outline_Thickness");

        // Lerp until close to target
        while (Mathf.Abs(current - target) > 0.001f)
        {
            current = Mathf.Lerp(current, target, Time.deltaTime * speed);
            SetShadowThickness(current);
            yield return null;
        }

        SetShadowThickness(target);
        _thicknessCoroutine = null;
    }
}
