using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
 public static GameManager Instance { get; private set; }

 [Header("Panels")]
 public GameObject gamePanel;
 public GameObject deathPanel;
 public GameObject pausePanel;
 public GameObject settingsPanel;

 [Header("Buttons")]
 public Button pauseButton;
 public Button resumeButton;
 public Button restartButton;
 public Button quitButton;
 public Button settingsButton;
 public Button closeSettingsButton;

 [Header("Settings")]
 public Slider musicVolumeSlider;
 public Slider sfxVolumeSlider;

 Pet _pet;

 void Awake()
 {
 if (Instance != null && Instance != this)
 {
 Destroy(gameObject);
 return;
 }
 Instance = this;
 DontDestroyOnLoad(gameObject);

 InitializeAudioManager();
 }

 void InitializeAudioManager()
 {
 if (FindObjectOfType<AudioManager>() == null)
 {
 GameObject audioGO = new GameObject("AudioManager", typeof(AudioManager));
 DontDestroyOnLoad(audioGO);
 }
 }

 void Start()
 {
 _pet = Pet.Instance;
 _pet.OnPetDied += OnPetDied;

 pauseButton.onClick.AddListener(PauseGame);
 resumeButton.onClick.AddListener(ResumeGame);
 restartButton.onClick.AddListener(RestartGame);
 quitButton.onClick.AddListener(QuitGame);
 settingsButton.onClick.AddListener(OpenSettings);
 closeSettingsButton.onClick.AddListener(CloseSettings);

 if (musicVolumeSlider) musicVolumeSlider.onValueChanged.AddListener(v => AudioManager.Instance.SetMusicVolume(v));
 if (sfxVolumeSlider) sfxVolumeSlider.onValueChanged.AddListener(v => AudioManager.Instance.SetSfxVolume(v));

 if (musicVolumeSlider && AudioManager.Instance) musicVolumeSlider.value = AudioManager.Instance.musicVolume;
 if (sfxVolumeSlider && AudioManager.Instance) sfxVolumeSlider.value = AudioManager.Instance.sfxVolume;

 ShowGamePanel();
 }

 void OnPetDied()
 {
 gamePanel.SetActive(false);
 deathPanel.SetActive(true);
 }

 void PauseGame()
 {
 Time.timeScale = 0f;
 gamePanel.SetActive(false);
 pausePanel.SetActive(true);
 }

 void ResumeGame()
 {
 Time.timeScale = 1f;
 pausePanel.SetActive(false);
 gamePanel.SetActive(true);
 }

 void RestartGame()
 {
 Time.timeScale = 1f;
 _pet.ResetPet();
 deathPanel.SetActive(false);
 pausePanel.SetActive(false);
 ShowGamePanel();
 }

 void QuitGame()
 {
#if UNITY_EDITOR
 UnityEditor.EditorApplication.isPlaying = false;
#else
 Application.Quit();
#endif
 }

 void OpenSettings()
 {
 gamePanel.SetActive(false);
 settingsPanel.SetActive(true);
 }

 void CloseSettings()
 {
 settingsPanel.SetActive(false);
 gamePanel.SetActive(true);
 }

 void ShowGamePanel()
 {
 gamePanel.SetActive(true);
 deathPanel.SetActive(false);
 pausePanel.SetActive(false);
 settingsPanel.SetActive(false);
 }

 void OnApplicationPause(bool pause)
 {
 if (pause && _pet && !_pet.data.isSleeping && !_pet.data.isDead)
 _pet.Sleep();
 }

 public static void BuildAndroid()
 {
 var playerSettings = UnityEditor.PlayerSettings.Android;
 UnityEditor.PlayerSettings.companyName = "LittleKitten";
 UnityEditor.PlayerSettings.productName = "Little Kitten Virtual Pet";
 UnityEditor.PlayerSettings.applicationIdentifier = "com.littlekitten.virtualpet";
 UnityEditor.PlayerSettings.defaultInterfaceOrientation = UnityEngine.UIScreenOrientation.Portrait;
 UnityEditor.PlayerSettings.Android.minSdkVersion = UnityEditor.AndroidSdkVersions.AndroidApiLevel21;
 UnityEditor.PlayerSettings.Android.targetSdkVersion = UnityEditor.AndroidSdkVersions.AndroidApiLevel34;

 var buildPlayerOptions = new UnityEditor.BuildPlayerOptions
 {
 scenes = new[] { "Assets/Scenes/Main.unity" },
 locationPathName = "Builds/LittleKitten.apk",
 target = UnityEditor.BuildTarget.Android,
 options = UnityEditor.BuildOptions.None
 };

 UnityEditor.BuildPipeline.BuildPlayer(buildPlayerOptions);
 UnityEditor.EditorApplication.Exit(0);
 }
}