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

    public AudioSource footStepSource;
    public AudioClip footstepsSound;
    public float stepThreshold = -0.95f;
    private bool stepped = false;
    public float inputthreshold = -0.1f;

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

            footStepHandle(velocity);
        }

        else

        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, Vector3.zero, Time.deltaTime);
        }
        lastPosition = transform.position;
    }

    void footStepHandle(float velocity)
    {
        if(velocity < 0.01f)
        {
            stepped = false;
            return;
        }

        float bob = Mathf.Sin(sinY);

        if (!stepped && bob <= stepThreshold)
        {
            PlayFootSteps();
            stepped = true;
        }
        else if(bob > stepThreshold)
        {
            stepped = false;
        }
    }

    void PlayFootSteps()
    {
        footStepSource.PlayOneShot(footstepsSound);
    }

}
