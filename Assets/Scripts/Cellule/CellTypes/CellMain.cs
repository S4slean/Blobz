using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;

//[RequireComponent(typeof(MeshCollider))]  //typeof(MeshRenderer), typeof(MeshFilter),
public class CellMain : PoolableObjects
{
    #region Variables
    [Tooltip("glissé l'un des srcyptable object structure ici")]
    public CelluleTemplate myCellTemplate;
    public bool isNexus;


    // public List<CelulleMain> outputCell;
    #region REFS
    public Material cellDefaultMat;

    public Animator anim;
    public TextMeshPro NBlob;
    public TextMeshPro NLink;
    public TextMeshPro NCurrentProximity;
    public Transform graphTransform;
    public Transform TargetPos;

    public List<LinkClass> links = new List<LinkClass>();
    protected List<LinkClass> outputLinks = new List<LinkClass>();

    public List<CellProximityDectection> inThoseCellProximity = new List<CellProximityDectection>();
    public List<CellProximityDectection> influencedByThoseCellProximity = new List<CellProximityDectection>();


    //public MeshFilter mF;
    //public MeshRenderer mR;
    // public MeshCollider mC;

    //public CellProximityDectection ProximityDectection;
    #endregion

    #region DEBUG
    public bool showDebug;

    public bool showlinks;
    public bool showRef;
    public bool noMoreLink;
    public int blobNumber;
    public bool hasBeenDrop;
    public bool limitedInLink;

    protected int currentIndex;


    #endregion

    #region STATS
    //Important for the communication into 

    protected int currentProximityLevel;
    protected int currentProximityTier;

    protected int currentLinkStockage;
    protected int currentBlobStockage;

    protected int currentTickForActivation;
    protected float currentTick;

    protected int currentEnergyPerClick;
    protected int currentEnergyCap;

    protected int currentSurproductionRate;
    protected float currentRejectPower;
    protected int currentRange;


    #endregion

    #region Etats/Boolean

    protected bool inDanger;
    protected bool isDead = false;
    public bool canBePlaced;
    protected bool isVisible;
    #endregion

    #region Anim Variable   
    protected bool haveExpulse;
    #endregion

    #endregion

    public virtual void Awake()
    {
        //ProximityDectection.parent = this;

        //mR.material = myCellTemplate.mat;
        //mF.mesh = myCellTemplate.mesh;
        //ProximityCheck();

    }

    public virtual void OnEnable()
    {
        //Pour le delegate qui gére le tick
        //TickManager.doTick += BlobsTick;
        //UI init 
        NBlob.text = (blobNumber + " / " + myCellTemplate.storageCapability);
        NLink.text = (links.Count + " / " + myCellTemplate.linkCapability);
        currentBlobStockage = myCellTemplate.storageCapability;
        currentLinkStockage = myCellTemplate.linkCapability;

        hasBeenDrop = false;


        if (isNexus)
        {
            StartCoroutine(WaitForInit());
        }

    }

    IEnumerator WaitForInit()
    {
        yield return new WaitForEndOfFrame();
        CellInitialisation();
    }

    public void CellInitialisation()
    {
        RessourceTracker.instance.AddCell(this);

        TickInscription();
        isDead = false;

        SetupVariable();
        RessourceTracker.instance.EnergyCapVariation(currentEnergyCap);

    }

    private void Start()
    {
        GraphSetup();
    }

