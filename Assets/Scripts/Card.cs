using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private Game mGame;
    private GameObject mGameObject;
    public int mColor;
    //0 is black
    public bool mSpecial;
    public int mNumber;
    //Anything over 9 is a special card
    //Example: a blue +2 would be 10


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start was called!");
        mGameObject = GameObject.FindGameObjectWithTag("game");
        mGame = mGameObject.GetComponent<Game>();
        Debug.Log("Start(" + mColor + ", " + mNumber + ")");
        if (mSpecial)
            GetComponent<SpriteRenderer>().sprite = mGame.getSpecialCardFace(mNumber);
        else
            GetComponent<SpriteRenderer>().sprite = mGame.getCardFace(mColor, mNumber);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Initialize(int mColor, int mNumber)
    {
        this.mColor = mColor;
        this.mNumber = mNumber;

    }

    public void InitializeSpecial(int mIndex)
    {
        this.mSpecial = true;
        this.mNumber = mIndex;
        this.mColor = 0;

    }

    private int count = 0;

    void OnMouseDown()
    {
        // do something
        // Detect left mouse button click
        if (ONO.DoCardsMatch(this, mGame.cardOnTop))
        {
            mGameObject.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;
            mGame.cardOnTop = this;
            if (mSpecial)
            {
                CanvasGroup ca = GameObject.Find("Wish").GetComponent<CanvasGroup>();
                ca.alpha = 1;
                ca.interactable = true;
            }
            Destroy(gameObject);
        }
    }
}
