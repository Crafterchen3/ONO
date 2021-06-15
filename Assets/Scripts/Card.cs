using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private Game game;
    private GameObject mgame;
    public int mColor;
    //0 is black
    public bool mSpecial;
    public int mNumber;
    //Anything over 9 is a special card
    //Example: a blue +2 would be 10
    

    // Start is called before the first frame update
    void Start()
    {
        mgame = GameObject.FindGameObjectWithTag("game");
        game = mgame.GetComponent<Game>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Card(int mColor, int mNumber)
    {
        this.mColor = mColor;
        this.mNumber = mNumber;
    }

    public Card(bool mSpecial, int mNumber)
    {
        this.mSpecial = mSpecial;
        this.mNumber = mNumber;
        this.mColor = 0;
    }

    void OnMouseOver()
    {
        // do something
        // Detect left mouse button click
        if (Input.GetMouseButtonDown(0))
        {

            Debug.Log("click");



            if (ONO.DoCardsMatch(this, game.cardOnTop)) {
                mgame.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
                game.cardOnTop = this;
                if (mSpecial)
                {
                    CanvasGroup ca =GameObject.Find("Wish").GetComponent<CanvasGroup>();
                    ca.alpha = 1;
                    ca.interactable = true;
                }
                Destroy(gameObject);
            }
                    
        }

    }
}
