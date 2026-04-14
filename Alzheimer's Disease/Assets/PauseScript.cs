using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private FirstPersonController _fps;
    private SFXManager _sfxManager;
    private Button _quitButton;
    private Slider _sfxVolumeSlider;
    private Slider _musicVolumeSlider;
    private Toggle _headBobbleToggle;

    private InputField _musicVolumeInputField;
    private InputField _sfxVolumeInputField;
    
    void Awake() {
        _quitButton = GetComponentInChildren<Button>();
        _sfxVolumeSlider = GetComponentsInChildren<Slider>()[1];
        _musicVolumeSlider = GetComponentsInChildren<Slider>()[0];
        _headBobbleToggle = GetComponentInChildren<Toggle>();    
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _sfxManager = SFXManager.Instance;
        _musicVolumeInputField = _musicVolumeSlider.GetComponentInChildren<InputField>();
        _sfxVolumeInputField = _sfxVolumeSlider.GetComponentInChildren<InputField>();
        _musicVolumeSlider.value = _sfxManager.GetMusicVolume() * 100f;
        _sfxVolumeSlider.value = _sfxManager.GetSFXVolume() * 100f; 

        _headBobbleToggle.isOn = _fps.GetHeadBobEnable();
        _quitButton.onClick.AddListener(() => Application.Quit());
        _musicVolumeInputField.text = _musicVolumeSlider.value.ToString();
        _sfxVolumeInputField.text = _sfxVolumeSlider.value.ToString();
        
        _sfxVolumeSlider.onValueChanged.AddListener((slidervalue) => _sfxManager.SetMusicVolume(slidervalue / 100f));
        _musicVolumeSlider.onValueChanged.AddListener((slidervalue) => _sfxManager.SetSFXVolume(slidervalue / 100f));
        _headBobbleToggle.onValueChanged.AddListener((isOn) => _fps.SetEnableHeadBob(isOn));

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
