using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public Card cardOnTop;
    public Sprite wish1;
    public Sprite wish2;
    public Sprite wish3;
    public Sprite wish4;
    public SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        cardOnTop = new Card(1, 4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void wish(int color)
    {
        cardOnTop.mColor = color;
        CanvasGroup ca = GameObject.Find("Wish").GetComponent<CanvasGroup>();
        ca.alpha = 0;
        ca.interactable = false;
        switch (color)
        {
            case 1:
                spriteRenderer.sprite = wish1;
                break;
            case 2:
                spriteRenderer.sprite = wish2;
                break;
            case 3:
                spriteRenderer.sprite = wish3;
                break;
            case 4:
                spriteRenderer.sprite = wish4;
                break;


        }
    }
}
