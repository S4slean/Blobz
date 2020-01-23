using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextScore : PoolableObjects
{
    public Transform myTransform;
    public TextMeshPro textScore;
    public Animator anim;

    private Vector3 baseScale;

    private void Awake()
    {
        if (myTransform == null)
        {
            myTransform = transform;
        }
        baseScale = myTransform.localScale;

    }

    public void PlayAnim(float amount)
    {

        myTransform.localScale = baseScale;
        myTransform.localScale *= Random.Range(0.7f, 1.3f);
        anim.speed = 1;
        anim.speed += amount;
        anim.Play("addScore");

    }




}
