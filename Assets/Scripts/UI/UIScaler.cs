using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScaler : MonoBehaviour
{
    public float ratio = .01f;


    // Update is called once per frame
    void Update()
    {
        transform.localScale =  Vector3.one * CameraController.instance.transform.position.y * ratio; 
    }
}
