using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestUI : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;



    public void UpdateUI()
    {
        if (QuestManager.instance == null || QuestManager.instance.currentQuest == null)
            return;

        StartCoroutine(WaitEnfOfFrameBeforeUpdates());

    }

     IEnumerator  WaitEnfOfFrameBeforeUpdates()
    {
        yield return new WaitForEndOfFrame();


        if (QuestManager.instance == null)
            yield  return null;

        title.text = QuestManager.instance.currentQuest.questTitle;

        if (QuestManager.instance.currentQuest.questDescription != "")
        {
            description.text = QuestManager.instance.currentQuest.questDescription;
        }
        else
        {
            switch (QuestManager.instance.currentQuest.questType)
            {
                case QuestManager.QuestType.Batiments:
                    description.text = "Build " + QuestManager.instance.currentQuest.cellNbrToObtain + " "
                        + QuestManager.instance.currentQuest.cellType.ToString() + " Cell. "
                        + QuestManager.instance.questProgress + "/" + QuestManager.instance.currentQuest.cellNbrToObtain;
                    break;

                case QuestManager.QuestType.Energy:
                    description.text = "Get a total of " + QuestManager.instance.currentQuest.energyToObtain + " Splouch. "
                        + QuestManager.instance.questProgress + "/" + QuestManager.instance.currentQuest.energyToObtain;
                    break;


                case QuestManager.QuestType.Exploration:

                    string cellName;
                    if (QuestManager.instance.currentQuest.anyCells)
                        cellName = "cell";
                    else
                        cellName = QuestManager.instance.currentQuest.colonialCellType.ToString();
                    description.text = "Build 1 " + cellName + " in the designed area";
                    break;

                case QuestManager.QuestType.Destruction:
                    description.text = "Destroy " + QuestManager.instance.questProgress + "/"
                        + QuestManager.instance.currentQuest.nbrOfObject +" " + QuestManager.instance.currentQuest.destructType.ToString();
                    break;

                case QuestManager.QuestType.Population:
                    description.text = "Reach a " + QuestManager.instance.currentQuest.populationObjective + " population. "
                        + RessourceTracker.instance.blobPop + "/" + QuestManager.instance.currentQuest.populationObjective;
                    break;
            }
        }
    }

}
