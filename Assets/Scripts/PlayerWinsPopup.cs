using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerWinsPopup : MonoBehaviour
{

    private TMP_Text text;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
        foreach (TMP_Text t in gameObject.GetComponentsInChildren<TMP_Text>())
            if (t.tag == "playerName")
                text = t;
        ONO.Current.PlayerWinsPopupPresent(this);
    }

    void OnMouseDown()
    {
        Hide();
        ONO.Current.game.DisplayScore();
    }

    public void Show(string name)
    {
        text.text = name;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
