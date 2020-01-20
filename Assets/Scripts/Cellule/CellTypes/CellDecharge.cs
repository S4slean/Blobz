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
            BlobNumberVariation(-1, BlobCheck() , false);
            RessourceTracker.instance.EnergyVariation(specifiqueStats);
            TextScore newTextescore = ObjectPooler.poolingSystem.GetPooledObject<TextScore>() as TextScore;
            newTextescore.Outpool();


            Debug.Log(myTransform.position);
            newTextescore.myTransform.position = graphTransform.position + new Vector3(Random.Range(-0.5f, 0.5f), 2, 0);
            newTextescore.textScore.text = ("+" + specifiqueStats.ToString());
            newTextescore.PlayAnim();
            actionmade = true;
        }


        if (actionmade)
        {
            anim.Play("PlayerInteraction", 0, 0f);
        }



    }

}
