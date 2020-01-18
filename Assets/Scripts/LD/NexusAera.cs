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
    public AudioClip discoverySound;

    private void Start()
    {
        if (alreadyRevealed)
        {
            anim.SetBool("Show", true);
        }
    }

    public void Reveal()
    {

            anim.SetBool("Show", true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (revealed)
            return;

        if (other.transform.tag != "Enemies")
        {


            revealed = true;
            UIManager.Instance.DisplayColonyBtn(this);
            SoundManager.instance.PlaySound(discoverySound);
            anim.SetBool("Show", true);

        }
    }

    public void Hide()
    {
        anim.SetBool("Show", false);
    }


}
