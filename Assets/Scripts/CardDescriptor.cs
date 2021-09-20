using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDescriptor : IComparable<CardDescriptor>
{
    public int Color;
    public bool Special;
    public int Number;
    public bool valid;
    public bool visible = false;

    // Colors:
    public const int BLACK  = 0;
    public const int RED    = 1;
    public const int YELLOW = 2;
    public const int GREEN  = 3;
    public const int BLUE   = 4;

    // Special cards with colors:
    public const int SKIP       = 10;
    public const int CHANGE_DIR = 11;
    public const int PLUS2      = 12;

    // Special cards without colors:
    public const int WISH = 1;
    public const int WISHPLUS4 = 2;

    public CardDescriptor(int color, int number)
    {
        Color = color;
        Number = number;
        Special = false;
    }

    public CardDescriptor(int number)
    {
        Color = 0;
        Number = number;
        Special = true;
    }

    public int CompareTo(CardDescriptor other)
    {
        // A null value means that this object is greater.
        if (other == null)
            return 1;
        else if (Special && !other.Special)
            return -1;
        else if (!Special && other.Special)
            return 1;
        else if (Special)
            return -1 * Number.CompareTo(other.Number);
        else if (Color == other.Color)
            return -1 * Number.CompareTo(other.Number);
        else
            return -1 * Color.CompareTo(other.Color);
    }

    public bool CanBePlayed(CardDescriptor cardOnTop)
    {
        if (Special)
            return (true);
        if (cardOnTop.Special)
            return Color == cardOnTop.Color;
        else
            return (Color == cardOnTop.Color) || (Number == cardOnTop.Number);
    }
}
