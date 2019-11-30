using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class QuestManager : MonoBehaviour
{

    public enum QuestType { Population, Batiments, Colonisation, Destruction , Empty};

    int currentQuestID;
    int currentQuestEventID;

    public QuestData currentQuest;
    public List<QuestData> QuestList = new List<QuestData>();
    public int questProgress;

    public static QuestManager instance;

    public bool desactiveQuest = false;

    public PopUp[] Suce;

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

        if (QuestList.Count > 0 && !desactiveQuest)
        {
            currentQuest = QuestList[currentQuestID];
            UIManager.Instance.DisplayUI(UIManager.Instance.QuestUI.gameObject);
            TickManager.doTick += CheckQuestSuccess;
        }
        else
        {
            Debug.Log("no more Quest");
            UIManager.Instance.HideUI(UIManager.Instance.QuestUI.gameObject);
            TickManager.doTick -= CheckQuestSuccess;
        }
    }

    public void ChangeQuest()
    {
        currentQuestID++;
        if (QuestList.Count > currentQuestID)
        {
            currentQuest = QuestList[currentQuestID];

            DisplayCurrentQuest();
            ResetQuestEventCount();
            PlayQuestEvent();
        }
        else
        {
            Debug.Log("no more Quest");
            UIManager.Instance.HideUI(UIManager.Instance.QuestUI.gameObject);
        }

    }

    public void DisplayCurrentQuest()
    {
        UIManager.Instance.DisplayUI(UIManager.Instance.QuestUI.gameObject);
        UIManager.Instance.QuestUI.UpdateUI();
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
                    case CellType.Productrice:
                        questProgress = RessourceTracker.instance.hatchNbr;
                        break;

                    case CellType.Stockage:
                        questProgress = RessourceTracker.instance.stockNbr;
                        break;

                    case CellType.Armory:
                        questProgress = RessourceTracker.instance.armoryNbr;
                        break;
                }

                if (questProgress >= currentQuest.cellNbrToObtain)
                    QuestSuccess();

                break;

            case QuestType.Colonisation:

                //créer un préfab de detection en OntriggerEnter.

                break;

            case QuestType.Destruction:

                bool success = true;
                questProgress = 0;

                foreach(GameObject obj in currentQuest.objectToDestroy)
                {
                    if (obj != null)
                    {
                        success = false;
                        questProgress++;
                    }
                }

                if (success)
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

    public void ResetQuestEventCount()
    {
        currentQuestEventID = 0;
    }

    public void PlayQuestEvent()
    {
        TickManager.instance.PauseTick();

        if(currentQuestEventID < currentQuest.questEvents.Length)
        {
            switch (currentQuest.questEvents[currentQuestEventID].eventType)
            {
                case QuestEvent.QuestEventType.Cinematic:

                    break;

                case QuestEvent.QuestEventType.PopUp:

                    break;

                case QuestEvent.QuestEventType.Weather:

                    break;

                case QuestEvent.QuestEventType.Function:

                    currentQuest.questEvents[currentQuestEventID].UEvent.Invoke();

                    break;
            }

            StartCoroutine(WaitBeforeNextEvent());
        }
        else
        {
            currentQuest.eventDone = true;
        }


    }

    public IEnumerator WaitBeforeNextEvent()
    {
        yield return new WaitForSeconds(currentQuest.questEvents[currentQuestEventID].eventDuration);
        currentQuestEventID++;
        PlayQuestEvent();
    }

    public void QuestSuccess()
    {

        Debug.Log("Quest Success");
        //Display Success
        //hide UI or play Disappearing anim


        ChangeQuest();
    }



    public void WriteQuestsFromCSV()
    {

    }

}
