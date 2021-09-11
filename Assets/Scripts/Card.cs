using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardDescriptor CardDescriptor;

    private Game game;
    private SpriteRenderer spriteRenderer;
    private Color validColor = new Color32(255, 255, 255, 255);
    private Color invalidColor = new Color32(128, 128, 128, 255);


    // Start is called before the first frame update
    void Start()
    {
        GameObject controllerObject = GameObject.FindGameObjectWithTag("game");
        game = controllerObject.GetComponent<Game>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        Render();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetDescriptor(CardDescriptor cardDescriptor)
    {
        this.CardDescriptor = cardDescriptor;
        Render();
    }

    private void Render()
    {
        if ((CardDescriptor != null) && (game != null))
        {
            spriteRenderer.sprite = game.GetCardFace(CardDescriptor);
            VisualizeValidity();
        }
    }

    public void VisualizeValidity()
    {
        spriteRenderer.color = (CardDescriptor.valid) ? validColor : invalidColor;
    }

    void OnMouseDown()
    {
        game.TryPlayCard(CardDescriptor, spriteRenderer.sprite);
    }
}
