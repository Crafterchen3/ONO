using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highscores : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        ONO.Current.HighscoresPresent(this);
    }

    public void Show()
    {
        gameObject.GetComponentInChildren<PopulateHighscores>().Populate();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        ONO.Current.launcher.Show();
    }
}
