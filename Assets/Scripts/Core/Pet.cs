using UnityEngine;
using System;

[Serializable]
public class PetData
{
 public float hunger = 100f;
 public float sleep = 100f;
 public float happiness = 100f;
 public float cleanliness = 100f;
 public float health = 100f;
 public long lastUpdateTicks;
 public bool isSleeping;
 public string petName = "Kitty";
}

public class Pet : MonoBehaviour
{
 public static Pet Instance { get; private set; }

 [Header("Config")]
 public PetStats stats;

 [Header("State")]
 public PetData data = new PetData();

 public event Action<float> OnHungerChanged;
 public event Action<float> OnSleepChanged;
 public event Action<float> OnHappinessChanged;
 public event Action<float> OnCleanlinessChanged;
 public event Action<float> OnHealthChanged;
 public event Action OnPetDied;
 public event Action OnPetSleep;
 public event Action OnPetWake;

 float _hunger, _sleep, _happiness, _cleanliness, _health;

 void Awake()
 {
 if (Instance != null && Instance != this)
 {
 Destroy(gameObject);
 return;
 }
 Instance = this;
 DontDestroyOnLoad(gameObject);
 Load();
 }

 void Update()
 {
 if (data.isSleeping) return;
 Tick(Time.deltaTime);
 }

 void OnApplicationPause(bool pause)
 {
 if (pause) Save();
 else Load();
 }

 void OnApplicationQuit() => Save();

 void Tick(float dt)
 {
 bool changed = false;

 _hunger = Mathf.Clamp(data.hunger - stats.hungerDecay * dt, 0, stats.maxHunger);
 _sleep = Mathf.Clamp(data.sleep - stats.sleepDecay * dt, 0, stats.maxSleep);
 _happiness = Mathf.Clamp(data.happiness - stats.happinessDecay * dt, 0, stats.maxHappiness);
 _cleanliness = Mathf.Clamp(data.cleanliness - stats.cleanlinessDecay * dt, 0, stats.maxCleanliness);
 _health = Mathf.Clamp(data.health - stats.healthDecay * dt, 0, stats.maxHealth);

 if (Mathf.Abs(_hunger - data.hunger) > 0.01f) { data.hunger = _hunger; OnHungerChanged?.Invoke(GetHungerNormalized()); changed = true; }
 if (Mathf.Abs(_sleep - data.sleep) > 0.01f) { data.sleep = _sleep; OnSleepChanged?.Invoke(GetSleepNormalized()); changed = true; }
 if (Mathf.Abs(_happiness - data.happiness) > 0.01f) { data.happiness = _happiness; OnHappinessChanged?.Invoke(GetHappinessNormalized()); changed = true; }
 if (Mathf.Abs(_cleanliness - data.cleanliness) > 0.01f) { data.cleanliness = _cleanliness; OnCleanlinessChanged?.Invoke(GetCleanlinessNormalized()); changed = true; }
 if (Mathf.Abs(_health - data.health) > 0.01f) { data.health = _health; OnHealthChanged?.Invoke(GetHealthNormalized()); changed = true; }

 if (changed) CheckCriticalState();
 }

 void CheckCriticalState()
 {
 if (data.hunger <= 0 || data.sleep <= 0 || data.happiness <= 0 || data.health <= 0)
 OnPetDied?.Invoke();
 }

 public void Feed()
 {
 if (data.isSleeping) return;
 data.hunger = Mathf.Min(data.hunger + stats.feedAmount, stats.maxHunger);
 data.happiness = Mathf.Min(data.happiness + stats.petAmount, stats.maxHappiness);
 data.cleanliness = Mathf.Max(data.cleanliness - stats.dirtAmount, 0);
 OnHungerChanged?.Invoke(GetHungerNormalized());
 OnHappinessChanged?.Invoke(GetHappinessNormalized());
 OnCleanlinessChanged?.Invoke(GetCleanlinessNormalized());
 }

 public void Sleep()
 {
 if (data.isSleeping) return;
 data.isSleeping = true;
 OnPetSleep?.Invoke();
 Invoke(nameof(WakeUp), 5f);
 }

 void WakeUp()
 {
 data.isSleeping = false;
 data.sleep = Mathf.Min(data.sleep + stats.sleepAmount, stats.maxSleep);
 data.hunger = Mathf.Max(data.hunger - stats.feedAmount * 0.5f, 0);
 data.happiness = Mathf.Min(data.happiness + stats.petAmount * 0.5f, stats.maxHappiness);
 OnSleepChanged?.Invoke(GetSleepNormalized());
 OnHungerChanged?.Invoke(GetHungerNormalized());
 OnHappinessChanged?.Invoke(GetHappinessNormalized());
 OnPetWake?.Invoke();
 }

