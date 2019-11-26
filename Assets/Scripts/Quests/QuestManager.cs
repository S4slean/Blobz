using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class QuestManager : MonoBehaviour
{

    public enum QuestType { Population, Batiments, Colonisation, Destruction };

    int currentQuestID;

    public QuestData currentQuest;
    public List<QuestData> QuestList = new List<QuestData>();
    public int questProgress;

    public static QuestManager instance;

    public bool desactiveQuest = false;

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
        }
        else
        {
            Debug.Log("no more Quest");
            UIManager.Instance.HideUI(UIManager.Instance.QuestUI.gameObject);
        }
    }

    public void ChangeQuest()
    {
        currentQuestID++;
        if (QuestList.Count > currentQuestID)
            currentQuest = QuestList[currentQuestID];
        else
        {
            Debug.Log("no more Quest");
            UIManager.Instance.HideUI(UIManager.Instance.QuestUI.gameObject);
        }

        DisplayCurrentQuest();
    }

    public void DisplayCurrentQuest()
    {
        UIManager.Instance.DisplayUI(UIManager.Instance.QuestUI.gameObject);
        UIManager.Instance.QuestUI.UpdateUI();
    }

    public void CheckQuestSuccess()
    {
        if (currentQuest == null)
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
               
        }

    }

    public void QuestSuccess()
    {
        //Display Success

        ChangeQuest();
    }



    public void WriteQuestsFromCSV()
    {

    }

}
