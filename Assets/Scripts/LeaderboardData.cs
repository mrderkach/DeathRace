using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LeaderboardData")]
public class LeaderboardData : ScriptableObject
{

    public List<string> names;
    public List<float> results;
}