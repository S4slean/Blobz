using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Image filledImage;
    public Animator anim;

    public GameObject graph;

    public void UpdateBar(float ratio, bool circleBar)
    {
        filledImage.fillAmount = ratio;
        anim.speed = 1 / (TickManager.instance.tickDuration - TickManager.instance.tickDuration / 1.8f);
        if (circleBar)
        {
            anim.Play("AmountModificationCercle");
        }
        else
        {
            anim.Play("AmountModification");
        }
    }

    public void ToggleRenderer(bool toggle)
    {
        graph.SetActive(toggle);
    }

}
