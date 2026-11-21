using UnityEngine;
using UnityEngine.UI;

public class ActionButtons : MonoBehaviour
{
 [Header("Buttons")]
 public Button feedButton;
 public Button playButton;
 public Button cleanButton;
 public Button sleepButton;
 public Button medicineButton;

 [Header("Audio")]
 public AudioClip clickSound;
 public AudioClip feedSound;
 public AudioClip playSound;
 public AudioClip cleanSound;
 public AudioClip sleepSound;
 public AudioClip medicineSound;

 AudioSource _audioSource;
 Pet _pet;

 void Awake()
 {
 _audioSource = gameObject.AddComponent<AudioSource>();
 _audioSource.playOnAwake = false;
 }

 void Start()
 {
 _pet = Pet.Instance;
 feedButton.onClick.AddListener(() => { PlaySound(clickSound); _pet.Feed(); PlaySound(feedSound); });
 playButton.onClick.AddListener(() => { PlaySound(clickSound); _pet.Play(); PlaySound(playSound); });
 cleanButton.onClick.AddListener(() => { PlaySound(clickSound); _pet.Clean(); PlaySound(cleanSound); });
 sleepButton.onClick.AddListener(() => { PlaySound(clickSound); _pet.Sleep(); PlaySound(sleepSound); });
 medicineButton.onClick.AddListener(() => { PlaySound(clickSound); _pet.GiveMedicine(); PlaySound(medicineSound); });

 _pet.OnPetSleep += () => SetInteractable(false);
 _pet.OnPetWake += () => SetInteractable(true);
 _pet.OnPetDied += () => SetInteractable(false);

 SetInteractable(!_pet.data.isSleeping);
 LoadAudioClips();
 }

 void LoadAudioClips()
 {
 clickSound = Resources.Load<AudioClip>("Audio/click");
 feedSound = Resources.Load<AudioClip>("Audio/eat");
 playSound = Resources.Load<AudioClip>("Audio/play");
 cleanSound = Resources.Load<AudioClip>("Audio/clean");
 sleepSound = Resources.Load<AudioClip>("Audio/sleep");
 medicineSound = Resources.Load<AudioClip>("Audio/medicine");
 }

 void SetInteractable(bool state)
 {
 feedButton.interactable = state;
 playButton.interactable = state;
 cleanButton.interactable = state;
 sleepButton.interactable = state;
 medicineButton.interactable = state;
 }

 void PlaySound(AudioClip clip)
 {
 if (clip && _audioSource) _audioSource.PlayOneShot(clip);
 }
}