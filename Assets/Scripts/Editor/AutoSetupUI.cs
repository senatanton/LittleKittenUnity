using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using TMPro;

public static class AutoSetupUI
{
 [MenuItem("Tools/Little Kitten/Auto-Setup UI References")]
 public static void Setup()
 {
 // Find or create Canvas
 Canvas canvas = Object.FindAnyObjectByType<Canvas>();
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
 bgGO = new GameObject("Background", typeof(Image));
 bgGO.transform.SetParent(canvas.transform, false);
 var img = bgGO.GetComponent<Image>();
 img.sprite = Resources.Load<Sprite>("Sprites/bg_main");
 img.type = Image.Type.Simple;
 var rt = bgGO.GetComponent<RectTransform>();
 rt.anchorMin = Vector2.zero;
 rt.anchorMax = Vector2.one;
 rt.offsetMin = rt.offsetMax = Vector2.zero;
 bgGO.transform.SetAsFirstSibling();
 }
 else
 {
 bgGO.transform.SetParent(canvas.transform, false);
 bgGO.transform.SetAsFirstSibling();
 }

 // Pet Visual (UI Image, not SpriteRenderer, since inside Canvas)
 GameObject petGO = GameObject.Find("Pet");
 if (!petGO)
 {
 petGO = new GameObject("Pet", typeof(Image), typeof(PetVisual));
 petGO.transform.SetParent(canvas.transform, false);
 var img = petGO.GetComponent<Image>();
 img.raycastTarget = false;
 var pv = petGO.GetComponent<PetVisual>();
 var rt = petGO.GetComponent<RectTransform>();
 rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.4f);
 rt.anchoredPosition = Vector2.zero;
 rt.sizeDelta = new Vector2(300, 300);
 }
 else
 {
 petGO.transform.SetParent(canvas.transform, false);
 var rt = petGO.GetComponent<RectTransform>();
 if (!rt) rt = petGO.AddComponent<RectTransform>();
 rt.anchorMin = rt.anchorMax = new Vector2(0.5f, 0.4f);
 rt.anchoredPosition = Vector2.zero;
 rt.sizeDelta = new Vector2(300, 300);
 var img = petGO.GetComponent<Image>();
 if (!img)
 {
 img = petGO.AddComponent<Image>();
 img.raycastTarget = false;
 }
 var pv = petGO.GetComponent<PetVisual>();
 if (!pv) petGO.AddComponent<PetVisual>();
 }

 // Stat Bars Container
 GameObject statsGO = GameObject.Find("StatBars");
 if (!statsGO)
 {
 statsGO = new GameObject("StatBars", typeof(RectTransform));
 statsGO.transform.SetParent(canvas.transform, false);
 var rt = statsGO.GetComponent<RectTransform>();
 rt.anchorMin = new Vector2(0.05f, 0.85f);
 rt.anchorMax = new Vector2(0.95f, 0.95f);
 rt.offsetMin = rt.offsetMax = Vector2.zero;
 var hlg = statsGO.AddComponent<HorizontalLayoutGroup>();
 hlg.spacing = 20;
 hlg.childAlignment = TextAnchor.MiddleCenter;
 hlg.childControlWidth = hlg.childControlHeight = true;
 }
 else
 {
 statsGO.transform.SetParent(canvas.transform, false);
 var rt = statsGO.GetComponent<RectTransform>();
 if (!rt) rt = statsGO.AddComponent<RectTransform>();
 rt.anchorMin = new Vector2(0.05f, 0.85f);
 rt.anchorMax = new Vector2(0.95f, 0.95f);
 rt.offsetMin = rt.offsetMax = Vector2.zero;
 var hlg = statsGO.GetComponent<HorizontalLayoutGroup>();
 if (!hlg) hlg = statsGO.AddComponent<HorizontalLayoutGroup>();
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
 new Color(1f, 0.3f, 0.3f) // health - red
 };

 // Clear all existing children of statsGO first
 for (int i = statsGO.transform.childCount - 1; i >= 0; i--)
 {
 Object.DestroyImmediate(statsGO.transform.GetChild(i).gameObject);
 }

 for (int i = 0; i < 5; i++)
 {
 string name = statNames[i];
 GameObject barGO = new GameObject(name);
 barGO.transform.SetParent(statsGO.transform, false);
 // Ensure StatBar component
 var sb = barGO.AddComponent<StatBar>();

 // Create fill Image (main Graphic on barGO)
 var fillImg = barGO.AddComponent<Image>();
 fillImg.color = statColors[i];
 fillImg.type = Image.Type.Filled;
 fillImg.fillMethod = Image.FillMethod.Horizontal;
 fillImg.fillOrigin = (int)Image.OriginHorizontal.Left;

 // Create background as CHILD (can only have one Graphic per GameObject)
 var bgGO_bg = new GameObject("Background");
 bgGO_bg.transform.SetParent(barGO.transform, false);
 var bgImg = bgGO_bg.AddComponent<Image>();
 bgImg.color = new Color(0, 0, 0, 0.5f);
 bgImg.type = Image.Type.Simple;
 var bgRt = bgGO_bg.GetComponent<RectTransform>();
 bgRt.anchorMin = Vector2.zero;
 bgRt.anchorMax = Vector2.one;
 bgRt.offsetMin = Vector2.zero;
 bgRt.offsetMax = Vector2.zero;

 // Label
 var label = new GameObject("Label", typeof(TextMeshProUGUI));
 label.transform.SetParent(barGO.transform, false);
 var labelText = label.GetComponent<TextMeshProUGUI>();
 labelText.fontSize = 24;
 labelText.alignment = TextAlignmentOptions.Left;
 var labelRt = label.GetComponent<RectTransform>();
 labelRt.anchorMin = Vector2.zero;
 labelRt.anchorMax = Vector2.one;
 labelRt.offsetMin = new Vector2(10, 0);
 labelRt.offsetMax = Vector2.zero;

 sb.statKey = statKeys[i];
 sb.fillColor = statColors[i];
 sb.fillImage = fillImg;
 sb.backgroundImage = bgImg;
 sb.labelText = labelText;
 }

 // Action Buttons
 GameObject buttonsGO = GameObject.Find("ActionButtons");
 if (!buttonsGO)
 {
 buttonsGO = new GameObject("ActionButtons", typeof(RectTransform), typeof(ActionButtons));
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
 else
 {
 buttonsGO.transform.SetParent(canvas.transform, false);
 var rt = buttonsGO.GetComponent<RectTransform>();
 if (!rt) rt = buttonsGO.AddComponent<RectTransform>();
 rt.anchorMin = new Vector2(0.1f, 0.05f);
 rt.anchorMax = new Vector2(0.9f, 0.25f);
 rt.offsetMin = rt.offsetMax = Vector2.zero;
 var hlg = buttonsGO.GetComponent<HorizontalLayoutGroup>();
 if (!hlg) hlg = buttonsGO.AddComponent<HorizontalLayoutGroup>();
 hlg.spacing = 15;
 hlg.childAlignment = TextAnchor.MiddleCenter;
 hlg.childControlWidth = hlg.childControlHeight = true;
 }

 var ab = buttonsGO.GetComponent<ActionButtons>();
 if (!ab) ab = buttonsGO.AddComponent<ActionButtons>();
 ab.feedButton = CreateButton("BtnFeed", "btn_feed", buttonsGO.transform, () => Pet.Instance.Feed());
 ab.playButton = CreateButton("BtnPlay", "btn_play", buttonsGO.transform, () => Pet.Instance.Play());
 ab.cleanButton = CreateButton("BtnClean", "btn_clean", buttonsGO.transform, () => Pet.Instance.Clean());
 ab.sleepButton = CreateButton("BtnSleep", "btn_sleep", buttonsGO.transform, () => Pet.Instance.Sleep());
 ab.medicineButton = CreateButton("BtnMedicine", "btn_medicine", buttonsGO.transform, () => Pet.Instance.GiveMedicine());

 EditorUtility.SetDirty(canvas);
 Debug.Log("UI Auto-setup complete (5 stats + 5 actions).");
 }

 static Button CreateButton(string name, string spriteName, Transform parent, System.Action onClick)
 {
 GameObject btnGO = parent.Find(name)?.gameObject;
 if (!btnGO)
 {
 btnGO = new GameObject(name, typeof(Button), typeof(Image));
 btnGO.transform.SetParent(parent, false);
 }
 var btn = btnGO.GetComponent<Button>();
 var img = btnGO.GetComponent<Image>();
 img.sprite = Resources.Load<Sprite>($"Sprites/{spriteName}");
 img.type = Image.Type.Simple;
 btn.onClick.RemoveAllListeners();
 btn.onClick.AddListener(() => onClick?.Invoke());
 var rt = btnGO.GetComponent<RectTransform>();
 rt.sizeDelta = new Vector2(120, 120);
 return btn;
 }
}








