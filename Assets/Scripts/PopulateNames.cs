using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopulateNames : MonoBehaviour
{
    public GameObject PlayerNamePrefab;
    public Sprite NormalPlayer;
    public Sprite VirtualPlayer;

    private VerticalLayoutGroup vLayout;

    void Start()
    {
        vLayout = gameObject.GetComponent<VerticalLayoutGroup>();
    }

    private void RenderPlayer(HighScore entry)
    {
        GameObject clone = (GameObject)Instantiate(PlayerNamePrefab, transform);
        foreach (TMP_Text t in clone.GetComponentsInChildren<TMP_Text>())
            if (t.tag == "playerName")
                t.text = entry.playerName;
        if (entry.isVirtual)
            clone.GetComponentInChildren<Image>().sprite = VirtualPlayer;
        else
            clone.GetComponentInChildren<Image>().sprite = NormalPlayer;
        clone.GetComponent<PlayerName>().isVirtual = entry.isVirtual;
    }

    public void Populate()
    {
        vLayout = gameObject.GetComponent<VerticalLayoutGroup>();
        for (int i = 0; i < vLayout.transform.childCount; i++)
            Destroy(vLayout.transform.GetChild(i).gameObject);
        foreach (HighScore h in ONO.Current.game.highScoreHistory.GetAllNames())
            RenderPlayer(h);
    }

}
