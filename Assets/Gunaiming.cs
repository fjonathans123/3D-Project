using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunaiming : MonoBehaviour
{
    public Vector3 aimOffset;
    public float sensitivity;
    public bool aiming;
    public playeMovement player;

    public Vector3 originalPos;

    public CrosshairManager cm;
    [HideInInspector] public Quaternion originalRotation;
    // Update is called once per frame
    void Update()
    {
        if (!gameObject.activeInHierarchy) return;
        if(Input.GetMouseButtonDown(1))
        {
            aiming = !aiming;
        }
        if (aiming)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, aimOffset, Time.deltaTime * sensitivity);
            player.changeFOV(30f);
            cm.ShowCrosshair(false);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime * sensitivity);

            if(Vector3.Distance(transform.localPosition, Vector3.zero) < 0.01f)
            {
                transform.localPosition = originalPos;
            }
            player.changeFOV(60f);
            cm.ShowCrosshair(true);
        }
    }

    public void AssignOriginalTransform()
    {
        originalPos = transform.localPosition;
        originalRotation = transform.localRotation;
    }
}
