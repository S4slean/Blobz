using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tourelleCollider : MonoBehaviour
{
    public SphereCollider myCollider;
    private CellTourelle parent;
    private bool hasTarget;


    private List<Blob> badBlobs = new List<Blob>();
    private List<Destructible> badCell = new List<Destructible>();


    public void Init(CellTourelle _parent)
    {
        parent = _parent;
        myCollider.radius = parent.myCellTemplate.tourelleAttackRadius;

    }

    //private void CheckBadCell()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, parent.GetCurrentRange());
    //    for (int i = 0; i < colliders.Length; i++)
    //    {
    //        if (colliders[i].tag == "enemies")
    //        {
    //            Destructible newDestructible = colliders[i].GetComponent<Destructible>();
    //            if (newDestructible != null)
    //            {
    //                badCell.Add(newDestructible);
    //            }
    //        }
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemies")
        {
            Blob blolMechant = other.GetComponent<Blob>();
            if (blolMechant != null)
            {
                badBlobs.Add(blolMechant);
            }
            else
            {
                Destructible cell = other.GetComponent<Destructible>();
                if (badCell != null)
                {
                    badCell.Add(cell);
                }
            }

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Enemies")
        {
            Blob blolMechant = other.GetComponent<Blob>();
            if (blolMechant != null)
            {
                badBlobs.Remove(blolMechant);
                CheckForTarget();

            }
            else
            {
                Destructible cell = other.GetComponent<Destructible>();
                if (badCell != null)
                {
                    badCell.Remove(cell);
                    CheckForTarget();
                }
            }
        }
    }

    public void Fire()
    {
        if (hasTarget)
        {
            Debug.Log("Fire");
            if (badBlobs[0] != null)
            {
                //ça sera un projectile 
                badBlobs[0].Destruct();
            }
            else
            {
                //foudra désactiver / réactiver le cllider des cell en reoncstruction 
                badCell[0].ReceiveDamage(parent.myCellTemplate.tourelleDamage);
            }
        }
    }

    private void CheckForTarget()
    {
        if (badBlobs.Count <= 0 && badCell.Count <= 0)
        {
            hasTarget = false;
        }
        else
        {
            hasTarget = true;
        }
    }
}
