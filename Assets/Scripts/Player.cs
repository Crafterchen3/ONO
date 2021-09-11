using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : MonoBehaviour
{
    public bool onoPressed = false;
    public bool isActivePlayer = false;
    public GameObject cardBacksidePrefab;
    public List<CardDescriptor> cardsOfPlayer = new List<CardDescriptor>();
    public string playerName;
    public float endMoveDelay = 0.5f;
    public int numberOfCardsToDraw = 1;

    private int sortingOrder = 10000;

    private int newCardsCount;

    private Game game;

    private TMP_Text text;
    private List<GameObject> backSides = new List<GameObject>();

    private Color inactiveColor = new Color32(255, 0, 0, 255);
    private Color activeColor = new Color32(255, 255, 0, 255);

    private bool drawingAllowed = true;

    private bool delayedEndMove;

    // Start is called before the first frame update
    void Start()
    {
        GameObject controllerObject = GameObject.FindGameObjectWithTag("game");
        game = controllerObject.GetComponent<Game>();

        text = gameObject.GetComponentInChildren<TMP_Text>();
        RenderName();
        game.PlayerPresent();
    }

    private void RenderName()
    {
        if ((playerName != null) && (text != null))
            text.text = playerName;
    }

    public void SetPlayerActive(bool active)
    {
        if (active)
        {
            text.color = activeColor;
            if (!isActivePlayer)
            {
                cardsOfPlayer.Sort();
                for (int i = 0; i < cardsOfPlayer.Count; i++)
                {
                    GameObject.Destroy(backSides[0]);
                    backSides.RemoveAt(0);
                    game.ShowCard(cardsOfPlayer[i]);
                }
            }
            isActivePlayer = true;
            drawingAllowed = true;
            game.ShowArrow(!ComputeCardValidity());
            if ((cardsOfPlayer.Count == 1) && !onoPressed)
                Draw(2);
        }
        else
        {
            text.color = inactiveColor;
            game.HideCards();
            newCardsCount = cardsOfPlayer.Count;
            isActivePlayer = false;
        }

    }

    private bool ComputeCardValidity()
    {
        bool result = false;
        foreach (CardDescriptor c in cardsOfPlayer)
        {
            if (numberOfCardsToDraw > 1)
                c.valid = ((c.Special == game.cardOnTop.Special) && (c.Number == game.cardOnTop.Number)); // both plus2 or plus4
            else
                c.valid = ONO.DoCardsMatch(c, game.cardOnTop);
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
        onoPressed = false;
        if (isActivePlayer)
        {
            game.HideArrow();
            drawingAllowed = false;
            numberOfCardsToDraw = 1;
            if (!ComputeCardValidity())
                ScheduleDelayedEndMove();
        }
    }


    private void RenderNewCard()
    {
        GameObject clone = Instantiate(cardBacksidePrefab, new Vector3(gameObject.transform.position.x + 0.1f, gameObject.transform.position.y + 0.04f, 0f), Quaternion.identity);
        
        SpriteRenderer sr = clone.GetComponent<SpriteRenderer>();
        sr.sortingOrder = sortingOrder;
        sortingOrder--;

        backSides.Add(clone);
    }

    public void Reset()
    {
        for (int i = 0; i < cardsOfPlayer.Count; i++)
        {
            GameObject.Destroy(backSides[0]);
            backSides.RemoveAt(0);
        }
        text.color = inactiveColor;
        drawingAllowed = true;
        isActivePlayer = false;
        cardsOfPlayer.Clear();
    }
}
