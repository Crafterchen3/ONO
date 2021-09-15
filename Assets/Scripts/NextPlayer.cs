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
        gameObject.SetActive(false);
        foreach (TMP_Text t in gameObject.GetComponentsInChildren<TMP_Text>())
            if (t.tag == "playerName")
                text = t;
        ONO.Current.NextPlayerPopupPresent(this, gameObject);
    }

    public void Show(string name)
    {
        text.text = name;
        gameObject.SetActive(true);
    }

}
