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

    private GameManager gm;

    public gunScript[] guns;
    // Start is called before the first frame update
    void Start()
    {
        stats = GameObject.FindGameObjectWithTag("stats").GetComponent<GameManager>();
        gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
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
                break;
            case 1:
                items[0].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[1].transform.DOMoveX(selectedItemPosition, 0.1f, true);
                items[2].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[3].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[4].transform.DOMoveX(DefaultPosition, 0.1f, true);
                break;
            case 2:
                items[0].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[1].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[2].transform.DOMoveX(selectedItemPosition, 0.1f, true);
                items[3].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[4].transform.DOMoveX(DefaultPosition, 0.1f, true);
                break;
            case 3:
                items[0].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[1].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[2].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[3].transform.DOMoveX(selectedItemPosition, 0.1f, true);
                items[4].transform.DOMoveX(DefaultPosition, 0.1f, true);
                UseItem();
                break;

            case 4:
                items[0].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[1].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[2].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[3].transform.DOMoveX(DefaultPosition, 0.1f, true);
                items[4].transform.DOMoveX(selectedItemPosition, 0.1f, true);
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
                data.healthAmount1--;
                stats.health++;
            }
            else if (healType == 1 && data.healthAmount2 > 0)
            {
                data.healthAmount2--;
                stats.health += 10;
            }
        }
        if (switches.selectedweapon == 4)
        {
            if (ammoType == 0 && data.ammoAmount1 > 0)
            {
                data.ammoAmount1--;
                guns[0].totalAmmo += 30;
            }
            else if (ammoType == 1 && data.ammoAmount2 > 0)
            {
                data.ammoAmount2--;
                guns[1].totalAmmo += 30;
            }
            else if (ammoType == 2 && data.ammoAmount3 > 0)
            {
                data.ammoAmount3--;
                guns[2].totalAmmo += 30;
            }
        }
    }
}
