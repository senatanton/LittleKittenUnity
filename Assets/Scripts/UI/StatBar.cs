using UnityEngine;
using UnityEngine.UI;

public class StatBar : MonoBehaviour
{
 [Header("Settings")]
 public string statKey = "hunger";
 public Color fillColor = Color.green;
 public Sprite backgroundSprite;
 public Sprite fillSprite;

 [Header("Auto-assigned")]
 public Image backgroundImage;
 public Image fillImage;

 Pet _pet;

 void Awake()
 {
 SetupImages();
 }

 void Start()
 {
 _pet = Pet.Instance;
 Subscribe();
 UpdateBar(GetInitialValue());
 }

 void SetupImages()
 {
 var rt = GetComponent<RectTransform>();
 rt.sizeDelta = new Vector2(200, 40);

 // Background
 GameObject bgGO = transform.Find("Background")?.gameObject;
 if (!bgGO)
 {
 bgGO = new GameObject("Background", typeof(Image));
 bgGO.transform.SetParent(transform, false);
 bgGO.transform.SetAsFirstSibling();
 }
 backgroundImage = bgGO.GetComponent<Image>();
 backgroundImage.sprite = backgroundSprite ?? Resources.Load<Sprite>("Sprites/stat_bg");
 backgroundImage.type = Image.Type.Sliced;
 var bgRT = bgGO.GetComponent<RectTransform>();
 bgRT.anchorMin = Vector2.zero;
 bgRT.anchorMax = Vector2.one;
 bgRT.offsetMin = bgRT.offsetMax = Vector2.zero;

 // Fill
 GameObject fillGO = transform.Find("Fill")?.gameObject;
 if (!fillGO)
 {
 fillGO = new GameObject("Fill", typeof(Image));
 fillGO.transform.SetParent(transform, false);
 }
 fillImage = fillGO.GetComponent<Image>();
 fillImage.sprite = fillSprite ?? Resources.Load<Sprite>("Sprites/stat_fill");
 fillImage.type = Image.Type.Filled;
 fillImage.fillMethod = Image.FillMethod.Horizontal;
 fillImage.fillOrigin = (int)Image.OriginHorizontal.Left;
 fillImage.color = fillColor;
 var fillRT = fillGO.GetComponent<RectTransform>();
 fillRT.anchorMin = Vector2.zero;
 fillRT.anchorMax = Vector2.one;
 fillRT.offsetMin = fillRT.offsetMax = Vector2.zero;

 // Label
 GameObject labelGO = transform.Find("Label")?.gameObject;
 if (!labelGO)
 {
 labelGO = new GameObject("Label", typeof(TMPro.TextMeshProUGUI));
 labelGO.transform.SetParent(transform, false);
 }
 var label = labelGO.GetComponent<TMPro.TextMeshProUGUI>();
 label.text = statKey.ToUpper();
 label.fontSize = 18;
 label.color = Color.white;
 label.alignment = TextAlignmentOptions.Center;
 var labelRT = labelGO.GetComponent<RectTransform>();
 labelRT.anchorMin = Vector2.zero;
 labelRT.anchorMax = Vector2.one;
 labelRT.offsetMin = labelRT.offsetMax = Vector2.zero;
 }

 float GetInitialValue()
 {
 return statKey switch
 {
 "hunger" => _pet.GetHungerNormalized(),
 "sleep" => _pet.GetSleepNormalized(),
 "happiness" => _pet.GetHappinessNormalized(),
 "cleanliness" => _pet.GetCleanlinessNormalized(),
 "health" => _pet.GetHealthNormalized(),
 _ => 1f
 };
 }

 void Subscribe()
 {
 switch (statKey)
 {
 case "hunger": _pet.OnHungerChanged += UpdateBar; break;
 case "sleep": _pet.OnSleepChanged += UpdateBar; break;
 case "happiness": _pet.OnHappinessChanged += UpdateBar; break;
 case "cleanliness": _pet.OnCleanlinessChanged += UpdateBar; break;
 case "health": _pet.OnHealthChanged += UpdateBar; break;
 }
 }

 void OnDestroy()
 {
 if (_pet == null) return;
 switch (statKey)
 {
 case "hunger": _pet.OnHungerChanged -= UpdateBar; break;
 case "sleep": _pet.OnSleepChanged -= UpdateBar; break;
 case "happiness": _pet.OnHappinessChanged -= UpdateBar; break;
 case "cleanliness": _pet.OnCleanlinessChanged -= UpdateBar; break;
 case "health": _pet.OnHealthChanged -= UpdateBar; break;
 }
 }

 public void UpdateBar(float normalized)
 {
 if (fillImage) fillImage.fillAmount = normalized;
 if (fillImage) fillImage.color = Color.Lerp(Color.red, fillColor, normalized);
 }
}