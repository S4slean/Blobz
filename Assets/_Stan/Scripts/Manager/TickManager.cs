using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TickManager : MonoBehaviour
{
    public delegate void Tick();
    public static Tick doTick;
    public static Tick doTick2;
    public static Tick doTick4;
    public static Tick doTick8;
    public static Tick doTick16;

    private float count = 0;
    private float ticksElapsed = 0;
    [SerializeField] [Range(0, 10)] private float tickDuration = 1;

    private void Update()
    {
        count += Time.deltaTime;
        if(count > tickDuration)
        {
            count -= tickDuration;

            if(doTick != null)
                doTick();

            ticksElapsed++;

            if (ticksElapsed % 2 == 0 && doTick2!=null)
                doTick2();

            if (ticksElapsed % 4 == 0 && doTick4 != null)
                doTick4();

            if (ticksElapsed % 8 == 0 && doTick8 != null)
                doTick8();

            if (ticksElapsed % 16 == 0 && doTick16 != null)
                doTick16();
        }
    }
}
