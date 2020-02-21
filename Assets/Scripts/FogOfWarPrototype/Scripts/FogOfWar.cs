using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWar : MonoBehaviour
{

    public static FogOfWar instance;



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
    private FogOfWarData[] fogDatas;
    private Vector4[] positions = new Vector4[1000];
    private float[] radius = new float[1000];
    private int currentIndex;


    private void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            //fogDatas = new FogOfWarData[1000];
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    public void AddFogOfWarInfluenceur(FogOfWarData fogOfWarInfluenceur)
    {
        if (fogOfWarInfluenceur.alreadyAddToFog)
        {
            return;
        }
        fogOfWarInfluenceur.alreadyAddToFog = true;
        fogDatas[currentIndex] = fogOfWarInfluenceur;
        currentIndex++;
    }


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
