using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ONO : MonoBehaviour
{

    private static int Rounds;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void setRounds(int rounds)
    {
        Rounds = rounds;
    }

    public static bool DoCardsMatch(Card card1, Card card2)
    {
        
        if (card1.mColor == card2.mColor || card1.mColor == 0)
        {
            return true;
        }

        if (card1.mNumber == card2.mNumber)
        {
            return true;
        }
        
        return false;
    }
}
