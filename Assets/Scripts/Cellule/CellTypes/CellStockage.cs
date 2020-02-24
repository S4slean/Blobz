using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellStockage : CellMain
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

        if (!actionmade && blobNumber > 0 )
        {
            BlobsTick();
            actionmade = true;
        }

        if (actionmade)
        {
            //anim.Play("PlayerInteraction", 0, 0f);
            anim.SetBool("makeAction", true);
        }

    }


}
