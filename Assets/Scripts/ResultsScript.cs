using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultsScript : MonoBehaviour {

    public Text rank;
    public Text info;
    public Text namesBoard;
    public Text timesBoard;
    public Text finesBoard;

    readonly List<string> endings = new List<string>{ "st", "nd", "rd", "th" };

    public void Show(int userRank, int laps, float bestLap, List<float> raceTimes, List<float> finesTimes)
    {
        rank.text = string.Format(
            "{0}{1} place!", userRank, endings[userRank-1]);
        info.text = string.Format(
            "Total laps: {0}\nBest lap: {1:f2}\nDeath penalty: {2:f2}", laps, bestLap, finesTimes[userRank - 1]);

        string names = "",
            results = "",
            fines = "";
        for (int i = 1; i < raceTimes.Count+1; ++i)
        {
            if (i != userRank)
            {
                names += string.Format("{0}. AI\n", i);
            } else
            {
                names += string.Format("{0}. You\n", i);
            }
            results += string.Format("{0:f2}\n", raceTimes[i - 1]);
            fines += string.Format("(+{0:f0})\n", finesTimes[i-1]);
        }
        namesBoard.text = names;
        timesBoard.text = results;
        finesBoard.text = fines;
    }
}
