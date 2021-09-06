using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnplayedCards : MonoBehaviour
{
    private Game game;

    // Start is called before the first frame update
    void Start()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("game");
        game = gameObject.GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        game.Draw(1);
    }

}
