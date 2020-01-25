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
    public int treblobchetNbr;
    public int broyeurNbr;
    public int passageNbr;
    public int blipblopNbr;
    public int dechargeNbr;
    public int divineNbr;
    public int exploNbr;
    public int coachRoomNbr;
    public int piloneNbr;
    public int towerNbr;


    public int normalBlob;
    public int soldierBlob;
    public int chargedBlob;
    public int madBlob;
    public int coachBlob;

    public int energyCap;
    public int energy;
    public int blobProduced;

    public int enemyKilled;

    private void Awake()
    {
        instance = this;
    }

    #region COMBAT
    public void AddKill()
    {
        enemyKilled++;
    }

    public void ResetKillCount()
    {
        enemyKilled = 0;
    }
    #endregion

    #region CELLS
    public void AddCell(CellMain cell)
    {
        cellNbr++;

        switch (cell.myCellTemplate.type)
        {
            case (CellType.Nexus):
                hatchNbr++;
                break;

            case (CellType.Stock):
                stockNbr++;
                break;

            case (CellType.Treblobchet):
                treblobchetNbr++;
                break;

            case (CellType.Crusher):
                broyeurNbr++;
                break;

            case (CellType.Accelerator):
                passageNbr++;
                break;

            case (CellType.BlipBlop):
                blipblopNbr++;
                break;

            case (CellType.Dump):
                dechargeNbr++;
                break;

            case (CellType.AerialStrike):
                divineNbr++;
                break;

            case (CellType.Academy):
                exploNbr++;
                break;

            case (CellType.Gym):
                coachRoomNbr++;
                break;

            case (CellType.Battery):
                piloneNbr++;
                break;

            case (CellType.Turret):
                towerNbr++;
                break;

              



        }

        if(UIManager.Instance.QuestUI.gameObject.activeSelf)
            UIManager.Instance.QuestUI.UpdateUI();

    }
    public void RemoveCell(CellMain cell)
    {
        cellNbr--;

        switch (cell.myCellTemplate.type)
        {
            case (CellType.Nexus):
                hatchNbr--;
                if(hatchNbr < 1)
                {
                    LevelManager.instance.LevelFailed();
                }
                break;

            case (CellType.Stock):
                stockNbr--;
                break;

            case (CellType.Treblobchet):
                treblobchetNbr--;
                break;

            case (CellType.Crusher):
                broyeurNbr--;
                break;

            case (CellType.Accelerator):
                passageNbr--;
                break;


            case (CellType.BlipBlop):
                blipblopNbr--;
                break;

            case (CellType.Dump):
                dechargeNbr--;
                break;

            case (CellType.AerialStrike):
                divineNbr--;
                break;

            case (CellType.Academy):
                exploNbr--;
                break;

            case (CellType.Gym):
                coachRoomNbr--;
                break;

            case (CellType.Battery):
                piloneNbr--;
                break;

            case (CellType.Turret):
                towerNbr--;
                break;
        }

        UIManager.Instance.QuestUI.UpdateUI();

    }
    #endregion

    #region BLOB
    public void AddBlob(Blob blob)
    {
        blobPop++;

        switch (blob.GetBlobType())
        {
            case BlobManager.BlobType.normal:

                normalBlob++;

                break;

            case BlobManager.BlobType.soldier:

                soldierBlob++;

                break;

            case BlobManager.BlobType.explorateur:

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

            case BlobManager.BlobType.explorateur:

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

            case BlobManager.BlobType.explorateur:

                chargedBlob += nbr;

                break;

            case BlobManager.BlobType.mad:

                madBlob += nbr;

                break;

            case BlobManager.BlobType.coach:

                coachBlob += nbr;

                break;


        }
    }
       
    public void RemoveBlob(Blob blob)
    {
        blobPop--;

        switch (blob.GetBlobType())
        {
            case BlobManager.BlobType.normal:

                normalBlob--;

                break;

            case BlobManager.BlobType.soldier:

                soldierBlob--;

                break;

            case BlobManager.BlobType.explorateur:

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

            case BlobManager.BlobType.explorateur:

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

            case BlobManager.BlobType.explorateur:

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
