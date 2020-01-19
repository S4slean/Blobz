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
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        anim.SetTrigger("FadeOut");
    }

    public void LoadScene(int index)
    {
        SceneManager.LoadSceneAsync(index, LoadSceneMode.Single);
        anim.Play("FadeOut");
    }

    public void ChangeScene(int index)
    {
        indexToLoad = index;
        anim.Play("FadeIn");
    }

    public void ChangeScene(string name)
    {
        indexToLoad = SceneManager.GetSceneByName(name).buildIndex;
        anim.Play("FadeIn");
    }

    public void ReplayLevel()
    {
        indexToLoad = SceneManager.GetActiveScene().buildIndex;
        anim.Play("FadeIn");
    }

    public void BackToLevelSelection()
    {
        indexToLoad = SceneManager.GetSceneByName("LevelSelection").buildIndex;
        anim.Play("FadeIn");
    }
}
