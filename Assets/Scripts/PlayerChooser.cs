using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChooser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        ONO.Current.NameChooserPresent(this);
    }

    public void Show()
    {
        gameObject.GetComponentInChildren<PopulateNames>().Populate();
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetChosenPlayer(string name)
    {
        ONO.Current.playerNames.SetChosenPlayer(name);
        Hide();
    }

    public void Cancel()
    {
        Hide();
    }
}
