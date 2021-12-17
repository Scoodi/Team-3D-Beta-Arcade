using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMainGameScene : MonoBehaviour
{
    public void LoadMainGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
