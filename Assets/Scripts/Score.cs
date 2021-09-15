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

    // Start is called before the first frame update
    void Start()
    {
        Player1GO = GameObject.Find("Player1");
        Player1 = Player1GO.GetComponent<TMP_Text>();
        Player2GO = GameObject.Find("Player2");
        Player2 = Player2GO.GetComponent<TMP_Text>();
        Player3GO = GameObject.Find("Player3");
        Player3 = Player3GO.GetComponent<TMP_Text>();
        Player4GO = GameObject.Find("Player4");
        Player4 = Player4GO.GetComponent<TMP_Text>();
        Player5GO = GameObject.Find("Player5");
        Player5 = Player5GO.GetComponent<TMP_Text>();
        Player6GO = GameObject.Find("Player6");
        Player6 = Player6GO.GetComponent<TMP_Text>();

        Player1PointsGO = GameObject.Find("Player1 Points");
        Player1Points = Player1PointsGO.GetComponent<TMP_Text>();
        Player2PointsGO = GameObject.Find("Player2 Points");
        Player2Points = Player2PointsGO.GetComponent<TMP_Text>();
        Player3PointsGO = GameObject.Find("Player3 Points");
        Player3Points = Player3PointsGO.GetComponent<TMP_Text>();
        Player4PointsGO = GameObject.Find("Player4 Points");
        Player4Points = Player4PointsGO.GetComponent<TMP_Text>();
        Player5PointsGO = GameObject.Find("Player5 Points");
        Player5Points = Player5PointsGO.GetComponent<TMP_Text>();
        Player6PointsGO = GameObject.Find("Player6 Points");
        Player6Points = Player6PointsGO.GetComponent<TMP_Text>();

        gameObject.SetActive(false);
        ONO.Current.ScorePopupPresent(this);
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

    public void Show()
    {
        HideUnhide();

        Player1.text = game.players[0].playerName;
        Player1Points.text = game.players[0].wonGames.ToString();
        Player2.text = game.players[1].playerName;
        Player2Points.text = game.players[1].wonGames.ToString();

        if (3 <= game.numberOfPlayers)
        {
            Player3.text = game.players[2].playerName;
            Player3Points.text = game.players[2].wonGames.ToString();
        }

        if (4 <= game.numberOfPlayers)
        {
            Player4.text = game.players[3].playerName;
            Player4Points.text = game.players[3].wonGames.ToString();
        }

        if (5 <= game.numberOfPlayers)
        {
            Player5.text = game.players[4].playerName;
            Player5Points.text = game.players[4].wonGames.ToString();
        }

        if (6 <= game.numberOfPlayers)
        {
            Player6.text = game.players[5].playerName;
            Player6Points.text = game.players[5].wonGames.ToString();
        }
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
