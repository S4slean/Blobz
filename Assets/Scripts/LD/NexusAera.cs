using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusAera : MonoBehaviour
{
    public int splouchCost = 100;
    public bool revealed = false;
    public ColonyBtn btn;

    private void OnTriggerEnter(Collider other)
    {
        if (revealed)
            return;

        if (other.transform.tag != "Enemies")
        {


            revealed = true;
            UIManager.Instance.DisplayColonyBtn(this);

        }
    }


}
