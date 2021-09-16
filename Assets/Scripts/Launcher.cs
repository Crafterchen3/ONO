using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Launcher : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        ONO.Current.LauncherPresent(this);
    }

    public void Show()
    {
        ONO.Current.gameGO.SetActive(false);
        gameObject.SetActive(true);
    }

    public void NewGame()
    {
        ONO.Current.game.Show();
        ONO.Current.playerNamesGO.SetActive(true);
        gameObject.SetActive(false);
    }

    public void HighScores()
    {
        gameObject.SetActive(false);
        ONO.Current.highscores.Show();
    }

    public void Quit()
    {
        Application.Quit();
    }

}
