using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIScript : MonoBehaviour
{
    public PlayerScript player;
    public LeaderboardManager leaderboard;

    public Text spsDisplay;
    public Text batteryDisplay;
    public Text distanceDisplay;

    public GameObject deathDisplay;
    public Text distanceAchievedDisplay;
    public Text bestDistanceDisplay;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateUI()
    {
        spsDisplay.text = "SPS = " + Mathf.Round(player.currentSpeed).ToString();
        distanceDisplay.text = player.maxDistanceTravelled.ToString();
        if (player.batteryRemaining > 0)
        {
            batteryDisplay.text = player.batteryRemaining.ToString() + "/" + player.maxBatteryLevel.ToString();
        } else
        {
            batteryDisplay.text = "Dead";
        }
    }

    public void DeathUI (float distanceAchieved, float bestDistance)
    {
        deathDisplay.SetActive(true);
        distanceAchievedDisplay.text = distanceAchieved.ToString();
        bestDistanceDisplay.text = bestDistance.ToString();
    }
}
