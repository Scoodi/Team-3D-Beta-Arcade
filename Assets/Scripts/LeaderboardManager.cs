using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardManager : MonoBehaviour
{
    public GameObject leaderboardContainer;
    public Text[] leaderboardDisplay; 
    public LeaderboardEntry[] leaderboardEntryArray;
    List<LeaderboardEntry> entries = new List<LeaderboardEntry>();

    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadScores();
            SetLeaderboardDisplay();
            leaderboardContainer.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveScores();
        }
    }

    public void SubmitScore (string entryName, float distanceReached)
    {
        entries.Add(new LeaderboardEntry { name = entryName, distance = distanceReached });
    }

    void SortScores ()
    {
        entries.Sort((LeaderboardEntry x, LeaderboardEntry y) => y.distance.CompareTo(x.distance));
    }

    void SaveScores ()
    {
        XMLManager.instance.SaveScores(entries);
    }

    void LoadScores ()
    {
        entries = XMLManager.instance.LoadScores();
    }
    void SetLeaderboardDisplay()
    {
        for (int i = 0; i < leaderboardDisplay.Length; i++)
        {
            leaderboardDisplay[i].text = entries[i].name + " " + entries[i].distance;
            Debug.Log(i.ToString());
        }
    }
}
