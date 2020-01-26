using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneHandler : MonoBehaviour
{
    public static SceneHandler instance;
    public Animator anim;
    private int indexToLoad;
    private string stringToLoad;
    private bool loadByString = false;

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += Load;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= Load;
    }

    public void Load(Scene scene,LoadSceneMode mode)
    {
        anim.SetTrigger("FadeOut");
    }

    public void LoadScene()
    {
        ClearInstances();


        if (!loadByString)
            SceneManager.LoadScene(indexToLoad, LoadSceneMode.Single);
        else
            SceneManager.LoadScene(stringToLoad, LoadSceneMode.Single);

    }

    public void ChangeScene(int index)
    {
        loadByString = false;
        indexToLoad = index;
        anim.Play("FadeIn");
    }

    public void ChangeScene(string name)
    {
        loadByString = true;
        stringToLoad = name;
        anim.Play("FadeIn");
    }

    public void ReplayLevel()
    {
        BlobManager.blobList.Clear();
      //  RessourceTracker.
        loadByString = false;
        indexToLoad = SceneManager.GetActiveScene().buildIndex;
        anim.Play("FadeIn");
    }

    public void BackToLevelSelection()
    {
        loadByString = true;
        stringToLoad = "LevelSelector";
        anim.Play("FadeIn");
        SoundManager.instance.PlayMenuMusic();
    }

    public void ClearInstances()
    {
        TickManager.doTick = null;

        BlobManager.instance = null;
        CellManager.Instance = null;
        CinematicManager.instance = null;
        InputManager.Instance = null;
        LevelManager.instance = null;
        QuestManager.instance = null;
        TickManager.instance = null;
        UIManager.Instance = null;
        RessourceTracker.instance = null;
    }
}
