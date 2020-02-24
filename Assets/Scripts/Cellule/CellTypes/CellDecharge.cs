using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDecharge : CellMain
{
    public override void OnShortLeftClickUp(RaycastHit hit)
    {

        actionmade = false;
        if (stuckBlobs.Count > 0)
        {
            stuckBlobs[stuckBlobs.Count - 1].Unstuck();
            actionmade = true;
        }
        else if (overLoad)
        {
            BlobNumberVariation(-1, BlobCheck(), false);
            actionmade = true;
        }

        if (blobNumber > 0 && !actionmade)
        {
            for (int i = 0; i < myCellTemplate.maxBlobShreddedPerClick; i++)
            {
                if (blobNumber > 0)
                {
                    BlobNumberVariation(-1, BlobCheck(), false);
                    RessourceTracker.instance.EnergyVariation(specifiqueStats);
                    TextScore newTextescore = ObjectPooler.poolingSystem.GetPooledObject<TextScore>() as TextScore;
                    newTextescore.Outpool();


                    newTextescore.myTransform.position = graphTransform.position + new Vector3(Random.Range(-1.5f, 1.5f), 2, 0);
                    newTextescore.textScore.text = ("+" + specifiqueStats.ToString());

                    newTextescore.PlayAnim(Random.Range(-0.15f , 0.15f));

                    actionmade = true;

                }
            }
        }

        if (actionmade)
        {
            // anim.Play("PlayerInteraction", 0, 0f);
            anim.SetBool("makeAction", true);
        }
    }

}
