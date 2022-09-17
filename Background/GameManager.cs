using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI overallTimer, currentTimer, FPS;
    public TextMeshPro version;
    static float overallTime;
    public static float currentTime;

    int qtyFPS = 0;
    float avgFPS = 0;

    [HideInInspector]
    public bool isDead, isFinished;

    AdsManager adsManager;

    public static Level[] levels;
    int currentLevel;

    static bool dataLoaded;

    GameObject collectable;
    public GameObject[] cages;

    private void Awake()
    {
        if (levels == null)
            levels = new Level[SceneManager.sceneCountInBuildSettings];

        currentLevel = SceneManager.GetActiveScene().buildIndex - 2;
        collectable = GameObject.FindGameObjectWithTag("Collectable");

        adsManager = FindObjectOfType<AdsManager>();
    }

    private void Start()
    {
        StartCoroutine(UpdateFPS());

        if (version != null)
            version.text = "v." + Application.version;

        if (!dataLoaded)
            LoadData();

        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            for (int i = 1; i < cages.Length + 1; i++)
            {
                if (levels[i - 1].finished)
                    Destroy(cages[i - 1]);
            }
        }

        if (currentLevel < 0)
            return;

        if (levels[currentLevel].collectable)
            Destroy(collectable);
    }

    public void FinishedLevel()
    {
        //currentTimer.color = Color.yellow;
        isFinished = true;

        if (adsManager != null)
            adsManager.ShowAd();

        if (levels[currentLevel].finished)
            return;

        levels[currentLevel].finished = true;
        SaveData();
    }
    public void GotCollectable()
    {
        if (levels[currentLevel].collectable)
            return;

        levels[currentLevel].collectable = true;
        SaveData();
    }

    void LoadData()
    {
        GameData data = SaveSystem.LoadGame(this);

        if (data == null)
        {
            dataLoaded = true;
            return;
        }

        for (int i = 0; i < levels.Length; i++)
        {
            levels[i].finished = data.finished[i];
            levels[i].collectable = data.collectable[i];
        }

        dataLoaded = true;
    }

    public void SaveData()
    {
        SaveSystem.SaveGame(this);
    }

    public void Restart()
    {
        if (adsManager != null)
            adsManager.ShowAd();

        StartCoroutine(WaitForRestart());
    }

    IEnumerator WaitForRestart()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator UpdateFPS()
    {
        float fps = 1f / Time.unscaledDeltaTime;
        ++qtyFPS;

        avgFPS += (fps - avgFPS) / qtyFPS;

        FPS.text = "FPS: " + (fps).ToString("0.00") + "\n" + "Avg: " + avgFPS.ToString("0.00") + "\n";

        yield return new WaitForSeconds(0.15f);

        StartCoroutine(UpdateFPS());
    }

    private void Update()
    {
        if(SceneManager.GetActiveScene().name != "Main Menu" && SceneManager.GetActiveScene().name != "Beta Over")
        {
            overallTime += Time.deltaTime;
            currentTime += Time.deltaTime;

            int minutes = Mathf.FloorToInt(overallTime / 60f);
            int seconds = Mathf.FloorToInt(overallTime % 60f);
            float milliseconds = overallTime % 1 * 100;

            overallTimer.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
            overallTimer.maxVisibleCharacters = 8;

            minutes = Mathf.FloorToInt(currentTime / 60f);
            seconds = Mathf.FloorToInt(currentTime % 60f);
            milliseconds = currentTime % 1 * 100;

            currentTimer.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
            currentTimer.maxVisibleCharacters = 8;
        }
    }
}

public struct Level
{
    public bool finished;
    public bool collectable;
}