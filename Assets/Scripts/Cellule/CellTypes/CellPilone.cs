using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellPilone : CellMain
{
    private int energie;

    public override void BlobsTick()
    {
        if (blobNumber > 0)
        {
            BlobNumberVariation(-1);
            ChargeEnergie();
        }
        else
        {
            energie--;
            if (energie <= 0)
            {
                //Possible probleme avec les colliders 
                // myProximityCollider[0].gameObject.SetActive(false);
                for (int i = 0; i < myProximityCollider.Length; i++)
                {
                    myProximityCollider[i].transform.position = myProximityCollider[i].initialPool.transform.position;
                    StartCoroutine(Desactive(myProximityCollider[i].gameObject));
                }
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
        energie = myCellTemplate.piloneMaxEnergie;


        for (int i = 0; i < myProximityCollider.Length; i++)
        {
            myProximityCollider[i].gameObject.SetActive(true);
            myProximityCollider[i].transform.position = transform.position;
        }
    }

    public override void SetupVariable()
    {
        base.SetupVariable();
        energie = 0;
        myProximityCollider[0].gameObject.SetActive(false);
    }

    //c'est ok avec ça ? 
    protected IEnumerator Desactive(GameObject _gameObject)
    {
        //A changé mais c'est pour test
        yield return new WaitForFixedUpdate();
        _gameObject.SetActive(false);
    }
}
