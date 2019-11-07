using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Cell", menuName = "Cell", order = 1)]
public class CelluleTemplate : ScriptableObject
{
    [Header ("REF IMPORTANT")]
    public GameObject[] cellsEnableToBuild;
    public GameObject blopPrefab;
    [Space(20f)]

    //public Mesh mesh;
    //public Material mat; 
    //public Collider collider;
    [Space(20f)]

    public CellType type;
    [Range(0f, 50f)]
    [Header ("Standard Stats")]
    public int EnergyCost = 5;
    [Range(0f, 300f)]
    public int range = 50;
    [Range(0, 1)]
    public float blobRatioAtDeath;
    [Range(5 ,100)]
    public int impulseForce_Death = 10;

    [Header("Productions Gestions")]
    [Range(0, 10)]
    public int prodPerTick;
    [Range(0, 10)]
    public int rejectPower_RF;
    [Range(0f, 50f)]
    public int storageCapability = 10;
    [Range(0f, 12f)]
    public int linkCapability = 6;

    [Header("Proximity Interaction")]
    [Range(0 , 5)]
    public int proximityLevelMax = 0;
    public CellType[] positivesInteractions;
    public CellType[] negativesInteractions;
}
