using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class buttonmanager : MonoBehaviour
{
    public Button playBtn, backBtn;
    private gameData stats;
    // Start is called before the first frame update
    void Start()
    {
        stats = GameObject.FindGameObjectWithTag("stats").GetComponent<gameData>();
        if (playBtn != null)
        {
            playBtn.onClick.AddListener(() =>
            {
                toggleLoadingCanvas(true);
                Loadingscript.Instance.LoadScene(1);
            });
        }

        if (backBtn != null)
        {
            backBtn.onClick.AddListener(() =>
            {
                toggleLoadingCanvas(true);
                stats.SaveData();
                Loadingscript.Instance.LoadScene(0);
            });
        }

        DestroyDuplicateCanvas();
    }

    private void toggleLoadingCanvas(bool isActive)
    {
        if(Loadingscript.Instance != null && Loadingscript.Instance.loadingCanvas != null)
            {
            Loadingscript.Instance.loadingCanvas.SetActive(isActive);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DestroyDuplicateCanvas()
    {
        GameObject[] canvases = GameObject.FindGameObjectsWithTag("LoadingCanvas");
        if (canvases.Length > 1)
        {
            foreach(GameObject canvas in canvases)
            {
                if(canvas.scene.name != null)
                {
                    Destroy(canvas);
                }
            }
        }
    }
}
