using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Cell", menuName = "Cell", order = 1)]
public class CelluleTemplate : ScriptableObject
{


   // [Header ("REF IMPORTANT")]
    public GameObject[] cellsEnableToBuild;
    public GameObject blopPrefab;
    public Color buttonColor;
    public Texture cellTexture;
    
    [Space(20f)]

    //public Mesh mesh;
    //public Material mat; 
    //public Collider collider;
    [Space(20f)]

    public CellType type;

    [Range(0f, 50f)]
    public int energyCost = 5;
    [Range(1 , 150)]
    public int energyCapBase = 10; 

    [Range(0f, 300f)]
    public int rangeBase = 50;
    [Range(0, 1)]
    public float blobRatioAtDeath;
    [Range(5 ,100)]
    public int impulseForce_Death = 10;


    [Range(0, 10)]
    public int prodPerTickBase;
    [Range(0, 10)]
    public int rejectPowerBase;
    [Range(0f, 50f)]
    public int storageCapability = 10;
    [Range(1, 5)]
    public int tickForActivationBase = 1;
    [Range(1, 100)]
    public int energyPerClick = 2;

    [Range(0f, 12f)]
    public int linkCapability = 6;


    [Range(0 , 5)]
    public int proximityLevelMax = 0;
    public CellType[] positivesInteractions;
    public CellType[] negativesInteractions;

    #region SPECIFICITE CELL 
    public int[] SurproductionRate;

    public float[] BlopPerTick;
    public int[] stockageCapacity;

    public int[] LinkCapacity;
    public int[] Range;
    public int[] tickForActivation;
    public int[] energyCap; 

    public int energyPerblop;
    #endregion

    #region InspectorCustom

    public StatsModificationType StatsModification;

    public bool REFS;
    public bool ToggleInfoBox = true;
    public bool STATS;
    public bool ProductionGestion;
    public bool ProximityGestion;

    #endregion
}
