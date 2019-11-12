﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEditor.Events;

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
    static GameObject blobManager;



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
        blobManager = Resources.Load("QuickSetUp/BlobManager") as GameObject;
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
        blobManager = Instantiate(blobManager);

        #region UI

        UIManager uiScript = uiManager.GetComponent<UIManager>();

        uiScript.cellSelection = FindObjectOfType<CellSelectionShop>();
        uiScript.SelectedCellUI = GameObject.Find("SelectedCell");
        uiScript.SelectedCellUI.SetActive(false);
        uiScript.QuestUI = FindObjectOfType<QuestUI>();
        uiScript.TopBar = FindObjectOfType<TopBarUI>();
        uiScript.tooltipUI = FindObjectOfType<TooltipUI>();
        uiScript.tooltipUI.gameObject.SetActive(false);
        uiScript.cellOptionsUI = FindObjectOfType<CellOptionsUI>();
        uiScript.cellOptionsUI.gameObject.SetActive(false);

        GameObject.Find("WorldSpace").GetComponent<Canvas>().worldCamera = camera.GetComponent<Camera>();

        #endregion

        #region POOLS

        #endregion

        #region CELLS_SHOP
        GameObject button = Resources.Load("QuickSetUp/Buttons/CellCreationButton") as GameObject;

        uiScript.cellSelection.buttonTypes = new Button[levelManager.GetComponent<LevelManager>().availablesCells.Length];

        for (int i = 0; i < levelManager.GetComponent<LevelManager>().availablesCells.Length ; i++)
        {
            GameObject objInstance = Instantiate(button);
            RectTransform rect = objInstance.GetComponent<RectTransform>();
            Button btn = objInstance.GetComponent<Button>();
            EventTrigger trigger = objInstance.GetComponent<EventTrigger>();
            CellConstructionButton constructeur = objInstance.GetComponent<CellConstructionButton>();

            uiManager.GetComponent<UIManager>().cellSelection.GetComponent<CellSelectionShop>().buttonTypes[i] = btn;
            rect.SetParent(uiManager.GetComponent<UIManager>().cellSelection.transform );

            //UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, new UnityEngine.Events.UnityAction(CellConstructionEvent));

            CellSelectionShop shop = uiScript.cellSelection;
            CellMain cell = levelManager.GetComponent<LevelManager>().availablesCells[i];
            UnityAction<CellMain> action = new UnityAction<CellMain>(shop.CellConstruction);


            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerClick;
            //entry.callback.AddListener(action);


            //btn.onClick.AddListener(delegate { action(cell); }) ;  
        }

        #endregion


    }

    //public static void CellConstructionEvent(int i)
    //{
    //    uiManager.GetComponent<UIManager>().cellSelection.CellConstruction(LevelManager.instance.availablesCells[i].blopPrefab.GetComponent<CellMain>());
    //}
}
