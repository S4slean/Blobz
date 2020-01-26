using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tourelleCollider : MonoBehaviour
{
    public SphereCollider myCollider;
    public Transform tourelleCanon;
    private CellTourelle parent;
    private bool hasTarget;




    private List<Blob> badBlobs = new List<Blob>();
    private List<Destructible> badCell = new List<Destructible>();
    private int currentCellTargetIndex;
    private int currentBlobTargetIndex;
    public BlobManager.BlobType blobToTarget;


    public void Init(CellTourelle _parent)
    {
        parent = _parent;
        myCollider.radius = parent.myCellTemplate.tourelleAttackRadius;
        myCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        Blob blolMechant = other.GetComponent<Blob>();
        if (blolMechant != null)
        {
            badBlobs.Add(blolMechant);
            CheckForTarget();
            return;
        }

        if (other.tag == "Enemies")
        {

            Destructible cell = other.GetComponent<Destructible>();
            if (badCell != null)
            {
                badCell.Add(cell);
                CheckForTarget();
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {

        Blob blolMechant = other.GetComponent<Blob>();
        if (blolMechant != null)
        {
            badBlobs.Remove(blolMechant);
            CheckForTarget();
            return;
        }

        if (other.tag == "Enemies")
        {
            Destructible cell = other.GetComponent<Destructible>();
            if (cell != null)
            {
                badCell.Remove(cell);
                CheckForTarget();
            }
        }

    }

    public void Fire()
    {
        if (!hasTarget)
        {
            CheckForTarget();
        }
        if (hasTarget)
        {
            parent.setCurrentTick(0);
            TourelleProjectile projectile = ObjectPooler.poolingSystem.GetPooledObject<TourelleProjectile>() as TourelleProjectile;
            projectile.Outpool();
            if (badBlobs.Count > 0)
            {
                // badBlobs[currentCellTargetIndex].Destruct();
                projectile.Init(tourelleCanon.position, badBlobs[currentBlobTargetIndex].transform.position, badBlobs[currentBlobTargetIndex]);
               // badBlobs.Remove(badBlobs[currentBlobTargetIndex]);
                parent.MunitionVariation(-1);
                CheckForTarget();
                return;
            }


            //badCell[currentCellTargetIndex].ReceiveDamage(parent.myCellTemplate.tourelleDamage);
            if (badCell[currentCellTargetIndex] != null)
            {
                projectile.Init(tourelleCanon.position, badCell[currentCellTargetIndex].transform.position, badCell[currentCellTargetIndex], parent.myCellTemplate.tourelleDamage);
                parent.MunitionVariation(-1);
            }
            CheckForTarget();

        }
    }

    private void CheckForTarget()
    {
        if (badBlobs.Count > 0)
        {
            hasTarget = false;
            for (int i = 0; i < badBlobs.Count; i++)
            {
                if (badBlobs[i].GetBlobType() == blobToTarget)
                {
                    currentBlobTargetIndex = i;
                    hasTarget = true;
                    return;
                }

            }
        }

        if (badCell.Count > 0)
        {
            hasTarget = false;
            for (int i = 0; i < badCell.Count; i++)
            {
                if (!badCell[i].isRuin)
                {
                    currentCellTargetIndex = i;
                    hasTarget = true;
                    Debug.Log(badCell[i], gameObject);
                    return;
                }

            }
        }
        hasTarget = false;

    }

    public void Death()
    {
        myCollider.enabled = false;
        badBlobs.Clear();
        badCell.Clear();
        currentBlobTargetIndex = 0;
        currentCellTargetIndex = 0;

        hasTarget = false;

    }
}
