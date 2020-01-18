using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blob : PoolableObjects
{

    #region REFS
    public Rigidbody rb;
    public Renderer rd;
    #endregion

    #region GENERAL
    [HideInInspector] public int tickCount = 0;
    public int lifeTime = 0;
    public int LifeSpan = 32;
    public BlobManager.BlobType blobType = BlobManager.BlobType.normal;
    #endregion

    #region ENNEMIES
    public Transform tagetTransform;
    public bool isStuck = false;
    public CellMain infectedCell;
    public int infectionAmount = 1;
    public bool knowsNexus = false;
    public bool cameFromVillage = false;
    public EnemyVillage village;
    #endregion

    #region SOLDIER
    public float flyTime = 3;
    public float flySpeed = 5;
    public bool canExplode = false;
    #endregion



    private void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        rd = GetComponent<Renderer>();


        UpdateMat();
        //rajoute le blob à la liste des blobs actifs dans la scène

        RessourceTracker.instance.AddBlob(this);
        BlobManager.blobList.Add(this);
    }

    public void ChangeType(BlobManager.BlobType newType)
    {
        if (newType == BlobManager.BlobType.mad)
        {
            tag = "Enemies";
        }
        else if (blobType == BlobManager.BlobType.mad)
        {
            tag = "Untagged";
        }

        RessourceTracker.instance.RemoveBlob(this);
        blobType = newType;
        UpdateMat();
        RessourceTracker.instance.AddBlob(this);

    }

    public void UpdateMat()
    {
        switch (blobType)
        {
            case BlobManager.BlobType.explorateur:

                rd.material = BlobManager.instance.chargedMat;
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

    private void Update()
    {
        if (blobType == BlobManager.BlobType.soldier && flyTime > 0)
            Fly();
    }

    public void Destruct()
    {
        if (infectedCell != null)
        {
            infectedCell.StockageCapabilityVariation(infectionAmount);
            infectedCell.stuckBlobs.Remove(this);
        }
        else
        {
            //retire le blob de la liste des blobs actifs dans la scène
            BlobManager.blobList.Remove(this);
        }

        if (cameFromVillage)
        {
            village.RemoveBlobFromVillageList(this);
        }

        //play death Anim
        tickCount = 0;
        lifeTime = 0;
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


    public void Jump(Vector3 direction)
    {
        transform.LookAt(transform.position + direction);

        rb.AddForce(direction + Random.insideUnitSphere, ForceMode.Impulse);

    }

    public void Fly()
    {
        rb.velocity = transform.forward * flySpeed;
        flyTime -= Time.deltaTime;
    }

    public void ReduceCapacity()
    {
        if (infectedCell != null)
        {
            infectedCell.StockageCapabilityVariation(-infectionAmount);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (blobType == BlobManager.BlobType.mad && collision.transform.tag == "Cell")
        {
            infectedCell = collision.transform.GetComponent<CellMain>();
            infectedCell.stuckBlobs.Add(this);
            infectedCell.StockageCapabilityVariation(-infectionAmount);
            rb.isKinematic = true;
            isStuck = true;
        }

        if (blobType == BlobManager.BlobType.soldier && canExplode)
            BlobManager.instance.Explode(this, 1);
    }

    public void SetOrigin(EnemyVillage newVillage)
    {
        cameFromVillage = true;
        village = newVillage;
    }

    public void Unstuck()
    {

        infectedCell.stuckBlobs.Remove(this);
        infectedCell = null;
        infectionAmount = 0;
        rb.isKinematic = false;
        isStuck = false;

    }

    public bool CheckIfOutOfVillage()
    {
        if ((transform.position - village.transform.position).sqrMagnitude > Mathf.Pow(village.boundariesRange, 2))
            return true;
        else
            return false;
    }

}
