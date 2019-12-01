﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuestPopUp : MonoBehaviour
{

    public TextMeshProUGUI TMPtitle;
    public TextMeshProUGUI TMPtext;
    public Image img;

    public void UpdatePopUp(string title, string text, Sprite sprite, bool isUsingSprite)
    {
        TMPtitle.text = title;
        TMPtext.text = text;
        
        if(img.sprite != null)
            img.sprite = sprite;

        SetRpgStyle(isUsingSprite);
    }

    void SetRpgStyle(bool isUsingSprite)
    {
        if (isUsingSprite)
        {
            img.enabled = true;
        }
        else
        {
            img.enabled = false;
        }
    }

}
