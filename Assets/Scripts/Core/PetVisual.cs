using UnityEngine;

public class PetVisual : MonoBehaviour
{
 [Header("Sprites (auto-loaded from Resources/Sprites/Pet/)")]
 public Sprite idleSprite;
 public Sprite happySprite;
 public Sprite sadSprite;
 public Sprite sleepSprite;
 public Sprite eatSprite;
 public Sprite playSprite;

 [Header("Components")]
 public SpriteRenderer spriteRenderer;
 public Animator animator;

 [Header("Settings")]
 public float bobAmount = 0.1f;
 public float bobSpeed = 1f;

 Pet _pet;
 Vector3 _startPos;
 Coroutine _bobRoutine;

 void Awake()
 {
 LoadSpritesFromResources();
 if (!spriteRenderer) spriteRenderer = GetComponent<SpriteRenderer>();
 }

 void Start()
 {
 _pet = Pet.Instance;
 _startPos = transform.localPosition;

 _pet.OnHungerChanged += UpdateVisual;
 _pet.OnSleepChanged += UpdateVisual;
 _pet.OnHappinessChanged += UpdateVisual;
 _pet.OnPetSleep += OnSleep;
 _pet.OnPetWake += OnWake;
 _pet.OnPetDied += OnDie;

 _bobRoutine = StartCoroutine(BobRoutine());
 UpdateVisual(_pet.GetHappinessNormalized());
 }

 void LoadSpritesFromResources()
 {
 idleSprite = Resources.Load<Sprite>("Sprites/Pet/idle");
 happySprite = Resources.Load<Sprite>("Sprites/Pet/happy");
 sadSprite = Resources.Load<Sprite>("Sprites/Pet/sad");
 sleepSprite = Resources.Load<Sprite>("Sprites/Pet/sleep");
 eatSprite = Resources.Load<Sprite>("Sprites/Pet/eat");
 playSprite = Resources.Load<Sprite>("Sprites/Pet/play");
 }

 void OnDestroy()
 {
 if (_pet == null) return;
 _pet.OnHungerChanged -= UpdateVisual;
 _pet.OnSleepChanged -= UpdateVisual;
 _pet.OnHappinessChanged -= UpdateVisual;
 _pet.OnPetSleep -= OnSleep;
 _pet.OnPetWake -= OnWake;
 _pet.OnPetDied -= OnDie;
 if (_bobRoutine != null) StopCoroutine(_bobRoutine);
 }

 void UpdateVisual(float _)
 {
 if (_pet.data.isSleeping) return;

 bool lowHunger = _pet.IsLow("hunger");
 bool lowSleep = _pet.IsLow("sleep");
 bool lowHappy = _pet.IsLow("happiness");

 if (lowHunger || lowSleep || lowHappy)
 SetSprite(sadSprite);
 else if (_pet.GetHappinessNormalized() > 0.7f)
 SetSprite(happySprite);
 else
 SetSprite(idleSprite);
 }

 void OnSleep() => SetSprite(sleepSprite);
 void OnWake() => UpdateVisual(0);
 void OnDie() => SetSprite(sadSprite);

 public void SetSprite(Sprite s)
 {
 if (s && spriteRenderer) spriteRenderer.sprite = s;
 }

 public void PlayEatAnimation()
 {
 SetSprite(eatSprite);
 Invoke(nameof(ResetSprite), 1f);
 }

 public void PlayPlayAnimation()
 {
 SetSprite(playSprite);
 Invoke(nameof(ResetSprite), 1f);
 }

 public void PlayPetAnimation()
 {
 transform.localScale = Vector3.one * 1.1f;
 Invoke(nameof(ResetScale), 0.2f);
 }

 void ResetSprite() => UpdateVisual(0);
 void ResetScale() => transform.localScale = Vector3.one;

 System.Collections.IEnumerator BobRoutine()
 {
 while (true)
 {
 if (!_pet.data.isSleeping)
 {
 float y = _startPos.y + Mathf.Sin(Time.time * bobSpeed) * bobAmount;
 transform.localPosition = new Vector3(_startPos.x, y, _startPos.z);
 }
 yield return null;
 }
 }
}