using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    private bool _onoPressed = false;
    public bool OnoPressed { get { return _onoPressed; } set { _onoPressed = value; RenderMessage(); } }

    private bool _skipped = false;
    public bool Skipped { get { return _skipped; } set { _skipped = value; RenderMessage(); } }

    public bool isActivePlayer = false;
    public GameObject cardBacksidePrefab;
    public List<CardDescriptor> cardsOfPlayer = new List<CardDescriptor>();
    public List<GameObject> cardsOfPlayGOs = new List<GameObject>();
    public string playerName;
    public bool isVirtualPlayer = false;
    public float endMoveDelay = 0.5f;
    public Quaternion cardQuaternion;
    public bool freezeXPosition;

    private int _noOfCardsToDraw = 1;
    public int NoOfCardsToDraw { get { return _noOfCardsToDraw; } set { SetNoOfCardsToDraw(value); } }

    private int sortingOrder = 10000;

    private int newCardsCount;

    private Game game;

    private TMP_Text playerNameText;
    private TMP_Text messageText;
    private List<string> messageQueue = new List<string>();
    private string displayedMessage;
    private Animator messageAnimator;

    private List<GameObject> backSides = new List<GameObject>();

    private bool drawingAllowed = true;

    private bool delayedEndMove;

    private bool sortingRequired = true;

    private int _wonGames = 0;
    private bool _isWinner = false;

    public int WonGames { get { return _wonGames; } }
    public bool IsWinner { get { return _isWinner; } }

    public PlayerSimulator simulator;

    // Start is called before the first frame update
    void Start()
    {
        GameObject controllerObject = GameObject.FindGameObjectWithTag("game");
        game = controllerObject.GetComponent<Game>();

        foreach (TMP_Text t in gameObject.GetComponentsInChildren<TMP_Text>())
        {
            if (t.tag == "playerName")
                playerNameText = t;
            else if (t.tag == "message")
                messageText = t;
        }

        messageAnimator = gameObject.GetComponentInChildren<Animator>();

        RenderName();
        game.PlayerPresent();
    }

    private void RenderName()
    {
        if ((playerName != null) && (playerNameText != null))
            playerNameText.text = playerName;
    }

    private void SetNoOfCardsToDraw(int cards)
    {
        _noOfCardsToDraw = cards;
        RenderMessage();
    }

    private void RenderMessage()
    {
        string message = "";

        if (cardsOfPlayer.Count == 1)
            if (OnoPressed)
                message = "Last card!";
            else
                message = "ONO penalty!";
        else if ((cardsOfPlayer.Count == 2) && OnoPressed)
            message = "ONO!";

        if (_noOfCardsToDraw > 1)
            if (message.Length == 0)
                message = "+" + _noOfCardsToDraw.ToString();
            else
                message = message + ", +" + _noOfCardsToDraw.ToString();

        if (_skipped)
            if (message.Length == 0)
                message = "Skipped";
            else
                message = message + ", skipped";

        if ((displayedMessage != null) && displayedMessage.Equals(message))
            return;

        displayedMessage = message;

        if ((message.Length == 0) && !messageAnimator.GetBool("NewText"))
            messageText.text = "";
        else
        {
            messageQueue.Add(message);
            if (!messageAnimator.GetBool("NewText"))
                messageAnimator.SetBool("NewText", true);
        }
    }

    public void StartOfAnimation()
    {
        messageText.text = messageQueue[0];
        messageQueue.RemoveAt(0);
    }

    public void EndOfAnimation()
    {
        while ((messageQueue.Count > 0) && (messageQueue[0].Length == 0))
        {
            messageText.text = "";
            messageQueue.RemoveAt(0);
        }
        if (messageQueue.Count == 0)
            messageAnimator.SetBool("NewText", false);
    }

    public void SetPlayerActive(bool active)
    {
        ONO.Current.skipMove.Hide();

        if (active)
        {
            if (!isVirtualPlayer)
                newCardsCount = 0;
            playerNameText.color = ONO.ActiveColor;
            if (!isActivePlayer && !isVirtualPlayer && ((game.numberOfHumanPlayers > 1) || sortingRequired))
            {
                cardsOfPlayer.Sort();
                sortingRequired = false;
                while (backSides.Count > 0)
                {
                    GameObject.Destroy(backSides[0]);
                    backSides.RemoveAt(0);
                }
                for (int i = 0; i < cardsOfPlayer.Count; i++)
                {
                    cardsOfPlayer[i].visible = false;
                    game.ShowCard(cardsOfPlayer[i]);
                }
            }
            else if (!isActivePlayer)
                game.TurnCards();
            isActivePlayer = true;
            drawingAllowed = true;
            ComputeCardValidity();
            if ((cardsOfPlayer.Count == 1) && !OnoPressed)
                Draw(2);
            RenderMessage();
        }
        else
        {
            playerNameText.color = ONO.InactiveColor;
            if (!isVirtualPlayer && ((game.numberOfHumanPlayers > 1) || sortingRequired))
            {
                game.HideCards();
                newCardsCount = cardsOfPlayer.Count;
            }
            else if (!isActivePlayer)
                game.TurnCards();
            isActivePlayer = false;
            if (OnoPressed && (cardsOfPlayer.Count > 1))
                OnoPressed = false;
            RenderMessage();
        }
    }


    public void PlayCard()
    {
        if (newCardsCount > 0)
            newCardsCount--;
        else if (backSides.Count > 0)
        {
            Destroy(backSides[0]);
            backSides.RemoveAt(0);
        }
    }

    private bool ComputeCardValidity()
    {
        bool result = false;
        foreach (CardDescriptor c in cardsOfPlayer)
        {
            if (_noOfCardsToDraw > 1)
                c.valid = ((c.Special == game.cardOnTop.Special) && (c.Number == game.cardOnTop.Number)); // both plus2 or plus4
            else
                c.valid = c.CanBePlayed(game.cardOnTop);
            result = result || c.valid;
            game.VisualizeValidity(c);
        }
        return result;
    }

    private float nextCardCreation = 0.0f;
    private float endMoveTime = 0f;

    private void ScheduleDelayedEndMove()
    {
        endMoveTime = Time.time + endMoveDelay;
        delayedEndMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if ((newCardsCount > 0) && (Time.time > nextCardCreation))
        {
            nextCardCreation = Time.time + game.cardCreationRate;
            RenderNewCard();
            newCardsCount--;
        }
        if (delayedEndMove && (Time.time > endMoveTime))
        {
            delayedEndMove = false;
            game.NextPlayer();
        }
    }

    public void SetName(string name, bool isVirtual)
    {
        // it is unknown whether Start was called before 
        this.playerName = name;
        this.isVirtualPlayer = isVirtual;
        if (isVirtual)
            simulator = new PlayerSimulator(this);
        RenderName();
    }

    private void CheckIfSortingIsRequired()
    {
        List<int> colorsFound = new List<int>();
        int lastColor = -1;

        sortingRequired = false;
        foreach (CardDescriptor c in cardsOfPlayer)
            if (c.Color != lastColor)
            {
                if (colorsFound.Contains(c.Color))
                {
                    sortingRequired = true;
                    return;
                }
                colorsFound.Add(lastColor);
                lastColor = c.Color;
            }
    }

    public void Draw(int numberOfCards)
    {
        if (!drawingAllowed)
            return;

        for (int i = 0; i < numberOfCards; i++)
        {
            CardDescriptor card = game.Draw();
            cardsOfPlayer.Add(card);
            if (isActivePlayer && !isVirtualPlayer)
                game.ShowCard(card, true);
            else
                newCardsCount++;
        }
        OnoPressed = false;
        if (isActivePlayer)
        {
            game.HideArrow();
            drawingAllowed = false;
            CheckIfSortingIsRequired();
            _noOfCardsToDraw = 1;
            if (!ComputeCardValidity())
                if (isVirtualPlayer)
                    game.NextPlayer();
                else
                    ScheduleDelayedEndMove();
            else
                ONO.Current.skipMove.Show();
            RenderMessage();
        }
    }


    private void RenderNewCard()
    {
        GameObject clone = Instantiate(cardBacksidePrefab, new Vector3(gameObject.transform.position.x + 0.1f, gameObject.transform.position.y + 0.04f, 0f), cardQuaternion);

        SpriteRenderer sr = clone.GetComponent<SpriteRenderer>();
        sr.sortingOrder = sortingOrder;
        sortingOrder--;

        if (freezeXPosition)
        {
            Rigidbody2D rb = clone.GetComponent<Rigidbody2D>();
            rb.constraints = RigidbodyConstraints2D.FreezePositionX;
        }

        backSides.Add(clone);
    }

    public void Wins()
    {
        _wonGames++;
        _isWinner = true;
    }

    public void Reset()
    {
        while (backSides.Count > 0)
        {
            GameObject.Destroy(backSides[0]);
            backSides.RemoveAt(0);
        }
        playerNameText.color = ONO.InactiveColor;
        drawingAllowed = true;
        isActivePlayer = false;
        sortingRequired = true;
        cardsOfPlayer.Clear();
        _isWinner = false;
        RenderMessage();
    }

    private void OnDestroy()
    {
        while (backSides.Count > 0)
        {
            Destroy(backSides[0]);
            backSides.RemoveAt(0);
        }

    }

}
