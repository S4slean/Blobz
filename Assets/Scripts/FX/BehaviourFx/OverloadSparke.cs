using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverloadSparke : MonoBehaviour
{
    public ParticleSystem p;
    public int initialSpikeNumber;
    public Transform myTransform;

    public Gradient[] gradients = new Gradient[5];

    private ParticleSystem.MinMaxCurve baseSpeedCurve;


    private void Awake()
    {
        var main = p.main;
        baseSpeedCurve = main.startSpeed;
        if (myTransform == null)
        {
            myTransform = transform;
        }
    }

    public void SetupPos(Vector3 pos)
    {
        myTransform.position = pos;
    }

    public void SetSpikeNumberAndSpeed(int overloadTier, float speedModif)
    {

        var main = p.main;
        main.startSpeed = new ParticleSystem.MinMaxCurve(baseSpeedCurve.constantMin + speedModif, baseSpeedCurve.constantMax + speedModif);

        var emission = p.emission;
        emission.SetBursts(new ParticleSystem.Burst[]
        {
            new ParticleSystem.Burst(0.0f , (short)(overloadTier + initialSpikeNumber)),
            new ParticleSystem.Burst(0.1f , (short)(overloadTier + initialSpikeNumber))
        });

        var colorOverLifeTime = p.colorOverLifetime;
        colorOverLifeTime.color = gradients[overloadTier];
        p.Play();

    }


}
