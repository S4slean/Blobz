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
    private int currentCellTargetIndex;


    public void Init(CellTourelle _parent)
    {
        parent = _parent;
        myCollider.radius = parent.myCellTemplate.tourelleAttackRadius;
    }


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
            CheckForTarget();
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
                if (cell != null)
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

               // badBlobs.Remove(badBlobs[0]);
            }
            else
            {
                //foudra désactiver / réactiver le cllider des cell en reoncstruction 
                badCell[currentCellTargetIndex].ReceiveDamage(parent.myCellTemplate.tourelleDamage);
                if (badCell[currentCellTargetIndex].isRuin)
                {
                    CheckForTarget();
                }
            }
        }
    }

    private void CheckForTarget()
    {
        if (badBlobs.Count > 0 )
        {
            hasTarget = true;
        }
        else if (badCell.Count > 0)
        {
            hasTarget = false;
            for (int i = 0; i < badCell.Count; i++)
            {
                if (!badCell[i].isRuin)
                {
                    currentCellTargetIndex = i;
                    hasTarget = true;
                    break;
                }
                
            }   
        }
        else
        {
            hasTarget = false;
        }


        //if (badBlobs.Count <= 0 && badCell.Count <= 0)
        //{
        //    hasTarget = false;
        //}
        //else
        //{
        //    hasTarget = true;
        //}
    }

    public void Death()
    {
        badBlobs.Clear();
        badCell.Clear();
        hasTarget = false;

    }
}
