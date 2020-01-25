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
        if (!loadByString)
            SceneManager.LoadSceneAsync(indexToLoad, LoadSceneMode.Single);
        else
            SceneManager.LoadSceneAsync(stringToLoad, LoadSceneMode.Single);

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
    }
}
