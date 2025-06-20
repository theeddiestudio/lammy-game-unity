using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // create singleton instance to allow other scripts to access this manager easily.
    public static AudioManager instance;

    [Header("Volume Controls")]
    [Range(0f, 0.3f)]
    public float musicVolume = 0.3f;
    [Range(0f, 1f)]
    public float sfxVolume = 1.0f;

    // AudioSources
    private AudioSource musicSource;
    private AudioSource sfxSource;

    [Header("Audio Clips")]
    public AudioClip backgroundMusic;
    public AudioClip jumpSound;
    public AudioClip scoreSound;
    public AudioClip crashSound;

    void Awake()
    {
        // Singleton Design Pattern
        // allow only one creation of this object
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        // Otherwise
        instance = this;
        DontDestroyOnLoad(gameObject);

        // Music 
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true; // Music should loop

        // sound effects
        sfxSource = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {
        // set volumes
        musicSource.volume = musicVolume;
        sfxSource.volume = sfxVolume;

        // play BGM
        if (backgroundMusic != null)
        {
            musicSource.clip = backgroundMusic;
            musicSource.Play();
            // musicSource.Pause(); // dont play until hit play
        }
    }

    // update is called once per frame
    void Update()
    {
        // change volume in RT from inspector
        if (musicSource.volume != musicVolume)
        {
            musicSource.volume = musicVolume;
        }
        if (sfxSource.volume != sfxVolume)
        {
            sfxSource.volume = sfxVolume;
        }
    }

    public void PauseMusic()
    {
        if (musicSource.isPlaying)
        {
            musicSource.Pause();
        }
    }

    public void UnpauseMusic()
    {
        if (!musicSource.isPlaying)
        {
            musicSource.UnPause();
        }
    }

    // play sound effect
    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void PlayJumpSound()
    {
        PlaySound(jumpSound);
    }

    public void PlayScoreSound()
    {
        PlaySound(scoreSound);
    }

    public void PlayCrashSound()
    {
        PlaySound(crashSound);
    }
}
