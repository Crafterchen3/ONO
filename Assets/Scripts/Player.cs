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
    public string playerName;
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

    private List<GameObject> backSides = new List<GameObject>();

    private bool drawingAllowed = true;

    private bool delayedEndMove;

    private int _wonGames = 0;
    private bool _isWinner = false;

    public int WonGames { get { return _wonGames; } }
    public bool IsWinner { get { return _isWinner; } }

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
        messageText.text = "";
        if (cardsOfPlayer.Count == 1)
            if (OnoPressed)
                messageText.text = "Last card!";
            else
                messageText.text = "ONO penalty!";
        else if ((cardsOfPlayer.Count == 2) && OnoPressed)
            messageText.text = "ONO!";

        if (_noOfCardsToDraw > 1)
            if (messageText.text.Length == 0)
                messageText.text = "+" + _noOfCardsToDraw.ToString();
            else
                messageText.text = messageText.text + ", +" + _noOfCardsToDraw.ToString();

        if (_skipped)
            if (messageText.text.Length == 0)
                messageText.text = "Skipped";
            else
                messageText.text = messageText.text + ", skipped";
    }

    public void SetPlayerActive(bool active)
    {
        ONO.Current.skipMove.Hide();

        if (active)
        {
            newCardsCount = 0;
            playerNameText.color = ONO.ActiveColor;
            if (!isActivePlayer)
            {
                cardsOfPlayer.Sort();
                while(backSides.Count > 0)
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
            game.HideCards();
            newCardsCount = cardsOfPlayer.Count;
            isActivePlayer = false;
            RenderMessage();
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

    public void SetName(string name)
    {
        // it is unknown whether Start was called before 
        this.playerName = name;
        RenderName();
    }

    public void Draw(int numberOfCards)
    {
        if (!drawingAllowed)
            return;

        for (int i = 0; i < numberOfCards; i++)
        {
            CardDescriptor card = game.Draw();
            cardsOfPlayer.Add(card);
            if (isActivePlayer)
                game.ShowCard(card);
            else
                newCardsCount++;
        }
        OnoPressed = false;
        if (isActivePlayer)
        {
            game.HideArrow();
            drawingAllowed = false;
            _noOfCardsToDraw = 1;
            if (!ComputeCardValidity())
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
        for (int i = 0; i < cardsOfPlayer.Count; i++)
        {
            GameObject.Destroy(backSides[0]);
            backSides.RemoveAt(0);
        }
        playerNameText.color = ONO.InactiveColor;
        drawingAllowed = true;
        isActivePlayer = false;
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
