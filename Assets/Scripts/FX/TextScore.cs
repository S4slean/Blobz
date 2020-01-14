using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextScore : PoolableObjects
{
    public Transform myTransform;
    public TextMeshPro textScore;
    public Animator anim;

    private void Awake()
    {
        if (myTransform == null)
        {
            myTransform = transform;
        }
    }

    public void PlayAnim()
    {
        anim.Play("addScore");
    }




}
