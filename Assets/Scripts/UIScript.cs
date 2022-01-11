using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIScript : MonoBehaviour
{
    public PlayerScript player;
    public LeaderboardManager leaderboard;

    public Text spsDisplay;
    public Text batteryDisplay;
    public Text distanceDisplay;

    float distanceStorage;

    public GameObject deathDisplay;
    public Text distanceAchievedDisplay;
    public Text bestDistanceDisplay;

    public GameObject submitScoreUI;
    public InputField nameInput;

    public GameObject leaderboardUI;

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
            batteryDisplay.text = "DEAD";
        }
    }

    public void DeathUI (float distanceAchieved)
    {
        deathDisplay.SetActive(true);
        distanceStorage = distanceAchieved;
        distanceAchievedDisplay.text = "YOU REACHED: " + distanceAchieved.ToString();
        bestDistanceDisplay.text = "YOUR BEST: " + leaderboard.GetCurrentBest().ToString();
    }

    public void SubmitUI (string openOrSubmit)
    {
        if (openOrSubmit == "open")
        {
            submitScoreUI.SetActive(true);
        } else
        {
            leaderboard.SubmitScore(nameInput.text, distanceStorage);
            submitScoreUI.SetActive(false);
            leaderboardUI.SetActive(true);
            leaderboard.SetLeaderboardDisplay();
        }
    }

    public void CloseLeaderboard()
    {
        leaderboardUI.SetActive(false);
    }

    public void MenuorRetry (string menuOrRetry)
    {
        if (menuOrRetry == "menu")
        {
            //load menu scene
            SceneManager.LoadScene("NewMainMenu");
        } else if (menuOrRetry == "retry")
        {
            //reload this scene
            SceneManager.LoadScene("SampleScene");
        }
    }
}
