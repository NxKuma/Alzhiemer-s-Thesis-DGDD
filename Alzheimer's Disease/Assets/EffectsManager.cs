using UnityEngine;
using System.Collections;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance { get; private set; }

    private Camera _mainCamera;
    [SerializeField] AnimationCurve _cameraShakeCurve;
    private Volume _postProcessingVolume;
    private Vignette _vignetteEffect;

    private Coroutine _cameraRefreshCoroutine;

    [SerializeField] private float _vignetteOnIntensity = 0.25f;
    [SerializeField] private float _vignetteOffIntensity = 0f;
    [SerializeField] private float _vignetteChangeSpeed = 1.5f;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        _postProcessingVolume = GetComponent<Volume>();
        if (_postProcessingVolume != null && _postProcessingVolume.profile != null)
        {
            _vignetteEffect = _postProcessingVolume.profile.TryGet<Vignette>(out var vignette) ? vignette : null;
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
        RefreshMainCamera();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        RefreshMainCamera();
    }

    private void RefreshMainCamera(int maxFramesToWait = 10)
    {
        if (_cameraRefreshCoroutine != null)
        {
            StopCoroutine(_cameraRefreshCoroutine);
        }

        _cameraRefreshCoroutine = StartCoroutine(RefreshMainCameraCoroutine(maxFramesToWait));
    }

    private IEnumerator RefreshMainCameraCoroutine(int maxFramesToWait)
    {
        for (int i = 0; i < maxFramesToWait; i++)
        {
            _mainCamera = Camera.main;
            if (_mainCamera != null)
            {
                yield break;
            }

            yield return null; // wait a frame for the scene to finish initializing
        }

        Debug.LogError("EffectsManager: No main camera found in the scene. Please ensure there is a camera tagged 'MainCamera'.");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Update()
    {
        //Testing Only - Remove later
        if(Input.GetKeyDown(KeyCode.V))
        {
            TriggerVignetteEffect();
            TriggerCameraShake();
        }
    }

    public void TriggerVignetteEffect(bool isOn = true)
    {        
        StartCoroutine(VignetteEffectCoroutine(isOn));
    }

    public void TriggerCameraShake(bool isOn = true, float duration = 0.85f)
    {
        StartCoroutine(CameraShakeCoroutine(isOn, duration));
    }

    private IEnumerator CameraShakeCoroutine(bool isOn, float duration)
    {
        if (_mainCamera == null)
        {
            yield break;
        }

        Vector3 originalPos = _mainCamera.transform.localPosition;
        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float strength = _cameraShakeCurve.Evaluate(elapsed / duration);
            _mainCamera.transform.localPosition = originalPos + Random.insideUnitSphere * strength;
            yield return null; // Wait for the next frame
        }

        _mainCamera.transform.localPosition = originalPos;
    }

    private IEnumerator VignetteEffectCoroutine(bool isOn)
    {
        if (_vignetteEffect == null)
        {
            yield break;
        }

        // Make sure the parameter is actually driving the post-processing.
        _vignetteEffect.active = true;
        _vignetteEffect.intensity.overrideState = true;

        float target = isOn ? _vignetteOnIntensity : _vignetteOffIntensity;
        while (!Mathf.Approximately(_vignetteEffect.intensity.value, target))
        {
            _vignetteEffect.intensity.value = Mathf.MoveTowards(
                _vignetteEffect.intensity.value,
                target,
                _vignetteChangeSpeed * Time.deltaTime);
            yield return null;
        }
    }


}
