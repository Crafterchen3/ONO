using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulateNames : MonoBehaviour
{
    public GameObject PlayerNamePrefab;
    private Game game;

    // Start is called before the first frame update
    void Start()
    {
        GameObject controllerObject = GameObject.FindGameObjectWithTag("game");
        game = controllerObject.GetComponent<Game>();

        Populate();
    }

    void RenderPlayer(string name)
    {
        GameObject clone = (GameObject)Instantiate(PlayerNamePrefab, transform);
        TMP_Text text = clone.GetComponent<TMP_Text>();
        text.text = name;
    }

    void Populate()
    {
        foreach (string n in game.highScoreHistory.GetAllNames())
            RenderPlayer(n);
    }

}
