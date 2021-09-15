using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ONOButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        ONO.Current.OnoButtonPresent(gameObject);
    }

    private void OnMouseDown()
    {
        ONO.Current.game.OnoPressed();
    }
}
