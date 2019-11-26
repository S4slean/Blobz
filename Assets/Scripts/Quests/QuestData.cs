using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestData.asset", menuName = "Blobz/Quest")]
public class QuestData : ScriptableObject
{
    
    public QuestManager.QuestType questType;

    public string QuestTitle;

    [Header("Population")]
    [Range(0,10000)]public int populationObjective;

    [Header("Batiments")]
    public CellType cellType = CellType.Productrice;
    [Range(1,10)]public int level = 1;
    [Range(1,100)]public int cellNbrToObtain = 1;

    [Header("Colonisation")]
    public bool anyCells = true;
    public CellType colonialCellType = CellType.Productrice;
    public Transform placeToGet;
    [Range(0,1000)]public float range;

    [Header("Destruction")]
    public GameObject[] objectToDestroy;
}