    public virtual void Died(bool intentionnalDeath)
    {
        isDead = true;
        //TickManager.doTick -= BlobsTick;

        RessourceTracker.instance.RemoveBlob(BlobManager.BlobType.normal, blobNumber);


        if (CellManager.Instance.originalPosOfMovingCell != new Vector3(0, 100, 0))
            RessourceTracker.instance.RemoveCell(this);

        if (InputManager.Instance.CellSelected == this)
        {
            UIManager.Instance.DesactivateCellShop();
        }


        TickDesinscription();
        int I = links.Count;
        for (int i = 0; i < I; i++)
        {
            if (i > I)
            {
                break;
            }
            links[I - i - 1].Break();
        }

        if (!intentionnalDeath)
        {
            //Spawn les blobs
            for (int i = 0; i < blobNumber; i++)
            {
                //Debug.Log("SI TU VOIS ÇA C'EST QUE LES BLOB SONT ENCORE INSTANCIE EN SALE AINSI QUE LEUR RIGIDBODY ");
                //GameObject blob = Instantiate(myCellTemplate.blopPrefab, transform.position, Quaternion.identity);
                //Rigidbody rb = blob.GetComponent<Rigidbody>();
                //Vector3 dir = new Vector3(Random.Range(-1, 1), Random.Range(0.1f, 1f), Random.Range(-1, 1)) * myCellTemplate.impulseForce_Death;
                //rb.AddForce(dir, ForceMode.Impulse);
                //blob.GetComponent<Blob>().blobType = BlobBehaviour.BlobType.mad;
            }
        }

        if (this == CellManager.Instance.selectedCell)
        {
            if (InputManager.Instance.DraggingLink)
            {
                CellManager.Instance.SupressCurrentLink();
            }
            InputManager.Instance.CleanBoolsRelatedToCell();
            UIManager.Instance.DesactivateCellShop();
            UIManager.Instance.CellDeselected();

            if (InputManager.Instance.objectMoved != null)
            {
                InputManager.Instance.objectMoved.Died(true);
                InputManager.Instance.movingObject = false;
            }

        }
        blobNumber = 0;
        SetupVariable();
        Inpool();
    }
    public virtual void BlobsTick()
    {
        BlobNumberVariation(myCellTemplate.prodPerTickBase);

        //ANIM
        haveExpulse = false;
        #region Ancienne Distribution

        //if (blobNumber > 0)
        //{

        //    currentTick++;
        //    if (currentTick == currentTickForActivation)
        //    {
        //        for (int i = 0; i < currentRejectPower; i++)
        //        {
        //            if (blobNumber > 0 && outputLinks.Count > 0)
        //            {
        //                if (currentIndex >= outputLinks.Count)
        //                {
        //                    return;
        //                }
        //                outputLinks[currentIndex].Transmitt();
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
    public virtual void StockageCapabilityVariation(int Amount)
    {
        currentBlobStockage += Amount;
        UpdateCaract();
        if (blobNumber > currentBlobStockage && !isDead)
        {
            Died(false);
        }
        UpdateCaract();
    }
    public virtual void ClickInteraction()
    {
        if (blobNumber > 0)
        {
            BlobNumberVariation(-1);
            //CellManager.Instance.EnergyVariation(currentEnergyPerClick);
            RessourceTracker.instance.EnergyVariation(currentEnergyPerClick);
        }

        anim.Play("PlayerInteraction", 0, 0f);


    }


    #region BLOB_GESTION
    //public void AddBlob(int Amount)
    //{
    //    blobNumber += Amount;

    //    RessourceTracker.instance.AddBlob(BlobManager.BlobType.normal, Amount);

    //    NBlob.text = (blobNumber + " / " + currentBlobStockage);
    //    if (blobNumber > currentBlobStockage && !isDead && !isNexus)
    //    {
    //        Died(false);
    //    }
    //    if (blobNumber > currentBlobStockage)
    //    {
    //        int blobToRemobe = blobNumber - currentBlobStockage;

    //        RessourceTracker.instance.RemoveBlob(BlobManager.BlobType.normal, blobToRemobe);


    //        blobNumber = currentBlobStockage;
    //    }
    //    UpdateCaract();
    //}
    //public void RemoveBlob(int Amount)
    //{
    //    blobNumber -= Amount;

    //    RessourceTracker.instance.RemoveBlob(BlobManager.BlobType.normal, Amount);
    //    //UI update
    //    UpdateCaract();
    //}
    public void BlobNumberVariation(int amount)
    {
        blobNumber += amount;

        RessourceTracker.instance.AddBlob(BlobManager.BlobType.normal, amount);

        if (currentBlobStockage <= 0)
        {
            Died(false);
        }
        else
        {
            float ratio = (float)blobNumber / (float)currentBlobStockage;
            int pourcentage = Mathf.FloorToInt(ratio * 100f);
            if (pourcentage >= 80)
            {
                inDanger = true;
                if (!isVisible)
                {
                    //ANIM UI 
                }
                //ANIM DANGER CELL 
            }
            else
            {
                inDanger = false;
            }
            NBlob.text = (pourcentage + " %");
        }
        //NBlob.text = (blobNumber + " / " + currentBlobStockage);

        if (blobNumber > currentBlobStockage && !isDead && !isNexus)
        {
            Died(false);
        }

        //Nexus 
        if (blobNumber > currentBlobStockage)
        {
            int blobToRemobe = blobNumber - currentBlobStockage;
            RessourceTracker.instance.RemoveBlob(BlobManager.BlobType.normal, blobToRemobe);
            blobNumber = currentBlobStockage;
        }
        //UpdateCaract();
    }
    //en prévision 
    //public void BlobNumberVariation(int amount , BlobManager.BlobType blobType)
    //{

    //}
    #endregion

    #region PROXIMITE_GESTION
    public void ProximityCheck()
    {
        // ProximityDectection.myCollider.radius = Mathf.SmoothDamp(0, myCellTemplate.range / 2, ref velocity, 0.01f);
        if (myCellTemplate.generateProximity)
        {
            for (int i = 0; i < myCellTemplate.proximityColliderNumber; i++)
            {
                CellProximityDectection newProximityCollider = ObjectPooler.poolingSystem.GetPooledObject<CellProximityDectection>() as CellProximityDectection;
                newProximityCollider.transform.localScale = new Vector3(1, 1, 1) * (myCellTemplate.proximityColliders[i].range / 2);
                newProximityCollider.transform.SetParent(transform);
                newProximityCollider.Init(myCellTemplate.proximityColliders[i].proximityLevel, transform);
                newProximityCollider.parent = this;

                newProximityCollider.Outpool();
            }
        }
        //ProximityDectection.myCollider.radius = currentRange / 2;
    }

    public void AddProximityInfluence(CellProximityDectection proximityToAdd)
    {
        bool becomeInfluenced = true;
        bool checkEnd = false;
        for (int i = 0; i < influencedByThoseCellProximity.Count; i++)
        {
            if (proximityToAdd.parent == influencedByThoseCellProximity[i].parent && !checkEnd)
            {
                if (Mathf.Abs(proximityToAdd.proximityLevel) <= Mathf.Abs(influencedByThoseCellProximity[i].proximityLevel))
                {
                    becomeInfluenced = false;
                    checkEnd = true;
                }
                else
                {
                    becomeInfluenced = true;
                    influencedByThoseCellProximity.Remove(influencedByThoseCellProximity[i]);
                }
            }
        }
        if (becomeInfluenced)
        {
            influencedByThoseCellProximity.Add(proximityToAdd);

        }
        ProximityLevelModification();
    }

    public void RemoveProximityInfluence(CellProximityDectection proximityToRemove)
    {
        inThoseCellProximity.Remove(proximityToRemove);
        for (int i = 0; i < influencedByThoseCellProximity.Count; i++)
        {
            if (proximityToRemove == influencedByThoseCellProximity[i])
            {
                influencedByThoseCellProximity.Remove(proximityToRemove);
                for (int y = 0; y < inThoseCellProximity.Count; y++)
                {
                    if (inThoseCellProximity[y].parent == proximityToRemove.parent)
                    {
                        AddProximityInfluence(inThoseCellProximity[y]);
                    }
                }
            }
        }
        //influencedByThoseCellProximity.Remove(proximityToRemove);
        //for (int y = 0; y < inThoseCellProximity.Count; y++)
        //{
        //    if (inThoseCellProximity[y].parent == proximityToRemove.parent)
        //    {
        //        AddProximityInfluence(inThoseCellProximity[y]);
        //    }
        //}

        ProximityLevelModification();
    }




    #region Ancien Système de proximité

    //public void AddToCellAtPromity(CellMain cellDetected)
    //{
    //    bool endFunction = false;
    //    //cellAtProximity.Add(cellDetected);
    //    CellType cellDetectedType = cellDetected.myCellTemplate.type;
    //    for (int i = 0; i < myCellTemplate.negativesInteractions.Length; i++)
    //    {
    //        if (cellDetectedType == myCellTemplate.negativesInteractions[i])
    //        {
    //            ProximityLevelModification(-1);
    //            endFunction = true;
    //            break;
    //        }
    //    }
    //    if (endFunction)
    //    {
    //        return;
    //    }

    //    for (int j = 0; j < myCellTemplate.positivesInteractions.Length; j++)
    //    {
    //        if (cellDetectedType == myCellTemplate.positivesInteractions[j])
    //        {
    //            ProximityLevelModification(1);
    //            break;
    //        }
    //    }
    //}
    //public void RemoveToCellAtPromity(CellMain cellDetected)
    //{
    //    //cellAtProximity.Remove(cellDetected);
    //    CellType cellDetectedType = cellDetected.myCellTemplate.type;

    //    for (int i = 0; i < myCellTemplate.negativesInteractions.Length; i++)
    //    {
    //        if (cellDetectedType == myCellTemplate.negativesInteractions[i])
    //        {
    //            ProximityLevelModification(+1);
    //            // Ajouter L'UI 
    //        }

    //    }
    //    for (int j = 0; j < myCellTemplate.positivesInteractions.Length; j++)
    //    {
    //        if (cellDetectedType == myCellTemplate.positivesInteractions[j])
    //        {
    //            ProximityLevelModification(-1);
    //        }
    //    }
    //}
    #endregion
    public virtual void ProximityLevelModification()
    {
        int LastProximityTier = currentProximityTier;
        int lastEnergyCap = currentEnergyCap;

        currentProximityLevel = 0;
        for (int i = 0; i < influencedByThoseCellProximity.Count; i++)
        {
            currentProximityLevel += influencedByThoseCellProximity[i].proximityLevel;
        }

        NCurrentProximity.text = currentProximityLevel.ToString();

        if (currentProximityLevel >= 0 && currentProximityLevel < myCellTemplate.proximityLevelMax)
        {
            currentProximityTier = currentProximityLevel;
        }
        else if (currentProximityLevel >= myCellTemplate.proximityLevelMax)
        {
            currentProximityTier = myCellTemplate.proximityLevelMax - 1;
        }
        else
        {
            currentProximityTier = 0;
        }

        if (currentProximityTier != LastProximityTier)
        {
            switch (myCellTemplate.StatsModification)
            {
                case StatsModificationType.Surproduction:
                    currentSurproductionRate = myCellTemplate.SurproductionRate[currentProximityTier];
                    break;

                case StatsModificationType.RejectForce:
                    currentRejectPower = myCellTemplate.BlopPerTick[currentProximityTier];
                    break;

                case StatsModificationType.StockageCapacity:
                    currentBlobStockage = myCellTemplate.stockageCapacity[currentProximityTier];
                    break;

                case StatsModificationType.LinkCapacity:
                    currentLinkStockage = myCellTemplate.LinkCapacity[currentProximityTier];
                    break;

                case StatsModificationType.Range:
                    currentRange = myCellTemplate.Range[currentProximityTier];
                    break;

                case StatsModificationType.TickForActivation:
                    currentTickForActivation = myCellTemplate.tickForActivation[currentProximityTier];
                    break;

                case StatsModificationType.EnergyCap:
                    currentEnergyCap = myCellTemplate.energyCap[currentProximityTier];
                    RessourceTracker.instance.EnergyCapVariation(currentEnergyCap - lastEnergyCap);
                    break;

                case StatsModificationType.Aucune:
                    break;
            }
        }
        UpdateCaract();
    }
    #endregion

    #region LINK_GESTION
    public virtual void AddLink(LinkClass linkToAdd, bool output)
    {
        links.Add(linkToAdd);

        if (output)
        {
            linkToAdd.Init(this);
            outputLinks.Add(linkToAdd);
            SortingLink();
        }
        else
        {
            linkToAdd.receivingCell = this;
        }

        if (links.Count >= myCellTemplate.linkCapability)
        {
            noMoreLink = true;
        }
        UpdateCaract();
    }
    public virtual void RemoveLink(LinkClass linkToRemove)
    {
        if (links.Count < currentLinkStockage)
        {
            noMoreLink = false;
        }
        outputLinks.Remove(linkToRemove);
        links.Remove(linkToRemove);

        UpdateCaract();
    }
    //tri les output links
    public virtual void SortingLink()
    {
        outputLinks = outputLinks.OrderBy(t => t.angle).ToList();
    }
    #endregion

    #region UTILITAIRE/GRAPH




    //public bool CheckLinkDistance(Vector3 pos)
    //{
    //    for (int i = 0; i < links.Count; i++)
    //    {
    //        if((links[i].originalCell.transform.position - pos).magnitude > links[i].originalCell.
    //    }


    //    return true;
    //}

    public virtual void TickInscription()
    {
        TickManager.doTick += BlobsTick;
    }
    public virtual void TickDesinscription()
    {
        TickManager.doTick -= BlobsTick;
    }


    //A changé au lieu de désactiver on peut juste désactiver les components ( c'est une micro opti ) 
    public override void Inpool()
    {
        if (Application.isEditor)
        {
            canBePool = true;
            gameObject.SetActive(false);
        }
        else
        {

            canBePool = true;
            transform.position = ObjectPooler.poolingSystem.transform.position;
            StartCoroutine(DesactiveGameObject(0.02f));
        }
    }
    public virtual void UpdateCaract()
    {
        NBlob.text = (blobNumber + " / " + currentBlobStockage);
        NLink.text = (links.Count + " / " + currentLinkStockage);
    }

    public virtual void GraphSetup()
    {
        Vector3 graphPos = transform.position + new Vector3(0, 0.1f, 0);
        graphTransform.position = graphPos;
    }

    private void SetupVariable()
    {
        currentLinkStockage = myCellTemplate.linkCapability;
        currentBlobStockage = myCellTemplate.storageCapability;
        currentSurproductionRate = myCellTemplate.SurproductionRate[0];
        currentRejectPower = myCellTemplate.rejectPowerBase;
        currentRange = myCellTemplate.rangeBase;
        currentTickForActivation = myCellTemplate.tickForActivationBase;

        currentEnergyPerClick = myCellTemplate.energyPerClick;
        currentEnergyCap = myCellTemplate.energyCapBase;

        //cellAtProximity.Clear();

        currentProximityLevel = 0;
        inDanger = false;
        isDead = false;

        if (myCellTemplate.generateProximity)
        {
            for (int i = 0; i < myCellTemplate.proximityColliders.Length; i++)
            {
                // Debug.Log(myCellTemplate.proximityColliders[i].proximityLevel + " "+ myCellTemplate.proximityColliders[i].range);
            }
        }
        if (myCellTemplate.limitedInLinks)
        {
            limitedInLink = true;
            GenerateLinkSlot();
        }
        RessourceTracker.instance.EnergyCapVariation(currentEnergyCap);
        ProximityCheck();
        ProximityLevelModification();
    }

    public int GetCurrentRange()
    {
        return currentRange / 2;
    }
    #endregion

    private void OnBecameInvisible()
    {
        isVisible = false;
    }
    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void GenerateLinkSlot()
    {
        int currentSlot = 0;
        float yOffset = 0.5f;
        if (myCellTemplate.numberOfOuputLinks < 4)
        {
            for (int i = 0; i < myCellTemplate.numberOfOuputLinks; i++)
            {
                float anglefrac = 2 * Mathf.PI / 8;

                //calcule de l'angle en foncttion du nombre de point
                float angle = anglefrac * currentSlot;
                Vector3 dir = new Vector3(Mathf.Sin(angle), yOffset, Mathf.Cos(angle));
                Vector3 pos = dir * myCellTemplate.slotDistance;

                LinkJointClass newSlot = ObjectPooler.poolingSystem.GetPooledObject<LinkJointClass>() as LinkJointClass;
                newSlot.Init(true);
                newSlot.transform.parent = this.transform;
                newSlot.transform.localPosition = pos;
                currentSlot++;
            }
        }
        if (myCellTemplate.numberOfInputLinks < 4)
        {
            for (int i = 0; i < myCellTemplate.numberOfInputLinks; i++)
            {

                float anglefrac = 2 * Mathf.PI / 8;

                //calcule de l'angle en foncttion du nombre de point
                float angle = anglefrac * currentSlot;
                Vector3 dir = new Vector3(Mathf.Sin(angle), yOffset, Mathf.Cos(angle));
                Vector3 pos = dir * myCellTemplate.slotDistance;

                LinkJointClass newSlot = ObjectPooler.poolingSystem.GetPooledObject<LinkJointClass>() as LinkJointClass;
                newSlot.Init(false);
                newSlot.transform.parent = this.transform;
                newSlot.transform.localPosition = pos;
                currentSlot++;
            }
        }

    }
}
