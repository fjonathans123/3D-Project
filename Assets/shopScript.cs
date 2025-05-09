using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class shopScript : MonoBehaviour
{
    public TextMeshProUGUI[] GunLevelText, GunPriceText;
    public int[] Gunlvl, GunPrice;

    public TextMeshProUGUI HealthKitText;
    public int HealthKitAmount, AmmoAmount;

    public TextMeshProUGUI moneyTxt;
    public TextMeshProUGUI[] ammoText;
    private gameData data;
    public conversationStarter cs;
    // Start is called before the first frame update
    void Start()
    {
        data = GameObject.FindGameObjectWithTag("stats").GetComponent<gameData>();
        Gunlvl[0] = data.levelwep1;
        Gunlvl[1] = data.levelwep2;
        Gunlvl[2] = data.levelwep3;
        GunPrice[0] = data.pricewep1;
        GunPrice[1] = data.pricewep2;
        GunPrice[2] = data.pricewep3;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < GunLevelText.Length; i++)
        {
            GunLevelText[i].text = Gunlvl[i].ToString();
        }

        for (int i = 0; i < GunPriceText.Length; i++)
        {
            GunPriceText[i].text = GunPrice[i].ToString();
        }

        HealthKitText.text = HealthKitAmount.ToString();
        ammoText[0].text = AmmoAmount.ToString();
        ammoText[1].text = AmmoAmount.ToString();
        ammoText[2].text = AmmoAmount.ToString();

        moneyTxt.text = data.money.ToString();
    }

    public void upgradeWeapon(int weaponIndex)
    {
       if(data.money > GunPrice[weaponIndex])
        {
            Gunlvl[weaponIndex]++;
            data.money -= GunPrice[weaponIndex];
            if(weaponIndex == 0)
            {
                GunPrice[weaponIndex] += 1;
                data.levelwep1 = Gunlvl[weaponIndex];
                data.pricewep1 = GunPrice[weaponIndex];
            }
            else if (weaponIndex == 1)
            {
                GunPrice[weaponIndex] += 3;
                data.levelwep2 = Gunlvl[weaponIndex];
                data.pricewep2= GunPrice[weaponIndex];
            }
            else if (weaponIndex == 2)
            {
                GunPrice[weaponIndex] += 4;
                data.levelwep3 = Gunlvl[weaponIndex];
                data.pricewep3 = GunPrice[weaponIndex];
            }

        }

    }

    public void buyitem(int itemindex)
    {
        int healthKitPrice = 20;
            if (itemindex==0 && HealthKitAmount > 0 && data.money > healthKitPrice)
        {
            HealthKitAmount--;
            data.money -= healthKitPrice;
            data.healthAmount1++;
        }

        if(itemindex == 1)
        {
            int ammoPrice = 10;
            if (AmmoAmount > 0 && data.money > ammoPrice)
            {
                AmmoAmount--;
                data.money -= ammoPrice;
                data.ammoAmount1++;
            }
        }

        if (itemindex == 2)
        {
            int ammoPrice = 5;
            if (AmmoAmount > 0 && data.money > ammoPrice)
            {
                AmmoAmount--;
                data.money -= ammoPrice;
                data.ammoAmount1++;
            }
        }
    }

    public void closeShop()
    {
        cs.inConversation = false;
    }
}
