using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build.Reporting;

public class SceneSetup : EditorWindow
{
 [MenuItem("LittleKitten/Setup Main Scene")]
 public static void SetupScene()
 {
 var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

 // Run auto-setup
 AutoSetupUI.Setup();

 // PetStats asset
 var statsAsset = ScriptableObject.CreateInstance<PetStats>();
 AssetDatabase.CreateAsset(statsAsset, "Assets/Resources/PetStats.asset");
 AssetDatabase.SaveAssets();

 // GameManager
 var gmGO = new GameObject("GameManager");
 var gm = gmGO.AddComponent<GameManager>();

 // Find UI elements
 var canvas = FindObjectOfType<Canvas>();
var pauseBtn = canvas.transform.Find("PauseButton")?.GetComponent<UnityEngine.UI.Button>();
var settingsBtn = canvas.transform.Find("SettingsButton")?.GetComponent<UnityEngine.UI.Button>();

 if (!pauseBtn)
 {
 pauseBtn = CreateButton("PauseButton", canvas.transform, "⏸", new Vector2(400, 400)).GetComponent<UnityEngine.UI.Button>();
 }
 if (!settingsBtn)
 {
 settingsBtn = CreateButton("SettingsButton", canvas.transform, "⚙", new Vector2(-400, 400)).GetComponent<UnityEngine.UI.Button>();
 }

 // Panels
 var deathPanel = CreatePanel("DeathPanel", canvas.transform);
 deathPanel.SetActive(false);
 var deathText = new GameObject("DeathText", typeof(TMPro.TextMeshProUGUI));
 deathText.transform.SetParent(deathPanel.transform, false);
 var dt = deathText.GetComponent<TMPro.TextMeshProUGUI>();
 dt.text = "Ваш питомец ушел...\nНажмите Рестарт";
 dt.fontSize = 48;
 dt.alignment = TextAlignmentOptions.Center;
 var dtRect = deathText.GetComponent<RectTransform>();
 dtRect.anchorMin = Vector2.zero;
 dtRect.anchorMax = Vector2.one;
 gm.deathPanel = deathPanel;

 var restartBtn = CreateButton("RestartButton", deathPanel.transform, "Рестарт").GetComponent<UnityEngine.UI.Button>();
 restartBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -100);
 gm.restartButton = restartBtn;

 var pausePanel = CreatePanel("PausePanel", canvas.transform);
 pausePanel.SetActive(false);
 pausePanel.GetComponent<UnityEngine.UI.Image>().color = new Color(0, 0, 0, 0.7f);
 gm.pausePanel = pausePanel;

