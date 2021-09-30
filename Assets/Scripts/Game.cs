using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public static bool keepCardGameObjets = false;

    public CardDescriptor cardOnTop;
    public Sprite wish1;
    public Sprite wish2;
    public Sprite wish3;
    public Sprite wish4;
    public GameObject cardPrefab;
    public GameObject playerPrefab;
    public float cardCreationRate = 0.01f;
    public int numberOfPlayers = 3;
    public int numberOfHumanPlayers;
    public LayoutManager.ILayout layout;

    private Sprite[] allCardFaces;

    private int sortingOrder = 10000;

    private List<CardDescriptor> allCards = new List<CardDescriptor>();
    public List<CardDescriptor> playedCards = new List<CardDescriptor>();
    private List<CardDescriptor> unplayedCards = new List<CardDescriptor>();
    private List<CardDescriptor> newCardsQueue = new List<CardDescriptor>();
    private List<GameObject> renderedCards = new List<GameObject>();
    private List<GameObject> renderedPlayedCards = new List<GameObject>();

    public List<Player> players = new List<Player>();
    private List<GameObject> playerGameObjects = new List<GameObject>();

    private Player currentPlayer;
    private int currentPlayerIndex = -1;
    private bool directionIsClockwise = true;
    private int seatedPlayers = 0;

    private int skippedPlayer = -1;

    private bool playerIsReady = false;

    public HighScoreHistory highScoreHistory;
    public Persistence persistence;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        allCardFaces = Resources.LoadAll<Sprite>("Textures/Cards/Cards");
        persistence = new Persistence(this);
        highScoreHistory = persistence.LoadHighScores();
        ONO.Current.GameControlPresent(this, gameObject);
    }

    public void Show()
    {
        ONO.Current.onoButton.SetActive(true);
        ONO.Current.unplayedCards.SetActive(true);
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        PrepareNewGame(1); // > 0, so that the game does not start
        ONO.Current.onoButton.SetActive(false);
        ONO.Current.unplayedCards.SetActive(false);
        gameObject.SetActive(false);
    }

    public void HideArrow()
    {
        ONO.Current.arrow.SetActive(false);
    }

    public void ShowArrow()
    {
        if (currentPlayer != null)
        {
            bool showArrowFlag = true;
            foreach (CardDescriptor c in currentPlayer.cardsOfPlayer)
                if (c.valid)
                {
                    showArrowFlag = false;
                    break;
                }

            ONO.Current.arrow.SetActive(showArrowFlag);
        }
    }

    public void PrepareNewGame(int noOfPlayers)
    {
        numberOfPlayers = noOfPlayers;
        players.Clear();
        foreach (GameObject g in playerGameObjects)
            Destroy(g);
        playerGameObjects.Clear();
        playedCards.Clear();
        unplayedCards.Clear();
        seatedPlayers = 0;
        foreach (GameObject go in renderedPlayedCards)
            Destroy(go);
        renderedPlayedCards.Clear();
        directionIsClockwise = true;
    }

    public void PlayerPresent()
    {
        seatedPlayers++;
        if ((seatedPlayers == numberOfPlayers))
            Initialize();
    }

    private void Initialize()
    {
        CreateCards();
        DistributeCards();
        ChooseTopCard();
        numberOfHumanPlayers = 0;
        foreach (Player p in players)
            if (!p.isVirtualPlayer)
                numberOfHumanPlayers++;
        NextPlayer();
    }

    public void CreatePlayer(int pos, string name, bool isVirtual)
    {
        GameObject clone = Instantiate(playerPrefab, layout.GetPositions(numberOfPlayers)[pos], layout.GetRotations(numberOfPlayers)[pos]);
        Player result = clone.GetComponent<Player>();
        result.SetName(name, isVirtual);
        result.cardQuaternion = layout.GetRotations(numberOfPlayers)[pos];
        result.freezeXPosition = layout.GetFreezePositionX(numberOfPlayers, pos);
        players.Add(result);
        playerGameObjects.Add(clone);
    }


    public void SetLayout(LayoutManager.ILayout layout)
    {
        this.layout = layout;
    }

    private void CreateCards()
    {
        allCards.Clear();
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
        for (int i = 0; i < 10; i++)
        {

            foreach (CardDescriptor c in allCards)
                toDistribute.Add(c);
            while (toDistribute.Count > 0)
            {
                int pick;
                if (toDistribute.Count == 1)
                    pick = 0;
                else
                    pick = Random.Range(0, (toDistribute.Count - 1) * 1024) % (toDistribute.Count - 1);
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
        for (int i = 0; i < renderedPlayedCards.Count - 1; i++)
            Destroy(renderedPlayedCards[i]);
        GameObject topGO = renderedPlayedCards[renderedPlayedCards.Count - 1];
        renderedPlayedCards.Clear();
        renderedPlayedCards.Add(topGO);
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
                playedCards.Add(top);
                if (!top.Special && (top.Number <= 9))
                    cardOnTop = top;
            }
        } while (cardOnTop == null);
        gameObject.GetComponent<SpriteRenderer>().sprite = GetCardFace(cardOnTop);
    }

    public CardDescriptor Draw()
    {
        CardDescriptor result = null;

        if (unplayedCards.Count == 0)
            TurnDecks();
        if (unplayedCards.Count > 0)
        {
            result = unplayedCards[unplayedCards.Count - 1];
            result.visible = playerIsReady;
            unplayedCards.RemoveAt(unplayedCards.Count - 1);
        }
        return result;
    }

    public bool TryPlayCard(CardDescriptor cardDescriptor)
    {
        if ((currentPlayer.isVirtualPlayer || cardDescriptor.visible) && cardDescriptor.valid)
        {
            currentPlayer.cardsOfPlayer.Remove(cardDescriptor);
            playedCards.Add(cardDescriptor);
            if (currentPlayer.isVirtualPlayer)
                ShowCardOnStack(cardDescriptor);
            else
            {
                MoveCard(cardDescriptor);
                if ((currentPlayer.cardsOfPlayer.IndexOf(cardDescriptor) == 0) && (currentPlayer.cardsOfPlayer.Count > 1))
                    foreach (BoxCollider2D c in renderedCards[1].GetComponents<BoxCollider2D>())
                        c.enabled = true; // react on click on the right side of the most-left card
            }
            cardOnTop = cardDescriptor;
            if (currentPlayer.cardsOfPlayer.Count > 0)
            {
                if (cardDescriptor.Special)
                {
                    if (cardDescriptor.Number == CardDescriptor.WISHPLUS4)
                        GetNextPlayer().NoOfCardsToDraw = currentPlayer.NoOfCardsToDraw == 1 ? 4 : currentPlayer.NoOfCardsToDraw + 4;
                    currentPlayer.NoOfCardsToDraw = 1;
                    if (currentPlayer.isVirtualPlayer)
                        Invoke("SimulatorWish", 0.2f);
                    else
                        ONO.Current.wishPopup.SetActive(true);
                }
                else
                {
                    switch (cardDescriptor.Number)
                    {
                        case CardDescriptor.PLUS2:
                            GetNextPlayer().NoOfCardsToDraw = currentPlayer.NoOfCardsToDraw == 1 ? 2 : currentPlayer.NoOfCardsToDraw + 2;
                            currentPlayer.NoOfCardsToDraw = 1;
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
            }
            else
                NextPlayer(); // this will end the game
            return true;
        }
        return false;
    }

    private void ShowCardOnStack(CardDescriptor cardDescriptor)
    {
        GameObject c = RenderNewCard(cardDescriptor, currentPlayer.gameObject.transform.position.x, currentPlayer.gameObject.transform.position.y);
        Card card = c.GetComponent<Card>();
        card.MoveToCardStack();
        renderedCards.Remove(c);
        renderedPlayedCards.Add(c);
    }

    private Card GetCardController(CardDescriptor cardDescriptor)
    {
        foreach (GameObject c in renderedCards)
        {
            Card card = c.GetComponent<Card>();
            if (card.descriptor == cardDescriptor)
                return card;
        }
        return null;

    }

    private void MoveCard(CardDescriptor cardDescriptor)
    {
        Card card = GetCardController(cardDescriptor);
        card.MoveToCardStack();
        renderedCards.Remove(card.gameObject);
        renderedPlayedCards.Add(card.gameObject);
    }

    public void VisualizeValidity(CardDescriptor descriptor)
    {
        foreach (GameObject c in renderedCards)
        {
            Card card = c.GetComponent<Card>();
            if (card.descriptor == descriptor)
            {
                card.VisualizeValidity();
                break;
            }
        }
    }

    public void DrawCard()
    {
        if ((currentPlayer != null) && currentPlayer.isActivePlayer && playerIsReady)
        {
            currentPlayer.Draw(currentPlayer.NoOfCardsToDraw);
            currentPlayer.NoOfCardsToDraw = 1;
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
            HideCards(); // required if only one human player
            return;
        }

        int nextIndex = GetNextPlayerIndex(increment);

        if (increment > 1)
        {
            skippedPlayer = GetNextPlayerIndex();
            players[skippedPlayer].Skipped = true;
        }

        if (nextIndex != currentPlayerIndex)
        {
            if (currentPlayer != null)
                currentPlayer.SetPlayerActive(false);
            currentPlayerIndex = nextIndex;
            currentPlayer = players[currentPlayerIndex];
            playerIsReady = false;
            currentPlayer.SetPlayerActive(true);
            if (currentPlayer.isVirtualPlayer || numberOfHumanPlayers <= 1)
                Invoke("NextPlayerIsReady", 0.5f);
            else
                ONO.Current.nextPlayerButton.Show(currentPlayer.playerName);
        }
        else
        {
            currentPlayer.SetPlayerActive(true);
            ShowArrow();
            if (currentPlayer.isVirtualPlayer)
                Invoke("NextPlayerIsReady", 0.5f);
        }
    }

    private void CurrentPlayerWins()
    {
        currentPlayer.Wins();
        highScoreHistory.PlayerHasWon(currentPlayer.playerName);
        persistence.SaveHighScores();

        ONO.Current.playerWinsDialog.Show(currentPlayer.playerName);
    }

    public void HideCards()
    {
        while (renderedCards.Count > 0)
        {
            Destroy(renderedCards[0]);
            renderedCards.RemoveAt(0);
        }
    }

    bool slideInFlag = false;

    public void ShowCard(CardDescriptor cardDescriptor, bool slideIn = false)
    {
        slideInFlag = slideIn;
        newCardsQueue.Add(cardDescriptor);
    }

    private GameObject RenderNewCard(CardDescriptor cardDescriptor)
    {
        float startX;

        if (slideInFlag)
            startX = -8.166648f;
        else
        if (renderedCards.Count > 0)
        {
            GameObject lastCard = renderedCards[renderedCards.Count - 1];
            startX = lastCard.transform.position.x - 1;
        }
        else
            startX = 6;
        return RenderNewCard(cardDescriptor, startX, -3.3f);
    }

    private GameObject RenderNewCard(CardDescriptor cardDescriptor, float x, float y)
    {
        GameObject clone = Instantiate(cardPrefab, new Vector3(x, y, 0f), Quaternion.identity);
        Card card = clone.GetComponent<Card>();
        if (playerIsReady)
            cardDescriptor.visible = true;
        card.SetDescriptor(cardDescriptor);

        if (renderedCards.Count == 0)
            foreach (BoxCollider2D c in clone.GetComponents<BoxCollider2D>())
                c.enabled = true; // react on click on the right side of the most-left card

        SpriteRenderer sr = clone.GetComponent<SpriteRenderer>();
        sr.sortingOrder = sortingOrder;
        sortingOrder--;

        renderedCards.Add(clone);
        return (clone);
    }

    public void TurnCards()
    {
        foreach (GameObject c in renderedCards)
        {
            Card card = c.GetComponent<Card>();
            card.descriptor.visible = !card.descriptor.visible;
            card.Render();
        }
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
        // red:    0, 1, 2, 3, 4, 5, 6, 7, 8, 9, skip, change dir, +2, special  
        // yellow: 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, skip, change dir, +2, special  
        // green:  0, 1, 2, 3, 4, 5, 6, 7, 8, 9, skip, change dir, +2, special  
        // blue:   0, 1, 2, 3, 4, 5, 6, 7, 8, 9, skip, change dir, +2, special  

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

    private void SimulatorWish()
    {
        Wish(currentPlayer.simulator.GetWishColor());
    }

    public void Wish(int color)
    {
        cardOnTop.Color = color;
        SpriteRenderer spriteRenderer = renderedPlayedCards[renderedPlayedCards.Count - 1].GetComponent<SpriteRenderer>();
        ONO.Current.wishPopup.SetActive(false);
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
            currentPlayer.OnoPressed = true;
    }

    public void SkipMovePressed()
    {
        NextPlayer();
    }

    public void NextPlayerIsReady()
    {
        if (skippedPlayer >= 0)
        {
            players[skippedPlayer].Skipped = false;
            skippedPlayer = -1;
        }

        if (!currentPlayer.isVirtualPlayer)
        {
            ONO.Current.nextPlayerPopup.SetActive(false);
            foreach (GameObject card in renderedCards)
                card.GetComponent<Card>().Render();
        }

        playerIsReady = true;
        ShowArrow();
        if (currentPlayer.isVirtualPlayer)
            currentPlayer.simulator.SimulateMove();
    }

    public void DisplayScore()
    {
        ONO.Current.scoreDialog.Show();
    }

    public void NextRound()
    {
        ONO.Current.scoreDialog.Hide();
        allCards.Clear();
        foreach (Player p in players)
        {
            allCards.AddRange(p.cardsOfPlayer);
            p.Reset();
        }
        allCards.AddRange(playedCards);
        allCards.AddRange(unplayedCards);
        playedCards.Clear();
        unplayedCards.Clear();
        foreach (GameObject go in renderedPlayedCards)
            Destroy(go);
        renderedPlayedCards.Clear();
        directionIsClockwise = true;
        DistributeCards();
        ChooseTopCard();
        NextPlayer();
    }

    public void Quit()
    {
        ONO.Current.scoreDialog.Hide();
        Hide();
        ONO.Current.launcher.Show();
    }

}
