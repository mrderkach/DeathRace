using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerScript : MonoBehaviour {
    public bool racing = false;
    public bool gamePaused = false;
    public int laps = 3;
    public float iconicTime = 50;
    public float DeathFine = 10;

    public GameObject pauseMenu;
    public GameObject mainMenu;
    public GameObject title;
    public ResultsScript resultsMenu;
    public GameObject Leaderboard;
    public GameObject enterYourName;
    public LeaderboardData leaderboardData;
    public Text yourTimeText;
    public Text yourTimeTextWin;
    public Text BestScoreNames;
    public Text BestScoreResults;
    public InputField enterNameInput;
    public Text inGameText;
    public GameObject healthUI;

    public GameObject player;
    public GameObject playerPosition;
    public GameObject playerPositionFinish;
    public GameObject enemies;
    public GameObject enemiesPositions;
    public GameObject enemiesPositionsFinish;
    public GameObject playerCamera;
    public GameObject menuCamera;
    public GameObject finishCamera;

    public float TotalTime = 0;
    public float BeginOfTime { get; private set; }
    public float maxDist = 0;

    // Use this for initialization
    void Start()
    {
        TurnOnMainMenu();
        racing = false;
        player.SetActive(false);
        foreach (Transform i in enemies.transform)
        {
            i.gameObject.SetActive(false);
        }
        playerCamera.SetActive(false);
        menuCamera.SetActive(true);
        finishCamera.SetActive(false);
    }

    public void TurnOnLeaderboard()
    {
        mainMenu.SetActive(false);
        title.SetActive(false);
        yourTimeText.gameObject.SetActive(false);
        enterYourName.SetActive(false);
        UpdateLeaderboardText();
        Leaderboard.SetActive(true);
    }

    public void TurnOnMainMenu()
    {
        mainMenu.SetActive(true);
        title.SetActive(true);
        Leaderboard.SetActive(false);
        enterYourName.SetActive(false);
        inGameText.transform.parent.gameObject.SetActive(false);
        resultsMenu.gameObject.SetActive(false);
        healthUI.SetActive(false);
        healthUI.GetComponent<HealthManager>().damageImage.gameObject.SetActive(false);
        pauseMenu.SetActive(false);
}

private void Update()
    {
        if (racing && Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused == true)
            {
                ContinueGame();
            }
            else if (gamePaused == false)
            {
                Time.timeScale = 0;
                gamePaused = true;
                healthUI.GetComponent<HealthManager>().damageImage.gameObject.SetActive(false);
                pauseMenu.SetActive(true);
            }
        }
        if (racing)
        {
            float curTime = player.GetComponent<CarGeneral>().finishTime;
            if (!player.GetComponent<CarGeneral>().finished)
            {
                healthUI.GetComponent<HealthManager>().UpdateRanks(GetPositions());
                curTime = Time.time - BeginOfTime;
            }
            string lap = "Finished";
            if (player.GetComponent<CarGeneral>().Lap == 0)
            {
                lap = string.Format("1/{0}", laps);
            } else if (player.GetComponent<CarGeneral>().Lap <= laps)
            {
                lap = string.Format("{0}/{1}", 
                    player.GetComponent<CarGeneral>().Lap.ToString(), laps);
            }
            inGameText.text = string.Format(
    "Lap: {0}\nTime: {1:f2}\nSpeed: {2:f2}\n",
    lap,
    curTime, 
    player.GetComponent<UnityStandardAssets.Vehicles.Car.CarController>().CurrentSpeedKMH);
        }
    }
    public void PrepareStartGame()
    {
        Time.timeScale = 1;
        mainMenu.SetActive(false);
        title.SetActive(false);
        inGameText.transform.parent.gameObject.SetActive(true);
        healthUI.GetComponent<HealthManager>().UpdateHealth(player.GetComponent<CarGeneral>().maxHealth, player);
        healthUI.SetActive(true);
        healthUI.GetComponent<HealthManager>().damageImage.gameObject.SetActive(true);

        player.SetActive(true);
        player.GetComponent<CarGeneral>().StartRace(playerPosition.transform);
        for (int i = 0; i < enemies.transform.childCount; ++i)
        {
            enemies.transform.GetChild(i).gameObject.SetActive(true);
            enemies.transform.GetChild(i).gameObject.GetComponent<CarGeneral>().StartRace(
                enemiesPositions.transform.GetChild(i));
            healthUI.GetComponent<HealthManager>().UpdateHealth(
                enemies.transform.GetChild(i).gameObject.GetComponent<CarGeneral>().maxHealth, 
                enemies.transform.GetChild(i).gameObject);
        }
        playerCamera.SetActive(true);
        menuCamera.SetActive(false);
        gameObject.GetComponent<Countdown>().StartTimer();
    }

    public void StartGame()
    {
        BeginOfTime = Time.time;
        player.GetComponent<CarGeneral>().lapStart = BeginOfTime;
        for (int i = 0; i < enemies.transform.childCount; ++i)
        {
            enemies.transform.GetChild(i).gameObject.GetComponent<CarGeneral>().lapStart = 
                BeginOfTime;
        }
        racing = true;
    }

        public void ShowResults()
    {
        healthUI.GetComponent<HealthManager>().damageImage.gameObject.SetActive(false);
        player.GetComponent<CarGeneral>().ResetCar(playerPositionFinish.transform);
        for (int i = 0; i < enemies.transform.childCount; ++i)
        {
            if (!enemies.transform.GetChild(i).gameObject.GetComponent<CarGeneral>().finished)
            {
                enemies.transform.GetChild(i).gameObject.GetComponent<CarGeneral>().AssignTime();
            }
            enemies.transform.GetChild(i).gameObject.GetComponent<CarGeneral>().ResetCar(
                enemiesPositionsFinish.transform.GetChild(i));
        }
        playerCamera.SetActive(false);
        finishCamera.SetActive(true);

        int rank = 1;
        float bestLap = player.GetComponent<CarGeneral>().bestLap;
        List<float> times = new List<float>
        {
            TotalTime
        };
        List<float> fines = new List<float> {};
        for (int i = 0; i < enemies.transform.childCount; ++i)
        {
            times.Add(enemies.transform.GetChild(i).gameObject.GetComponent<CarGeneral>().finishTime +
                enemies.transform.GetChild(i).gameObject.GetComponent<CarGeneral>().CurFine);
        }
        times.Sort();
        for (int i = 0; i < times.Count; ++i)
        {
            if (times[i] == TotalTime)
            {
                rank = i + 1;
                fines.Add(player.GetComponent<CarGeneral>().CurFine);
            } else
            {
                for (int j = 0; j < enemies.transform.childCount; ++j)
                {
                    if (times[i] == enemies.transform.GetChild(j).gameObject.GetComponent<CarGeneral>().finishTime +
                        enemies.transform.GetChild(j).gameObject.GetComponent<CarGeneral>().CurFine)
                    {
                        fines.Add(enemies.transform.GetChild(j).gameObject.GetComponent<CarGeneral>().CurFine);
                    }
                }
            }
        }

        resultsMenu.Show(rank, laps, bestLap, times, fines);
        resultsMenu.gameObject.SetActive(true);
    }

    public void FinishGame()
    {
        Time.timeScale = 1;
        inGameText.transform.parent.gameObject.SetActive(false);
        healthUI.SetActive(false);
        healthUI.GetComponent<HealthManager>().damageImage.gameObject.SetActive(false);
        gameObject.GetComponent<Countdown>().StopTimer();
        racing = false;
        player.SetActive(false);
        for (int i = 0; i < enemies.transform.childCount; ++i)
        {
            enemies.transform.GetChild(i).gameObject.SetActive(false);
        }
        playerCamera.SetActive(false);
        menuCamera.SetActive(true);
    }

    public void ContinueGame()
    {
        Time.timeScale = 1;
        gamePaused = false;
        pauseMenu.SetActive(false);
        healthUI.GetComponent<HealthManager>().damageImage.gameObject.SetActive(true);
    }

    public void RestartGame()
    {
        pauseMenu.SetActive(false);
        FinishGame();
        PrepareStartGame();
    }

    public void ExitToMenu()
    {
        pauseMenu.SetActive(false);
        FinishGame();
        TurnOnMainMenu();
    }

    List<GameObject> GetPositions()
    {
        List<GameObject> result = new List<GameObject> ();
        List<KeyValuePair<float, GameObject>> distances = new List<KeyValuePair<float, GameObject>>
        {
            new KeyValuePair<float, GameObject> 
            (player.GetComponent<UnityStandardAssets.Utility.WaypointProgressTracker>().ProgressDistance, player)
        };
        for (int i = 0; i < enemies.transform.childCount; ++i)
        {
            distances.Add(new KeyValuePair<float, GameObject>(
                enemies.transform.GetChild(i).gameObject.GetComponent<
                    UnityStandardAssets.Utility.WaypointProgressTracker>().ProgressDistance,
                enemies.transform.GetChild(i).gameObject));
        }
        distances.Sort(Compare2);
        for (int i = 0; i < distances.Count; ++i)
        {
            result.Add(distances[i].Value);
        }
        return result;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void TurnOnLeaderboardWithUpdate()
    {
        finishCamera.SetActive(false);
        FinishGame();
        resultsMenu.gameObject.SetActive(false);
        int num = leaderboardData.results.Count;
        if (num == 0 || leaderboardData.results[Mathf.Min(num - 1, 10)] > TotalTime)
        {
            enterYourName.SetActive(true);
            yourTimeTextWin.text = string.Format("Your time: {0}\nPlease enter your name:", TotalTime);
        }
        else
        {
            TurnOnLeaderboardWithResult();
        }
    }

    public void TurnOnLeaderboardWithResult()
    {
        if (enterYourName.activeSelf)
        {
            string name = enterNameInput.text;
            if (name.Length == 0)
            {
                name = "Player";
            }
            int i = 0;

            while (leaderboardData.results[i] < TotalTime) ++i;

            leaderboardData.results.Insert(i, TotalTime);
            leaderboardData.names.Insert(i, name);
            if (leaderboardData.results.Count > 9)
            {
                leaderboardData.results.RemoveAt(10);
                leaderboardData.names.RemoveAt(10);
            }
        }
        mainMenu.SetActive(false);
        title.SetActive(false);
        enterYourName.SetActive(false);
        yourTimeText.gameObject.SetActive(true);
        yourTimeText.text = string.Format("Your time: {0}", TotalTime);
        UpdateLeaderboardText();
        Leaderboard.SetActive(true);
    }

    void UpdateLeaderboardText()
    {
        string score_names = "",
            score_results = "";
        for (int i = 0; i < 10; ++i)
        {
            score_names += string.Format("{0}. {1}\n", i + 1, leaderboardData.names[i]);
            score_results += string.Format("{0:f2}\n", leaderboardData.results[i]);
        }
        BestScoreNames.text = score_names;
        BestScoreResults.text = score_results;
    }

    static int Compare2(KeyValuePair<float, GameObject> a, KeyValuePair<float, GameObject> b)
    {
        return a.Key.CompareTo(b.Key);
    }
}
