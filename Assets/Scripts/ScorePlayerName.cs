using UnityEngine;

public class ScorePlayerName : MonoBehaviour
{
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInParent<Score>().PlayerPresent(index, gameObject);
    }
}
