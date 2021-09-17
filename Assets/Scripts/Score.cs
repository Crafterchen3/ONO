using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    Game game;

    GameObject Player1GO;
    TMP_Text Player1;
    GameObject Player2GO;
    TMP_Text Player2;
    GameObject Player3GO;
    TMP_Text Player3;
    GameObject Player4GO;
    TMP_Text Player4;
    GameObject Player5GO;
    TMP_Text Player5;
    GameObject Player6GO;
    TMP_Text Player6;

    GameObject Player1PointsGO;
    TMP_Text Player1Points;
    GameObject Player2PointsGO;
    TMP_Text Player2Points;
    GameObject Player3PointsGO;
    TMP_Text Player3Points;
    GameObject Player4PointsGO;
    TMP_Text Player4Points;
    GameObject Player5PointsGO;
    TMP_Text Player5Points;
    GameObject Player6PointsGO;
    TMP_Text Player6Points;

    int presentPlayers = 0;
    int presentPlayerPoints = 0;


    public void PlayerPresent(int index, GameObject gameObject)
    {
        switch (index)
        {
            case 1:
                Player1GO = gameObject;
                Player1 = Player1GO.GetComponent<TMP_Text>();
                break;
            case 2:
                Player2GO = gameObject;
                Player2 = Player2GO.GetComponent<TMP_Text>();
                break;
            case 3:
                Player3GO = gameObject;
                Player3 = Player3GO.GetComponent<TMP_Text>();
                break;
            case 4:
                Player4GO = gameObject;
                Player4 = Player4GO.GetComponent<TMP_Text>();
                break;
            case 5:
                Player5GO = gameObject;
                Player5 = Player5GO.GetComponent<TMP_Text>();
                break;
            case 6:
                Player6GO = gameObject;
                Player6 = Player6GO.GetComponent<TMP_Text>();
                break;
        }
        presentPlayers++;
        InstanceCheck();
    }

    public void PlayerPointsPresent(int index, GameObject gameObject)
    {
        switch (index)
        {
            case 1:
                Player1PointsGO = gameObject;
                Player1Points = Player1PointsGO.GetComponent<TMP_Text>();
                break;
            case 2:
                Player2PointsGO = gameObject;
                Player2Points = Player2PointsGO.GetComponent<TMP_Text>();
                break;
            case 3:
                Player3PointsGO = gameObject;
                Player3Points = Player3PointsGO.GetComponent<TMP_Text>();
                break;
            case 4:
                Player4PointsGO = gameObject;
                Player4Points = Player4PointsGO.GetComponent<TMP_Text>();
                break;
            case 5:
                Player5PointsGO = gameObject;
                Player5Points = Player5PointsGO.GetComponent<TMP_Text>();
                break;
            case 6:
                Player6PointsGO = gameObject;
                Player6Points = Player6PointsGO.GetComponent<TMP_Text>();
                break;
        }
        presentPlayerPoints++;
        InstanceCheck();
    }

    private void InstanceCheck()
    {
        if ((presentPlayers == 6) && (presentPlayerPoints == 6))
        {
            gameObject.SetActive(false);
            ONO.Current.ScorePopupPresent(this);

        }
    }

    private void HideUnhide()
    {
        game = ONO.Current.game;

        Player3GO.SetActive(3 <= game.numberOfPlayers);
        Player4GO.SetActive(4 <= game.numberOfPlayers);
        Player5GO.SetActive(5 <= game.numberOfPlayers);
        Player6GO.SetActive(6 <= game.numberOfPlayers);
        Player3PointsGO.SetActive(3 <= game.numberOfPlayers);
        Player4PointsGO.SetActive(4 <= game.numberOfPlayers);
        Player5PointsGO.SetActive(5 <= game.numberOfPlayers);
        Player6PointsGO.SetActive(6 <= game.numberOfPlayers);
    }


    private void ShowPlayerInfo(TMP_Text name, TMP_Text points, Player player)
    {
        name.text = player.playerName;
        points.text = player.ToString();
        if (player.IsWinner)
        {
            name.color = ONO.ActiveColor;
            points.color = ONO.ActiveColor;
        }
        else
        {
            name.color = ONO.InactiveColor;
            points.color = ONO.InactiveColor;
        }
    }
    public void Show()
    {
        HideUnhide();

        ShowPlayerInfo(Player1, Player1Points, game.players[0]);
        ShowPlayerInfo(Player2, Player2Points, game.players[1]);

        if (3 <= game.numberOfPlayers)
            ShowPlayerInfo(Player3, Player3Points, game.players[2]);

        if (4 <= game.numberOfPlayers)
            ShowPlayerInfo(Player4, Player4Points, game.players[3]);

        if (5 <= game.numberOfPlayers)
            ShowPlayerInfo(Player5, Player5Points, game.players[4]);

        if (6 <= game.numberOfPlayers)
            ShowPlayerInfo(Player6, Player6Points, game.players[5]);

        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
