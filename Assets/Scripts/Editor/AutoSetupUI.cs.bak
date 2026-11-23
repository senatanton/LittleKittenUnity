using UnityEngine;
using UnityEditor;

public static class AutoSetupUI
{
 [MenuItem("Tools/Little Kitten/Auto-Setup UI References")]
 public static void Setup()
 {
 // Find or create Canvas
 Canvas canvas = FindObjectOfType<Canvas>();
 if (!canvas)
 {
 GameObject canvasGO = new GameObject("Canvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
 canvas = canvasGO.GetComponent<Canvas>();
 canvas.renderMode = RenderMode.ScreenSpaceOverlay;
 var scaler = canvasGO.GetComponent<CanvasScaler>();
 scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
 scaler.referenceResolution = new Vector2(1920, 1080);
 }

 // Background
 GameObject bgGO = GameObject.Find("Background");
 if (!bgGO)
 {
 bgGO = new GameObject("Background", typeof(UnityEngine.UI.Image));
 bgGO.transform.SetParent(canvas.transform, false);
 var img = bgGO.GetComponent<UnityEngine.UI.Image>();
 img.sprite = Resources.Load<Sprite>("Sprites/bg_main");
 img.type = UnityEngine.UI.Image.Type.Simple;
 var rt = bgGO.GetComponent<RectTransform>();
 rt.anchorMin = Vector2.zero;
 rt.anchorMax = Vector2.one;
 rt.offsetMin = rt.offsetMax = Vector2.zero;
 bgGO.transform.SetAsFirstSibling();
 }

 // Pet Visual
 GameObject petGO = GameObject.Find("Pet");
 if (!petGO)
 {
 petGO = new GameObject("Pet", typeof(SpriteRenderer), typeof(PetVisual));
 petGO.transform.SetParent(canvas.transform, false);
 var sr = petGO.GetComponent<SpriteRenderer>();
 sr.sortingOrder = 10;
 var pv = petGO.GetComponent<PetVisual>();
 var rt = petGO.GetComponent<RectTransform>();
 rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.4f);
 rt.anchoredPosition = Vector2.zero;
 rt.sizeDelta = new Vector2(300, 300);
 }

 // Stat Bars Container
 GameObject statsGO = GameObject.Find("StatBars");
 if (!statsGO)
 {
 statsGO = new GameObject("StatBars");
 statsGO.transform.SetParent(canvas.transform, false);
 var rt = statsGO.AddComponent<RectTransform>();
 rt.anchorMin = new Vector2(0.05f, 0.85f);
 rt.anchorMax = new Vector2(0.95f, 0.95f);
 rt.offsetMin = rt.offsetMax = Vector2.zero;
 var hlg = statsGO.AddComponent<HorizontalLayoutGroup>();
 hlg.spacing = 20;
 hlg.childAlignment = TextAnchor.MiddleCenter;
 hlg.childControlWidth = hlg.childControlHeight = true;
 }

 // Create 5 Stat Bars (Hunger, Sleep, Happiness, Cleanliness, Health)
 string[] statNames = { "Hunger", "Sleep", "Happiness", "Cleanliness", "Health" };
 string[] statKeys = { "hunger", "sleep", "happiness", "cleanliness", "health" };
 Color[] statColors = {
 new Color(1f, 0.6f, 0.2f), // hunger - orange
 new Color(0.3f, 0.6f, 1f), // sleep - blue
 new Color(1f, 0.8f, 0.2f), // happiness - yellow
 new Color(0.2f, 1f, 0.5f), // cleanliness - green
 new Color(1f, 0.3f, 0.3f)  // health - red
 };

 for (int i = 0; i < 5; i++)
 {
 string name = statNames[i];
 GameObject barGO = statsGO.transform.Find(name)?.gameObject;
 if (!barGO)
 {
 barGO = new GameObject(name, typeof(StatBar));
 barGO.transform.SetParent(statsGO.transform, false);
 }
 var sb = barGO.GetComponent<StatBar>();
 sb.statKey = statKeys[i];
 sb.fillColor = statColors[i];
 sb.backgroundSprite = Resources.Load<Sprite>("Sprites/stat_bg");
 sb.fillSprite = Resources.Load<Sprite>("Sprites/stat_fill");
 }

 // Action Buttons
 GameObject buttonsGO = GameObject.Find("ActionButtons");
 if (!buttonsGO)
 {
 buttonsGO = new GameObject("ActionButtons", typeof(ActionButtons));
 buttonsGO.transform.SetParent(canvas.transform, false);
 var rt = buttonsGO.GetComponent<RectTransform>();
 rt.anchorMin = new Vector2(0.1f, 0.05f);
 rt.anchorMax = new Vector2(0.9f, 0.25f);
 rt.offsetMin = rt.offsetMax = Vector2.zero;
 var hlg = buttonsGO.AddComponent<HorizontalLayoutGroup>();
 hlg.spacing = 15;
 hlg.childAlignment = TextAnchor.MiddleCenter;
 hlg.childControlWidth = hlg.childControlHeight = true;
 }

 var ab = buttonsGO.GetComponent<ActionButtons>();
 ab.feedButton = CreateButton("BtnFeed", "btn_feed", buttonsGO.transform, () => Pet.Instance.Feed());
 ab.playButton = CreateButton("BtnPlay", "btn_play", buttonsGO.transform, () => Pet.Instance.Play());
 ab.cleanButton = CreateButton("BtnClean", "btn_clean", buttonsGO.transform, () => Pet.Instance.Clean());
 ab.sleepButton = CreateButton("BtnSleep", "btn_sleep", buttonsGO.transform, () => Pet.Instance.Sleep());
 ab.medicineButton = CreateButton("BtnMedicine", "btn_medicine", buttonsGO.transform, () => Pet.Instance.GiveMedicine());

 EditorUtility.SetDirty(canvas);
 Debug.Log("UI Auto-setup complete (5 stats + 5 actions).");
 }

 static UnityEngine.UI.Button CreateButton(string name, string spriteName, Transform parent, System.Action onClick)
 {
 GameObject btnGO = parent.Find(name)?.gameObject;
 if (!btnGO)
 {
 btnGO = new GameObject(name, typeof(UnityEngine.UI.Button), typeof(UnityEngine.UI.Image));
 btnGO.transform.SetParent(parent, false);
 }
 var btn = btnGO.GetComponent<UnityEngine.UI.Button>();
 var img = btnGO.GetComponent<UnityEngine.UI.Image>();
 img.sprite = Resources.Load<Sprite>($"Sprites/{spriteName}");
 img.type = UnityEngine.UI.Image.Type.Simple;
 btn.onClick.RemoveAllListeners();
 btn.onClick.AddListener(() => onClick?.Invoke());
 var rt = btnGO.GetComponent<RectTransform>();
 rt.sizeDelta = new Vector2(120, 120);
 return btn;
 }
}