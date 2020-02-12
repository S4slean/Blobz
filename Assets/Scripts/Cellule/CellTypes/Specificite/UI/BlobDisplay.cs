using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlobDisplay : MonoBehaviour
{
    public Transform myTransform;
    public SpriteRenderer background;
    public float baseScale = 0.75f;


    public TextMeshPro blobText;
    public Color[] lerpTextColor = new Color[2];

   // public Animator anim;


    private void Awake()
    {
        if (myTransform == null)
        {
            myTransform = transform;
        }
    }

    public void UpdateUI(int blobNumber , int stockageCapacity )
    {
        blobText.text = (blobNumber + " / " + stockageCapacity);
        float ratio = 0;
        ratio = (float)blobNumber / (float)stockageCapacity;

        //anim.speed = 1 / (TickManager.instance.tickDuration - TickManager.instance.tickDuration / 1.8f);
        float ratioClamp = ratio > 1 ? 1 : ratio;

        myTransform.localScale = (Vector3.one* baseScale) * (1f + baseScale * ratioClamp);


        background.color = Color.Lerp(lerpTextColor[0], lerpTextColor[1], ratioClamp);
    }

}
