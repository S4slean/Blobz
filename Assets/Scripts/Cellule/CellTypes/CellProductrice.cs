using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellProductrice : CellMain
{
    private int productionBonusRatio;
    private int productionBonusPacket;

    public override void BlobsTick()
    {
        haveExpulse = false;
        //ça marche bien mais à voir si quand 1 batiment meure la produciton saute avec ou pas
        //if ((int)Random.Range(0, 101) <= productionBonusRatio)
        //{
        //    BlobNumberVariation(myCellTemplate.prodPerTickBase , BlobManager.BlobType.normal);
        //}

        int productionPerTick = myCellTemplate.prodPerTickBase/* * (1 + productionBonusPacket)*/;

        BlobNumberVariation(productionPerTick , BlobManager.BlobType.normal , true);


        #region Ancien Systeme de Tick 
        //if (blobNumber > 0)
        //{
        //    currentTick++;
        //    if (currentTick == currentTickForActivation)
        //    {
        //        Debug.Log("tickPass");
        //        for (int i = 0; i < currentRejectPower; i++)
        //        {
        //            if (blobNumber > 0 && outputLinks.Count > 0)
        //            {
        //                if (currentIndex >= outputLinks.Count)
        //                {
        //                    return;
        //                }
        //                outputLinks[currentIndex].Transmitt(1);
        //                currentIndex++;
        //                currentIndex = Helper.LoopIndex(currentIndex, outputLinks.Count);
        //                haveExpulse = true;
        //            }
        //        }
        //        currentTick = 0;
        //    }
        //}
        #endregion
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
                    outputLinks[i].Transmitt(1 , BlobCheck());
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


    public void ProductriceProximityGestion(CellProximityDectection collider, bool enter)
    {
        ProductionVariationByProximity(collider.productionBonusRatio, enter);
        //Il faut choisir le plus grand 
        //for (int i = 0; i < myProximityCollider.Length; i++)
        //{
        //    if (collider == myProximityCollider[i])
        //    {
        //        ProductionVariationByProximity(collider.productionBonusRatio, enter);
        //    }
        //}
    }

    public override void Died(bool intentionnalDeath)
    {

        productionBonusPacket = 0;
        productionBonusRatio = 0;
        base.Died(intentionnalDeath);

    }

    private void ProductionVariationByProximity(int amount, bool addition)
    {
        int multiplier;
        multiplier = (addition == true) ? 1 : -1;

        productionBonusRatio += multiplier * amount;

        if (productionBonusRatio >= 100)
        {
            productionBonusPacket++;
            int reste = productionBonusRatio - 100;
            productionBonusRatio = 0;
            productionBonusRatio += reste;
        }
        if (productionBonusRatio < 0)
        {
            productionBonusPacket--;
            int reste = productionBonusRatio;
            productionBonusRatio = 100;
            productionBonusRatio += reste;
        }

    }

}
