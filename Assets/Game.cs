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
    public GameObject cardPrefab;
    public Card cardTemplate;

    private Sprite[] allCards;

    // Start is called before the first frame update
    void Start()
    {
        cardOnTop = new Card();
        cardOnTop.mColor = 1;
        cardOnTop.mNumber = 4;
        allCards = Resources.LoadAll<Sprite>("Textures/Cards/Cards");
        Debug.Log("allCards.Length = " + allCards.Length);

        cardTemplate = cardPrefab.GetComponent<Card>();
        cardTemplate.mNumber = 5;
        cardTemplate.mColor = 2;
        Instantiate(cardPrefab, new Vector3(-8.166648f, -3.3f, 0f), Quaternion.identity );
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Sprite getCardFace(int color, int number)
    {
        // red:    0, 1, 2, 3, 4, 5, 6, 7, 8, 9, skip, exchange, +2, special  
        // yellow: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, skip, exchange, +2, special  
        // green:  0, 1, 2, 3, 4, 5, 6, 7, 8, 9, skip, exchange, +2, special  
        // blue:   0, 1, 2, 3, 4, 5, 6, 7, 8, 9, skip, exchange, +2, special  

        int index = (color - 1) * 14 + number;
        Debug.Log("(" + color + ", " + number + " index = "+index);

        return allCards[index];
    }

    public Sprite getSpecialCardFace(int index)
    {
        if (index == 1)
            return getCardFace(1, 13);
        else
            return getCardFace(2, 13);
    }

    public void Wish(int color)
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
