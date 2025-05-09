using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverResult : MonoBehaviour
{
    public Text GameOverScore;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GameOverScore.text = "Score: " + GameManager.score;
    }

    public void LoadScene(int scene)
    {
        SceneManager.LoadScene(scene);
    }
}
