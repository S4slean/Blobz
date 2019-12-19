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

    public int normalBlob;
    public int soldierBlob;
    public int chargedBlob;
    public int madBlob;

    public int energyCap;
    public int energy;
    public int blobProduced;

    private void Awake()
    {
        instance = this;
    }



    #region CELLS
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

        UIManager.Instance.QuestUI.UpdateUI();

    }
    public void RemoveCell(CellMain cell)
    {
        cellNbr--;

        switch (cell.myCellTemplate.type)
        {
            case (CellType.Productrice):
                hatchNbr--;
                if(hatchNbr < 1)
                {

                }
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

        UIManager.Instance.QuestUI.UpdateUI();

    }
    #endregion

    #region BLOB
    public void AddBlob(Blob blob)
    {
        blobPop++;

        switch (blob.blobType)
        {
            case BlobManager.BlobType.normal:

                normalBlob++;

                break;

            case BlobManager.BlobType.soldier:

                soldierBlob++;

                break;

            case BlobManager.BlobType.charged:

                chargedBlob++;

                break;

            case BlobManager.BlobType.mad:

                madBlob++;

                break;
        }
    }
    public void AddBlob(BlobManager.BlobType blobType)
    {
        blobPop++;

        switch (blobType)
        {
            case BlobManager.BlobType.normal:

                normalBlob++;

                break;

            case BlobManager.BlobType.soldier:

                soldierBlob++;

                break;

            case BlobManager.BlobType.charged:

                chargedBlob++;

                break;

            case BlobManager.BlobType.mad:

                madBlob++;

                break;
        }
    }
    public void AddBlob(BlobManager.BlobType blobType, int nbr)
    {
        blobPop += nbr;

        switch (blobType)
        {
            case BlobManager.BlobType.normal:

                normalBlob += nbr;

                break;

            case BlobManager.BlobType.soldier:

                soldierBlob += nbr;

                break;

            case BlobManager.BlobType.charged:

                chargedBlob += nbr;

                break;

            case BlobManager.BlobType.mad:

                madBlob += nbr;

                break;
        }
    }
       
    public void RemoveBlob(Blob blob)
    {
        blobPop--;

        switch (blob.blobType)
        {
            case BlobManager.BlobType.normal:

                normalBlob--;

                break;

            case BlobManager.BlobType.soldier:

                soldierBlob--;

                break;

            case BlobManager.BlobType.charged:

                chargedBlob--;

                break;

            case BlobManager.BlobType.mad:

                madBlob--;

                break;
        }
    }
    public void RemoveBlob(BlobManager.BlobType blobType)
    {
        blobPop--;

        switch (blobType)
        {
            case BlobManager.BlobType.normal:

                normalBlob--;

                break;

            case BlobManager.BlobType.soldier:

                soldierBlob--;

                break;

            case BlobManager.BlobType.charged:

                chargedBlob--;

                break;

            case BlobManager.BlobType.mad:

                madBlob--;

                break;
        }
    }
    public void RemoveBlob(BlobManager.BlobType blobType, int nbr)
    {
        blobPop -= nbr;

        switch (blobType)
        {
            case BlobManager.BlobType.normal:

                normalBlob -= nbr;

                break;

            case BlobManager.BlobType.soldier:

                soldierBlob -= nbr;

                break;

            case BlobManager.BlobType.charged:

                chargedBlob -= nbr;

                break;

            case BlobManager.BlobType.mad:

                madBlob -= nbr;

                break;
        }
    }
    #endregion

    #region ENERGY
    public void EnergyVariation(int amount)
    {
        energy += amount;
        EnergyCheckIfInCap();
    }

    public void EnergyCapVariation(int amount)
    {
        energyCap += amount;
        EnergyCheckIfInCap();
    }

    public void EnergyCheckIfInCap()
    {
        if (energy > energyCap)
        {
            energy = energyCap;
        }
    }

    #endregion
}
