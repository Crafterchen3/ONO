using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NextPlayer : MonoBehaviour
{
    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        foreach (TMP_Text t in gameObject.GetComponentsInChildren<TMP_Text>())
            if (t.tag == "playerName")
                text = t;
        GameObject controllerObject = GameObject.FindGameObjectWithTag("game");
        Game game = controllerObject.GetComponent<Game>();
        game.NextPlayerPopupPresent(this, gameObject);
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Show(string name)
    {
        text.text = name;
        gameObject.SetActive(true);
    }

}
