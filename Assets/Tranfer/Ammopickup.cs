using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammopickup : MonoBehaviour
{
    public List<gunScript> guns = new List<gunScript>();
    public Weaponswap weapon;
    void Start()
    {
        weapon = GameObject.Find("weaponSwitch").GetComponent<Weaponswap>();
    }

     void Update()
    {
        FindGuns();
    }

    void FindGuns()
    {
        guns.Clear();
        gunScript[] gunsFound = Resources.FindObjectsOfTypeAll<gunScript>();
        guns.AddRange(gunsFound);
    }
    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (weapon.selectedweapon == 0)
            {
                if (guns[0].totalAmmo < guns[0].MaxAmmo - guns[0].magazineSize)
                {
                    if (((guns[0].MaxAmmo - guns[0].magazineSize) - guns[0].totalAmmo) > guns[0].magazineSize)
                    {
                        Debug.Log("Ammo refill ");
                        guns[0].totalAmmo += guns[0].magazineSize;
                    }
                    else
                    {
                        Debug.Log("Ammo refill full");
                        guns[0].totalAmmo = guns[0].MaxAmmo - guns[0].magazineSize;
                    }
                    Destroy(gameObject);
                }
                
            }
           else if (weapon.selectedweapon == 1)
            {
                if (guns[1].totalAmmo < guns[1].MaxAmmo - guns[1].magazineSize)
                {
                    if (((guns[1].MaxAmmo - guns[1].magazineSize) - guns[1].totalAmmo) > guns[1].magazineSize)
                    {
                        guns[1].totalAmmo += guns[1].magazineSize;
                    }
                    else
                    {
                        guns[1].totalAmmo = guns[1].MaxAmmo - guns[1].magazineSize;
                    }
                    Destroy(gameObject);
                }
            }
            else if (weapon.selectedweapon == 2)
            {
                if (guns[2].totalAmmo < guns[2].MaxAmmo - guns[2].magazineSize)
                {
                    if (((guns[2].MaxAmmo - guns[2].magazineSize) - guns[2].totalAmmo) > guns[2].magazineSize)
                    {
                        guns[2].totalAmmo += guns[2].magazineSize;
                    }
                    else
                    {
                        guns[2].totalAmmo = guns[2].MaxAmmo - guns[2].magazineSize;
                    }
                    Destroy(gameObject);
                }
            }

        }
    }
}

