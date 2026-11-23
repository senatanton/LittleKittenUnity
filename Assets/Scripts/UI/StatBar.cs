using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatBar : MonoBehaviour
{
 [Header("Settings")]
 public string statKey = "hunger";
 public Color fillColor = Color.green;
 public Sprite backgroundSprite;

 [Header("References (auto-assigned if empty)")]
 public Image fillImage;
 public Image backgroundImage;
 public TMPro.TextMeshProUGUI labelText;

 void Reset()
 {
 fillImage = GetComponentInChildren<Image>();
 var imgs = GetComponentsInChildren<Image>();
 if (imgs.Length > 1) backgroundImage = imgs[1];
 labelText = GetComponentInChildren<TMPro.TextMeshProUGUI>();
 }

 public void SetValue(float current, float max)
 {
 if (fillImage != null)
 fillImage.fillAmount = max > 0 ? Mathf.Clamp01(current / max) : 0f;
 if (labelText != null)
 labelText.text = $"{statKey}: {Mathf.RoundToInt(current)}/{Mathf.RoundToInt(max)}";
 }
}
