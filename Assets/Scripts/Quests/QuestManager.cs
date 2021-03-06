﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Events;



public class QuestManager : MonoBehaviour
{

    public enum QuestType { Population, Energy, Batiments, Exploration, Destruction, Empty };

    public int currentQuestID;
    public int currentQuestEventID;
    public int currentMsgID;

    public QuestData currentQuest;
    [HideInInspector] public int questProgress;
    public List<QuestData> QuestList = new List<QuestData>();
    public SpaceDetection[] explorationObjectives;

    public static QuestManager instance;

    public bool desactiveQuest = false;

    public UnityEvent[] levelEvents;




    QuestPopUp popUp;


    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
            Destroy(gameObject);

        CheckForInitialQuest();
    }

    #region QUEST_HANDLING

    public void DestructionCheck(Destructible.DestructType destructObjectType)
    {
        if (currentQuest == null)
            return;

        if (currentQuest.questType != QuestType.Destruction)
            return;

        if (destructObjectType == currentQuest.destructType)
            questProgress++;

        CheckQuestSuccess();
    }
    public void DisplayCurrentQuest()
    {
        UIManager.Instance.DisplayUI(UIManager.Instance.QuestUI.gameObject);
        UIManager.Instance.QuestUI.UpdateUI();
    }
    public void ChangeQuest()
    {
        currentQuestID++;
        questProgress = 0;
        RessourceTracker.instance.enemyKilled = 0;
        if (QuestList.Count > currentQuestID)
        {
            currentQuest = QuestList[currentQuestID];

            DisplayCurrentQuest();
            ResetQuestEventCount();

            PlayQuestEvent();
        }
        else
        {
            UIManager.Instance.HideUI(UIManager.Instance.QuestUI.gameObject);
            TickManager.doTick -= CheckQuestSuccess;
        }

    }
    private void CheckForInitialQuest()
    {
        if (QuestList.Count > 0 && !desactiveQuest)
        {
            currentQuest = QuestList[currentQuestID];
            UIManager.Instance.DisplayUI(UIManager.Instance.QuestUI.gameObject);
            PlayQuestEvent();
            TickManager.doTick += CheckQuestSuccess;
        }
        else
        {
            Debug.Log("no more Quest");
            UIManager.Instance.HideUI(UIManager.Instance.QuestUI.gameObject);

        }
    }
    public void CheckQuestSuccess()
    {
        if (currentQuest == null && !currentQuest.eventDone)
            return;


        switch (currentQuest.questType)
        {
            case QuestType.Batiments:

                switch (currentQuest.cellType)
                {
                    case CellType.Nexus:
                        questProgress = RessourceTracker.instance.hatchNbr;
                        break;

                    case CellType.Stock:
                        questProgress = RessourceTracker.instance.stockNbr;
                        break;

                    case CellType.Crusher:
                        questProgress = RessourceTracker.instance.broyeurNbr;
                        break;

                    case CellType.Accelerator:
                        questProgress = RessourceTracker.instance.passageNbr;
                        break;

                    case CellType.BlipBlop:
                        questProgress = RessourceTracker.instance.blipblopNbr;
                        break;

                    case CellType.Dump:
                        questProgress = RessourceTracker.instance.dechargeNbr;
                        break;

                    case CellType.AerialStrike:
                        questProgress = RessourceTracker.instance.divineNbr;
                        break;

                    case CellType.Academy:
                        questProgress = RessourceTracker.instance.exploNbr;
                        break;

                    case CellType.Gym:
                        questProgress = RessourceTracker.instance.coachRoomNbr;
                        break;

                    case CellType.Battery:
                        questProgress = RessourceTracker.instance.piloneNbr;
                        break;

                    case CellType.Turret:
                        questProgress = RessourceTracker.instance.towerNbr;
                        break;

                    case CellType.Treblobchet:
                        questProgress = RessourceTracker.instance.treblobchetNbr;
                        break;
                }

                if (questProgress >= currentQuest.cellNbrToObtain)
                    QuestSuccess();

                break;

            case QuestType.Energy:

                questProgress = RessourceTracker.instance.energy;

                if (questProgress >= currentQuest.energyToObtain)
                    QuestSuccess();

                break;

            case QuestType.Exploration:

                //créer un préfab de detection en OntriggerEnter.

                break;

            case QuestType.Destruction:

                if (currentQuest.destructType == Destructible.DestructType.EnemyBlob)
                    questProgress = RessourceTracker.instance.enemyKilled;

                if (questProgress >= currentQuest.nbrOfObject)
                    QuestSuccess();

                break;

            case QuestType.Population:

                if (RessourceTracker.instance.blobPop >= currentQuest.populationObjective)
                    QuestSuccess();

                break;

            case QuestType.Empty:
                QuestSuccess();
                break;

        }

    }
    public void QuestSuccess()
    {

        //Debug.Log("Quest Success");
        //Display Success
        //hide UI or play Disappearing anim

        if(!currentQuest.isEndingQuest)
            SoundManager.instance.PlayQuestSuccessSound();
        else
        {
            SoundManager.instance.PlayVictorySound();
        }
        UIManager.Instance.HidePersistentMsg();

        ChangeQuest();
    }

    #endregion


    #region QUESTEVENTS_HANDLING

    public void PlayQuestEvent()
    {


        QuestEventManager.instance.StartQuestEvent();

        if (currentQuestEventID < currentQuest.questEvents.Length)
        {


            QuestEventManager.instance.UpdateEventType();

            switch (currentQuest.questEvents[currentQuestEventID].eventType)
            {
                case QuestEvent.QuestEventType.Cinematic:

                    CinematicManager.instance.GoToCamOfIndex(currentQuest.questEvents[currentQuestEventID].virtualCamIndex);
                    StartCoroutine(WaitBeforNextEvent());

                    break;

                case QuestEvent.QuestEventType.PopUp:

                    if (currentMsgID == 0)
                    {
                        QuestEventManager.instance.popUpIsSkippable = false;
                        StartCoroutine(WaitBeforeSkip());

                    }
                    DisplayPopUp();

                    break;

                case QuestEvent.QuestEventType.Weather:

                    break;

                case QuestEvent.QuestEventType.Function:

                    currentQuest.questEvents[currentQuestEventID].UEvent.Invoke();
                    StartCoroutine(WaitBeforNextEvent());

                    break;

                case QuestEvent.QuestEventType.UnlockCell:

                    LevelManager.instance.UnlockNewCell(currentQuest.questEvents[currentQuestEventID].cellType);
                    StartCoroutine(WaitBeforNextEvent());

                    break;

                case QuestEvent.QuestEventType.ModifyGameSpeed:
                    TickManager.instance.tickDuration = currentQuest.questEvents[currentQuestEventID].newTickDuration;
                    StartCoroutine(WaitBeforNextEvent());
                    break;

                case QuestEvent.QuestEventType.PersistentMessage:

                    DisplayPersistentMessage();
                    StartCoroutine(WaitBeforNextEvent());


                    break;

                case QuestEvent.QuestEventType.screenShake:

                    CameraController.instance.GetComponent<CinemachineImpulseSource>().GenerateImpulse();

                    break;

                case QuestEvent.QuestEventType.SwitchScene:

                    SceneHandler.instance.ChangeScene(currentQuest.questEvents[currentQuestEventID].levelName);

                    break;

                case QuestEvent.QuestEventType.LevelEvent:
                    levelEvents[currentQuest.questEvents[currentQuestEventID].levelEvent].Invoke();
                    StartCoroutine(WaitBeforNextEvent());
                    break;


            }

        }
        else
        {
            currentQuest.eventDone = true;
            QuestEventManager.instance.EndQuestEvent();
        }


    }
    public void DisplayPersistentMessage()
    {
        PopUpData persistentMsg = currentQuest.questEvents[currentQuestEventID].persistentMsg;
        popUp = UIManager.Instance.persistentMsg;
        popUp.UpdatePersistentMsg(persistentMsg.Text, persistentMsg.sprite);
        UIManager.Instance.DisplayPopUp(popUp);
    }
    private void DisplayPopUp()
    {
        PopUpData popUpData = currentQuest.questEvents[currentQuestEventID].popUpsMsg[currentMsgID];


        if (popUpData.rpgStyle)
        {
            popUp = UIManager.Instance.questEventPopUpOverlay;
            UIManager.Instance.DisplayPopUp(popUp);
        }
        else
        {
            popUp = UIManager.Instance.questEventPopUpWorld;
            UIManager.Instance.DisplayPopUp(popUp);



            popUp.transform.position = popUpData.offset;
        }

        if (popUpData.usingSprite == false || popUpData.sprite == null)
        {
            popUp.img.gameObject.SetActive(false);
        }
        else
        {
            popUp.img.gameObject.SetActive(true);
        }

        popUp.UpdatePopUp(popUpData.Title, popUpData.Text, popUpData.sprite, popUpData.usingSprite);
    }
    public IEnumerator WaitBeforeSkip()
    {
        yield return new WaitForSeconds(1);
        QuestEventManager.instance.popUpIsSkippable = true;
    }
    public IEnumerator WaitBeforNextEvent()
    {
        yield return new WaitForSeconds(currentQuest.questEvents[currentQuestEventID].eventDuration);
        EndEvent();
    }
    public void GoToNextEvent()
    {
        currentQuestEventID++;

        PlayQuestEvent();
    }
    public void GoToNextMsg()
    {
        currentMsgID++;

        if (currentMsgID < currentQuest.questEvents[currentQuestEventID].popUpsMsg.Length)
        {
            DisplayPopUp();

        }
        else
        {
            ResetMsgCount();
            EndEvent();
        }

    }
    public void EndEvent()
    {
        switch (currentQuest.questEvents[currentQuestEventID].eventType)
        {

            case QuestEvent.QuestEventType.PopUp:

                
                popUp.anim.Play("Hide");
                break;
        }

        GoToNextEvent();
    }
    public void ResetQuestEventCount()
    {
        currentQuestEventID = 0;
        ResetMsgCount();
    }
    public void ResetMsgCount()
    {
        currentMsgID = 0;
    }

    #endregion



    #region UTILITY

    public void WriteQuestsFromCSV()
    {

    }


    #endregion
}
