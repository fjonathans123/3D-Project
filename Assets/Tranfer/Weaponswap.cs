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
        cm = GameObject.FindGameObjectWithTag("UI").GetComponentInChildren<CrosshairManager>();
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
            var aim = scroll.gunAim[i];
            bool isSelected = (i == selectedweapon && selectedweapon < 3);

            if (weapon != null)
            {
                ResetAiming(aim, weapon.gameObject);
                if (isSelected)
                {
                    weapon.gameObject.SetActive(true);
                    weapon.GetComponentInChildren<Renderer>().enabled = true;
                    anim.SetInteger("weaponSelection", selectedweapon);
                }
                else
                {
                    weapon.GetComponentInChildren<Renderer>().enabled = false;
                    weapon.gameObject.SetActive(false);
                }
            }
            if (selectedweapon > 2)
            {
                cm.ShowCrosshair(false);
            }
            i++;
        }

        if (selectedweapon >= transform.childCount || selectedweapon < 0)
        {
            scroll.resetAim();
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

    void ResetAiming(Gunaiming gun, GameObject weapon)
    {
        gun.aiming = false;
        gun.player.changeFOV(60f);
        weapon.transform.localPosition = gun.originalPos;
        weapon.transform.localRotation = gun.originalRotation;
    }
}
