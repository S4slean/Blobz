using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TourelleProjectile : PoolableObjects
{
    [Range(0, 1)]
    public float travelRatio;
    public Animator anim;

    private Vector3[] pos = new Vector3[2];
    private bool inTravel;

    public ParticleSystem[] particleSystems;


    private bool targetIsBlob;
    private Destructible currentCellTarget;
    private Blob currentblobTarget;
    private int damageToDeal;


    public void Travel()
    {
      //  Debug.Log(transform.position);
        transform.position = Vector3.Lerp(pos[0], pos[1], travelRatio);
    }



    public void Init(Vector3 startPos , Vector3 endPos , Blob blobTarget)
    {
        targetIsBlob = true;
        currentblobTarget = blobTarget;

        pos[0] = startPos;
        pos[1] = endPos;
        travelRatio = 0;
        inTravel = true;
        transform.position = pos[0];
        anim.Play("Shoot");
    }

    public void Init(Vector3 startPos, Vector3 endPos , Destructible cellTarget , int damage)
    {
        targetIsBlob = false;
        currentCellTarget = cellTarget;
        damageToDeal = damage;


        pos[0] = startPos;
        pos[1] = endPos;
        travelRatio = 0;
        inTravel = true;
        transform.position = pos[0];
        anim.Play("Shoot");
    }

    public void EndTravel()
    {
        inTravel = false;
        if (targetIsBlob)
        {
            currentblobTarget.ReceiveDamage();
        }
        else
        {
            currentCellTarget.ReceiveDamage(damageToDeal);
        }


        Inpool();

    }


    public void Update()
    {
        if (inTravel)
        {
            Travel();
        }
    }

}
