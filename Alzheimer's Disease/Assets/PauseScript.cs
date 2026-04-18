using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PauseScript : MonoBehaviour
{
    [SerializeField] private FirstPersonController _fps;
    private SFXManager _sfxManager;
    private Music _musicManager;
    private Button _quitButton;
    private Slider _sfxVolumeSlider;
    private Slider _musicVolumeSlider;
    private Toggle _headBobbleToggle;

    private TMP_InputField _musicVolumeInputField;
    private TMP_InputField _sfxVolumeInputField;

    private CanvasManager _canvasManager;
    
    void Awake() {
        _quitButton = this.gameObject.transform.GetChild(0).GetComponentInChildren<Button>();
        _musicVolumeSlider = this.gameObject.transform.GetChild(0).GetComponentsInChildren<Slider>()[0];
        _sfxVolumeSlider = this.gameObject.transform.GetChild(0).GetComponentsInChildren<Slider>()[1];
        _headBobbleToggle = this.gameObject.transform.GetChild(0).GetComponentInChildren<Toggle>();    
    }

    public void QuitGame() {
        Application.Quit();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.transform.GetComponent<CanvasGroup>().alpha = 0f;
        //Get the appropriate instances
        _canvasManager = CanvasManager.Instance;
        _sfxManager = SFXManager.Instance;
        _musicManager = Music.Instance;
        //Get the input fields that are children of the sliders
        _musicVolumeInputField = _musicVolumeSlider.gameObject.transform.GetComponentInChildren<TMP_InputField>();
        _sfxVolumeInputField = _sfxVolumeSlider.gameObject.transform.GetComponentInChildren<TMP_InputField>();
        Debug.Log("Music Input Field: " + _musicVolumeInputField.name);
        Debug.Log("SFX Input Field: " + _sfxVolumeInputField.name);
        _musicVolumeSlider.value = _musicManager.GetMusicVolume();
        _sfxVolumeSlider.value = _sfxManager.GetSFXVolume(); 

        _headBobbleToggle.isOn = _fps.GetHeadBobEnable();
        // _quitButton.onClick.AddListener(() => Application.Quit());
        _musicVolumeInputField.text = (_musicVolumeSlider.value*100f).ToString("F0");
        _sfxVolumeInputField.text = (_sfxVolumeSlider.value*100f).ToString("F0");

        _sfxVolumeSlider.onValueChanged.AddListener((slidervalue) => {
            _sfxManager.SetSFXVolume((float)Math.Round(slidervalue, 2));
            _sfxVolumeInputField.text = Math.Round(slidervalue*100f, 2).ToString("F0");
        });
        _musicVolumeSlider.onValueChanged.AddListener((slidervalue) => {
            _musicManager.SetMusicVolume((float)Math.Round(slidervalue, 2));
            _musicVolumeInputField.text = Math.Round(slidervalue*100f, 2).ToString("F0");
        });
        _headBobbleToggle.onValueChanged.AddListener((isOn) => _fps.SetEnableHeadBob(isOn));

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
        }
    }

    //Toggle the pause menu on and off. When on, player movement is disabled, cursor is unlocked and visible, and the canvas group is set to interactable. When off, player movement is enabled, cursor is locked and invisible, and the canvas group is set to non-interactable.
    void PauseToggle(){
        if (_canvasManager.GetPlayerState() !=  CanvasManager.EPlayerState.Roam 
        && _canvasManager.GetPlayerState() != CanvasManager.EPlayerState.Paused) return;

        if (_canvasManager.GetPlayerState() == CanvasManager.EPlayerState.Roam)
        {
            _canvasManager.SetPlayerState((int)CanvasManager.EPlayerState.Paused);
            // GameEventsManager.Instance.playerEvents.DisablePlayerMovement();
            _fps.StopStartPlayer(false);
            this.transform.GetComponent<CanvasGroup>().alpha = 1f;
            this.transform.GetComponent<CanvasGroup>().interactable = true;
            this.transform.GetComponent<CanvasGroup>().blocksRaycasts = true;
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            _canvasManager.SetPlayerState((int)CanvasManager.EPlayerState.Roam);
            // GameEventsManager.Instance.playerEvents.EnablePlayerMovement();
            _fps.StopStartPlayer(true);
            // _ = sFXManager.PlaySFX("Pause");
            this.transform.GetComponent<CanvasGroup>().alpha = 0f;
            this.transform.GetComponent<CanvasGroup>().interactable = false;
            this.transform.GetComponent<CanvasGroup>().blocksRaycasts = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

    }
}
