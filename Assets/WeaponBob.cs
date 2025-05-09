using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBob : MonoBehaviour
{
    public float magnitude, idleSpeed, walkSpeedMultiplier, walkSpeedMax, aimReduction;

    public playeMovement player;

    float sinY = 0f;
    float sinX = 0f;
    Vector3 lastPosition;
    // Start is called before the first frame update
    void Start()
    {
        lastPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.isGrounded)
        {
            float delta = Time.deltaTime * idleSpeed;
            float velocity = (lastPosition - transform.position).magnitude * walkSpeedMultiplier;
            delta += Mathf.Clamp(velocity, 0, walkSpeedMax);

            sinX += delta / 2;
            sinY += delta;

            float magnitude = this.magnitude;

            transform.localPosition = Vector3.zero + Vector3.up * Mathf.Sin(sinY) * magnitude;
            transform.localPosition += Vector3.right * Mathf.Sin(sinX) * magnitude;
        }

        else

        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime);
        }
        lastPosition = transform.position;
    }

}
