using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SizingProblem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Hide();
        ONO.Current.SizingProblemPopupPresent(this);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

}
