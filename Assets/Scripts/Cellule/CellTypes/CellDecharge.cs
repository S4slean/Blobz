using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDecharge : CellMain
{
    public override void OnShortLeftClickUp(RaycastHit hit)
    {
        if (blobNumber > 0)
        {
            BlobNumberVariation(-1, BlobCheck());
            //CellManager.Instance.EnergyVariation(currentEnergyPerClick);
            RessourceTracker.instance.EnergyVariation(specifiqueStats);
            TextScore newTextescore = ObjectPooler.poolingSystem.GetPooledObject<TextScore>() as TextScore;
            newTextescore.Outpool();


            Debug.Log(myTransform.position);
            newTextescore.myTransform.position = myTransform.position + new Vector3(Random.Range(-0.5f, 0.5f), 2, 0);
            newTextescore.textScore.text = ("+" + specifiqueStats.ToString());
            newTextescore.PlayAnim();
        }
        // à quoi servent les 2 dernier parametre
        //AnimBroyage
        anim.Play("PlayerInteraction", 0, 0f);

    }

}
