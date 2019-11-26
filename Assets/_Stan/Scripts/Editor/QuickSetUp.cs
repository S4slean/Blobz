using System.Collections;
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
    static GameObject ground;



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
            || FindObjectOfType<LevelManager>() != null
            || FindObjectOfType<BlobManager>() != null
            || GameObject.FindGameObjectWithTag("Ground"))
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

        if (FindObjectOfType<BlobManager>() != null)
            DestroyImmediate(FindObjectOfType<BlobManager>().gameObject);

        if (GameObject.FindGameObjectWithTag("Ground") != null)
            DestroyImmediate(GameObject.FindGameObjectWithTag("Ground"));

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
        ground = Resources.Load("QuickSetUp/Ground") as GameObject;
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
        ground = Instantiate(ground);

        ground.transform.position = Vector3.zero;


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

        ObjectPooler pooler = pools.GetComponent<ObjectPooler>();
        LevelManager lvlMng = levelManager.GetComponent<LevelManager>();

        pooler.poolItems = new List<ObjectPoolItem>();

        ObjectPoolItem linkPoolItem = new ObjectPoolItem();
        linkPoolItem.objectToPool = Resources.Load("QuickSetUp/Link") as GameObject;
        linkPoolItem.AmountToPool = 200;
        pooler.poolItems.Add(linkPoolItem);


        ObjectPoolItem blobPoolItem = new ObjectPoolItem();
        blobPoolItem.objectToPool = Resources.Load("QuickSetUp/Blob") as GameObject;
        blobPoolItem.AmountToPool = 200;
        pooler.poolItems.Add(blobPoolItem);

        for (int i = 0; i < lvlMng.availablesCells.Length; i++)
        {
            ObjectPoolItem cellPoolItem = new ObjectPoolItem();
            cellPoolItem.objectToPool = lvlMng.availablesCells[i];
            cellPoolItem.AmountToPool = 100;
            pooler.poolItems.Add(cellPoolItem);
        }

        PoolCustomInpector.GeneratePools();

        #endregion

        #region CELLS_SHOP
        GameObject button = Resources.Load("QuickSetUp/Buttons/CellCreationButton") as GameObject;

        uiScript.cellSelection.buttonTypes = new GameObject[levelManager.GetComponent<LevelManager>().availablesCells.Length];

        for (int i = 0; i < lvlMng.availablesCells.Length ; i++)
        {
            GameObject objInstance = Instantiate(button);
            objInstance.name = lvlMng.availablesCells[i].name + "Button";
            RectTransform rect = objInstance.GetComponent<RectTransform>();
            objInstance.GetComponent<Image>().color = lvlMng.availablesCells[i].GetComponent<CellMain>().myCellTemplate.buttonColor;


            uiManager.GetComponent<UIManager>().cellSelection.GetComponent<CellSelectionShop>().buttonTypes[i] = objInstance;
            rect.SetParent(uiManager.GetComponent<UIManager>().cellSelection.transform );

            //UnityEditor.Events.UnityEventTools.AddPersistentListener(btn.onClick, new UnityEngine.Events.UnityAction(CellConstructionEvent));

            EventTrigger trigger = objInstance.GetComponent<EventTrigger>();
            CellSelectionShop shop = uiScript.cellSelection;

            CellMain cell = levelManager.GetComponent<LevelManager>().availablesCells[i].GetComponent<CellMain>();
            UnityAction<CellMain> action = new UnityAction<CellMain>(shop.CellConstruction);

            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;

            GraphicRaycaster raycaster = FindObjectOfType<GraphicRaycaster>();

            

            UnityEventTools.AddObjectPersistentListener<CellMain>(entry.callback, action, cell);

            trigger.triggers.Add(entry);

        }

        #endregion


    }

}
