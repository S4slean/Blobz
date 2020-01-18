using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WarningMessage : MonoBehaviour
{
    public Animator anim;
    public TextMeshProUGUI txt;

    public void SetText(string msg)
    {
        txt.text = msg;
    }

    public void Display(string msg)
    {
        SetText(msg);
        anim.Play("Show");
    }

}
