using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class Loadingscript : MonoBehaviour
{

    public GameObject loadingScreen;
    public Slider progressBar;
    public TextMeshProUGUI progressText;
    public Image loadingPanel;

    public float fadeInDuration;

    public float minimumLoadingTime = 2.0f;
    public float simulateSpeed = 0.5f;
    public List<ProgressPoint> progressPoints = new List<ProgressPoint>();

    [System.Serializable]
    public class ProgressPoint
    {
        [Range(0, 1)]
        public float progressValue;
        public float pauseDuration;
    }

    public GameObject loadingCanvas;

    private static Loadingscript instance;

    public static Loadingscript Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Loadingscript>();
            }
            return instance;
        }

    }

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        MoveLoadingCanvasToDontDestroy();
    }

    private void MoveLoadingCanvasToDontDestroy()
    {
        GameObject canvas = GameObject.Find("Loading canvas");

        if(canvas == null)
        {
            canvas = Resources.FindObjectsOfTypeAll<GameObject>().FirstOrDefault(obj => obj.name == "Loading canvas");
        }

        if(canvas!=null && canvas.scene.name == null)
        {
            loadingCanvas = canvas;
            return;
        }

        if (canvas != null)
        {
            DontDestroyOnLoad(canvas);
            loadingCanvas = canvas;
        }
    }

    public void ShowLoading()
    {
        GameObject canvas = GameObject.Find("Loading canvas");
        if (canvas != null)
        {
            canvas.SetActive(true);
        }
    }

    public void HideLoading()
    {
        GameObject canvas = GameObject.Find("Loading canvas");

        if (canvas != null)
        {
            canvas.SetActive(false);
        }
    }

    public void LoadScene(int sceneIndex)
    {
        Time.timeScale = 1f;
        StartCoroutine(LoadSceneWithProgress(sceneIndex));
    }

    private IEnumerator LoadSceneWithProgress(int sceneIndex)
    {
        yield return StartCoroutine(FadeInLoadingScreen());

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        float displayProgress = 0f;
        float elapseTime = 0f;
        int currentProgressIndex = 0;
        float realProgress = 0f;

        while(!operation.isDone)
        {
            realProgress = Mathf.Clamp01(operation.progress / 0.9f);
            displayProgress = Mathf.MoveTowards(displayProgress, realProgress, simulateSpeed * Time.deltaTime);

            progressBar.value = displayProgress;
            if (progressText != null)
            {
                progressText.text = $"{(displayProgress * 100): 0}%";
            }

            if(currentProgressIndex < progressPoints.Count)
            {
                ProgressPoint point = progressPoints[currentProgressIndex];

                if(displayProgress >= point.progressValue)
                {
                    yield return new WaitForSeconds(point.pauseDuration);
                    currentProgressIndex++;
                }
            }

            elapseTime += Time.deltaTime;
            if (realProgress >= 0.9f && displayProgress >= 1.0f && elapseTime >= minimumLoadingTime)
            {
                operation.allowSceneActivation = true;
                while (!operation.isDone)
                {
                    yield return null;
                }
                yield return StartCoroutine(FadeOutLoadingScreen());

                break;
            }
            yield return null;
        }


    }

    private IEnumerator FadeInLoadingScreen()
    {
        if(progressBar != null && progressText != null)
        {
            progressBar.value = 0f;
            progressText.text = "0%";
        }
        Debug.Log("show loading");
        ShowLoading();
        SetRaycastTarget(true);
        Color startColor = new Color(0, 0, 0, 0);
        Color endColor = new Color(0, 0, 0, 1);
        float timer = 0f;

        while(timer < fadeInDuration)
        {
            float t = timer / fadeInDuration;
            UpdateAlphaForPanel(t);
            timer += Time.deltaTime;
            yield return null;
        }
        UpdateAlphaForPanel(1f);
        SetRaycastTarget(false);
    }

    private IEnumerator FadeOutLoadingScreen()
    {
        SetRaycastTarget(true);
        Color startColor = new Color(0, 0, 0, 1);
        Color endColor = new Color(0, 0, 0, 0);
        float timer = 0f;

        while (timer < fadeInDuration)
        {
            float t = timer / fadeInDuration;
            UpdateAlphaForPanel(1 - t);
            timer += Time.deltaTime;
            yield return null;
        }
        UpdateAlphaForPanel(0f);
        HideLoading();
    }

    private void UpdateAlphaForPanel(float alpha)
    {
        Graphic[] graphics = loadingScreen.GetComponentsInChildren<Graphic>(true);
        foreach(Graphic graphic in graphics)
        {
            Color color = graphic.color;
            color.a = alpha;
            graphic.color = color;
        }
    }
    private void SetRaycastTarget(bool enable)
    {
        Graphic[] graphics = loadingScreen.GetComponentsInChildren<Graphic>(true);
        foreach (Graphic graphic in graphics)
        {
            graphic.raycastTarget = enable;
        }
    }
}
