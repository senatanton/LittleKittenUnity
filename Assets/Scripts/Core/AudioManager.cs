using UnityEngine;

public class AudioManager : MonoBehaviour
{
 public static AudioManager Instance { get; private set; }

 [Header("Music")]
 public AudioClip backgroundMusic;
 public float musicVolume = 0.3f;

 [Header("SFX")]
 public float sfxVolume = 1f;

 AudioSource _musicSource;
 AudioSource _sfxSource;

 void Awake()
 {
 if (Instance && Instance != this) { Destroy(gameObject); return; }
 Instance = this;
 DontDestroyOnLoad(gameObject);

 _musicSource = gameObject.AddComponent<AudioSource>();
 _musicSource.loop = true;
 _musicSource.volume = musicVolume;
 _musicSource.playOnAwake = false;

 _sfxSource = gameObject.AddComponent<AudioSource>();
 _sfxSource.volume = sfxVolume;
 _sfxSource.playOnAwake = false;

 LoadClips();
 }

 void LoadClips()
 {
 backgroundMusic = Resources.Load<AudioClip>("Audio/bg_music");
 }

 void Start()
 {
 if (backgroundMusic) PlayMusic(backgroundMusic);
 }

 public void PlayMusic(AudioClip clip)
 {
 if (!clip) return;
 _musicSource.clip = clip;
 _musicSource.Play();
 }

 public void StopMusic() => _musicSource.Stop();

 public void SetMusicVolume(float v) { musicVolume = v; _musicSource.volume = v; }
 public void SetSfxVolume(float v) { sfxVolume = v; _sfxSource.volume = v; }

 public void PlaySFX(AudioClip clip)
 {
 if (clip) _sfxSource.PlayOneShot(clip);
 }
}