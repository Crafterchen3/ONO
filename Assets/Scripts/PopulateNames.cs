using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulateNames : MonoBehaviour
{
    public GameObject PlayerNamePrefab;

    private void RenderPlayer(string name)
    {
        GameObject clone = (GameObject)Instantiate(PlayerNamePrefab, transform);
        foreach (TMP_Text t in clone.GetComponentsInChildren<TMP_Text>())
            if (t.tag == "playerName")
                t.text = name;
    }

    public void Populate()
    {
        foreach (string n in ONO.Current.game.highScoreHistory.GetAllNames())
            RenderPlayer(n);
    }

}
