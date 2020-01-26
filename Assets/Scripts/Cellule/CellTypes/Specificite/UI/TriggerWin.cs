using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerWin : MonoBehaviour
{
    public void WinTrigger()
    {
        LevelManager.instance.LevelSuccessed();
    }

}
