using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ColonyBtn : PoolableObjects
{
    [HideInInspector]public int cost = 100;
    [HideInInspector] public Vector3 point;

    public TextMeshProUGUI txt;
    public Animator anim;
    public NexusAera nexus;

    private bool clicked = false;

    public void BuildNexus()
    {

        if(RessourceTracker.instance.energy > cost && !clicked)
        {
            clicked = true;

            CellProductrice newProd = ObjectPooler.poolingSystem.GetPooledObject<CellProductrice>() as CellProductrice;
            newProd.Outpool();
            newProd.transform.position = point;
            newProd.CellInitialisation();
            newProd.GenerateLinkSlot();

            nexus.Hide();

            anim.SetBool("Show", false);
        }
        else
        {
            UIManager.Instance.WarningMessage("You don't have enough Sploosh !");
        }
    }

    public void UpdateText()
    {
        txt.SetText("New Nexus Cost: " + cost);
    }
}
