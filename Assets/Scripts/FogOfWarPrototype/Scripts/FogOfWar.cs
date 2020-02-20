using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour 
{
    //   [SerializeField]
    //   GameObject[] objects;
    //   Vector4[] positions = new Vector4[100];
    //// Use this for initialization
    //void Start () {
    //       //positions = new Vector4[100];
    //   }

    //// Update is called once per frame
    //void Update () {

    //       for (int i = 0; i < 100; i++)
    //       {
    //           positions[i] = objects[i].transform.position;
    //       }
    //       Shader.SetGlobalVectorArray("_Positions", positions);
    //}

    [SerializeField]
    FogOfWarData[] fogDatas;
    Vector4[] positions = new Vector4[300];
    float[] radius = new float[300];

    private void Update()
    {
        for (int i = 0; i < fogDatas.Length; i++)
        {
            positions[i] = fogDatas[i].MyTransform.position;
            radius[i] = fogDatas[i].rangeOfSight;
        }
        Shader.SetGlobalVectorArray("_FogPositions", positions);
        Shader.SetGlobalFloatArray("_FogRadius", radius);
    }
}
