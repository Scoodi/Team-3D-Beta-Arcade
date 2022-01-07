using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadMainGameScene : MonoBehaviour
{
    #region Load Main Game
    public void LoadMainGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    }
    #endregion

    #region Load Settings
    public void Setting()
    {
        Debug.Log("Loading Setting");
    }
    #endregion

    #region Load Main Menu
    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 0);
    }
    #endregion

    #region Reset Game
    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    #endregion


    #region Quit Game
    public void QuitGame()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
    #endregion








}










