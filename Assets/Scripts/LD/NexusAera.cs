using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusAera : MonoBehaviour
{
    public int splouchCost = 100;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag != "enemies")
        {

            if (other.GetComponent<Blob>())
            {
                UIManager.Instance.DisplayColonyBtn(this);
            }
        }
    }


}
