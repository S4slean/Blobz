using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellGraphAnim : MonoBehaviour
{
    public ParticleSystem fxPlayerInteraction;

    public void PlayerInteraction()
    {
        fxPlayerInteraction.Play();
    }

}
