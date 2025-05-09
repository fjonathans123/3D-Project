using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class titleScreen : MonoBehaviour
{
    public Text money, enemyDestroyed;
    private gameData data;

    void Start()
    {
        Time.timeScale = 1f;
        data = GameObject.FindGameObjectWithTag("stats").GetComponent<gameData>();
        data.LoadData();
        //data.money = 300;
        data.SaveData();
    }

    void Update()
    {
        money.text = "Money: " + data.money.ToString();
        enemyDestroyed.text = "Enemy Destroyed; " + data.enemyDestroyed.ToString();
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}