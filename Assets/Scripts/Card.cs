using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{

    public int mColor;
    public bool mSpecial;
    public int mNumber;
    //Anything over 9 is a special card
    //Example: a blue +2 would be 10
    

    // Start is called before the first frame update
    void Start()
    {
        
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
    }
}
