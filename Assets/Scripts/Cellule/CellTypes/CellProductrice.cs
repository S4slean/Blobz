using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CellProductrice : CellMain
{
    //private int productionBonusRatio;
    //private int productionBonusPacket;
    private int currentLevel = 0;
    private int currentExp = 0;
    private bool canLevelUp;
    private bool levelUpButtonOn;
    private bool isLevelMax;
    public TextMeshPro prodDisplay;


    private int Life;
    public ProgressBar progressBar;
    public ProgressBar expBar;
    public CellButon levelUpButton;

    private List<CellMain> CellThatGiveExp = new List<CellMain>();


    public override void BlobsTick()
    {
        //int production = outputLinks.Count + productionBonusPacket;
        // prodDisplay.text = production.ToString();
        prodDisplay.text = (currentLevel+1).ToString();

        if (stuckBlobs.Count <= 0 && Life < myCellTemplate.maxLifeProd)
        {
            StockageCapabilityVariation(1);
        }

        if (!overLoad)
        {
            overloadStack = 0;
            // ça marche bien mais à voir si quand 1 batiment meure la produciton saute avec ou pas
            //if ((int)Random.Range(0, 101) <= productionBonusRatio)
            //{
            //    BlobNumberVariation(myCellTemplate.prodPerTickBase, BlobManager.BlobType.normal, true);
            //}

            //int productionPerTick = myCellTemplate.prodPerTickBase * (1 + productionBonusPacket);

            //BlobNumberVariation(productionPerTick, BlobManager.BlobType.normal, true);
            haveExpulse = false;

            int nbrTransmission = 0;
            for (int i = 0; i < myCellTemplate.levelProduction[currentLevel].blobsProduction; i++)
            {

                if (currentIndex < outputLinks.Count)
                {
                    outputLinks[currentIndex].Transmitt(1, BlobManager.BlobType.normal);
                    haveExpulse = true;
                    currentIndex++;
                    currentIndex = Helper.LoopIndex(currentIndex, outputLinks.Count);
                    nbrTransmission++;
                }
                else
                {
                   // Debug.Log(currentIndex + "Reset Index" + "LinkCount" + outputLinks.Count);
                    currentIndex = 0;
                }

            }

            if (haveExpulse && anim != null)
            {
                anim.Play("BlobExpulsion");
            }
        }
        else
        {
            if (!LevelManager.instance.cellInvicible)
            {
                overloadSparke.SetSpikeNumberAndSpeed(overloadStack, overloadStack * 0.3f);
                overloadStack++;
                if (overloadStack > myCellTemplate.overLoadTickMax)
                {
                    Died(false);
                }
            }
        }
    }


    public override void StockageCapabilityVariation(int Amount)
    {
        Life += Amount;
        float ratio = (float)Life / (float)myCellTemplate.maxLifeProd;
        if (Life < myCellTemplate.maxLifeProd)
        {
            progressBar.ToggleRenderer(true);
            //Display Barre
        }
        else
        {
            progressBar.ToggleRenderer(false);
        }


        if (Life <= 0)
        {
            ToggleOverload(true);
        }
        if (Life > 0 && overLoad)
        {
            ToggleOverload(false);
        }
        HandleAlerts();
        progressBar.UpdateBar(ratio, true);

    }
    public override void BlobNumberVariation(int amount, BlobManager.BlobType _blobType, bool transmission)
    {
        switch (_blobType)
        {
            case BlobManager.BlobType.normal:
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.normal, amount);
                normalBlobNumber += amount;
                break;

            case BlobManager.BlobType.coach:
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.coach, amount);
                if (!transmission)
                {
                    if (amount < 0)
                    {
                        for (int i = 0; i < amount; i++)
                        {
                            blobCoaches.Remove(blobCoaches[blobCoaches.Count - 1]);
                        }
                    }
                }
                break;

            case BlobManager.BlobType.explorateur:
                explorateurBlobNumber += amount;
                RessourceTracker.instance.AddBlob(BlobManager.BlobType.explorateur, amount);
                break;
        }
        blobNumber = normalBlobNumber + blobCoaches.Count + explorateurBlobNumber;
    }
    //public void ProductriceProximityGestion(CellProximityDectection collider, bool enter)
    //{
    //    ProductionVariationByProximity(collider.productionBonusRatio, enter);
    //}
    public void ProductionVariationByProximity(int amount, bool addition)
    {
        int multiplier;
        multiplier = (addition == true) ? 1 : -1;

        currentExp += multiplier * amount;

        int expNeeded = myCellTemplate.levelProduction[currentLevel + 1].expNeeded;

        if (currentExp >= expNeeded)
        {
            currentExp = expNeeded;
            if (!isLevelMax && !levelUpButtonOn)
            {
                canLevelUp = true;
                levelUpButton.ToggleButton(true);
            }
        }
        else
        {
            canLevelUp = false;
        }

        float ratio = (float)currentExp / (float)expNeeded;
        expBar.UpdateBar(ratio, true);
        //productionBonusRatio += multiplier * amount;

        //if (productionBonusRatio >= 100)
        //{
        //    productionBonusPacket++;
        //    int reste = productionBonusRatio - 100;
        //    productionBonusRatio = 0;
        //    productionBonusRatio += reste;
        //}
        //if (productionBonusRatio < 0)
        //{
        //    productionBonusPacket--;
        //    int reste = productionBonusRatio;
        //    productionBonusRatio = 100;
        //    productionBonusRatio += reste;
        //}
    }
    public void LevelUp()
    {
        currentExp = 0;
        currentLevel += 1;
        baseR.sprite = myCellTemplate.levelProduction[currentLevel].spriteLevel;
        levelUpButton.ToggleButton(false);
        UpdateExpLevelUp();
        if (currentLevel + 1 >= myCellTemplate.levelProduction.Length)
        {
            isLevelMax = true;
            expBar.gameObject.SetActive(false);
        }
    }
    public override void Died(bool intentionnalDeath)
    {
        //productionBonusPacket = 0;
        //productionBonusRatio = 0;
        currentLevel = 0;
        levelUpButton.ToggleButton(false);
        CellThatGiveExp.Clear();
        base.Died(intentionnalDeath);
    }
    public override void SetupVariable()
    {
        Life = myCellTemplate.maxLifeProd;

        canLevelUp = false;
        isLevelMax = false;
        levelUpButtonOn = false;
        currentLevel = 0;
        baseR.sprite = myCellTemplate.levelProduction[currentLevel].spriteLevel;

        int expNeeded = myCellTemplate.levelProduction[currentLevel + 1].expNeeded;
        float ratioExp = (float)currentExp / (float)expNeeded;
        expBar.gameObject.SetActive(true);
        expBar.UpdateBar(ratioExp, true);

        float ratio = (float)Life / (float)myCellTemplate.maxLifeProd;
        progressBar.transform.position = graphTransform.position;

        progressBar.UpdateBar(ratio, true);
        progressBar.ToggleRenderer(false);

        base.SetupVariable();
    }
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

        if (canLevelUp)
        {
            LevelUp();
            actionmade = true;
        }

        if (actionmade)
        {
            anim.Play("PlayerInteraction", 0, 0f);
        }

    }

    public void AddExpCell(CellMain cellToAdd)
    {
        bool alreadyAdded = false;
        for (int i = 0; i < CellThatGiveExp.Count; i++)
        {
            if (cellToAdd == CellThatGiveExp[i])
            {
                alreadyAdded = true;
            }
        }
        if (alreadyAdded)
        {
            return;
        }

        CellThatGiveExp.Add(cellToAdd);

        int expAmount = 0;
        for (int i = 0; i < CellThatGiveExp.Count; i++)
        {
            expAmount += CellThatGiveExp[i].myCellTemplate.expAmount;
        }
        int removePreviousExpAmount = 0;
        for (int i = 0; i < currentLevel + 1; i++)
        {
            removePreviousExpAmount += myCellTemplate.levelProduction[i].expNeeded;
        }


        currentExp = expAmount - removePreviousExpAmount;

        int expNeeded = myCellTemplate.levelProduction[currentLevel + 1].expNeeded;

        if (currentExp >= expNeeded)
        {
            currentExp = expNeeded;
            if (!isLevelMax && !levelUpButtonOn)
            {
                canLevelUp = true;
                levelUpButton.ToggleButton(true);
            }
        }
        else
        {
            canLevelUp = false;
        }

        float ratio = (float)currentExp / (float)expNeeded;
        expBar.UpdateBar(ratio, true);
    }
    public void UpdateExpLevelUp()
    {


        int expAmount = 0;
        for (int i = 0; i < CellThatGiveExp.Count; i++)
        {
            expAmount += CellThatGiveExp[i].myCellTemplate.expAmount;
        }
        int removePreviousExpAmount = 0;
        for (int i = 0; i < currentLevel + 1; i++)
        {
            removePreviousExpAmount += myCellTemplate.levelProduction[i].expNeeded;
        }


        currentExp = expAmount - removePreviousExpAmount;

        int expNeeded = myCellTemplate.levelProduction[currentLevel + 1].expNeeded;

        if (currentExp >= expNeeded)
        {
            currentExp = expNeeded;
            if (!isLevelMax && !levelUpButtonOn)
            {
                canLevelUp = true;
                levelUpButton.ToggleButton(true);
            }
        }
        else
        {
            canLevelUp = false;
        }

        float ratio = (float)currentExp / (float)expNeeded;
        expBar.UpdateBar(ratio, true);
    }
}
