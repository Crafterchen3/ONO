using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PopulateHighscores : MonoBehaviour
{
    public GameObject PlayerNamePrefab;

    private VerticalLayoutGroup vLayout;

    // Start is called before the first frame update
    void Start()
    {
        vLayout = gameObject.GetComponent<VerticalLayoutGroup>();
    }

    void RenderPlayer(HighScore score)
    {
        GameObject clone = (GameObject)Instantiate(PlayerNamePrefab, transform);
        foreach (TMP_Text t in clone.GetComponentsInChildren<TMP_Text>())
            if (t.tag == "playerName")
                t.text = score.playerName;
            else
                t.text = score.highscore.ToString();
    }

    public void Populate()
    {
        for (int i = 0; i < vLayout.transform.childCount; i++)
            Destroy(vLayout.transform.GetChild(i).gameObject);
        foreach (HighScore h in ONO.Current.game.highScoreHistory.GetHighscores())
            RenderPlayer(h);
    }
}
