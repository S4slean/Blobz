using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSalle : MonoBehaviour
{
    public List<BlobCoach> myCoachs;


    




}
[System.Serializable]
public struct BlobCoach
{
    public CellSalle origianlSalle;
    public int currentLife;
    public CellMain inThisCell;

}
