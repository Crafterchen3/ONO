using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopulateNames : MonoBehaviour
{
    public GameObject PlayerNamePrefab;

    // Start is called before the first frame update
    void Start()
    {
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
        RenderPlayer("Benjamin");
        RenderPlayer("Bettina");
        RenderPlayer("Paul");
        RenderPlayer("Thomas");
    }

}
