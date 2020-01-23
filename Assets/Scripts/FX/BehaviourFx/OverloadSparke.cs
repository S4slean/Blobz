using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OverloadSparke : MonoBehaviour
{
    public ParticleSystem p;
    public int initialSpikeNumber;

    public Gradient[] gradients = new Gradient[5]; 


    public void SetSpikeNumberAndSpeed(int overloadTier , float speedMin , float speedMax)
    {

        var main = p.main;
        main.startSpeed = new ParticleSystem.MinMaxCurve(speedMin, speedMax);

        var emission = p.emission;
        emission.burstCount = initialSpikeNumber + overloadTier;

        var colorOverLifeTime = p.colorOverLifetime;
        colorOverLifeTime.color = gradients[overloadTier];
        p.Play();

    }


}
