using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    #region Main Game
    public void MainGame()
    {
        SceneManager.LoadScene("SampleScene");
    }
    #endregion

    #region Tutorial
    public void TutorialGame ()
    {
        SceneManager.LoadScene("TutorialMap");
        Time.timeScale = 1f;
    }
    #endregion

    #region Quit Game
    public void QuitGame ()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    #endregion
}
