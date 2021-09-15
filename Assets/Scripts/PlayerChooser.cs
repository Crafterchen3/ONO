using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChooser : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        ONO.Current.NameChooserPresent(gameObject);
    }

    public void SetChosenPlayer(string name)
    {
        ONO.Current.playerNames.SetChosenPlayer(name);
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }
}
