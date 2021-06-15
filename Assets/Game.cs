using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public Card cardOnTop;

    // Start is called before the first frame update
    void Start()
    {
        cardOnTop = new Card(1, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
