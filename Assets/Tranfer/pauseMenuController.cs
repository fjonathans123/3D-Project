using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class pauseMenuController : MonoBehaviour
{
    public GameObject pauseMenuUI, gamePlayUI, dialogeUI;
    public bool isPaused = false;
    public bool isinventory = false;
    private gameData stats; 
    public conversationStarter cs;
    // Start is called before the first frame update
    void Start()
    {
        stats = GameObject.FindGameObjectWithTag("stats").GetComponent<gameData>();
        Resume();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        if (!isPaused)
        {
            if(cs != null)
            {
                if (cs.inConversation)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            else if (isinventory)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    public void Resume()
    {
        gamePlayUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        dialogeUI.SetActive(true);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gamePlayUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        dialogeUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Exit()
    {
        stats.SaveData();
        SceneManager.LoadScene(0);
    }
}
