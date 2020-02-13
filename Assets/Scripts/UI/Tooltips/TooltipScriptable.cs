using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Tooltip.asset", menuName = "Blobz/Tooltip")]
public class TooltipScriptable : ScriptableObject
{
    public string objectName;
    [TextArea(4,10)]public string description;

}
