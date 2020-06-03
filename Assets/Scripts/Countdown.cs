using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Countdown : MonoBehaviour
{

    public int timeToStart = 5;
    public Text timerField;

    private int timer;
    private float timeDelta = 0;
    private bool timerStarted = false;

    private void Awake()
    {
        timerField.gameObject.SetActive(false);
    }

    public void StartTimer()
    {
        timer = timeToStart;
        timeDelta = 0;
        timerStarted = true;
        timerField.gameObject.SetActive(true);
    }

    public void StopTimer()
    {
        timerStarted = false;
        timerField.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (timerStarted)
        {
            timeDelta += Time.deltaTime;

            if (timeDelta > 1)
            {
                timer -= 1;
                timeDelta -= 1;
            }

            if (timer > 0)
            {
                timerField.text = timer.ToString();
            }
            else if (timer == 0)
            {
                timerField.text = "START";
                gameObject.GetComponent<ManagerScript>().StartGame();
            }
            else
            {
                StopTimer();
            }
        }
    }
}
