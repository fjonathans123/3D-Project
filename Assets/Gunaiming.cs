using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunaiming : MonoBehaviour
{
    public Vector3 aimOffset;
    public float sensitivity;
    public bool aiming;
    public playeMovement player;


    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            aiming = !aiming;
        }
        if (aiming)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimOffset, Time.deltaTime * sensitivity);
            player.changeFOV(30f);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * sensitivity);
            player.changeFOV(60f);
        }
    }
}
