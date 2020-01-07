using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellDivine : CellMain
{
    private bool isLoaded;
    private int energie;
    public divineCellButon activationButton;

    

    public override void BlobsTick()
    {
        if (blobNumber > 0)
        {
            BlobNumberVariation(-1 , BlobCheck());
            Charge(1);
        }
        BlobNumberVariation(myCellTemplate.prodPerTickBase , BlobManager.BlobType.normal);

        //ANIM
        haveExpulse = false;

        if (blobNumber > 0)
        {
            currentTick++;
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

    private void Charge(int amount)
    {
        if (!isLoaded)
        {
            energie += amount;
            if (energie >= myCellTemplate.maxEnergie)
            {
                isLoaded = true;
                energie = myCellTemplate.maxEnergie;
                //
                Debug.Log("Loaded , anim à déclencher ");
            }
        }
    }
    public void Decharge()
    {

    }

    public override void SetupVariable()
    {
        energie = 0;
        isLoaded = false;
        base.SetupVariable();
    }

    public override void OnShortLeftClickUp(RaycastHit hit)
    {
        if (isLoaded)
        {
            InputManager.SwitchInputMode(InputManager.InputMode.divineShot);
            //passer la reference de l'input de tir 
        }
        else
        {
            //dipslay not enough charge 
        }
    }

    public override void ProximityLevelModification()
    {
        base.ProximityLevelModification();

        UIManager.Instance.UpdateShootingArea(specifiqueStats);
    }
}
