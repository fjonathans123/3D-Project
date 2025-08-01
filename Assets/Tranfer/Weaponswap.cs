using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class Weaponswap : MonoBehaviour
{
    public int selectedweapon = 0;
    private pauseMenuController pause;

    //public Transform[] leftHandGrip, rightHandGrip;
    public TwoBoneIKConstraint leftHand, rightHand;
    //public RigBuilder rigging;
    public Animator anim;
    // Start is called before the first frame update

    private CrosshairManager cm;

    public WheelScrolling scroll;
    void Start()
    {
        pause = GameObject.FindGameObjectWithTag("UI").GetComponent<pauseMenuController>();
        cm = GameObject.FindGameObjectWithTag("UI").GetComponent<CrosshairManager>();
        selectweapon();
    }

    // Update is called once per frame
    void Update()
    {
       if(pause.isPaused == false)
        {
            int previousSelectedWeapon = selectedweapon;
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                if (selectedweapon >= transform.childCount - 1)
                    selectedweapon = 0;
                else selectedweapon++;
            }

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                if (selectedweapon <= 0)
                    selectedweapon = transform.childCount - 1;
                else selectedweapon--;
            }
            if (previousSelectedWeapon != selectedweapon)
            {
                selectweapon();
            }
        }
        
    }

    void selectweapon()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedweapon)
            {
                weapon.gameObject.SetActive(true);
                weapon.GetComponentInChildren<Renderer>().enabled = true;
                //if(selectedweapon < 2)
                //{
                //rightHand.data.target = rightHandGrip[selectedweapon];
                //leftHand.data.target = leftHandGrip[selectedweapon];
                //rigging.Build();

                //}
                anim.SetInteger("weaponSelection", selectedweapon);
            }
            else
            {
                scroll.resetAim();
                weapon.GetComponentInChildren<Renderer>().enabled = false;
                StartCoroutine(disableAiming(scroll.gunAim[i], weapon.gameObject));
                cm.HideCrosshair();
            }
            i++;
        }
    }

    IEnumerator disableAiming(Gunaiming aim , GameObject weapon)
    {
        while(aim != null &&  aim.aiming)
        {
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        weapon.SetActive(false);
    }
}
