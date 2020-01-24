using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestEventManager : MonoBehaviour
{
    public static QuestEventManager instance;

    [HideInInspector]public bool inQuestEvent;
    public bool popUpIsSkippable = false;

    [Header("EventPositions")]
    Transform[] eventsPos;

    QuestEvent.QuestEventType currentEventType;


    private void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }



    public void StartQuestEvent()
    {
        if (inQuestEvent)
            return;

        TickManager.instance.PauseTick();
        CameraController.instance.isPanning = false;
        InputManager.Instance.enabled = false;
        CameraController.instance.enabled = false;
        inQuestEvent = true;

    }

    public void EndQuestEvent()
    {
        if (!inQuestEvent)
            return;

        QuestManager.instance.ResetQuestEventCount();
        CinematicManager.instance.ReturnToMainCam();
        inQuestEvent = false;
        InputManager.Instance.enabled = true;
        CameraController.instance.enabled = true;
        TickManager.instance.RestartTick();
    }

    public void UpdateEventType()
    {
        currentEventType = QuestManager.instance.currentQuest.questEvents[QuestManager.instance.currentQuestEventID].eventType;
    }

    public void Update()
    {
        if (!inQuestEvent)
            return;

        #region HANDLE_INPUTS
        

        switch (currentEventType)
        {
            case QuestEvent.QuestEventType.PopUp:

                if (Input.GetMouseButtonDown(0) && popUpIsSkippable)
                {
                    QuestManager.instance.GoToNextMsg();
                }


                break;
        }

        #endregion
    }
}
