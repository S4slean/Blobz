using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusAera : MonoBehaviour
{
    public bool alreadyRevealed;
    public int splouchCost = 100;
    private bool revealed = false;
    public ColonyBtn btn;
    public Animator anim;

    private void Start()
    {
        if (alreadyRevealed)
        {
            anim.SetBool("Show", true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (revealed)
            return;

        if (other.transform.tag != "Enemies")
        {


            revealed = true;
            UIManager.Instance.DisplayColonyBtn(this);
            anim.SetBool("Show", true);

        }
    }


}
