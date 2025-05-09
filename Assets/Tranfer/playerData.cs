using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class playerData
{
    public int money;
    public int enemyDestroyed;
    public int[] weaponDamage, weaponLevel, weaponPrice;


    [Header("Level")]
    public int levelwep1, levelwep2, levelwep3;

    [Header("Stats")]
    public int pricewep1, pricewep2, pricewep3;

    public int ammoAmount1, ammoAmount2, ammoAmount3, healthKitAmount1, healthKitAmount2;

    public playerData(gameData data)
    {
        money = data.money;
        enemyDestroyed = data.enemyDestroyed;
        levelwep1 = data.levelwep1;
        levelwep2 = data.levelwep2;
        levelwep3 = data.levelwep3;
        pricewep1 = data.pricewep1;
        pricewep2 = data.pricewep2;
        pricewep3 = data.pricewep3;
        ammoAmount1 = data.ammoAmount1;
        ammoAmount2 = data.ammoAmount2;
        ammoAmount3 = data.ammoAmount3;
        healthKitAmount1 = data.healthAmount1;
        healthKitAmount2 = data.healthAmount2;
    }
}
