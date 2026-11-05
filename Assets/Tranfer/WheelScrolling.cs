using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class WheelScrolling : MonoBehaviour
{
    private GameManager stats;
    public Weaponswap switches;
    public float selectedItemPosition, DefaultPosition;
    public Transform[] items;
    public gameData data;

    public float holdDuration;
    private float holdTime;
    private bool isHolding;

    public int healType, ammoType;

    public gunScript[] guns;
    public Gunaiming[] gunAim;

    // Start is called before the first frame update
    void Start()
    {
        stats = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        foreach(var aim in gunAim)
        {
            aim.AssignOriginalTransform();
        }
    }

    // Update is called once per frame
    void Update()
    {
        switch (switches.selectedweapon)
        {
            case 0:
                items[0].transform.DOMoveX(selectedItemPosition, 0.1f, true);
                items[1].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[2].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[3].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[4].transform.DOMoveX(DefaultPosition, 0.1f, true);
                gunAim[1].aiming = false;
                gunAim[2].aiming = false;

                break;
            case 1:
                items[0].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[1].transform.DOMoveX(selectedItemPosition, 0.1f, true);
                items[2].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[3].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[4].transform.DOMoveX(DefaultPosition, 0.1f, true);
                gunAim[0].aiming = false;
                gunAim[2].aiming = false;
                break;
            case 2:
                items[0].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[1].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[2].transform.DOMoveX(selectedItemPosition, 0.1f, true);
                items[3].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[4].transform.DOMoveX(DefaultPosition, 0.1f, true);
                gunAim[1].aiming = false;
                gunAim[0].aiming = false;
                break;
            case 3:
                items[0].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[1].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[2].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[3].transform.DOMoveX(selectedItemPosition, 0.1f, true);
                items[4].transform.DOMoveX(DefaultPosition, 0.1f, true);
                resetAim();
                UseItem();
                break;

            case 4:
                items[0].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[1].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[2].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[3].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[4].transform.DOMoveX(selectedItemPosition, 0.1f, true);
                resetAim();
                UseItem();
                break;
        }
    }

    private void UseItem()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            isHolding = true;
            holdTime = 0f;
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            isHolding = false;
            holdTime = 0f;
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (switches.selectedweapon == 3)
            {
                healType++;
                if (healType > 1)
                {
                    healType = 0;
                }
                else if (healType < 0)
                {
                    healType = 1;
                }
            }
            if (switches.selectedweapon == 4)
            {
                ammoType++;
                if (ammoType > 2)
                {
                    ammoType = 0;
                }
                else if (ammoType < 0)
                {
                    ammoType = 2;
                }
            }
        }


        if (isHolding)
        {
            holdTime += Time.deltaTime;

            if (holdTime >= holdDuration)
            {
                Use();
                isHolding = false;
                holdTime = 0f;
            }
        }
    }

    void Use()
    {
        if (switches.selectedweapon == 3)
        {
            if (healType == 0 && data.healthAmount1 >0)
            {
                if (stats.health < stats.MaxHealth)
                {
                    data.healthAmount1--;
                    stats.health += 1;
                    if (stats.health > stats.MaxHealth)
                    {
                        stats.health = stats.MaxHealth;
                    }
                }
                else
                    Debug.Log("Health Full");
            }
            else if (healType == 1 && data.healthAmount2 > 0)
            {
                if (stats.health < stats.MaxHealth)
                {
                    data.healthAmount2--;
                    stats.health += 10;
                    if (stats.health > stats.MaxHealth)
                    {
                        stats.health = stats.MaxHealth;
                    }
                }
                else
                    Debug.Log("Health Full");
            }
        }
        if (switches.selectedweapon == 4)
        {
            if (ammoType == 0 && data.ammoAmount1 > 0)
            {
                if(guns[0].totalAmmo < guns[0].MaxAmmo - guns[0].magazineSize)
                {
                    data.ammoAmount1--;
                    guns[0].totalAmmo += 30;
                    if (guns[0].totalAmmo > guns[0].MaxAmmo - guns[0].magazineSize)
                    {
                        guns[0].totalAmmo = guns[0].MaxAmmo - guns[0].magazineSize;
                    }
                }
                else
                {
                    Debug.Log("ammo full");
                }
            }
            else if (ammoType == 1 && data.ammoAmount2 > 0)
            {
                if (guns[1].totalAmmo < guns[1].MaxAmmo - guns[1].magazineSize)
                {
                    data.ammoAmount2--;
                    guns[1].totalAmmo += 30;
                    if (guns[1].totalAmmo > guns[1].MaxAmmo - guns[1].magazineSize)
                    {
                        guns[1].totalAmmo = guns[1].MaxAmmo - guns[1].magazineSize;
                    }
                }
                else
                {
                    Debug.Log("ammo full");
                }
            }
            else if (ammoType == 2 && data.ammoAmount3 > 0)
            {
                if (guns[2].totalAmmo < guns[2].MaxAmmo - guns[2].magazineSize)
                {
                    data.ammoAmount3--;
                    guns[2].totalAmmo += 30;
                    if (guns[2].totalAmmo > guns[2].MaxAmmo - guns[2].magazineSize)
                    {
                        guns[2].totalAmmo = guns[2].MaxAmmo - guns[2].magazineSize;
                    }
                }
                else
                {
                    Debug.Log("ammo full");
                }
            }
        }
    }
    public void resetAim()
    {
        gunAim[0].aiming = false;
        gunAim[1].aiming = false;
        gunAim[2].aiming = false;
    }

}


