using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float _spinDegPerSec = 15.0f;
    [SerializeField] private float _spinAmp= 0.5f;
    [SerializeField] private float _spinFreq = 1.0f;
    private Transform _camera;
    private Vector3 _tempPos = new Vector3 ();
    private Vector3 _posOffset = new Vector3();

    void Awake()
    {
        _camera = Camera.main.transform;
        _posOffset = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0f, Time.deltaTime * _spinDegPerSec, 0f), Space.World);

        // Float up/down with a Sin()
        _tempPos = _posOffset;
        _tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * _spinFreq) * _spinAmp;

        transform.position = _tempPos;

        Vector3 lookPos = _camera.position - transform.position;
        lookPos.y = 0; // ignore vertical tilt
        transform.rotation = Quaternion.LookRotation(lookPos);
    }
}
