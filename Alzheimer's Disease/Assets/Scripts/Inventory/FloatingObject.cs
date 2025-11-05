using UnityEngine;

public class FloatingObject : MonoBehaviour
{
    [SerializeField] private float _spinDegPerSec = 15.0f;
    [SerializeField] private float _spinAmp= 1.0f;
    [SerializeField] private float _spinFreq = 1.0f;
    [SerializeField] private float _bobSmoothing = 3f;
    [SerializeField] private float _rotationSmooth = 8f;
    private Transform _camera;
    private Vector3 _tempPos = new Vector3 ();
    private Vector3 _posOffset = new Vector3();
    private float _spinAngle = 0f;

    void Awake()
    {
        _camera = Camera.main.transform;
        _posOffset = transform.position;
    }
    // Update is called once per frame
    void Update()
    {
    // accumulate spin angle (degrees)
    _spinAngle = (_spinAngle + _spinDegPerSec * Time.deltaTime) % 360f;

    // Float up/down with a Sin() -> compute target then ease toward it using SmoothStep
        _tempPos = _posOffset;
        float targetY = _posOffset.y + Mathf.Sin(Time.time * Mathf.PI * _spinFreq) * _spinAmp;
        _tempPos.y = targetY;

        // ease from current position toward target using SmoothStep for nicer motion
        float t = Mathf.Clamp01(_bobSmoothing * Time.deltaTime);
        float ease = Mathf.SmoothStep(0f, 1f, t);
        transform.position = Vector3.Lerp(transform.position, _tempPos, ease);

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
