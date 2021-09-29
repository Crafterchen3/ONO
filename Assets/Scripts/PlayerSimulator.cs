using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSimulator
{
    private Player player;

    public PlayerSimulator(Player player)
    {
        this.player = player;
    }

    private int wishColor;

    private CardDescriptor ChooseCard()
    {
        int[] numberOfCardsPerColor = new int[4];
        CardDescriptor plus4 = null;
        CardDescriptor wish = null;
        CardDescriptor plus2 = null;
        CardDescriptor changeDir = null;
        CardDescriptor skip = null;
        bool regularCardFound = false;

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
                numberOfCardsPerColor[c.Color - 1]++;
                if (c.valid)
                {
                    regularCardFound = true;
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

        if (regularCardFound)
        {
            if (plus2 != null)
                return plus2;
            if (skip != null)
                return skip;
            if (changeDir != null)
                return changeDir;

            foreach (CardDescriptor c in player.cardsOfPlayer)
                if ((c.valid) && !c.Special)
                    return c;
        }
        else
        {
            int maxCards = 0;
            wishColor = Random.Range(0, 63) % 4 + 1;
            for (int i = 0; i < 4; i++)
                if (numberOfCardsPerColor[i] > maxCards)
                {
                    wishColor = i + 1;
                    maxCards = numberOfCardsPerColor[i];
                }
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
