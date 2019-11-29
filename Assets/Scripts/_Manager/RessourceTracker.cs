using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceTracker : MonoBehaviour
{
    public static RessourceTracker instance;

    public int blobPop;
    public int cellNbr;

    public int hatchNbr;
    public int stockNbr;
    public int armoryNbr;
    public int broyeurNbr;
    public int autorouteNbr;

    public int energy;
    public int blobProduced;

    private void Awake()
    {
        instance = this;
    }


    public void AddCell(CellMain cell)
    {
        cellNbr++;

        switch (cell.myCellTemplate.type)
        {
            case (CellType.Productrice):
                hatchNbr++;
                break;

            case (CellType.Stockage):
                stockNbr++;
                break;

            case (CellType.Armory):
                armoryNbr++;
                break;

            case (CellType.Broyeur):
                broyeurNbr++;
                break;

            case (CellType.Passage):
                autorouteNbr++;
                break;
        }

    }

    public void RemoveCell(CellMain cell)
    {
        cellNbr--;

        switch (cell.myCellTemplate.type)
        {
            case (CellType.Productrice):
                hatchNbr--;
                break;

            case (CellType.Stockage):
                stockNbr--;
                break;

            case (CellType.Armory):
                armoryNbr--;
                break;

            case (CellType.Broyeur):
                broyeurNbr--;
                break;

            case (CellType.Passage):
                autorouteNbr--;
                break;
        }

    }
}
