using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryScript : MonoBehaviour
{
    // Start is called before the first frame update
    private gameData data;
    private int healthKit1Amount, healthKit2Amount;
    public TextMeshProUGUI healthAmount, manaAmount, health1amount, health2Amount;
    private pauseMenuController pause;

    private GameManager gameManager;

    public string itemValue;

    void Start()
    {
        data = GameObject.FindGameObjectWithTag("stats").GetComponent<gameData>();
        pause = GameObject.FindGameObjectWithTag("UI").GetComponent<pauseMenuController>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        healthAmount.text = gameManager.health.ToString() + "/ " + gameManager.MaxHealth.ToString();
        manaAmount.text = gameManager.mana.ToString() + "/ " + gameManager.maxMana.ToString();

        health1amount.text = "X " + data.healthAmount1.ToString();
        health2Amount.text = "X " + data.healthAmount2.ToString();
    }

    public void closeInventory()
    {
        pause.isinventory = false; 
    }

    public void useItem()
    {
        if (itemValue == "HealingPotion" && data.healthAmount1 > 0 && gameManager.health < gameManager.MaxHealth)
        {
            if(gameManager.health < gameManager.MaxHealth)
            {
                gameManager.health += 2;
            }
            if (gameManager.health > gameManager.MaxHealth)
            {
                gameManager.health = gameManager.MaxHealth;
            }
            data.healthAmount1 -= 1;
        }
        if (itemValue == "GreaterHealingPotion" && data.healthAmount2 > 0 && gameManager.health < gameManager.MaxHealth)
        {
            gameManager.health = gameManager.MaxHealth;
            data.healthAmount2 -= 1;
        }
    }
}    