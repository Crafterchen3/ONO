using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkipMove : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Hide();
        ONO.Current.SkipMovePresent(this);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        ONO.Current.game.SkipMovePressed();
        Hide();
    }
}
