using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class gameData : MonoBehaviour
{
    [Header ("Stats")]
    public int money, enemyDestroyed;
    //public int[] weaponDamage, weaponLevel, weaponPrice;

    [Header("Level")]
    public int levelwep1, levelwep2, levelwep3;

    [Header("Stats")]
    public int pricewep1, pricewep2, pricewep3;

    public int healthAmount1, healthAmount2, ammoAmount1, ammoAmount2, ammoAmount3;

    public void SaveData()
    {
        saveSystem.SaveData(this);
    }

    public void LoadData()
    {
        playerData data = saveSystem.LoadData();
        string path = Application.persistentDataPath + "/saves.game";

        if (!File.Exists(path))
        {
            money = 0;
            enemyDestroyed = 0;
            levelwep1 = 1;
            levelwep2 = 1;
            levelwep3 = 1;
            pricewep1 = 1;
            pricewep2 = 3;
            pricewep3 = 4;
            healthAmount1 = 0;
            healthAmount2 = 0;
            ammoAmount1 = 0;
            ammoAmount2 = 0; 
            ammoAmount3 = 0;
        }
        else
        {
        money = data.money;
        levelwep1 = data.levelwep1;
        levelwep2 = data.levelwep2;
        levelwep3 = data.levelwep3;
        pricewep1 = data.pricewep1;
        pricewep2 = data.pricewep2;
        pricewep3 = data.pricewep3;
        healthAmount1 = data.healthKitAmount1;
        healthAmount2 = data.healthKitAmount2;
        ammoAmount1 = data.ammoAmount1; 
        ammoAmount2 = data.ammoAmount1;
        ammoAmount3 = data.ammoAmount1;

        }


    }
}
