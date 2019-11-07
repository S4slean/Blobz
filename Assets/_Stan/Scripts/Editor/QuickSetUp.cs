﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

public class QuickSetUp : Editor
{

    static GameObject UI;
    static GameObject pools;
    static GameObject tickManager;
    static GameObject uiManager;
    static GameObject cellManager;
    static GameObject inputManager;
    static GameObject questManager;
    static GameObject resourceTracker;
    static GameObject camera;
    static GameObject levelManager;


    [MenuItem("Blobz/Set Up Scene")]
    static void SetUpScene()
    {
        if (!SceneIsClean())
        {
            CleanScene();
        }

        GetResources();
        BuildNewScene();

    }

    static bool SceneIsClean()
    {
        if (FindObjectOfType<UIManager>() != null
            || FindObjectOfType<TickManager>() != null
            || FindObjectOfType<InputManager>() != null
            || FindObjectOfType<QuestManager>() != null
            || FindObjectOfType<RessourceTracker>() != null
            || FindObjectOfType<CellManager>() != null
            || GameObject.FindGameObjectWithTag("UI") != null
            || GameObject.Find("--Pools--") != null
            || FindObjectOfType<CameraController>() != null
            || FindObjectOfType<LevelManager>() != null)
        {
            return false;
        }
        else
            return true;

    }

    static void CleanScene()
    {
        if (FindObjectOfType<UIManager>() != null)
            DestroyImmediate(FindObjectOfType<UIManager>().gameObject);

        if (FindObjectOfType<TickManager>() != null)
            DestroyImmediate(FindObjectOfType<TickManager>().gameObject);

        if (FindObjectOfType<InputManager>() != null)
            DestroyImmediate(FindObjectOfType<InputManager>().gameObject);

        if (FindObjectOfType<QuestManager>() != null)
            DestroyImmediate(FindObjectOfType<QuestManager>().gameObject);

        if (FindObjectOfType<RessourceTracker>() != null)
            DestroyImmediate(FindObjectOfType<RessourceTracker>().gameObject);

        if (FindObjectOfType<CellManager>() != null)
            DestroyImmediate(FindObjectOfType<CellManager>().gameObject);

        if (GameObject.FindGameObjectWithTag("UI") != null)
            DestroyImmediate(GameObject.FindGameObjectWithTag("UI"));

        if (GameObject.FindGameObjectWithTag("Pools") != null)
            DestroyImmediate(GameObject.FindGameObjectWithTag("Pools"));

        if (FindObjectOfType<Camera>() != null)
            DestroyImmediate(FindObjectOfType<Camera>().gameObject);

        if (FindObjectOfType<LevelManager>() != null)
            DestroyImmediate(FindObjectOfType<LevelManager>().gameObject);

        Debug.Log("Scene Cleaned");
    }

    static void GetResources()
    {
        UI = Resources.Load("QuickSetUp/--UI--",typeof(GameObject)) as GameObject;
        pools = Resources.Load<GameObject>("QuickSetUp/--Pools--") ;
        tickManager = Resources.Load("QuickSetUp/TickManager") as GameObject;
        questManager = Resources.Load("QuickSetUp/QuestManager") as GameObject;
        uiManager = Resources.Load("QuickSetUp/UIManager") as GameObject;
        cellManager = Resources.Load("QuickSetUp/CellManager") as GameObject;
        inputManager = Resources.Load("QuickSetUp/InputManager") as GameObject;
        resourceTracker = Resources.Load("QuickSetUp/RessourceTracker") as GameObject;
        camera = Resources.Load("QuickSetUp/--MainCamera--") as GameObject;
        levelManager = Resources.Load("QuickSetUp/LevelManager") as GameObject;
    }

    static void BuildNewScene()
    {
        UI = Instantiate(UI);
        pools = Instantiate(pools);
        questManager = Instantiate(questManager);
        uiManager = Instantiate(uiManager);
        tickManager = Instantiate(tickManager);
        cellManager = Instantiate(cellManager);
        inputManager = Instantiate(inputManager);
        resourceTracker = Instantiate(resourceTracker);
        camera = Instantiate(camera);
        levelManager = Instantiate(levelManager);

        #region UI

        UIManager uiScript = uiManager.GetComponent<UIManager>();

        uiScript.cellSelection = GameObject.Find("ChooseACell").GetComponent<CellSelectionShop>();
        uiScript.SelectedCellUI = GameObject.Find("SelectedCell");
        uiScript.SelectedCellUI.SetActive(false);
        uiScript.QuestUI = GameObject.Find("QuestUI").GetComponent<QuestUI>();
        uiScript.TopBar = GameObject.Find("TopBar").GetComponent<TopBarUI>();
        uiScript.tooltipUI = GameObject.Find("CellToolTips").GetComponent<TooltipUI>();
        uiScript.tooltipUI.gameObject.SetActive(false);

        GameObject.Find("WorldSpace").GetComponent<Canvas>().worldCamera = camera.GetComponent<Camera>();

        #endregion

        #region POOLS

        #endregion

        #region CELLS

        #endregion


    }
}
