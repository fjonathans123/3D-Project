using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponJumpAnimation : MonoBehaviour
{
    public playeMovement player;
    public Gunaiming aim;
    public float jumpIntensity, jumpSmooth, landingIntensity, landingSmooth;
    public float recoverySpeed;
    float impactForce = 0;

    // Update is called once per frame
    void Update()
    {
       if (!player.isGrounded)
        {
            float yVelocity = player.velocity.y;
            impactForce = -yVelocity * landingIntensity; 

            if (aim.aiming)
            {
                yVelocity = Mathf.Max(yVelocity, 0);
            }

            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(yVelocity * jumpIntensity, 0, 0), Time.deltaTime * jumpSmooth);
        }

       else if(player.isGrounded & impactForce >= 0)
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.Euler(impactForce, 0, 0), Time.deltaTime * landingSmooth);
            impactForce -= recoverySpeed * Time.deltaTime;
        }
       
        else
        {
            transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, Time.deltaTime * landingSmooth);
        }
    }
}

