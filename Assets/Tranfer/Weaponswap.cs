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
    void Start()
    {
        pause = GameObject.FindGameObjectWithTag("UI").GetComponent<pauseMenuController>();
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

                //if(selectedweapon < 2)
                //{
                //rightHand.data.target = rightHandGrip[selectedweapon];
                //leftHand.data.target = leftHandGrip[selectedweapon];
                //rigging.Build();

                //}
                anim.SetInteger("weaponSelection", selectedweapon);
            }
            else
                weapon.gameObject.SetActive(false);
            i++;
        }
    }
}
