using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TopBarUI : MonoBehaviour
{

    public TextMeshProUGUI energy;

    public void UpdateUI()
    {
        energy.text = "Energy : " + RessourceTracker.instance.energy;
    }

}
