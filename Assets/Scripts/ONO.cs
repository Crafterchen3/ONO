using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static UnityEngine.GUI;

public class ONO
{

    private static ONO _current;


    public static ONO Current
    {
        get
        {
            if (_current == null)
                _current = new ONO();
            return _current;
        }
        set { }
    }

    public static Color InactiveColor = new Color32(255, 157, 0, 255);
    public static Color ActiveColor = new Color32(255, 255, 0, 255);

    public PlayerNames playerNames;

    public GameObject gameGO;
    public Game game;

    public GameObject wishPopup;

    public GameObject unplayedCards;
    public GameObject onoButton;

    public GameObject nextPlayerPopup;
    public NextPlayer nextPlayerButton;

    public Score scoreDialog;

    public GameObject arrow;

    public Launcher launcher;

    public PlayerChooser playerChooser;

    public Highscores highscores;

    public SkipMove skipMove;

    private SizingProblem sizingProblem;

    public PlayerWinsPopup playerWinsDialog;

    private int instanceCount = 0;

    // Instance 1
    public void WishPopupPresent(GameObject wishPopup)
    {
        this.wishPopup = wishPopup;
        InstanceCheck();
    }

    // Instance 2
    public void DrawArrowPresent(GameObject arrow)
    {
        this.arrow = arrow;
        InstanceCheck();
    }

    // Instance 3
    public void NextPlayerPopupPresent(NextPlayer nextPlayer, GameObject nextPlayerGameObject)
    {
        nextPlayerPopup = nextPlayerGameObject;
        nextPlayerButton = nextPlayer;
        InstanceCheck();
    }

    // Instance 4
    public void PlayerWinsPopupPresent(PlayerWinsPopup playerWinsPopup)
    {
        playerWinsDialog = playerWinsPopup;
        InstanceCheck();
    }

    // Instance 5
    public void ScorePopupPresent(Score score)
    {
        scoreDialog = score;
        InstanceCheck();
    }

    // Instance 6
    public void PlayerNamesPopupPresent(PlayerNames playerNames)
    {
        this.playerNames = playerNames;
        InstanceCheck();
    }

    // Instance 7
    public void GameControlPresent(Game game, GameObject gameObject)
    {
        gameGO = gameObject;
        this.game = game;
        InstanceCheck();
    }

    // Instance 8
    public void LauncherPresent(Launcher launcher)
    {
        this.launcher = launcher;
        InstanceCheck();
    }

    // Instance 9
    public void NameChooserPresent(PlayerChooser playerChooser)
    {
        this.playerChooser = playerChooser;
        InstanceCheck();
    }

    // Instance 10
    public void UnplayedCardsPresent(GameObject gameObject)
    {
        unplayedCards = gameObject;
        InstanceCheck();
    }

    // Instance 11
    public void OnoButtonPresent(GameObject gameObject)
    {
        onoButton = gameObject;
        InstanceCheck();
    }

    // Instance 12
    public void HighscoresPresent(Highscores highscores)
    {
        this.highscores = highscores;
        InstanceCheck();
    }


    // Instance 13
    public void SkipMovePresent(SkipMove skipMove)
    {
        this.skipMove = skipMove;
        InstanceCheck();
    }

    // Instance 14
    private LayoutManager.ILayout layout;
 
    public void SetLayout(LayoutManager.ILayout layout)
    {
        this.layout = layout;
        InstanceCheck();
    }

    // Instance 15
    internal void SizingProblemPopupPresent(SizingProblem sizingProblem)
    {
        this.sizingProblem = sizingProblem;
        InstanceCheck();
    }

    private void InstanceCheck()
    {
        instanceCount++;
        if (instanceCount >= 15)
        {
            if (layout != null)
            {
                game.SetLayout(layout);
                launcher.Show();
            }
            else
                sizingProblem.Show();
        }
    }
}
