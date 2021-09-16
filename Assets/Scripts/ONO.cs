using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

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

    public GameObject playerNamesGO;
    public PlayerNames playerNames;

    public GameObject gameGO;
    public Game game;

    public GameObject wishPopup;

    public GameObject unplayedCards;
    public GameObject onoButton;

    public GameObject nextPlayerPopup;
    public NextPlayer nextPlayerButton;

    public GameObject playerWinsPopup;
    public PlayerWins playerWinsDialog;

    public Score scoreDialog;

    public GameObject arrow;

    public Launcher launcher;

    public GameObject PlayerChooserGO;

    public Highscores highscores;

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
    public void PlayerWinsPopupPresent(PlayerWins playerWins, GameObject gameObject)
    {
        playerWinsPopup = gameObject;
        playerWinsDialog = playerWins;
        InstanceCheck();
    }

    // Instance 5
    public void ScorePopupPresent(Score score)
    {
        scoreDialog = score;
        InstanceCheck();
    }

    // Instance 6
    public void PlayerNamesPopupPresent(PlayerNames playerNames, GameObject gameObject)
    {
        this.playerNames = playerNames;
        playerNamesGO = gameObject;
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
    public void NameChooserPresent(GameObject chooser)
    {
        PlayerChooserGO = chooser;
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

    private void InstanceCheck()
    {
        instanceCount++;
        if (instanceCount >= 12)
            launcher.Show();
    }
}
