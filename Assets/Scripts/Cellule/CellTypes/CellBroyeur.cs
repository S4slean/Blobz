using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellBroyeur : CellMain
{
    public override void BlobsTick()
    {
        haveExpulse = false;



        if (blobNumber > 0)
        {
            int energyGainThisActivation = 0;
            currentTick++;
            if (currentTick == currentTickForActivation)
            {
                for (int i = 0; i < blobNumber; i++)
                {
                    if (blobNumber > 0)
                    {

                        BlobNumberVariation(-1, BlobCheck());
                        CellManager.Instance.EnergyVariation(specifiqueStats);
                        haveExpulse = true;
                        energyGainThisActivation += specifiqueStats;

                    }
                }
                TextScore newTextescore = ObjectPooler.poolingSystem.GetPooledObject<TextScore>() as TextScore;
                newTextescore.Outpool();

                newTextescore.myTransform.position = myTransform.position + new Vector3(Random.Range(-0.5f, 0.5f), 0, 0);
                newTextescore.textScore.text = ("+" + energyGainThisActivation.ToString());
                newTextescore.PlayAnim();

                //for (int i = 0; i < outputLinks.Count; i++)
                //{
                //    if (blobNumber <= 0)
                //    {
                //        break;
                //    }
                //    //Pour l'instant il y a moyen que si une cellule creve la prochaine 
                //    //soit sauté mai squand il y aura les anim , ce sera plus possible
                //    outputLinks[i].Transmitt(1 , BlobCheck());
                //    haveExpulse = true;

                //}

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
            //AnimBroyage

        }
    }
}
