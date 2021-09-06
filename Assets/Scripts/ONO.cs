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

    public static bool DoCardsMatch(CardDescriptor card1, CardDescriptor card2)
    {
        
        if (card1.Color == card2.Color || card1.Color == 0)
        {
            return true;
        }

        if (card1.Number == card2.Number)
        {
            return true;
        }
        
        return false;
    }
}
