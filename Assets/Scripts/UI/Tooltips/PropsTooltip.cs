using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PropsTooltip : MonoBehaviour
{
    public TextMeshProUGUI title;
    public TextMeshProUGUI description;

    public void UpdateTooltipInfos(TooltipScriptable tooltipData)
    {
        title.text = tooltipData.objectName;
        description.text = tooltipData.description;
    }
}
