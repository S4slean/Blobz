using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Cell", menuName = "Cell", order = 1)]
public class CelluleTemplate : ScriptableObject
{

    public GameObject[] cellsEnableToBuild;
    public GameObject blopPrefab;
    public Color buttonColor;
    public Texture cellTexture;

    [Space(20f)]


    [Space(20f)]
    [Range(1, 10)]
    public int prodLevelMax = 1;
    public ProdLevel[] levelProduction;


    public CellType type;
    public string description;
    public string descriptionProximity;
    public string descriptionClick;
    [Range (0 , 200)]
    public int expAmount = 10;
    [Range (0.2f , 10f)] 
    public float lineOfSight = 1;

    public bool generateProximity;
    [Range(1, 5)]
    public int proximityColliderNumber = 1;
    public ProximityCollider[] proximityColliders;

    #region STATS
    [Range(0, 1000)]
    public int energyCost = 5;
    [Range(0, 1500)]
    public int energyCapBase = 10;

    [Range(0f, 300f)]
    public int rangeBase = 50;

    #endregion
    [Range(1, 6)]
    public int overLoadTickMax = 4;
    [Range(1, 10)]
    public int overloadTreshHold = 5;
    [Range(0, 50)]
    public int blobSpawnRatioAtDeath;
    [Range(0, 50)]
    public int blobSpawnAdditionnalRatioAtDeath;


    [Range(0, 10)]
    public int prodPerTickBase;
    [Range(0, 10)]
    public int rejectPowerBase;
    [Range(0f, 1000)]
    public int storageCapability = 10;
    [Range(1, 5)]
    public int tickForActivationBase = 1;

    [Range(1, 100)]
    public int energyPerClick = 2;

    #region LINK
    public bool limitedInLinks;
    [Range(0.5f, 5f)]
    public float slotDistance = 3;
    [Range(0, 10)]
    public int numberOfOuputLinks = 1;
    [Range(0, 10)]
    public int numberOfInputLinks = 1;
    [Range(0, 20)]
    public int numberOfFlexLinks = 1;

    #endregion

    #region PROXIMITY 
    [Range(0, 5)]
    public int proximityLevelMax = 0;
    [Range(-3, 3)]
    public int proximityInfluenceMultiplier = 1;

    public CellType[] positivesInteractions;
    public CellType[] negativesInteractions;


    public int[] SurproductionRate;

    public float[] BlopPerTick;
    public int[] stockageCapacity;

    public int[] LinkCapacity;
    public int[] rangeLien;
    public int[] tickForActivation;
    public int[] energyCap;

    public int[] specifique;

    public int energyPerblop;
    #endregion

    #region InspectorCustom

    public StatsModificationType StatsModification;

    public bool REFS;
    public bool ToggleInfoBox = true;
    public bool STATS;
    public bool ProductionGestion;
    public bool ProximityGestion;
    public bool clickInteraction;

    #endregion

    #region SPECIFIC VARIABLE

    [Range(1, 15)]
    public int maxEnergie = 5;

    [Range(0.4f, 5)]
    public float explosionRadius = 0.8f;

    [Range(1, 10)]
    public int maxBlobShreddedPerClick = 1;


    [Range(1, 100)]
    public int maxBlobCoach;

    [Range(1.2f, 6f)]
    public float magazineDragRange = 1.5f;
    [Range(0.1f, 0.5f)]
    public float minDragRatio = 0.2f;

    [Range(1, 60)]
    public int shotPower = 25;
    [Range(1, 30)]
    public int verticalConstantPower = 10;
    [Range(1, 10)]
    public float verticalOffset = 1;
    [Range(0.25f, 4f)]
    public float magazinSlotDistance = 3;



    [Range(1, 10)]
    public int tourelleMaxMun = 3;
    [Range(1, 10)]
    public int tourelleDamage = 5;
    [Range(3, 20)]
    public int tourelleAttackRadius = 6;

    [Range(1, 10)]
    public int clickBeforeLaunch;
    [Range(1, 5)]
    public int blobLostPerTick;

    [Range(1, 100)]
    public int maxLifeProd;


       
    #endregion
}

[System.Serializable]
public struct ProximityCollider
{
    [Range(1, 5)]
    public int proximityLevel;
    [Range(5, 100)]
    public int range;
    [Range(0, 75)]
    public int productionBonusRatio;
}

[System.Serializable]
public struct ProdLevel
{
    [Range(1, 10)]
    public int blobsProduction;
    [Range(10, 1000)]
    public int expNeeded;
    public Sprite spriteLevel;


}
