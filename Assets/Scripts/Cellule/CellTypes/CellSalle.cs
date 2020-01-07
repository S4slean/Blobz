using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellSalle : CellMain
{
    public List<BlobCoach> myCoachs;
    private int myCoachBlobNumber;

    public override void BlobNumberVariation(int amount, BlobManager.BlobType _blobType)
    {
        switch (_blobType)
        {
            case BlobManager.BlobType.normal:
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.coach, amount);
                myCoachBlobNumber += amount;
                BlobCoach newCoach = new BlobCoach();
                newCoach.origianlSalle = this;
                newCoach.inThisCell = this;
                //newCoach.currentLife
                myCoachs.Add(newCoach);
                break;
            case BlobManager.BlobType.coach:
                coachBlobNumber += amount;
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.coach, amount);

                if (coachBlobNumber < 0)
                {
                    coachBlobNumber = 0;
                    hasBlobCoach = false;
                    coachIcon.SetActive(false);
                    ProximityLevelModification();
                }
                else
                {
                    hasBlobCoach = true;
                    coachIcon.SetActive(true);
                    ProximityLevelModification();
                }
                break;
            case BlobManager.BlobType.explorateur:
                explorateurBlobNumber += amount;
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.explorateur, amount);
                break;
        }
        blobNumber = myCoachBlobNumber + coachBlobNumber + explorateurBlobNumber;


        float ratio = (float)blobNumber / (float)currentBlobStockage;
        int pourcentage = Mathf.FloorToInt(ratio * 100f);
        if (pourcentage >= 80)
        {
            inDanger = true;
            if (!isVisible)
            {
                CellAlert alert = ObjectPooler.poolingSystem.GetPooledObject<CellAlert>() as CellAlert;
                UIManager.Instance.DisplayCellAlert(transform, alert);
            }
            //ANIM DANGER CELL 
        }
        else
        {
            inDanger = false;
        }

        if (blobNumber > currentBlobStockage && !isDead && !isNexus)
        {
            Died(false);
        }

        //Nexus 
        if (blobNumber > currentBlobStockage)
        {
            int blobToRemobe = blobNumber - currentBlobStockage;
            RessourceTracker.instance.RemoveBlob(BlobManager.BlobType.normal, blobToRemobe);
            blobNumber = currentBlobStockage;
        }
        UpdateCaract();
    }






}
[System.Serializable]
public struct BlobCoach
{
    public CellSalle origianlSalle;
    public int currentLife;
    public CellMain inThisCell;

    private void LooseLife()
    {

    }

    public void ChangeCell()
    {

    }
}
