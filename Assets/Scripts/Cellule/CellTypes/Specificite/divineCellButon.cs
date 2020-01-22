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
            anim.Play("Open");
        }
        else
        {
            anim.Play("Close");
        }
    }




}

