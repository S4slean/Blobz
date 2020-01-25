using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivineSparke : MonoBehaviour
{
    public ParticleSystem circle;
    public Transform myTRansform;



    private void Awake()
    {
        if (myTRansform ==null)
        {
            myTRansform = transform;
        }
       
    }

    public void PlayFx(float radius , Vector3 pos)
    {
        var main = circle.main;
        main.startSize = 1.4f * radius;

        myTRansform.position = pos;

        circle.Play();

    }
}
