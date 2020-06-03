using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishScript : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CarMarker") && 
            other.transform.parent.parent.gameObject.GetComponent<
                UnityStandardAssets.Utility.WaypointProgressTracker>().finishingLap)
        {
            float curTime = Time.time;
            ++other.transform.parent.parent.GetComponent<CarGeneral>().Lap;
            other.transform.parent.parent.GetComponent<CarGeneral>().bestLap = Mathf.Min(
                other.transform.parent.parent.GetComponent<CarGeneral>().bestLap,
                curTime - other.transform.parent.parent.GetComponent<CarGeneral>().lapStart);
            other.transform.parent.parent.GetComponent<CarGeneral>().lapStart = curTime;
            other.transform.parent.parent.gameObject.GetComponent<
                UnityStandardAssets.Utility.WaypointProgressTracker>().finishingLap = false;
            if (other.transform.parent.parent.GetComponent<CarGeneral>().Lap >
                other.transform.parent.parent.GetComponent<CarGeneral>()._manager.laps)
            {
                other.transform.parent.parent.GetComponent<CarGeneral>().finished = true;
                other.transform.parent.parent.GetComponent<CarGeneral>().finishTime =
                    curTime - other.transform.parent.parent.GetComponent<CarGeneral>()._manager.BeginOfTime;
            }
        }
    }
}

