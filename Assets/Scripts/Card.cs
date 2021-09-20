using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    public CardDescriptor descriptor;

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
        if ((descriptor != null) && (descriptor.visible))
            Render();
    }

    private bool sendToCardStack = false;
    float finalRotation;

    public void MoveToCardStack()
    {
        sendToCardStack = true;
        finalRotation = Random.Range(0, 359);
        foreach (BoxCollider2D bc in gameObject.GetComponents<BoxCollider2D>())
        {
            bc.enabled = false;
        }
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = ONO.Current.game.playedCards.Count + 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (sendToCardStack)
            if (Vector3.Distance(transform.position, Vector3.zero) < 0.0001f)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                sendToCardStack = false;
            }
            else
            {
                float step = 20f * Time.deltaTime;
                float rotationStep = 360f * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, Vector3.zero, step);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, 0, finalRotation), rotationStep);
            }
    }

    public void SetDescriptor(CardDescriptor cardDescriptor)
    {
        this.descriptor = cardDescriptor;
        if (cardDescriptor.visible)
            Render();
    }

    public void Render()
    {
        if ((descriptor != null) && (game != null))
        {
            spriteRenderer.sprite = game.GetCardFace(descriptor);
            descriptor.visible = true;
            VisualizeValidity();
        }
    }

    public void VisualizeValidity()
    {
        spriteRenderer.color = (descriptor.valid) ? validColor : invalidColor;
    }

    void OnMouseDown()
    {
        game.TryPlayCard(descriptor, spriteRenderer.sprite);
    }
}
