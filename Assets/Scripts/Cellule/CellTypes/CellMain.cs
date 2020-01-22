using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using UnityEngine.EventSystems;

//[RequireComponent(typeof(MeshCollider))]  //typeof(MeshRenderer), typeof(MeshFilter),
public class CellMain : PoolableObjects, PlayerAction
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
    public ProgressBar stockageBar;
    public Transform graphTransform;
    public Transform TargetPos;
    private CellAlert alert;




    public GameObject coachIcon;
    public GameObject exploIcon;


    public MeshRenderer domeMR, spriteMR;
    private Material domeInitialMat, spriteInitialMat;

    public List<LinkClass> links = new List<LinkClass>();
    protected List<LinkClass> outputLinks = new List<LinkClass>();

    public List<CellProximityDectection> inThoseCellProximity = new List<CellProximityDectection>();
    public List<CellProximityDectection> influencedByThoseCellProximity = new List<CellProximityDectection>();

    public List<Blob> stuckBlobs = new List<Blob>();

    protected CellProximityDectection[] myProximityCollider;
    public Collider ownCollider;

    public LinkJointClass[] linkJoints;

    public List<BlobCoach> blobCoaches = new List<BlobCoach>();

    public Transform myTransform;
    #endregion

    #region DEBUG
    public bool showDebug;

    public bool showlinks;
    public bool showRef;
    public bool noMoreLink;

    public int blobNumber;
    public int normalBlobNumber;
    public int coachBlobNumber;
    public int explorateurBlobNumber;
    public bool hasBlobCoach;

    public bool hasBeenDrop;
    public bool limitedInLink;

    protected int currentIndex;

    protected int blolbNumberAtOverload;

    #endregion

    #region STATS
    //Important for the communication into 

    protected int currentProximityLevel;
    protected int currentProximityTier;

    //  protected int currentLinkStockage;
    protected int currentBlobStockage;

    protected int currentTickForActivation;
    protected float currentTick;

    protected int currentEnergyPerClick;
    protected int currentEnergyCap;

    protected int currentSurproductionRate;
    protected float currentRejectPower;
    protected int currentRange;
    public int specifiqueStats;



    #endregion

    #region Etats/Boolean

    protected bool inDanger;
    protected bool isDead = false;
    protected bool alertDisplayed = false;


    protected bool overLoad = false;
    protected int overloadStack;

    protected bool actionmade = false;

    public bool canBePlaced;
    public bool isVisible;
    protected bool hasExplo;
    #endregion

    #region Anim Variable   
    protected bool haveExpulse;
    #endregion

    #endregion

    public virtual void Awake()
    {
        GetInitialMat();
        if (ownCollider == null)
        {
            ownCollider = GetComponent<Collider>();
        }
        if (myTransform == null)
        {
            myTransform = transform;
        }

        ownCollider.enabled = false;
    }

    public virtual void OnEnable()
    {
        //Pour le delegate qui gére le tick
        //TickManager.doTick += BlobsTick;
        //UI init 
        NBlob.text = (blobNumber + " / " + myCellTemplate.storageCapability);
        currentBlobStockage = myCellTemplate.storageCapability;

        //Ancien Lien
        //NLink.text = (links.Count + " / " + myCellTemplate.linkCapability);
        //currentLinkStockage = myCellTemplate.linkCapability;

        hasBeenDrop = false;



        if (isNexus)
        {
            StartCoroutine(WaitForInit());
        }

    }

    private void Start()
    {
        GraphSetup();
    }

    public virtual void CellInitialisation()
    {
        if (ownCollider == null)
        {
            ownCollider = GetComponent<Collider>();
        }
        RessourceTracker.instance.AddCell(this);
        ownCollider.enabled = true;
        TickInscription();
        isDead = false;

        SetupVariable();
        RestoreInitialMat();
    }

    public virtual void BlobsTick()
    {
        if (myCellTemplate.StatsModification == StatsModificationType.StockageCapacity)
        {
            if (stuckBlobs.Count <= 0 && currentBlobStockage < myCellTemplate.stockageCapacity[currentProximityTier])
            {
                StockageCapabilityVariation(1);
            }
        }
        else
        {
            if (stuckBlobs.Count <= 0 && currentBlobStockage < myCellTemplate.storageCapability)
            {
                StockageCapabilityVariation(1);
            }
        }


        if (!overLoad)
        {
            overloadStack = 0;
            if (myCellTemplate.prodPerTickBase > 0)
            {
                BlobNumberVariation(myCellTemplate.prodPerTickBase, BlobManager.BlobType.normal, true);
            }

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
                if (currentTick >= currentTickForActivation)
                {
                    for (int i = 0; i < outputLinks.Count; i++)
                    {
                        if (blobNumber > 0 && outputLinks.Count > 0)
                        {
                            if (currentIndex < outputLinks.Count)
                            {
                                BlobManager.BlobType blobType = BlobCheck();

                                if (blobType != BlobManager.BlobType.aucun)
                                {
                                    outputLinks[currentIndex].Transmitt(1, BlobCheck());
                                    haveExpulse = true;
                                    currentIndex++;
                                    currentIndex = Helper.LoopIndex(currentIndex, outputLinks.Count);

                                }
                            }
                            else
                            {
                                currentIndex = 0;
                            }
                        }
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
        else
        {
            if (!LevelManager.instance.cellInvicible)
            {
                overloadStack++;
                if (overloadStack >= myCellTemplate.overLoadTickMax)
                {
                    Died(false);
                }
            }
        }
    }

    public virtual void Died(bool intentionnalDeath)
    {

        isDead = true;
        //TickManager.doTick -= BlobsTick;
        //ownCollider.enabled = false;

        RessourceTracker.instance.RemoveBlob(BlobManager.BlobType.normal, blobNumber);
        RessourceTracker.instance.EnergyCapVariation(-currentEnergyCap);


        //if (CellManager.Instance.originalPosOfMovingCell != new Vector3(0, 100, 0))
        RessourceTracker.instance.RemoveCell(this);

        if (CellManager.Instance.selectedCell == this)
        {
            UIManager.Instance.DesactivateCellShop();
        }
        if (InputManager.Instance.selectedElement == this as PlayerAction)
        {
            InputManager.Instance.DeselectElement();
        }

        TickDesinscription();

        int B = stuckBlobs.Count;
        for (int y = 0; y < B; y++)
        {
            stuckBlobs[0].Unstuck();
        }
        //  stuckBlobs.Clear();

        int I = links.Count;
        for (int i = 0; i < I; i++)
        {
            if (i > I)
            {
                break;
            }
            links[0].Break();
        }

        for (int i = 0; i < linkJoints.Length; i++)
        {
            linkJoints[i].Inpool();
            linkJoints[i] = null;

        }
        #region SpawnBlobAtDeath
        int blobAmount = (int)Mathf.Ceil((float)blolbNumberAtOverload / (float)10);

        if (!intentionnalDeath)
        {
            blobAmount += (int)Mathf.Floor((float)blobAmount / (float)2);
        }
        //Spawn les blobs
        for (int i = 0; i < blobAmount; i++)
        {
            Blob spawnBlob = ObjectPooler.poolingSystem.GetPooledObject<Blob>() as Blob;
            spawnBlob.transform.position = transform.position + new Vector3(Random.Range(-2, 2), Random.Range(0, 3), Random.Range(-2, 2));
            spawnBlob.Outpool();
            spawnBlob.ChangeType(BlobManager.BlobType.mad);
            spawnBlob.RandomJump();
        }
        #endregion
        blobCoaches.Clear();

        if (this == CellManager.Instance.selectedCell)
        {
            InputManager.Instance.StopCurrentAction();
        }

        if (myCellTemplate.generateProximity)
        {
            //Met dans la pull les enfants
            for (int i = 0; i < myProximityCollider.Length; i++)
            {
                myProximityCollider[i].Inpool();
            }
        }


        inThoseCellProximity.Clear();
        influencedByThoseCellProximity.Clear();

        blobNumber = 0;
        //SetupVariable();
        if (initialPool == null)
        {
            transform.position += new Vector3(0, -200, 0);
            Destroy(gameObject);
        }
        else
        {
            Inpool();
        }
    }

    #region BLOB_GESTION

    public virtual void StockageCapabilityVariation(int Amount)
    {
        currentBlobStockage += Amount;


        if (currentBlobStockage <= 0)
        {
            ToggleOverload(true);
            return;
        }

        if (blobNumber > currentBlobStockage && !isDead && !isNexus)
        {
            ToggleOverload(true);
            return;
        }

        if (currentBlobStockage < 0)
        {
            currentBlobStockage = 0;
            Died(false);
        }

        if (!overLoad)
        {
            float ratio = 0;
            if (myCellTemplate.StatsModification == StatsModificationType.StockageCapacity)
            {
                ratio = (float)blobNumber / (float)myCellTemplate.stockageCapacity[currentProximityTier];
            }
            else
            {
                ratio = (float)blobNumber / (float)myCellTemplate.storageCapability;
            }
            stockageBar.UpdateBar(ratio, true);
        }

        HandleAlerts();
        UpdateCaract();
    }

    public virtual void BlobNumberVariation(int amount, BlobManager.BlobType _blobType, bool transmission)
    {

        blobAddCheckType(amount, _blobType, transmission);

        CheckForCoach(_blobType);

        CheckForExplo(_blobType);


        if (blobNumber > currentBlobStockage && !isDead && !isNexus)
        {
            ToggleOverload(true);
        }

        if (overLoad)
        {
            if (blobNumber < currentBlobStockage - myCellTemplate.overloadTreshHold)
            {
                ToggleOverload(false);
            }
        }

        if (!overLoad)
        {
            float ratio = 0;
            if (myCellTemplate.StatsModification == StatsModificationType.StockageCapacity)
            {
                ratio = (float)blobNumber / (float)myCellTemplate.stockageCapacity[currentProximityTier];
            }
            else
            {
                ratio = (float)blobNumber / (float)myCellTemplate.storageCapability;
            }
            stockageBar.UpdateBar(ratio, true);
        }


        HandleAlerts();

        //Nexus 
        if (isNexus)
        {
            BlobAmountCheck(amount, _blobType);
        }
        UpdateCaract();

    }

    protected void HandleAlerts()
    {
        float ratio = (float)blobNumber / (float)currentBlobStockage;
        int pourcentage = Mathf.FloorToInt(ratio * 100f);

        if (pourcentage >= 80)
        {
            if (!inDanger)
                inDanger = true;
            //ANIM DANGER CELL 
        }
        else
        {
            if (inDanger)
                inDanger = false;
        }

        if (overLoad)
        {
            if (!isVisible && !alertDisplayed)
            {
                Debug.Log("Alert !");
                alert = ObjectPooler.poolingSystem.GetPooledObject<CellAlert>() as CellAlert;
                UIManager.Instance.DisplayCellAlert(this, alert);
                alertDisplayed = true;
            }

            if (isVisible && alertDisplayed)
            {
                UIManager.Instance.HideCellAlert(alert);
                alert = null;
                alertDisplayed = false;
            }
        }
        else
        {
            if (alertDisplayed)
            {
                UIManager.Instance.HideCellAlert(alert);
                alert = null;
                alertDisplayed = false;
            }
        }
    }

    public virtual void BlobAmountCheck(int amount, BlobManager.BlobType _blobType)
    {
        if (blobNumber > currentBlobStockage)
        {

            int blobToRemobe = blobNumber - currentBlobStockage;
            switch (_blobType)
            {
                case BlobManager.BlobType.normal:
                    RessourceTracker.instance.RemoveBlob(BlobManager.BlobType.normal, blobToRemobe);
                    normalBlobNumber -= blobToRemobe;
                    break;
                case BlobManager.BlobType.coach:

                    RessourceTracker.instance.RemoveBlob(BlobManager.BlobType.coach, blobToRemobe);
                    for (int i = 0; i < blobToRemobe; i++)
                    {
                        Debug.Log("Coach");
                        blobCoaches[blobCoaches.Count - 1].Death();
                    }
                    break;
                case BlobManager.BlobType.explorateur:
                    RessourceTracker.instance.RemoveBlob(BlobManager.BlobType.explorateur, blobToRemobe);
                    explorateurBlobNumber -= blobToRemobe;
                    break;
            }
            blobNumber = normalBlobNumber + explorateurBlobNumber + blobCoaches.Count;
        }
    }

    public virtual void CheckForCoach(BlobManager.BlobType _blobType)
    {
        if (_blobType == BlobManager.BlobType.coach)
        {
            if (blobCoaches.Count <= 0)
            {
                // coachBlobNumber = 0;
                hasBlobCoach = false;
                coachIcon.SetActive(false);
                ProximityLevelModification();
            }
            else
            {
                hasBlobCoach = true;
                coachIcon.SetActive(true);
                ProximityLevelModification();
            }
        }
    }

    public virtual void CheckForExplo(BlobManager.BlobType _blobType)
    {
        if (_blobType == BlobManager.BlobType.explorateur)
        {
            if (explorateurBlobNumber <= 0)
            {
                explorateurBlobNumber = 0;
                hasExplo = false;
                exploIcon.SetActive(false);
                ProximityLevelModification();
            }
            else
            {
                hasExplo = true;
                exploIcon.SetActive(true);
                ProximityLevelModification();
            }
        }
    }

    public virtual void blobAddCheckType(int amount, BlobManager.BlobType _blobType, bool transmission)
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

    public virtual BlobManager.BlobType BlobCheck()
    {
        if (explorateurBlobNumber > 0)
        {
            if ((int)Random.Range(1, 101) <= 40)
            {
                return BlobManager.BlobType.explorateur;
            }
        }

        if (blobCoaches.Count > 1)
        {
            if ((int)Random.Range(1, 101) <= 40)
            {
                return BlobManager.BlobType.coach;
            }
        }

        if (normalBlobNumber > 0)
        {
            return BlobManager.BlobType.normal;
        }

        else
        {
            if (explorateurBlobNumber > 0)
            {
                return BlobManager.BlobType.explorateur;
            }
            else if (blobCoaches.Count > 1)
            {
                return BlobManager.BlobType.coach;
            }
            else
            {
                return BlobManager.BlobType.aucun;
            }

        }
    }



    #endregion

    #region PROXIMITE_GESTION

    #region ANCIEN SYSTEM

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

    public void GenerateProximity()
    {
        // ProximityDectection.myCollider.radius = Mathf.SmoothDamp(0, myCellTemplate.range / 2, ref velocity, 0.01f);
        if (myCellTemplate.generateProximity)
        {
            myProximityCollider = new CellProximityDectection[myCellTemplate.proximityColliderNumber];
            for (int i = 0; i < myCellTemplate.proximityColliderNumber; i++)
            {
                CellProximityDectection newProximityCollider = ObjectPooler.poolingSystem.GetPooledObject<CellProximityDectection>() as CellProximityDectection;
                myProximityCollider.SetValue(newProximityCollider, i);

                newProximityCollider.transform.localScale = new Vector3(1, 1, 1) * (myCellTemplate.proximityColliders[i].range / 2);
                newProximityCollider.transform.SetParent(transform);
                newProximityCollider.productionBonusRatio = myCellTemplate.proximityColliders[i].productionBonusRatio;
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
            //if (proximityToAdd.parent == influencedByThoseCellProximity[i].parent && !checkEnd)
            //{
            if (!checkEnd)
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
        //for (int i = 0; i < influencedByThoseCellProximity.Count; i++)
        //{
        //    if (proximityToRemove == influencedByThoseCellProximity[i])
        //    {
        //        influencedByThoseCellProximity.Remove(proximityToRemove);
        //        for (int y = 0; y < inThoseCellProximity.Count; y++)
        //        {
        //            AddProximityInfluence(inThoseCellProximity[y]);
        //        }
        //    }
        //}

        influencedByThoseCellProximity.Remove(proximityToRemove);
        for (int y = 0; y < inThoseCellProximity.Count; y++)
        {
            AddProximityInfluence(inThoseCellProximity[y]);
        }
        ProximityLevelModification();
    }

    public virtual void ProximityLevelModification()
    {
        int LastProximityTier = currentProximityTier;
        int lastEnergyCap = currentEnergyCap;

        if (!hasBlobCoach)
        {
            currentProximityLevel = 0;
            for (int i = 0; i < influencedByThoseCellProximity.Count; i++)
            {
                currentProximityLevel += influencedByThoseCellProximity[i].proximityLevel;
            }

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
        }
        else
        {
            currentProximityTier = myCellTemplate.proximityLevelMax - 1;

        }



        if (myCellTemplate.StatsModification == StatsModificationType.Aucune)
        {
            NCurrentProximity.gameObject.SetActive(false);
        }
        NCurrentProximity.text = currentProximityTier.ToString();

        //if (currentProximityTier != LastProximityTier)
        //{
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

            //case StatsModificationType.LinkCapacity:
            //    currentLinkStockage = myCellTemplate.LinkCapacity[currentProximityTier];
            //    break;

            case StatsModificationType.rangeLien:
                currentRange = myCellTemplate.rangeLien[currentProximityTier];
                break;

            case StatsModificationType.TickForActivation:
                currentTickForActivation = myCellTemplate.tickForActivation[currentProximityTier];
                break;

            case StatsModificationType.EnergyCap:
                currentEnergyCap = myCellTemplate.energyCap[currentProximityTier];
                RessourceTracker.instance.EnergyCapVariation(currentEnergyCap - lastEnergyCap);
                break;
            case StatsModificationType.Spécifique:
                specifiqueStats = myCellTemplate.specifique[currentProximityTier];
                break;

            case StatsModificationType.Aucune:
                break;
                //}
        }
        UpdateCaract();
    }

    public int GetProximityTier()
    {
        return currentProximityTier;
    }
    #endregion

    #region LINK_GESTION
    public virtual void AddLinkReferenceToCell(LinkClass linkToAdd, bool output)
    {
        links.Add(linkToAdd);

        if (output)
        {
            linkToAdd.AngleFromCell(this);
            // linkToAdd.joints[0] = CheckForAvailableJointOfType(linkJointType.output);
            outputLinks.Add(linkToAdd);
            SortingLink();
        }

        //Ancien Link
        //if (links.Count >= myCellTemplate.linkCapability)
        //{
        //    noMoreLink = true;
        //}
        UpdateCaract();
    }
    public virtual void RemoveLink(LinkClass linkToRemove, bool isOutput)
    {
        //if (links.Count < currentLinkStockage)
        //{
        //    noMoreLink = false;
        //}

        if (isOutput)
        {
            jointReset(linkToRemove.joints[0]);
            outputLinks.Remove(linkToRemove);
        }
        else
        {
            jointReset(linkToRemove.joints[1]);

        }

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

    //public override void Inpool()
    //{
    //    if (Application.isEditor)
    //    {
    //        canBePool = true;
    //        gameObject.SetActive(false);
    //    }
    //    else
    //    {

    //        canBePool = true;
    //        transform.position = ObjectPooler.poolingSystem.transform.position;
    //        StartCoroutine(DesactiveGameObject());
    //    }
    //}

    public virtual void UpdateCaract()
    {
        NBlob.text = (blobNumber + " / " + currentBlobStockage);
        //NLink.text = (links.Count + " / " + currentLinkStockage);
    }
    public virtual void GraphSetup()
    {
        Vector3 graphPos = transform.position + new Vector3(0, 0.1f, 0);
        graphTransform.position = graphPos;
    }
    public virtual void SetupVariable()
    {
        // currentLinkStockage = myCellTemplate.linkCapability;
        currentBlobStockage = myCellTemplate.storageCapability;
        // currentSurproductionRate = myCellTemplate.SurproductionRate[0];
        currentRejectPower = myCellTemplate.rejectPowerBase;
        currentRange = myCellTemplate.rangeBase;
        currentTickForActivation = myCellTemplate.tickForActivationBase;

        currentEnergyPerClick = myCellTemplate.energyPerClick;
        currentEnergyCap = myCellTemplate.energyCapBase;

        //cellAtProximity.Clear();
        normalBlobNumber = 0;
        //coachBlobNumber = 0;


        explorateurBlobNumber = 0;
        hasBlobCoach = false;


        currentProximityLevel = 0;
        inDanger = false;
        isDead = false;
        blolbNumberAtOverload = 0;


        ToggleOverload(false);

        overloadStack = 0;


        RessourceTracker.instance.EnergyCapVariation(currentEnergyCap);
        GenerateProximity();
        ProximityLevelModification();
    }
    public int GetCurrentRange()
    {
        return currentRange / 2;
    }
    #endregion

    #region SLOT 

    public LinkJointClass CheckForAvailableJointOfType(linkJointType checkType)
    {
        for (int i = 0; i < linkJoints.Length; i++)
        {

            if (linkJoints[i].typeOfJoint == checkType && linkJoints[i].disponible)
            {
                return linkJoints[i];
            }

            if (linkJoints[i].typeOfJoint == linkJointType.flex && linkJoints[i].disponible)
            {
                return linkJoints[i];
            }
        }
        return null;
    }

    public void GenerateLinkSlot()
    {
        int currentSlot = 0;
        float yOffset = 0.1f;
        int maxJoint = myCellTemplate.numberOfFlexLinks;
        linkJoints = new LinkJointClass[maxJoint];

        if (myCellTemplate.limitedInLinks)
        {
            maxJoint += myCellTemplate.numberOfOuputLinks + myCellTemplate.numberOfInputLinks;
            linkJoints = new LinkJointClass[maxJoint];


            for (int i = 0; i < myCellTemplate.numberOfOuputLinks; i++)
            {
                currentSlot = SlotGeneration(currentSlot, yOffset, maxJoint, linkJointType.output);
            }

            for (int y = 0; y < myCellTemplate.numberOfInputLinks; y++)
            {
                currentSlot = SlotGeneration(currentSlot, yOffset, maxJoint, linkJointType.input);
            }
        }

        for (int x = 0; x < myCellTemplate.numberOfFlexLinks; x++)
        {
            currentSlot = SlotGeneration(currentSlot, yOffset, maxJoint, linkJointType.flex);
        }
    }

    private int SlotGeneration(int currentSlot, float yOffset, int maxJoint, linkJointType _type)
    {
        float anglefrac = 2 * Mathf.PI / (maxJoint);

        //calcule de l'angle en foncttion du nombre de point
        float angle = anglefrac * currentSlot;
        Vector3 dir = new Vector3(Mathf.Sin(angle), 0, Mathf.Cos(angle));
        Vector3 pos = dir * myCellTemplate.slotDistance + graphTransform.transform.position;


        LinkJointClass newSlot = ObjectPooler.poolingSystem.GetPooledObject<LinkJointClass>() as LinkJointClass;
        linkJoints[currentSlot] = newSlot;

        newSlot.Outpool();
        newSlot.transform.parent = this.transform;
        newSlot.transform.position = pos;
        newSlot.transform.localRotation = Quaternion.Euler(90, 0, 0);
        newSlot.Init(_type);
        currentSlot++;
        return currentSlot;
    }

    public void jointReset(LinkJointClass joint)
    {
        int flexDetected = 0;
        for (int i = 0; i < linkJoints.Length; i++)
        {
            if (linkJoints[i].typeOfJoint == linkJointType.flex)
            {
                flexDetected++;
            }
        }
        if (flexDetected < myCellTemplate.numberOfFlexLinks)
        {
            joint.typeOfJoint = linkJointType.flex;
            joint.GraphUpdate();
        }
        joint.disponible = true;
    }

    #endregion

    #region Graph MODIFICATION 
    private void GetInitialMat()
    {
        domeInitialMat = domeMR.material;
        spriteInitialMat = spriteMR.material;

    }
    public void RestoreInitialMat()
    {
        domeMR.material = domeInitialMat;
        spriteMR.material = spriteInitialMat;
    }
    public void ChangeDeplacementMat(bool canBePlaced)
    {
        if (canBePlaced)
        {
            domeMR.material = CellManager.Instance.allowedBuildingMat;
            spriteMR.material = CellManager.Instance.allowedBuildingSpriteMat;
        }
        else
        {
            domeMR.material = CellManager.Instance.refusedBuildingMat;
            spriteMR.material = CellManager.Instance.refusedBuldingSpriteMask;
        }
    }
    #endregion

    #region PLAYER ACTION INTERFACE

    public virtual void OnLeftClickDown(RaycastHit hit)
    {
        InputManager.Instance.SelectCell();
    }

    //Interaction 
    public virtual void OnShortLeftClickUp(RaycastHit hit)
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

        //else if (blobNumber > 0)
        //{
        //    BlobNumberVariation(-1, BlobCheck(), false);
        //    //CellManager.Instance.EnergyVariation(currentEnergyPerClick);
        //    RessourceTracker.instance.EnergyVariation(currentEnergyPerClick);
        //    actionMade = true;
        //}

        if (actionmade)
        {
            anim.Play("PlayerInteraction", 0, 0f);
        }
    }


    public virtual void OnLeftClickHolding(RaycastHit hit)
    {
        if (CheckForAvailableJointOfType(linkJointType.output) == null)
        {

            Debug.Log("This cell is full , alerte message");
            return;

        }
        UIManager.Instance.DisplayCellShop(InputManager.Instance.selectedCell);
    }
    public virtual void OnLongLeftClickUp(RaycastHit hit)
    {
        UIManager.Instance.DesactivateCellShop();
    }


    public virtual void OnDragStart(RaycastHit hit)
    {
        UIManager.Instance.DesactivateCellShop();

        if (CellManager.Instance.CreatenewLink())
            CellManager.Instance.newCell = false;
    }
    public virtual void OnLeftDrag(RaycastHit hit)
    {
        CellManager.Instance.DragNewlink(hit);
    }
    public virtual void OnDragEnd(RaycastHit hit)
    {
        CellManager.Instance.ValidateNewLink(hit);
    }


    public virtual void OnShortRightClick(RaycastHit hit)
    {
        UIManager.Instance.DisplayCellOptions(this);
    }
    public virtual void OnRightClickWhileHolding(RaycastHit hit)
    {
        UIManager.Instance.DesactivateCellShop();
    }
    public virtual void OnRightClickWhileDragging(RaycastHit hit)
    {
        CellManager.Instance.SupressCurrentLink();
        CellManager.Instance.DeselectElement();
    }

    public virtual void OnmouseIn(RaycastHit hit)
    {
        UIManager.Instance.LoadToolTip(transform.position, this, false, true);
    }
    public virtual void OnMouseOut(RaycastHit hit)
    {
        UIManager.Instance.UnloadToolTip();
    }

    public virtual void OnSelect()
    {
        CellManager.Instance.selectedCell = this;
    }

    public virtual void OnDeselect()
    {
        UIManager.Instance.DeselectElement();
        CellManager.Instance.selectedCell = null;
    }

    public virtual void StopAction()
    {
        UIManager.Instance.DesactivateCellShop();
        UIManager.Instance.HideCellOptions();

        if (InputManager.Instance.dragging)
        {
            CellManager.Instance.SupressCurrentLink();
        }

    }
    #endregion

    #region Collider Proximity

    private void OnTriggerEnter(Collider other)
    {
        CellProximityDectection collider = other.GetComponent<CellProximityDectection>();
        if (collider != null && collider.parent != this)
        {
            for (int i = 0; i < inThoseCellProximity.Count; i++)
            {
                if (inThoseCellProximity[i] == collider)
                {
                    return;
                }
            }
            inThoseCellProximity.Add(collider);
            //parent.AddToCellAtPromity(cell);
            AddProximityInfluence(collider);
            //OVERRIDE POSSIBLE 
            if (collider.parent.myCellTemplate.type == CellType.Nexus)
            {
                CellProductrice prod = collider.parent as CellProductrice;
                prod.ProductriceProximityGestion(collider, true);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CellProximityDectection collider = other.GetComponent<CellProximityDectection>();
        if (collider != null && collider.parent != this)
        {
            RemoveProximityInfluence(collider);
            //OVERRIDE POSSIBLE 
            if (collider.parent.myCellTemplate.type == CellType.Nexus)
            {
                CellProductrice prod = collider.parent as CellProductrice;
                prod.ProductriceProximityGestion(collider, false);
            }
        }

    }
    #endregion

    #region OVERLOAD

    protected void ToggleOverload(bool isOverload)
    {
        overLoad = isOverload;
        if (stockageBar != null)
        {
            stockageBar.ToggleRenderer(isOverload);
        }
        //FX.setActive()
        if (isOverload)
        {
            Debug.Log("enterInOverload ", gameObject);
            blolbNumberAtOverload = blobNumber;
        }
    }

    #endregion

    IEnumerator WaitForInit()
    {
        yield return new WaitForEndOfFrame();
        CellInitialisation();
        GenerateLinkSlot();
    }
}
