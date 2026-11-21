using UnityEngine;

[CreateAssetMenu(fileName = "PetStats", menuName = "LittleKitten/Pet Stats")]
public class PetStats : ScriptableObject
{
 [Header("Max Values")]
 public float maxHunger = 100f;
 public float maxSleep = 100f;
 public float maxHappiness = 100f;
 public float maxCleanliness = 100f;
 public float maxHealth = 100f;

 [Header("Decay Rates (per second)")]
 public float hungerDecay = 0.02f;
 public float sleepDecay = 0.015f;
 public float happinessDecay = 0.01f;
 public float cleanlinessDecay = 0.008f;
 public float healthDecay = 0.005f;

 [Header("Action Effects")]
 public float feedAmount = 25f;
 public float sleepAmount = 40f;
 public float playAmount = 15f;
 public float petAmount = 5f;
 public float cleanAmount = 30f;
 public float medicineAmount = 35f;
 public float dirtAmount = 5f;

 [Header("Thresholds")]
 [Range(0f, 1f)] public float lowThreshold = 0.3f;
 [Range(0f, 1f)] public float criticalThreshold = 0.1f;
}