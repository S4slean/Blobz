using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Malee;

[CreateAssetMenu(fileName = "QuestData.asset", menuName = "Blobz/Quest")]
public class QuestData : ScriptableObject
{
    
    public QuestManager.QuestType questType;

    public string questTitle;
    public string questDescription;
    public bool eventDone = false;
    public bool isEndingQuest = false;

    [Header("Population")]
    [Range(0,10000)]public int populationObjective;


    [Header("Energy")]
    [Range(0, 1000000)] public int energyToObtain;

    [Header("Batiments")]
    public CellType cellType = CellType.Nexus;
    [Range(1,10)]public int level = 1;
    [Range(1,100)]public int cellNbrToObtain = 1;

    [Header("Exploration")]
    public bool blobExplo = false;
    public bool anyCells = true;
    public CellType colonialCellType = CellType.Nexus;
    public int placeToGet;

    [Header("Destruction")]
    public Destructible.DestructType destructType;
    public int nbrOfObject;


    [Header("Events")]
    [Reorderable]
    public QuestEventList questEvents;



    
}

[System.Serializable]
public class QuestEventList : ReorderableArray<QuestEvent>
{

}

[System.Serializable]
public struct QuestEvent
{
    public enum QuestEventType { Cinematic, PopUp, Weather, Function, UnlockCell, ModifyGameSpeed, PersistentMessage, screenShake, SwitchScene, LevelEvent}

    public QuestEventType eventType;
    public float eventDuration;

    public int virtualCamIndex;

    public PopUpData[] popUpsMsg;

    public UnityEvent UEvent;

    public bool foldout;

    public CellType cellType;

    public float newTickDuration;

    public PopUpData persistentMsg;

    public string levelName;

    public int levelEvent;

    public int GetLength()
    {
        UEvent.GetPersistentEventCount();
        return UEvent.GetPersistentEventCount();
    }

}



[System.Serializable]
public struct PopUpData
{
    public bool rpgStyle;

    public Transform anchor;
    public Vector3 offset;

    public bool usingSprite;
    public Sprite sprite;

    public string Title;
    public string Text;
}
