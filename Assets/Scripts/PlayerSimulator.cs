using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSimulator
{
    private Player player;

    private class CardCounter : IComparable<CardCounter>
    {
        public int color;
        public int counter = 0;
        public CardDescriptor validCard = null;

        public CardCounter(int color)
        {
            this.color = color;
        }

        // Default comparer 
        public int CompareTo(CardCounter compareColor)
        {
            // A null value means that this object is greater.
            if (compareColor == null)
                return -1;

            else
                return -1 * this.counter.CompareTo(compareColor.counter);
        }

    }


    public PlayerSimulator(Player player)
    {
        this.player = player;
    }

    private int wishColor;

    private CardDescriptor ChooseCard()
    {
        List<CardCounter> numberOfCardsPerColor = new List<CardCounter>();

        CardDescriptor plus4 = null;
        CardDescriptor wish = null;
        CardDescriptor plus2 = null;
        CardDescriptor changeDir = null;
        CardDescriptor skip = null;
        bool regularCardFound = false;

        for (int i = 0; i < 4; i++)
            numberOfCardsPerColor.Add(new CardCounter(i));

        foreach (CardDescriptor c in player.cardsOfPlayer)

            if (c.Special)
            {
                if (c.Number == CardDescriptor.WISH)
                    wish = c;
                else
                    plus4 = c;
            }
            else
            {
                numberOfCardsPerColor[c.Color - 1].counter++;
                if (c.valid)
                {
                    regularCardFound = true;
                    numberOfCardsPerColor[c.Color - 1].validCard = c;
                    switch (c.Number)
                    {
                        case CardDescriptor.PLUS2:
                            plus2 = c;
                            break;
                        case CardDescriptor.CHANGE_DIR:
                            changeDir = c;
                            break;
                        case CardDescriptor.SKIP:
                            skip = c;
                            break;
                    }
                }
            }

        // let numberOfCardsPerColor[0].color point to the color with the maximum number of cards
        numberOfCardsPerColor.Sort();

        if (regularCardFound)
        {
            if (plus2 != null)
                return plus2;
            if (skip != null)
                return skip;
            if (changeDir != null)
                return changeDir;

            for (int counter = 0; counter < 4; counter++)
                if (numberOfCardsPerColor[counter].validCard != null)
                    return numberOfCardsPerColor[counter].validCard;
        }
        else
        {
            wishColor = numberOfCardsPerColor[0].color + 1;
            if (wish != null)
                return wish;
            if (plus4 != null)
                return plus4;
        }

        return null;
    }

    public void SimulateMove()
    {
        CardDescriptor chosenCard = ChooseCard();

        if (ONO.Current.game.cardOnTop.Color == 0)
            Debug.Log("Black card on top");

        if (chosenCard == null)
        {
            ONO.Current.game.DrawCard();
            chosenCard = ChooseCard();
        }

        if (chosenCard != null)
        {
            if (player.cardsOfPlayer.Count == 2)
                ONO.Current.game.OnoPressed();
            ONO.Current.game.TryPlayCard(chosenCard);
            player.PlayCard();
        }
    }

    public int GetWishColor()
    {
        return wishColor;
    }
}
