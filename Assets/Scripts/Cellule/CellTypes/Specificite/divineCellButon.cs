using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class divineCellButon : MonoBehaviour
{
    public Animator anim;



    public void ToggleButton(bool toggle)
    {
        if (toggle)
        {
            Debug.Log("Oui");
            anim.Play("Open");
        }
        else
        {
            Debug.Log("Non");
            anim.Play("Close");
        }
    }




}

