using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerNames : MonoBehaviour
{

    public Sprite NormalPlayer;
    public Sprite VirtualPlayer;

    GameObject NoOfLayersChooserGO;
    TMP_Text NoOfPayersText;
    int NoOfPlayers = 2;

    GameObject Player1GO;
    TMP_InputField Player1;

    GameObject Player2GO;
    TMP_InputField Player2;

    GameObject Player3GO;
    TMP_InputField Player3;

    GameObject Player4GO;
    TMP_InputField Player4;

    GameObject Player5GO;
    TMP_InputField Player5;

    GameObject Player6GO;
    TMP_InputField Player6;

    GameObject ChoosePlayer1GO;
    GameObject ChoosePlayer2GO;
    GameObject ChoosePlayer3GO;
    GameObject ChoosePlayer4GO;
    GameObject ChoosePlayer5GO;
    GameObject ChoosePlayer6GO;

    GameObject ChangePlayerKind1GO;
    GameObject ChangePlayerKind2GO;
    GameObject ChangePlayerKind3GO;
    GameObject ChangePlayerKind4GO;
    GameObject ChangePlayerKind5GO;
    GameObject ChangePlayerKind6GO;

    bool[] IsVirtualPlayer = new bool[6];

    // Start is called before the first frame update
    void Start()
    {
        NoOfLayersChooserGO = GameObject.Find("NoOfPlayers");
        NoOfPayersText = NoOfLayersChooserGO.GetComponent<TMP_Text>();
        Player1GO = GameObject.Find("Player1");
        Player1 = Player1GO.GetComponent<TMP_InputField>();
        Player2GO = GameObject.Find("Player2");
        Player2 = Player2GO.GetComponent<TMP_InputField>();
        Player3GO = GameObject.Find("Player3");
        Player3 = Player3GO.GetComponent<TMP_InputField>();
        Player4GO = GameObject.Find("Player4");
        Player4 = Player4GO.GetComponent<TMP_InputField>();
        Player5GO = GameObject.Find("Player5");
        Player5 = Player5GO.GetComponent<TMP_InputField>();
        Player6GO = GameObject.Find("Player6");
        Player6 = Player6GO.GetComponent<TMP_InputField>();
        ChoosePlayer1GO = GameObject.Find("ChoosePlayer1");
        ChoosePlayer2GO = GameObject.Find("ChoosePlayer2");
        ChoosePlayer3GO = GameObject.Find("ChoosePlayer3");
        ChoosePlayer4GO = GameObject.Find("ChoosePlayer4");
        ChoosePlayer5GO = GameObject.Find("ChoosePlayer5");
        ChoosePlayer6GO = GameObject.Find("ChoosePlayer6");
        ChangePlayerKind1GO = GameObject.Find("PlayerKind1");
        ChangePlayerKind2GO = GameObject.Find("PlayerKind2");
        ChangePlayerKind3GO = GameObject.Find("PlayerKind3");
        ChangePlayerKind4GO = GameObject.Find("PlayerKind4");
        ChangePlayerKind5GO = GameObject.Find("PlayerKind5");
        ChangePlayerKind6GO = GameObject.Find("PlayerKind6");

        HideUnhide();

        Hide();

        ONO.Current.PlayerNamesPopupPresent(this);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void HideUnhide()
    {
        Player3GO.SetActive(3 <= NoOfPlayers);
        Player4GO.SetActive(4 <= NoOfPlayers);
        Player5GO.SetActive(5 <= NoOfPlayers);
        Player6GO.SetActive(6 <= NoOfPlayers);
        ChoosePlayer3GO.SetActive(3 <= NoOfPlayers);
        ChoosePlayer4GO.SetActive(4 <= NoOfPlayers);
        ChoosePlayer5GO.SetActive(5 <= NoOfPlayers);
        ChoosePlayer6GO.SetActive(6 <= NoOfPlayers);
        ChangePlayerKind3GO.SetActive(3 <= NoOfPlayers);
        ChangePlayerKind4GO.SetActive(4 <= NoOfPlayers);
        ChangePlayerKind5GO.SetActive(5 <= NoOfPlayers);
        ChangePlayerKind6GO.SetActive(6 <= NoOfPlayers);
    }

    public void MorePlayers()
    {
        if (NoOfPlayers < ONO.Current.game.layout.GetMaxPlayers())
        {
            NoOfPlayers++;
            NoOfPayersText.text = NoOfPlayers.ToString();
            HideUnhide();
        }
    }

    public void LessPlayers()
    {
        if (NoOfPlayers > 2)
        {
            NoOfPlayers--;
            NoOfPayersText.text = NoOfPlayers.ToString();
            HideUnhide();
        }
    }

    private int currentPlayerToChoose = 0;

    public void ChoosePlayer(int playerNo)
    {
        currentPlayerToChoose = playerNo;
        ONO.Current.playerChooser.Show();
    }

    private void RenderPlayerKind(int playerNo)
    {
        Sprite kind;

        if (IsVirtualPlayer[playerNo - 1])
            kind = VirtualPlayer;
        else
            kind = NormalPlayer;
        switch (playerNo)
        {
            case 1:
                ChangePlayerKind1GO.GetComponent<Image>().sprite = kind;
                break;
            case 2:
                ChangePlayerKind2GO.GetComponent<Image>().sprite = kind;
                break;
            case 3:
                ChangePlayerKind3GO.GetComponent<Image>().sprite = kind;
                break;
            case 4:
                ChangePlayerKind4GO.GetComponent<Image>().sprite = kind;
                break;
            case 5:
                ChangePlayerKind5GO.GetComponent<Image>().sprite = kind;
                break;
            case 6:
                ChangePlayerKind6GO.GetComponent<Image>().sprite = kind;
                break;
        }
    }

    public void ChangePlayerKind(int playerNo)
    {
        IsVirtualPlayer[playerNo - 1] = !IsVirtualPlayer[playerNo - 1];
        RenderPlayerKind(playerNo);
    }

    public void SetChosenPlayer(string name, bool isVirtual)
    {
        IsVirtualPlayer[currentPlayerToChoose - 1] = isVirtual;
        RenderPlayerKind(currentPlayerToChoose);

        switch (currentPlayerToChoose)
        {
            case 1:
                Player1.text = name;
                break;
            case 2:
                Player2.text = name;
                break;
            case 3:
                Player3.text = name;
                break;
            case 4:
                Player4.text = name;
                break;
            case 5:
                Player5.text = name;
                break;
            case 6:
                Player6.text = name;
                break;
        }
    }

    public string GetPlayerName(int index)
    {
        switch (index)
        {
            case 0:
                return Player1.text;
            case 1:
                return Player2.text;
            case 2:
                return Player3.text;
            case 3:
                return Player4.text;
            case 4:
                return Player5.text;
            case 5:
                return Player6.text;
        }
        return "";
    }

    public bool GetPlayerIsVirtual(int index)
    {
        return IsVirtualPlayer[index];
    }

    public void StartGame()
    {
        Game game = ONO.Current.game;
        game.PrepareNewGame(NoOfPlayers);
        bool newPlayer = false;
        for (int i = 0; i < NoOfPlayers; i++)
        {
            game.CreatePlayer(i, GetPlayerName(i), GetPlayerIsVirtual(i));
            newPlayer = game.highScoreHistory.AddName(GetPlayerName(i), GetPlayerIsVirtual(i)) || newPlayer;
        }
        if (newPlayer)
            game.persistence.SaveHighScores();

        Hide();
        game.Show();
    }

}
