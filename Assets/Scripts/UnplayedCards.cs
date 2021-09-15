using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnplayedCards : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        ONO.Current.UnplayedCardsPresent(gameObject);
    }

    void OnMouseDown()
    {
        ONO.Current.game.DrawCard();
    }

}
