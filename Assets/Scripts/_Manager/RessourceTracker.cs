﻿using System.Collections;
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

            case (CellType.Treblochet):
                armoryNbr++;
                break;

            case (CellType.Broyeur):
                broyeurNbr++;
                break;

            case (CellType.Passage):
                passageNbr++;
                break;

            case (CellType.BlipBlop):
                blipblopNbr++;
                break;

            case (CellType.Decharge):
                dechargeNbr++;
                break;

            case (CellType.Divine):
                divineNbr++;
                break;

            case (CellType.Exploration):
                exploNbr++;
                break;

            case (CellType.LaSalle):
                coachRoomNbr++;
                break;

            case (CellType.Pilone):
                piloneNbr++;
                break;

            case (CellType.Tourelle):
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
            case (CellType.Productrice):
                hatchNbr--;
                if(hatchNbr < 1)
                {
                    LevelManager.instance.LevelFailed();
                }
                break;

            case (CellType.Stockage):
                stockNbr--;
                break;

            case (CellType.Treblochet):
                armoryNbr--;
                break;

            case (CellType.Broyeur):
                broyeurNbr--;
                break;

            case (CellType.Passage):
                passageNbr--;
                break;


            case (CellType.BlipBlop):
                blipblopNbr--;
                break;

            case (CellType.Decharge):
                dechargeNbr--;
                break;

            case (CellType.Divine):
                divineNbr--;
                break;

            case (CellType.Exploration):
                exploNbr--;
                break;

            case (CellType.LaSalle):
                coachRoomNbr--;
                break;

            case (CellType.Pilone):
                piloneNbr--;
                break;

            case (CellType.Tourelle):
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
