using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [Header("Source")]
    public AudioSource musicSource;
    public AudioSource UISFXSource;

    [Header("Musics")]
    public AudioClip menuMusic;
    public AudioClip level1Music;
    public AudioClip badBlobzMusic;

    [Header("Curves")]
    public AnimationCurve fadingCurve;

    private bool fading = false;
    private float count = 0;
    public float fadingTime = 2;


    private bool firstMusicSource = true;
    private bool musicPaused = false;
    private AudioClip nextMusic;

    // Start is called before the first frame update
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
            ChangeMusic(badBlobzMusic);

        if (fading)
        {
            musicSource.volume = fadingCurve.Evaluate(count / fadingTime);
            count += Time.deltaTime;

            if(count >= fadingTime && !musicPaused)
            {
                musicSource.clip = nextMusic;
                musicSource.volume = 1;
                fading = false;
                count = 0;
                musicSource.Play();
            }

        }
    }


    #region MUSIC
    public void PlayMusic()
    {
        musicPaused = false;
        musicSource.volume = 1;
        musicSource.Play();
    }

    public void PauseMusic()
    {
        FadeMusic();
        musicPaused = true;
    }

    public void ChangeMusic(AudioClip newMusic)
    {
        FadeMusic();
        nextMusic = newMusic;
    }

    public void FadeMusic()
    {
        fading = true;


    }
    #endregion

    #region SYSTEM_SOUNDS

    public void PlaySound(AudioClip clip)
    {
        UISFXSource.PlayOneShot(clip);
    }

    #endregion

}
