using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceDetection : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        QuestManager.instance.QuestSuccess();
    }

}
