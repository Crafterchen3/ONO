using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    const int maxPlayers = 6;

    public CardDescriptor cardOnTop;
    public Sprite wish1;
    public Sprite wish2;
    public Sprite wish3;
    public Sprite wish4;
    public GameObject cardPrefab;
    public GameObject playerPrefab;
    public float cardCreationRate = 0.5f;
    public int numberOfPlayers = 3;

    private Sprite[] allCardFaces;
    private SpriteRenderer spriteRenderer;
    private GameObject wishPopup;

    private GameObject nextPlayerPopup;
    private NextPlayer nextPlayerButton;

    private int sortingOrder = 10000;

    private List<CardDescriptor> allCards = new List<CardDescriptor>();
    private List<CardDescriptor> playedCards = new List<CardDescriptor>();
    private List<CardDescriptor> unplayedCards = new List<CardDescriptor>();
    private List<CardDescriptor> newCardsQueue = new List<CardDescriptor>();
    private List<GameObject> renderedCards = new List<GameObject>();

    private Vector3[] playerPositions = new Vector3[maxPlayers];

    private List<Player> players = new List<Player>();
    private Player currentPlayer;
    private int currentPlayerIndex = -1;
    private bool directionIsClockwise = true;
    private int seatedPlayers = 0;

    // Start is called before the first frame update
    void Start()
    {
        allCardFaces = Resources.LoadAll<Sprite>("Textures/Cards/Cards");
        spriteRenderer = GetComponent<SpriteRenderer>();
        wishPopup = GameObject.Find("Wish");
        wishPopup.SetActive(false);
        CreatePlayers();
    }

    public void PlayerPresent()
    {
        seatedPlayers++;
        if ((seatedPlayers == numberOfPlayers) && (nextPlayerPopup != null))
            Initialize();
    }

    public void NextPlayerPopupPresent(NextPlayer nextPlayer, GameObject nextPlayerGameObject)
    {
        nextPlayerPopup = nextPlayerGameObject;
        nextPlayerButton = nextPlayer;
        if ((seatedPlayers == numberOfPlayers) && (nextPlayerPopup != null))
            Initialize();
    }

    private void Initialize()
    {
        CreateCards();
        DistributeCards();
        ChooseTopCard();
        spriteRenderer.sprite = GetCardFace(cardOnTop);
        NextPlayer();
    }

    private Player CreatePlayer(int pos, string name)
    {
        GameObject clone = Instantiate(playerPrefab, playerPositions[pos], Quaternion.identity);
        Player result = clone.GetComponent<Player>();
        result.SetName(name);
        return result;
    }


    private void CreatePlayers()
    {
        playerPositions[0] = new Vector3(-9.5f, 3f, 0f);
        playerPositions[1] = new Vector3(-4f, 3f, 0f);
        playerPositions[2] = new Vector3(1.5f, 3f, 0f);

        players.Add(CreatePlayer(0, "Thomas"));
        players.Add(CreatePlayer(1, "Paul"));
        players.Add(CreatePlayer(2, "Benjamin"));
    }

    private void CreateCards()
    {
        for (int c = 0; c < 18; c++)
            for (int color = 1; color <= 4; color++)
            {
                if (c == 5)
                    allCards.Add(new CardDescriptor(CardDescriptor.WISHPLUS4));
                if (c == 9)
                    allCards.Add(new CardDescriptor(CardDescriptor.WISH));
                for (int number = 0; number <= CardDescriptor.PLUS2; number++)
                {
                    if ((number > 9) && (c >= 8))
                        continue;
                    allCards.Add(new CardDescriptor(color, number));
                }
            }
    }

    private void Mix()
    {
        List<CardDescriptor> toDistribute = new List<CardDescriptor>();
        for (int i = 0; i < 5; i++)
        {

            foreach (CardDescriptor c in allCards)
                toDistribute.Add(c);
            while (toDistribute.Count > 0)
            {
                int pick = Random.Range(0, toDistribute.Count - 1);
                CardDescriptor card = toDistribute[pick];
                unplayedCards.Add(card);
                toDistribute.RemoveAt(pick);
            }
            allCards.Clear();
            allCards.AddRange(unplayedCards);
            unplayedCards.Clear();
        }
        int cut = Random.Range(0, allCards.Count - 1);
        while (allCards.Count > cut)
        {
            CardDescriptor card = allCards[cut];
            unplayedCards.Add(card);
            allCards.RemoveAt(cut);
        }
        unplayedCards.AddRange(allCards);
    }

    private void DistributeCards()
    {
        Mix();
        foreach (Player p in players)
            p.Draw(5);
    }

    private void TurnDecks()
    {
        playedCards.RemoveAt(playedCards.Count - 1); // top card
        while (playedCards.Count > 1)
        {
            unplayedCards.Add(playedCards[playedCards.Count - 1]);
            playedCards.RemoveAt(playedCards.Count - 1);
        }
        playedCards.Add(cardOnTop);
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

    public CardDescriptor Draw()
    {
        CardDescriptor result = null;

        if (unplayedCards.Count == 0)
            TurnDecks();
        if (unplayedCards.Count > 0)
        {
            result = unplayedCards[unplayedCards.Count - 1];
            unplayedCards.RemoveAt(unplayedCards.Count - 1);
        }
        return result;
    }

    public bool PlayCard(CardDescriptor cardDescriptor, Sprite cardFace)
    {
        if (cardDescriptor.valid)
        {
            currentPlayer.cardsOfPlayer.Remove(cardDescriptor);
            playedCards.Add(cardDescriptor);
            spriteRenderer.sprite = cardFace;
            cardOnTop = cardDescriptor;
            if (cardDescriptor.Special)
            {
                if (cardDescriptor.Number == CardDescriptor.WISHPLUS4)
                    GetNextPlayer().numberOfCardsToDraw = currentPlayer.numberOfCardsToDraw == 1 ? 4 : currentPlayer.numberOfCardsToDraw + 4;
                currentPlayer.numberOfCardsToDraw = 1;
                wishPopup.SetActive(true);
            }
            else
            {
                switch (cardDescriptor.Number)
                {
                    case CardDescriptor.PLUS2:
                        GetNextPlayer().numberOfCardsToDraw = currentPlayer.numberOfCardsToDraw == 1 ? 2 : currentPlayer.numberOfCardsToDraw + 2;
                        currentPlayer.numberOfCardsToDraw = 1;
                        break;
                    case CardDescriptor.CHANGE_DIR:
                        if (numberOfPlayers < 3)
                        {
                            NextPlayer(2);
                            return true;
                        }
                        else
                            directionIsClockwise = !directionIsClockwise;
                        break;
                    case CardDescriptor.SKIP:
                        NextPlayer(2);
                        return true;
                }
                NextPlayer();
            }
            return true;
        }
        return false;
    }

    public void DrawCard()
    {
        if ((currentPlayer != null) && currentPlayer.isActivePlayer)
        {
            currentPlayer.Draw(currentPlayer.numberOfCardsToDraw);
            currentPlayer.numberOfCardsToDraw = 1;
        }
    }

    private int GetNextPlayerIndex(int increment = 1)
    {
        int index;
        if (directionIsClockwise)
            index = (currentPlayerIndex + increment) % players.Count;
        else
        {
            index = (currentPlayerIndex + players.Count - increment) % players.Count;
        }
        return index;
    }

    private Player GetNextPlayer(int increment = 1)
    {
        return players[GetNextPlayerIndex(increment)];
    }

    public void NextPlayer(int increment = 1)
    {
        if ((currentPlayer != null) && (currentPlayer.cardsOfPlayer.Count == 0))
        {
            currentPlayer.SetPlayerActive(false);
            CurrentPlayerWins();
            return;
        }

        int nextIndex = GetNextPlayerIndex(increment);

        if (nextIndex != currentPlayerIndex)
        {
            if (currentPlayer != null)
                currentPlayer.SetPlayerActive(false);
            currentPlayerIndex = nextIndex;
            currentPlayer = players[currentPlayerIndex];
            nextPlayerButton.Show(currentPlayer.playerName);
        }
        else
            currentPlayer.SetPlayerActive(true);
    }

    private void CurrentPlayerWins()
    {
    }

    public void HideCards()
    {
        while (renderedCards.Count > 0)
        {
            GameObject.Destroy(renderedCards[0]);
            renderedCards.RemoveAt(0);
        }
    }

    public void ShowCard(CardDescriptor cardDescriptor)
    {
        newCardsQueue.Add(cardDescriptor);
    }

    private void RenderNewCard(CardDescriptor cardDescriptor)
    {
        GameObject clone = Instantiate(cardPrefab, new Vector3(-8.166648f, -3.3f, 0f), Quaternion.identity);
        Card card = clone.GetComponent<Card>();
        card.SetDescriptor(cardDescriptor);

        if (renderedCards.Count == 0)
            foreach (BoxCollider2D c in clone.GetComponents<BoxCollider2D>())
                c.enabled = true; // react on click on the right side of the card

        SpriteRenderer sr = clone.GetComponent<SpriteRenderer>();
        sr.sortingOrder = sortingOrder;
        sortingOrder--;

        renderedCards.Add(clone);
    }

    private float nextCardCreation = 0.0f;

    // Update is called once per frame
    void Update()
    {
        if ((newCardsQueue.Count > 0) && (Time.time > nextCardCreation))
        {
            nextCardCreation = Time.time + cardCreationRate;
            RenderNewCard(newCardsQueue[0]);
            newCardsQueue.RemoveAt(0);
        }

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
            return GetCardFace(3, 13);
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
        NextPlayer();
    }

    public void OnoPressed()
    {
        if (currentPlayer != null)
            currentPlayer.onoPressed = true;
    }

    public void NextPlayerIsReady()
    {
        nextPlayerPopup.SetActive(false);
        currentPlayer.SetPlayerActive(true);
    }
}
