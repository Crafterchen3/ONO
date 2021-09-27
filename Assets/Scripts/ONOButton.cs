using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ONOButton : MonoBehaviour
{
    private Animator messageAnimator;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
        messageAnimator = gameObject.GetComponentInChildren<Animator>();
        ONO.Current.OnoButtonPresent(gameObject);
    }

    private void OnMouseDown()
    { 
        messageAnimator.SetBool("Pressed", true);
        ONO.Current.game.OnoPressed();

    }

    public void StartOfAnimation()
    {
    }

    public void EndOfAnimation()
    {
       messageAnimator.SetBool("Pressed", false);
    }

}
