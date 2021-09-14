using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerName : MonoBehaviour
{
    private TMP_Text playerName;
    private PlayerChooser chooser;

    // Start is called before the first frame update
    void Start()
    {
        playerName = gameObject.GetComponent<TMP_Text>();
        GameObject chooserGO = GameObject.Find("Name Chooser");
        chooser = chooserGO.GetComponent<PlayerChooser>();
    }

    public void ButtonPressed()
    {
        chooser.SetChosenPlayer(playerName.text);
    }

}
