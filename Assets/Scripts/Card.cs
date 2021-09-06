using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    private Game game;
    private CardDescriptor cardDescriptor;
    private SpriteRenderer spriteRenderer;


    // Start is called before the first frame update
    void Start()
    {
        GameObject gameObject = GameObject.FindGameObjectWithTag("game");
        game = gameObject.GetComponent<Game>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Render();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDescriptor(CardDescriptor cardDescriptor)
    {
        this.cardDescriptor = cardDescriptor;
        Render();
    }

    private void Render()
    {
        if ((cardDescriptor != null) && (game != null))
            spriteRenderer.sprite = game.GetCardFace(cardDescriptor);
    }

    void OnMouseDown()
    {
        if (game.PlayCard(cardDescriptor, spriteRenderer.sprite))
            Destroy(base.gameObject);
    }
}
