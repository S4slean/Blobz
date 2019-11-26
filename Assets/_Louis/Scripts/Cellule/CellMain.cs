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


    // public List<CelulleMain> outputCell;
    #region REFS
    public TextMeshPro NBlob;
    public TextMeshPro NLink;
    public TextMeshPro NCurrentProximity;
    public Transform graphTransform;

    public Transform TargetPos;

    //public MeshFilter mF;
    //public MeshRenderer mR;
    // public MeshCollider mC;

    public CellProximityDectection ProximityDectection;
    #endregion


    #region DEBUG
    public bool showDebug;
    public bool showlinks;
    public bool showRef;
    public List<LinkClass> links = new List<LinkClass>();
    public bool noMoreLink;
    public int BlobNumber;
    public bool hasBeenDrop;
    #endregion

    #region STATS
    //Important for the communication into 
    protected List<LinkClass> outputLinks = new List<LinkClass>();
    protected List<CellMain> cellAtProximity = new List<CellMain>();
    protected int currentIndex;

    protected int currentProximityLevel;
    protected int currentProximityTier;

    protected int currentLinkStockage;
    protected int currentBlobStockage;

    protected int currentSurproductionRate;
    protected float currentRejectPower;
    protected int currentRange;

    protected bool isDead = false;
    protected float velocity;
    #endregion

    #endregion

    public virtual void Awake()
    {
        ProximityDectection.parent = this;
        //mR.material = myCellTemplate.mat;
        //mF.mesh = myCellTemplate.mesh;
        //ProximityCheck();

    }

    public virtual void OnEnable()
    {
        //Pour le delegate qui gére le tick
        //TickManager.doTick += BlobsTick;
        //UI init 
        NBlob.text = (BlobNumber + " / " + myCellTemplate.storageCapability);
        NLink.text = (links.Count + " / " + myCellTemplate.linkCapability);
        currentBlobStockage = myCellTemplate.storageCapability;
        currentLinkStockage = myCellTemplate.linkCapability;

        CellInitialisation();

    }

    private void CellInitialisation()
    {
        TickInscription();
        isDead = false;
        cellAtProximity.Clear();
        currentProximityLevel = 0;
        ProximityCheck();
        ProximityLevelModification(0);
    }

    private void Start()
    {
        GraphSetup();
    }


    public virtual void Died(bool intentionnalDeath)
    {
        isDead = true;
        //TickManager.doTick -= BlobsTick;
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
            for (int i = 0; i < BlobNumber; i++)
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

        }
        BlobNumber = 0;
        SetupVariable();
        Inpool();
    }
    public virtual void BlobsTick()
    {
        //AddBlob(myCellTemplate.prodPerTick);

        //ça marche bien mais à voir si quand 1 batiment meure la produciton saute avec ou pas
        for (int i = 0; i < currentRejectPower; i++)
        {
            if (BlobNumber > 0 && outputLinks.Count > 0)
            {
                if (currentIndex >= outputLinks.Count)
                {
                    return;
                }
                outputLinks[currentIndex].Transmitt();
                currentIndex++;
                currentIndex = Helper.LoopIndex(currentIndex, outputLinks.Count);
            }
        }
        AddBlob(myCellTemplate.prodPerTickBase);


    }
    public virtual void StockageCapabilityVariation(int Amount)
    {
        currentBlobStockage += Amount;
        UpdateCaract();
        if (BlobNumber > currentBlobStockage && !isDead)
        {
            Died(false);
        }
        UpdateCaract();
    }
    public virtual void ClickInteraction()
    {
        if (BlobNumber > 0)
        {
            RemoveBlob(1);
            CellManager.Instance.EnergyVariation(10);
        }

    }


    #region BLOB_GESTION
    public void AddBlob(int Amount)
    {
        BlobNumber += Amount;
        NBlob.text = (BlobNumber + " / " + currentBlobStockage);
        if (BlobNumber > currentBlobStockage && !isDead)
        {
            Died(false);
        }
        UpdateCaract();
    }
    public void RemoveBlob(int Amount)
    {
        BlobNumber -= Amount;
        //UI update
        UpdateCaract();
    }
    #endregion

    #region PROXIMITE_GESTION
    public void ProximityCheck()
    {
        // ProximityDectection.myCollider.radius = Mathf.SmoothDamp(0, myCellTemplate.range / 2, ref velocity, 0.01f);
        ProximityDectection.myCollider.radius = myCellTemplate.rangeBase / 2;
    }
    public void AddToCellAtPromity(CellMain cellDetected)
    {
        bool endFunction = false;
        cellAtProximity.Add(cellDetected);
        CellType cellDetectedType = cellDetected.myCellTemplate.type;
        for (int i = 0; i < myCellTemplate.negativesInteractions.Length; i++)
        {
            if (cellDetectedType == myCellTemplate.negativesInteractions[i])
            {
                ProximityLevelModification(-1);
                endFunction = true;
                break;
            }
        }
        if (endFunction)
        {
            return;
        }

        for (int j = 0; j < myCellTemplate.positivesInteractions.Length; j++)
        {
            if (cellDetectedType == myCellTemplate.positivesInteractions[j])
            {
                ProximityLevelModification(1);
                break;
            }
        }
    }
    public void RemoveToCellAtPromity(CellMain cellDetected)
    {
        cellAtProximity.Remove(cellDetected);
        CellType cellDetectedType = cellDetected.myCellTemplate.type;

        for (int i = 0; i < myCellTemplate.negativesInteractions.Length; i++)
        {
            if (cellDetectedType == myCellTemplate.negativesInteractions[i])
            {
                ProximityLevelModification(+1);
                // Ajouter L'UI 
            }

        }
        for (int j = 0; j < myCellTemplate.positivesInteractions.Length; j++)
        {
            if (cellDetectedType == myCellTemplate.positivesInteractions[j])
            {
                ProximityLevelModification(-1);
            }
        }
    }
    public virtual void ProximityLevelModification(int Amout)
    {
        currentProximityLevel += Amout;
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

            case StatsModificationType.Aucune:
                break;
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
        NBlob.text = (BlobNumber + " / " + currentBlobStockage);
        NLink.text = (links.Count + " / " + currentLinkStockage);
    }

    public virtual void GraphSetup()
    {
        Vector3 graphPos = transform.position + new Vector3(0, 0, 0);
        graphTransform.position = graphPos;
    }

    private void SetupVariable()
    {
        currentLinkStockage = myCellTemplate.linkCapability;
        currentBlobStockage = myCellTemplate.storageCapability;
        currentSurproductionRate = myCellTemplate.SurproductionRate[0];
        currentRejectPower = myCellTemplate.rejectPowerBase;
        currentRange = myCellTemplate.rangeBase;
        currentProximityLevel = 0;
        cellAtProximity.Clear();

        ProximityCheck();
        ProximityLevelModification(0);
    }
    #endregion

}