 var resumeBtn = CreateButton("ResumeButton", pausePanel.transform, "Продолжить").GetComponent<UnityEngine.UI.Button>();
 resumeBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 80);
 gm.resumeButton = resumeBtn;

 var quitBtn = CreateButton("QuitButton", pausePanel.transform, "Выйти в меню").GetComponent<UnityEngine.UI.Button>();
 quitBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -20);
 gm.quitButton = quitBtn;

 var settingsPanel = CreatePanel("SettingsPanel", canvas.transform);
 settingsPanel.SetActive(false);
 settingsPanel.GetComponent<UnityEngine.UI.Image>().color = new Color(0.1f, 0.1f, 0.1f, 0.95f);
 gm.settingsPanel = settingsPanel;

 var musicSlider = CreateSlider("MusicVolume", settingsPanel.transform, "Громкость музыки", 0, 1, 0.3f);
 gm.musicVolumeSlider = musicSlider;

 var sfxSlider = CreateSlider("SfxVolume", settingsPanel.transform, "Громкость звуков", 0, 1, 1f);
 gm.sfxVolumeSlider = sfxSlider;

 var closeSettingsBtn = CreateButton("CloseSettingsButton", settingsPanel.transform, "Закрыть").GetComponent<UnityEngine.UI.Button>();
 closeSettingsBtn.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -200);
 gm.closeSettingsButton = closeSettingsBtn;

 gm.gamePanel = canvas.gameObject;
 gm.pauseButton = pauseBtn;
 gm.settingsButton = settingsBtn;

 EditorSceneManager.SaveScene(scene, "Assets/Scenes/Main.unity");
 AssetDatabase.SaveAssets();
 AssetDatabase.Refresh();
 Debug.Log("Main scene setup complete! Run 'Auto-Setup UI References' if needed.");
 }

 public static void BuildAndroid()
 {
 // Ensure scene exists
 if (!System.IO.File.Exists("Assets/Scenes/Main.unity"))
 {
 SetupScene();
 }

 var buildPlayerOptions = new BuildPlayerOptions
 {
 scenes = new[] { "Assets/Scenes/Main.unity" },
 locationPathName = "build/LittleKitten.apk",
 target = BuildTarget.Android,
 options = BuildOptions.None
 };

 var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
 var summary = report.summary;

 if (summary.result == BuildResult.Succeeded)
 {
 Debug.Log($"Build succeeded: {summary.totalSize} bytes");
 }
 else if (summary.result == BuildResult.Failed)
 {
 Debug.LogError("Build failed");
 foreach (var step in report.steps)
 {
 foreach (var msg in step.messages)
 {
 if (msg.type == LogType.Error)
 Debug.LogError(msg.content);
 }
 }
 }
 }
 
 static GameObject CreatePanel(string name, Transform parent)
 {
 var go = new GameObject(name, typeof(RectTransform), typeof(UnityEngine.UI.Image));
 go.transform.SetParent(parent, false);
 var img = go.GetComponent<UnityEngine.UI.Image>();
 img.color = new Color(0.1f, 0.1f, 0.1f, 0.9f);
 var rt = go.GetComponent<RectTransform>();
 rt.anchorMin = Vector2.zero;
 rt.anchorMax = Vector2.one;
 rt.offsetMin = rt.offsetMax = Vector2.zero;
 return go;
 }

 static GameObject CreateButton(string name, Transform parent, string text, Vector2? pos = null)
 {
 var go = new GameObject(name, typeof(RectTransform), typeof(UnityEngine.UI.Image), typeof(UnityEngine.UI.Button));
 go.transform.SetParent(parent, false);
 var img = go.GetComponent<UnityEngine.UI.Image>();
 img.color = new Color(0.2f, 0.5f, 0.8f);
 var btn = go.GetComponent<UnityEngine.UI.Button>();
 var colors = btn.colors;
 colors.highlightedColor = new Color(0.3f, 0.6f, 0.9f);
 colors.pressedColor = new Color(0.15f, 0.4f, 0.7f);
 btn.colors = colors;

 var textGO = new GameObject("Text", typeof(TMPro.TextMeshProUGUI));
 textGO.transform.SetParent(go.transform, false);
 var t = textGO.GetComponent<TMPro.TextMeshProUGUI>();
 t.text = text;
 t.fontSize = 32;
 t.alignment = TextAlignmentOptions.Center;
 t.enableAutoSizing = true;
 t.fontSizeMin = 18;
 t.fontSizeMax = 36;
 var tRect = textGO.GetComponent<RectTransform>();
 tRect.anchorMin = Vector2.zero;
 tRect.anchorMax = Vector2.one;
 tRect.offsetMin = tRect.offsetMax = new Vector2(10, 5);

 var le = go.AddComponent<UnityEngine.UI.LayoutElement>();
 le.preferredWidth = 200;
 le.preferredHeight = 80;

 if (pos.HasValue)
 {
 var rt = go.GetComponent<RectTransform>();
 rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 1f);
 rt.anchoredPosition = new Vector2(pos.Value.x, pos.Value.y - 50);
 }

 return go;
 }

 static UnityEngine.UI.Slider CreateSlider(string name, Transform parent, string label, float min, float max, float defaultVal)
 {
 var go = new GameObject(name, typeof(RectTransform), typeof(UnityEngine.UI.Slider), typeof(UnityEngine.UI.Image));
 go.transform.SetParent(parent, false);
 var slider = go.GetComponent<UnityEngine.UI.Slider>();
 slider.minValue = min;
 slider.maxValue = max;
 slider.value = defaultVal;

 var bg = go.GetComponent<UnityEngine.UI.Image>();
 bg.color = new Color(0.2f, 0.2f, 0.2f);

 var fillGO = new GameObject("Fill Area", typeof(RectTransform), typeof(UnityEngine.UI.Image));
 fillGO.transform.SetParent(go.transform, false);
 var fillBg = fillGO.GetComponent<UnityEngine.UI.Image>();
 fillBg.color = new Color(0.3f, 0.3f, 0.3f);

 var handleGO = new GameObject("Handle Slide Area", typeof(RectTransform), typeof(UnityEngine.UI.Image));
 handleGO.transform.SetParent(fillGO.transform, false);
 var handle = handleGO.GetComponent<UnityEngine.UI.Image>();
 handle.color = new Color(0.2f, 0.6f, 1f);

 var handleRect = handleGO.GetComponent<RectTransform>();
 handleRect.sizeDelta = new Vector2(20, 20);

 slider.fillRect = fillGO.GetComponent<RectTransform>();
 slider.handleRect = handleRect;

 var labelGO = new GameObject("Label", typeof(TMPro.TextMeshProUGUI));
 labelGO.transform.SetParent(go.transform, false);
 var labelText = labelGO.GetComponent<TMPro.TextMeshProUGUI>();
 labelText.text = label;
 labelText.fontSize = 24;
 labelText.alignment = TextAlignmentOptions.MiddleLeft;
 var labelRect = labelGO.GetComponent<RectTransform>();
 labelRect.anchorMin = new Vector2(0, 0.5f);
 labelRect.anchorMax = new Vector2(0, 0.5f);
 labelRect.pivot = new Vector2(0, 0.5f);
 labelRect.anchoredPosition = new Vector2(-200, 0);

 var rt = go.GetComponent<RectTransform>();
 rt.sizeDelta = new Vector2(400, 40);

 return slider;
 }
}