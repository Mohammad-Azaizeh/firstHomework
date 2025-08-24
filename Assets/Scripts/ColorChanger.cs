using UnityEngine;

public class ColorChanger : MonoBehaviour
{
    public enum PlayerColor { Red, Green, Blue }

    [SerializeField]  PlayerColor currentColor = PlayerColor.Red;
    [SerializeField]  Color redColor = Color.red;
    [SerializeField]  Color greenColor = Color.green;
    [SerializeField]  Color blueColor = Color.blue;

     SpriteRenderer spriteRenderer;
     Collider2D playerCollider;

    public PlayerColor CurrentColor {
        get { 
            return currentColor;
        } 
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerCollider = GetComponent<Collider2D>();
        UpdateColorVisual();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeColor(PlayerColor.Red);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            ChangeColor(PlayerColor.Green);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            ChangeColor(PlayerColor.Blue);
        }
    }

    public void ChangeColor(PlayerColor newColor)
    {
        currentColor = newColor;
        UpdateColorVisual();
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
    }

    void UpdateColorVisual()
    {
        switch (currentColor)
        {
            case PlayerColor.Red:
                spriteRenderer.color = redColor;
                break;
            case PlayerColor.Green:
                spriteRenderer.color = greenColor;
                break;
            case PlayerColor.Blue:
                spriteRenderer.color = blueColor;
                break;
        }
    }
}