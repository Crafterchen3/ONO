using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    public CardDescriptor cardOnTop;
    public Sprite wish1;
    public Sprite wish2;
    public Sprite wish3;
    public Sprite wish4;
    public GameObject cardPrefab;

    private Sprite[] allCardFaces;
    private SpriteRenderer spriteRenderer;
    private GameObject wishPopup;

    private bool onoPressed = false;

    private List<CardDescriptor> allCards = new List<CardDescriptor>();
    private List<CardDescriptor> playedCards = new List<CardDescriptor>();
    private List<CardDescriptor> unplayedCards = new List<CardDescriptor>();
    private List<CardDescriptor> cardsOfPlayer = new List<CardDescriptor>();

    // Start is called before the first frame update
    void Start()
    {
        allCardFaces = Resources.LoadAll<Sprite>("Textures/Cards/Cards");
        spriteRenderer = GetComponent<SpriteRenderer>();
        wishPopup = GameObject.Find("Wish");
        wishPopup.SetActive(false);

        CreateCards();
        DistributeCards();
        ChooseTopCard();
        spriteRenderer.sprite = GetCardFace(cardOnTop);
    }

    private void CreateCards()
    {
        for (int color = 1; color <= 4; color++)
            for (int number = 0; number <= CardDescriptor.PLUS2; number++)
                allCards.Add(new CardDescriptor(color, number));
        for (int i = 0; i < 8; i++)
            allCards.Add(new CardDescriptor(CardDescriptor.WISH));
        for (int i = 0; i < 4; i++)
            allCards.Add(new CardDescriptor(CardDescriptor.WISHPLUS4));
    }

    private void DistributeCards()
    {
        List<CardDescriptor> toDistribute = new List<CardDescriptor>();
        foreach (CardDescriptor c in allCards)
            toDistribute.Add(c);
        while (toDistribute.Count > 0)
        {
            int pick = Random.Range(0, toDistribute.Count - 1);
            unplayedCards.Add(toDistribute[pick]);
            toDistribute.RemoveAt(pick);
        }
        Draw(5);
    }

    private void TurnDecks()
    {
        while (playedCards.Count > 0)
        {
            unplayedCards.Add(playedCards[playedCards.Count - 1]);
            playedCards.RemoveAt(playedCards.Count - 1);
        }

    }

    public void ChooseTopCard()
    {
        cardOnTop = null;
        do
        {
            if (unplayedCards.Count > 0)
            {
                CardDescriptor top = unplayedCards[unplayedCards.Count - 1];
                unplayedCards.RemoveAt(unplayedCards.Count - 1);
                if (top.Special || (top.Number > 9))
                    playedCards.Add(top);
                else
                    cardOnTop = top;
            }
        } while (cardOnTop == null);
    }

    public void Draw(int numberOfCards)
    {
        for (int i = 0; i < numberOfCards; i++)
        {
            if (unplayedCards.Count == 0)
                TurnDecks();
            if (unplayedCards.Count > 0)
            {
                AssignCardToPlayer(unplayedCards[unplayedCards.Count - 1]);
                unplayedCards.RemoveAt(unplayedCards.Count - 1);
            }
        }
        onoPressed = false;
    }

    public bool PlayCard(CardDescriptor cardDescriptor, Sprite cardFace)
    {
        if (ONO.DoCardsMatch(cardDescriptor, cardOnTop))
        {
            cardsOfPlayer.Remove(cardDescriptor);
            playedCards.Add(cardDescriptor);
            spriteRenderer.sprite = cardFace;
            cardOnTop = cardDescriptor;
            if (cardDescriptor.Special)
                wishPopup.SetActive(true);
            if ((cardsOfPlayer.Count == 1) && !onoPressed)
                Draw(2);
            onoPressed = false;
            return true;
        }
        return false;
    }

    private void AssignCardToPlayer(CardDescriptor cardDescriptor)
    {
        cardsOfPlayer.Add(cardDescriptor);
        GameObject clone = Instantiate(cardPrefab, new Vector3(-8.166648f, -3.3f, 0f), Quaternion.identity);
        Card card = clone.GetComponent<Card>();
        card.SetDescriptor(cardDescriptor);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public Sprite GetCardFace(int color, int number)
    {
        // red:    0, 1, 2, 3, 4, 5, 6, 7, 8, 9, skip, exchange, +2, special  
        // yellow: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, skip, exchange, +2, special  
        // green:  0, 1, 2, 3, 4, 5, 6, 7, 8, 9, skip, exchange, +2, special  
        // blue:   0, 1, 2, 3, 4, 5, 6, 7, 8, 9, skip, exchange, +2, special  

        int index = (color - 1) * 14 + number;

        return allCardFaces[index];
    }

    public Sprite GetCardFace(CardDescriptor cardDescriptor)
    {
        if (cardDescriptor.Special)
            return GetSpecialCardFace(cardDescriptor.Number);
        else
            return GetCardFace(cardDescriptor.Color, cardDescriptor.Number);
    }

    public Sprite GetSpecialCardFace(int index)
    {
        if (index == 1)
            return GetCardFace(1, 13);
        else
            return GetCardFace(2, 13);
    }

    public void Wish(int color)
    {
        cardOnTop.Color = color;
        wishPopup.SetActive(false);
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

    public void OnoPressed()
    {
        Debug.Log("OnoPressed");
        onoPressed = true;
    }
}