 public void Play()
 {
 if (data.isSleeping) return;
 data.happiness = Mathf.Min(data.happiness + stats.playAmount, stats.maxHappiness);
 data.hunger = Mathf.Max(data.hunger - stats.feedAmount * 0.3f, 0);
 data.sleep = Mathf.Max(data.sleep - stats.sleepDecay * 2f, 0);
 data.cleanliness = Mathf.Max(data.cleanliness - stats.dirtAmount, 0);
 OnHappinessChanged?.Invoke(GetHappinessNormalized());
 OnHungerChanged?.Invoke(GetHungerNormalized());
 OnSleepChanged?.Invoke(GetSleepNormalized());
 OnCleanlinessChanged?.Invoke(GetCleanlinessNormalized());
 }

 public void Pet()
 {
 if (data.isSleeping) return;
 data.happiness = Mathf.Min(data.happiness + stats.petAmount, stats.maxHappiness);
 OnHappinessChanged?.Invoke(GetHappinessNormalized());
 }

 public void Clean()
 {
 if (data.isSleeping) return;
 data.cleanliness = Mathf.Min(data.cleanliness + stats.cleanAmount, stats.maxCleanliness);
 data.happiness = Mathf.Min(data.happiness + stats.petAmount * 0.5f, stats.maxHappiness);
 OnCleanlinessChanged?.Invoke(GetCleanlinessNormalized());
 OnHappinessChanged?.Invoke(GetHappinessNormalized());
 }

 public void GiveMedicine()
 {
 if (data.isSleeping) return;
 data.health = Mathf.Min(data.health + stats.medicineAmount, stats.maxHealth);
 data.happiness = Mathf.Max(data.happiness - stats.petAmount, 0);
 OnHealthChanged?.Invoke(GetHealthNormalized());
 OnHappinessChanged?.Invoke(GetHappinessNormalized());
 }

 public float GetHungerNormalized() => data.hunger / stats.maxHunger;
 public float GetSleepNormalized() => data.sleep / stats.maxSleep;
 public float GetHappinessNormalized() => data.happiness / stats.maxHappiness;
 public float GetCleanlinessNormalized() => data.cleanliness / stats.maxCleanliness;
 public float GetHealthNormalized() => data.health / stats.maxHealth;

 public bool IsLow(string stat)
 {
 float v = stat switch
 {
 "hunger" => GetHungerNormalized(),
 "sleep" => GetSleepNormalized(),
 "happiness" => GetHappinessNormalized(),
 "cleanliness" => GetCleanlinessNormalized(),
 "health" => GetHealthNormalized(),
 _ => 1f
 };
 return v <= stats.lowThreshold;
 }

 public bool IsCritical(string stat)
 {
 float v = stat switch
 {
 "hunger" => GetHungerNormalized(),
 "sleep" => GetSleepNormalized(),
 "happiness" => GetHappinessNormalized(),
 "cleanliness" => GetCleanlinessNormalized(),
 "health" => GetHealthNormalized(),
 _ => 1f
 };
 return v <= stats.criticalThreshold;
 }

 void Save()
 {
 data.lastUpdateTicks = DateTime.UtcNow.Ticks;
 string json = JsonUtility.ToJson(data);
 PlayerPrefs.SetString("PetData", json);
 PlayerPrefs.Save();
 }

 void Load()
 {
 if (!PlayerPrefs.HasKey("PetData")) return;
 string json = PlayerPrefs.GetString("PetData");
 data = JsonUtility.FromJson<PetData>(json);
 ApplyOfflineProgress();
 }

 void ApplyOfflineProgress()
 {
 long now = DateTime.UtcNow.Ticks;
 long elapsedTicks = now - data.lastUpdateTicks;
 float elapsedSeconds = elapsedTicks / (float)TimeSpan.TicksPerSecond;
 if (elapsedSeconds <= 0) return;

 if (!data.isSleeping)
 {
 data.hunger = Mathf.Max(data.hunger - stats.hungerDecay * elapsedSeconds, 0);
 data.sleep = Mathf.Max(data.sleep - stats.sleepDecay * elapsedSeconds, 0);
 data.happiness = Mathf.Max(data.happiness - stats.happinessDecay * elapsedSeconds, 0);
 data.cleanliness = Mathf.Max(data.cleanliness - stats.cleanlinessDecay * elapsedSeconds, 0);
 data.health = Mathf.Max(data.health - stats.healthDecay * elapsedSeconds, 0);
 }
 else
 {
 float sleepGain = stats.sleepAmount * (elapsedSeconds / 300f);
 data.sleep = Mathf.Min(data.sleep + sleepGain, stats.maxSleep);
 if (data.sleep >= stats.maxSleep * 0.9f) data.isSleeping = false;
 }

 data.lastUpdateTicks = now;
 }

 public void ResetPet()
 {
 data = new PetData();
 OnHungerChanged?.Invoke(1f);
 OnSleepChanged?.Invoke(1f);
 OnHappinessChanged?.Invoke(1f);
 OnCleanlinessChanged?.Invoke(1f);
 OnHealthChanged?.Invoke(1f);
 }
}