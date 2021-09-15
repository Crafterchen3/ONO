using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePlayerPoints : MonoBehaviour
{
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponentInParent<Score>().PlayerPointsPresent(index, gameObject);
    }
}
