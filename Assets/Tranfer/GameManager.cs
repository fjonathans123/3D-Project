using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public Text healthText, enemyHPTxt, scoretext, elementtext, armortext; 
    public float health, mana;
    public static int score;
    public int element;
    private ElementController gun;
    public float FireMana, WaterMana, LightningMana, armor, healthcooldown, Maxarmor, MaxHealth, maxMana;

    public Image armorImg, healthImg, overshieldImg;

    public Weaponswap switches;
    public WheelScrolling items;

    public TextMeshProUGUI moneyTxt;
    private gameData data;


    // Start is called before the first frame update
    void Start()
    {
        gun = GameObject.Find("Player").GetComponent<ElementController>();
        data = GameObject.FindGameObjectWithTag("stats").GetComponent<gameData>();
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = "health: " + health;
        scoretext.text = "score: " + score;
        armortext.text = "armor: " + (int)armor;
        moneyTxt.text = "Money: " + data.money;



    if (switches.selectedweapon < 3)
        {
            if (gun.element == 1)
            {
                elementtext.text = "Element: None";
            }
            else if (gun.element == 2)
            {
                elementtext.text = "Element: Fire";
            }
            else if (gun.element == 3)
            {
                elementtext.text = "Element: Ice";
            }
            else if (gun.element == 4)
            {
                elementtext.text = "Element: Lightning";
            }
        }

        if (switches.selectedweapon == 3)
        {
            if (items.healType == 0)
            {
                elementtext.text = "Normal Heal";
            }
            else if (items.healType == 1)
            {
                elementtext.text = "Greater Heal";
            }
        }
        if (switches.selectedweapon == 4)
        {
            if (items.ammoType == 0)
            {
                elementtext.text = "Gun 1 Ammo";
            }
            else if (items.ammoType == 1)
            {
                elementtext.text = "Gun 2 Ammo";
            }
            else if (items.ammoType == 2)
            {
                elementtext.text = "Gun 3 Ammo";
            }
        }

        if (healthcooldown <= 0f && health < MaxHealth/2)
        {
            health += 0.1f;
            if (health > MaxHealth/2)
            {
                health = MaxHealth/2;
            }
        }
        else if (healthcooldown > 0f)
        {
            healthcooldown -= 0.01f;
        }

        if(armor > Maxarmor + 0.01f)
        {
            overshieldImg.fillAmount = (armor - Maxarmor) / (Maxarmor / 2);
            armor -= 0.01f;
        }
        else
        {
            overshieldImg.fillAmount = 0f;
        }

        armorImg.fillAmount = armor / Maxarmor;
        healthImg.fillAmount = health / MaxHealth;
    }
}
