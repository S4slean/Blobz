using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BlobDisplay : MonoBehaviour
{
    public Image background;


    public TextMeshPro blobText;
    public Color[] lerpTextColor = new Color[2];

    public Animator anim;


    public void UpdateUI(int blobNumber , int stockageCapacity )
    {
        blobText.text = (blobNumber + " / " + stockageCapacity);
        float ratio = 0;
        ratio = (float)blobNumber / (float)stockageCapacity;


    }

}
