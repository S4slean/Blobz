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

    [Header("SysytemSounds")]
    public AudioClip discoverySound;
    public AudioClip questSuccessSound;
    public AudioClip victorySound;

    [Header("Curves")]
    public AnimationCurve fadingCurve;

    private bool fading = false;
    private float count = 0;
    public float fadingTime = 2;


    private bool firstMusicSource = true;

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

    private void Start()
    {

    }

    private void Update()
    {
        if (fading)
        {

        }
    }

    public void PlayMusic()
    {
        musicSource.Play();
    }

    public void PauseMusic()
    {

    }

    public void ChangeMusic(AudioClip newMusic)
    {

    }

    public void FadeMusic()
    {
        fading = true;


    }


}
