using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDecharge : CellMain
{

    public override void OnShortLeftClickUp(RaycastHit hit)
    {
        if (blobNumber > 0)
        {
            BlobNumberVariation(-1 , BlobCheck());
            //CellManager.Instance.EnergyVariation(currentEnergyPerClick);
            RessourceTracker.instance.EnergyVariation(currentEnergyPerClick);
        }
        // à quoi servent les 2 dernier parametre
        anim.Play("PlayerInteraction", 0, 0f);
    }

}
