using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Xml.Serialization;
using System.IO;

public class XMLManager : MonoBehaviour
{
    public static XMLManager instance;
    public Leaderboard leaderboard;

    [System.Serializable]
    public class Leaderboard
    {
        public List<LeaderboardEntry> list = new List<LeaderboardEntry>();
    }

    void Awake()
    {
        instance = this;

        if (!Directory.Exists(Application.persistentDataPath + "/Leaderboard/"))
        {
            Directory.CreateDirectory(Application.persistentDataPath + "/Leaderboard/");
        }
    }
    public void SaveScores(List<LeaderboardEntry> scoresToSave)
    {
        leaderboard.list = scoresToSave;
        XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
        FileStream stream = new FileStream(Application.persistentDataPath + "/Leaderboard/highscores.xml", FileMode.Create);
        serializer.Serialize(stream, leaderboard);
        stream.Close();
    }

    public List<LeaderboardEntry> LoadScores()
    {
        if (File.Exists(Application.persistentDataPath + "/Leaderboard/highscores.xml"))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(Leaderboard));
            FileStream stream = new FileStream(Application.persistentDataPath + "/Leaderboard/highscores.xml", FileMode.Open);
            leaderboard = serializer.Deserialize(stream) as Leaderboard;
        }

        return leaderboard.list;
    }
}
