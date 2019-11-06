using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Range(0,2)]public float timeToMaxSpeed =1;
    [Range(0, 2)] public float timeToZeroSpeed = 1;
    public float camSpeed = 5;
    float count;

    public static CameraController instance;

    private void Start()
    {
        originPos = new Vector3(0, 20, 0);
        transform.position = originPos;
        transform.LookAt(Vector3.down);

        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    Vector3 moveDir;

    public void MoveCamera()
    {

        mouseDir = new Vector3(Input.mousePosition.x - Screen.width / 2, 0, Input.mousePosition.y - Screen.height / 2);

        if (MouseIsInBorder())
            moveDir = mouseDir.normalized;
        else
            moveDir = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;



        if (moveDir != Vector3.zero)
        {
            count +=  Time.deltaTime / timeToMaxSpeed;
            count = Mathf.Clamp01(count);
        }

        else
        {
            count -= Time.deltaTime / timeToZeroSpeed;
            count = Mathf.Clamp01(count);
        }


        originPos += moveDir * count * camSpeed * Time.deltaTime;

        tiltDir = mouseDir * tiltCount * tiltLength / 100;


        transform.position = originPos + tiltDir;

    }

    Vector3 originPos;
    Vector3 mouseDir;
    Vector3 tiltDir;
    [Range(0,0.001f)]public float tiltToMaxSpeed = 1;
    [Range(0, 0.001f)] public float tiltToZeroSpeed = 1;
    float tiltCount;
    public float tiltLength = 1;

    public void TiltCamera()
    {
        tiltCount += Time.deltaTime/tiltToMaxSpeed;
        tiltCount = Mathf.Clamp01(tiltCount);
    }

    public bool MouseIsInBorder()
    {
        if(Input.mousePosition.x < Screen.width/50 
            || Input.mousePosition.x > (Screen.width - (Screen.width / 50)) 
            || Input.mousePosition.y < Screen.height/50 
            || Input.mousePosition.y > (Screen.height - (Screen.height / 50)))
        {
            return true;
        }

        return false;
    }

    public void MoveCamWithMouse()
    {
        originPos += mouseDir * count * camSpeed * Time.deltaTime;
    }

    public void DecreaseTiltCount()
    {
        tiltCount -= Time.deltaTime/tiltToZeroSpeed;
        tiltCount = Mathf.Clamp01(tiltCount);
    }
}
