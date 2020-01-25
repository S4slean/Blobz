using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreblobchetUISlot : MonoBehaviour
{

    public SpriteRenderer sR;
    public Sprite[] sprites = new Sprite[3];


    public void UpdateType(BlobManager.BlobType type)
    {

        switch (type)
        {

            case BlobManager.BlobType.explorateur:
                sR.sprite = sprites[0];
                break;

            case BlobManager.BlobType.soldier:
                sR.sprite = sprites[1];
                break;

            case BlobManager.BlobType.aucun:
                sR.sprite = sprites[2];

                break;
        }
    }
}
