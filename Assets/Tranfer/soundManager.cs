using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundManager : MonoBehaviour
{
    public static AudioClip playerShootSound;
    static AudioSource asrc; 
    // Start is called before the first frame update
    void Start()
    {
        playerShootSound = Resources.Load<AudioClip>("playerShoot");
        asrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //asrc.Play();
    }

    public static void PlaySound(string clip)
    {
        switch(clip)
        {
            case "playerShoot":
                asrc.PlayOneShot(playerShootSound);
                break;
        }
    }
}
