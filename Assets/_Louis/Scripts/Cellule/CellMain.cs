using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

//[RequireComponent(typeof(MeshCollider))]  //typeof(MeshRenderer), typeof(MeshFilter),
public class CellMain : PoolableObjects
{
    #region Variables
    [Tooltip("glissé l'un des srcyptable object structure ici")]
    public CelluleTemplate myCellTemplate;


    // public List<CelulleMain> outputCell;
    [Header("REF")]
    public TextMeshPro NBlob;
    public TextMeshPro NLink;

    //public MeshFilter mF;
    //public MeshRenderer mR;
    public MeshCollider mC;

    public CellProximityDectection ProximityDectection;


    [Header("Debug")]
    public List<LinkClass> links = new List<LinkClass>();
    public bool noMoreLink;
    public int BlobNumber;
    public bool hasBeenDrop;

    //Important for the communication into 
    protected List<LinkClass> outputLinks = new List<LinkClass>();
    protected List<CellMain> cellAtProximity = new List<CellMain>();
    protected int currentLinkStockage;
    protected int currentBlobStockage;
    protected int currentProximityLevel;

    // protected MeshCollider mC;
    protected int currentIndex;
    protected bool isDead = false;


    protected float velocity;
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
        TickInscription();
        //UI init 
        NBlob.text = (BlobNumber + " / " + myCellTemplate.storageCapability);
        NLink.text = (links.Count + " / " + myCellTemplate.linkCapability);
        isDead = false;
        currentBlobStockage = myCellTemplate.storageCapability;
        currentLinkStockage = myCellTemplate.linkCapability;

        ProximityCheck();
    }


    public virtual void Died()
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


        for (int i = 0; i < BlobNumber; i++)
        {
            //Debug.Log("SI TU VOIS ÇA C'EST QUE LES BLOB SONT ENCORE INSTANCIE EN SALE AINSI QUE LEUR RIGIDBODY ");
            //GameObject blob = Instantiate(myCellTemplate.blopPrefab, transform.position, Quaternion.identity);
            //Rigidbody rb = blob.GetComponent<Rigidbody>();
            //Vector3 dir = new Vector3(Random.Range(-1, 1), Random.Range(0.1f, 1f), Random.Range(-1, 1)) * myCellTemplate.impulseForce_Death;
            //rb.AddForce(dir, ForceMode.Impulse);
            //blob.GetComponent<Blob>().blobType = BlobBehaviour.BlobType.mad;
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

        Inpool();
    }
    public virtual void BlobsTick()
    {
        //AddBlob(myCellTemplate.prodPerTick);

        //ça marche bien mais à voir si quand 1 batiment meure la produciton saute avec ou pas
        for (int i = 0; i < myCellTemplate.rejectPower_RF; i++)
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
        AddBlob(myCellTemplate.prodPerTick);


    }

    public virtual void AddBlob(int Amount)
    {
        BlobNumber += Amount;
        NBlob.text = (BlobNumber + " / " + currentBlobStockage);
        if (BlobNumber > currentBlobStockage && !isDead)
        {
            Died();
        }
        UpdateCaract();
    }
    public virtual void RemoveBlob(int Amount)
    {
        BlobNumber -= Amount;
        //UI update
        UpdateCaract();
    }

    public virtual void StockageCapabilityVariation(int Amount)
    {
        currentBlobStockage += Amount;
        UpdateCaract();
        if (BlobNumber > currentBlobStockage && !isDead)
        {
            Died();
        }
        UpdateCaract();
    }

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
            linkToAdd.receveingCell = this;
        }

        if (links.Count >= myCellTemplate.linkCapability)
        {
            noMoreLink = true;
        }
        UpdateCaract();
    }
    public virtual void RemoveLink(LinkClass linkToRemove)
    {
        if (links.Count < myCellTemplate.linkCapability)
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

    public virtual void UpdateCaract()
    {
        NBlob.text = (BlobNumber + " / " + currentBlobStockage);
        NLink.text = (links.Count + " / " + currentLinkStockage);
    }
    public virtual void ClickInteraction()
    {
        if (BlobNumber > 0)
        {
            RemoveBlob(1);
            CellManager.Instance.EnergyVariation(10);
        }

    }

    public virtual void ProximityCheck()
    {
       // ProximityDectection.myCollider.radius = Mathf.SmoothDamp(0, myCellTemplate.range / 2, ref velocity, 0.01f);
        ProximityDectection.myCollider.radius =  myCellTemplate.range / 2;
    }
    public virtual void AddToCellAtPromity(CellMain cellDetected)
    {
        cellAtProximity.Add(cellDetected);

        for (int i = 0; i < myCellTemplate.negativesInteractions.Length; i++)
        {
            CellType cellDetectedType = cellDetected.myCellTemplate.type;
            if (cellDetectedType == myCellTemplate.negativesInteractions[i])
            {
                ProximityLevelModification(1);
                // Ajouter L'UI 
            }

            else
            {
                for (int j = 0; j < myCellTemplate.positivesInteractions.Length; j++)
                {
                    if (cellDetectedType == myCellTemplate.positivesInteractions[i])
                    {
                        ProximityLevelModification(-1);
                    }
                    else
                    {
                        Debug.Log("Pas d'interaction entre ces 2 cellules");
                    }
                }
            }

        }

    } 
    public virtual void ProximityLevelModification(int Amout)
    {
        if (Mathf.Abs(currentProximityLevel) <= myCellTemplate.proximityLevelMax)
        {
            currentProximityLevel += Amout;
        }
    }

    public virtual void TickInscription()
    {
        TickManager.doTick += BlobsTick;
    }
    public virtual void TickDesinscription()
    {
        TickManager.doTick -= BlobsTick;
    }
}
