using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : PoolableObjects
{

    #region REFS
    public Rigidbody rb;
    public Renderer rd;
    public Animator anim;
    public GameObject sword;
    #endregion

    [Header("General")]
    #region GENERAL
    public int tickCount = 0;
    public int lifeTime = 0;
    public int LifeSpan = 32;
    [SerializeField] private BlobManager.BlobType blobType = BlobManager.BlobType.normal;
    private float jumpForce = 5;
    public Transform carriableSocket;
    private Vector3 flagPos;
    [HideInInspector] public Transform originCell;
    #endregion

    #region EXPLO
    [HideInInspector] public Transform resourceTransform;
    [HideInInspector] public Carriable carriedObject;
    #endregion

    [Header("Enemy")]
    #region ENNEMIES
    public Transform tagetTransform;
    public bool isStuck = false;
    public CellMain infectedCell;
    public int infectionAmount = 1;
    public bool knowsNexus = false;
    public bool cameFromVillage = false;
    public EnemyVillage village;
    #endregion

    [Header("Soldier")]
    #region SOLDIER
    public float flyTime = 3;
    public float flySpeed = 5;
    public bool canExplode = false;
    #endregion



    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        // rd = GetComponent<Renderer>();
        damaged = false;

        UpdateMat();
        //rajoute le blob à la liste des blobs actifs dans la scène

        anim.Play("Appear");
        RessourceTracker.instance.AddBlob(this);
        BlobManager.blobList.Add(this);
    }

    public void AssignFlagPos(Vector3 newFlagPos)
    {
        flagPos = newFlagPos;
    }

    public bool CheckIfInFlagRadius()
    {


        if (blobType == BlobManager.BlobType.soldier)
        {
            return Vector3.SqrMagnitude(transform.position - flagPos) < Mathf.Pow(BlobManager.instance.soldierFlagRadius, 2);
        }
        else if (blobType == BlobManager.BlobType.explorateur)
        {
            return Vector3.SqrMagnitude(transform.position - flagPos) < Mathf.Pow(BlobManager.instance.exploFlagRadius, 2);
        }
        else
        {
            Debug.Log($"this Blob {blobType} isn't supposed to check for flag");
            return false;
        }



    }

    public void ChangeType(BlobManager.BlobType newType)
    {
        if (blobType == BlobManager.BlobType.mad)
        {
            tag = "Untagged";
        }

        switch (newType)
        {
            case BlobManager.BlobType.soldier:
                sword.SetActive(true);
                jumpForce = BlobManager.instance.soldierJumpForce;
                break;

            case BlobManager.BlobType.mad:
                sword.SetActive(false);
                jumpForce = BlobManager.instance.enemyJumpForce;
                tag = "Enemies";

                break;

            case BlobManager.BlobType.explorateur:
                sword.SetActive(false);
                jumpForce = BlobManager.instance.exploJumpForce;

                break;
        }




        RessourceTracker.instance.RemoveBlob(this);
        blobType = newType;
        UpdateMat();
        RessourceTracker.instance.AddBlob(this);

    }

    public BlobManager.BlobType GetBlobType()
    {
        return blobType;
    }

    public void UpdateMat()
    {
        switch (blobType)
        {
            case BlobManager.BlobType.explorateur:

                rd.material = BlobManager.instance.exploMat;
                break;

            case BlobManager.BlobType.normal:

                rd.material = BlobManager.instance.normalMat;
                break;

            case BlobManager.BlobType.mad:

                rd.material = BlobManager.instance.angryMat;
                break;

            case BlobManager.BlobType.soldier:

                rd.material = BlobManager.instance.soldierMat;
                break;
        }
    }

    //private void Update()
    //{
    //    if (blobType == BlobManager.BlobType.soldier && flyTime > 0)
    //        Fly();
    //}
    public void GetBackInPool()
    {
        if (infectedCell != null)
        {
            infectedCell.StockageCapabilityVariation(infectionAmount);
            infectedCell.stuckBlobs.Remove(this);
        }

        //retire le blob de la liste des blobs actifs dans la scène
        BlobManager.blobList.Remove(this);

        if (cameFromVillage)
        {
            village.RemoveBlobFromVillageList(this);
        }

        //play death Anim
        tickCount = 0;
        lifeTime = 10;
        blobType = BlobManager.BlobType.normal;
        tagetTransform = null;

        infectedCell = null;
        isStuck = false;
        infectionAmount = 0;

        knowsNexus = false;
        cameFromVillage = false;
        village = null;

        Inpool();

    }

    private bool damaged = false;

    public void ReceiveDamage()
    {
        //if (damged)
        //    return;

        if (!damaged)
        {
            RessourceTracker.instance.AddKill();
        }
        damaged = true;
        Destruct();

    }




    public void Destruct()
    {
        if (carriedObject != null)
        {
            carriedObject.GetDropped();
            carriedObject = null;
        }

        anim.Play("Death");
        //event --> GetBackInPool();
    }

    #region DEPLACEMENT
    public void JumpTowards(Transform target)
    {
        transform.LookAt(target);
        JumpForward();
    }

    public void JumpTowards(Vector3 destination)
    {
        transform.LookAt(destination);
        JumpForward();
    }

    public void JumpTowardFlag()
    {
        JumpTowards(flagPos);
    }


    public void RandomJump()
    {
        Vector3 circle = Random.insideUnitCircle;
        circle = new Vector3(circle.x, 0, circle.y).normalized;
        transform.LookAt(transform.position + circle);
        JumpForward();
    }

    public void JumpBackToCell()
    {
        transform.LookAt(originCell);
        JumpForward();
    }

    public void JumpForward()
    {
        anim.Play("Jump");
        rb.AddForce((transform.forward * 3 + Vector3.up * 2 + transform.right * Random.Range(-.6f, .6f)).normalized * jumpForce, ForceMode.Impulse);
    }

    public void JumpBackward()
    {

        rb.AddForce((-transform.forward * 3 + Vector3.up * 2 + transform.right * Random.Range(-.6f, .6f)).normalized * jumpForce, ForceMode.Impulse);
    }

    public void Fly()
    {
        rb.velocity = transform.forward * flySpeed;
        flyTime -= Time.deltaTime;
    }

    #endregion

    #region CELL INTERACTION
    private void OnCollisionEnter(Collision collision)
    {
        anim.Play("Bounce");


        if (blobType == BlobManager.BlobType.mad && collision.transform.tag == "Cell")
        {
            infectedCell = collision.transform.GetComponent<CellMain>();
            infectedCell.stuckBlobs.Add(this);
            infectedCell.StockageCapabilityVariation(-infectionAmount);
            rb.isKinematic = true;
            isStuck = true;
        }

        if (blobType == BlobManager.BlobType.soldier)
            BlobManager.instance.TryAttack(this, 1);
    }

    public void ReduceCapacity()
    {
        if (infectedCell != null)
        {
            if (infectedCell.canBePool)
            {
                Unstuck();
            }
            infectedCell.StockageCapabilityVariation(-infectionAmount);
        }
    }

    public void Unstuck()
    {
        if (infectedCell != null)
        {
            infectedCell.stuckBlobs.Remove(this);
            infectedCell.StockageCapabilityVariation(infectionAmount);
        }
        infectedCell = null;
        infectionAmount = 1;
        rb.isKinematic = false;
        isStuck = false;
        JumpBackward();

    }

    #endregion


    public void SetOrigin(EnemyVillage newVillage)
    {
        cameFromVillage = true;
        village = newVillage;
    }
    public bool CheckIfOutOfVillage()
    {
        if (village == null)
            return false;

        if ((transform.position - village.transform.position).sqrMagnitude > Mathf.Pow(village.boundariesRange, 2))
            return true;
        else
            return false;
    }

}
