using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPilone : CellMain
{
    private int energie;

    //Faudra mettre ça en cell Template
    private int MaxEnergie;

    public override void BlobsTick()
    {
        if (blobNumber > 0 )
        {
            BlobNumberVariation(-1);
            ChargeEnergie();
        }
        else
        {
            energie--;
            if (energie <= 0 )
            {
                myProximityCollider[0].gameObject.SetActive(false);
            }
        }
        BlobNumberVariation(myCellTemplate.prodPerTickBase);

        //ANIM
        haveExpulse = false;

        if (blobNumber > 0)
        {
            currentTick++;
            if (currentTick == currentTickForActivation)
            {
                for (int i = 0; i < outputLinks.Count; i++)
                {
                    if (blobNumber <= 0)
                    {
                        break;
                    }
                    //Pour l'instant il y a moyen que si une cellule creve la prochaine 
                    //soit sauté mai squand il y aura les anim , ce sera plus possible
                    outputLinks[i].Transmitt(1);
                    haveExpulse = true;
                }
                currentTick = 0;
            }
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

    private void ChargeEnergie()
    {
        energie = MaxEnergie;
        myProximityCollider[0].gameObject.SetActive(true);
    }

}
