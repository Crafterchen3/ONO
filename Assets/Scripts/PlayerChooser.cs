using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChooser : MonoBehaviour
{
    private PlayerNames playerNames;

    // Start is called before the first frame update
    void Start()
    {
        GameObject namesGO = GameObject.Find("Player Names");
        playerNames = namesGO.GetComponent<PlayerNames>();
        playerNames.NameChooserPresent(gameObject);
        gameObject.SetActive(false);
    }

    public void SetChosenPlayer(string name)
    {
        playerNames.SetChosenPlayer(name);
        gameObject.SetActive(false);
    }

    public void Cancel()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
