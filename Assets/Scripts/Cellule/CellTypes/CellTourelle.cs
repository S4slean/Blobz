using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellTourelle : CellMain
{
    private int munitions;
    private bool isLoaded;

    public tourelleCollider tourelleCollider;


    public override void BlobsTick()
    {
        if (blobNumber > 0)
        {
            BlobNumberVariation(-1 , BlobCheck());
            MunitionVariation(1);
        }
        BlobNumberVariation(myCellTemplate.prodPerTickBase , BlobManager.BlobType.normal);

        //ANIM
        haveExpulse = false;

        if (blobNumber > 0)
        {
            currentTick++;
            if (currentTick == currentTickForActivation && isLoaded)
            {
                //TIR
                tourelleCollider.Fire();
            }

            for (int i = 0; i < outputLinks.Count; i++)
            {
                if (blobNumber <= 0)
                {
                    break;
                }
                //Pour l'instant il y a moyen que si une cellule creve la prochaine 
                //soit sauté mai squand il y aura les anim , ce sera plus possible
                outputLinks[i].Transmitt(1 , BlobCheck());
                haveExpulse = true;
            }
            currentTick = 0;
        }
        else
        {
            currentTick = 0;
        }

        if (haveExpulse)
        {
            anim.Play("BlobExpulsion");
        }

    }

    private void MunitionVariation(int amount)
    {
        munitions += amount;
        if (munitions <= 0)
        {
            munitions = 0;
            isLoaded = false;
            return;
        }
        isLoaded = true;

        if (munitions > myCellTemplate.tourelleMaxMun)
        {
            munitions = myCellTemplate.tourelleMaxMun;
        }
    }

    public override void SetupVariable()
    {
        munitions = 0;
        isLoaded = false;
        tourelleCollider.Init(this);
        base.SetupVariable();
    }

}
