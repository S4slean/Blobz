using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class energieTopBar : MonoBehaviour
{
    public Animator anim;
    private int toMuchEnergyRepetition;


    public void CheckEnergieAmount()
    {
        if (RessourceTracker.instance.energy >= RessourceTracker.instance.energyCap)
        {
            anim.SetBool("toMuchEnergy", true);
        }
        else
        {
            anim.SetBool("toMuchEnergy", false);
        }
    }

    public void AddToMuchEnergyRepetition()
    {
        toMuchEnergyRepetition++;
        if (toMuchEnergyRepetition >= 3)
        {
            anim.SetBool("toMuchRepetition", true);
            toMuchEnergyRepetition = 0;
        }
    }

    public void ToMuchRepetitionReset()
    {
        anim.SetBool("toMuchRepetition", false);
    }

}
