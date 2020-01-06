using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class VillageSelectionBtn : MonoBehaviour
{
    [HideInInspector] public int SplouchAmount = 0;
    [HideInInspector] public EnemyVillage village;
    public TextMeshProUGUI txt;

    public void GiveSplouch()
    {

        //anim de splouch

        RessourceTracker.instance.EnergyVariation(SplouchAmount);
        village.DeleteVillage();
        UIManager.Instance.HideVillageSelection();
    }

    public void SpawnNexus()
    {
        village.DeleteVillage();
        UIManager.Instance.HideVillageSelection();

        CellProductrice newProd;
        newProd = ObjectPooler.poolingSystem.GetPooledObject<CellProductrice>() as CellProductrice;
        newProd.Outpool();
        newProd.transform.position = village.transform.position;
        newProd.CellInitialisation();
    }

    public void UpdateText()
    {
        txt.SetText("Splouch : " + SplouchAmount);
    }
}
