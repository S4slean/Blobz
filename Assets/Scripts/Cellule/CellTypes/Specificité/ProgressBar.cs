using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image filledImage;
    public Animator anim;

    public void UpdateBar(float ratio)
    {
        filledImage.fillAmount = ratio;
        anim.speed = 1 / (TickManager.instance.tickDuration - TickManager.instance.tickDuration / 1.8f);
        anim.Play("AmountModification");
    }
}
