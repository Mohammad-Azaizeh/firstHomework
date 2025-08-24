using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType { Static, MovingHorizontal, MovingVertical, Rotating }

    
    [SerializeField]  ColorChanger.PlayerColor obstacleColor = ColorChanger.PlayerColor.Red;
    [SerializeField]  Color redColor = Color.red;
    [SerializeField]  Color greenColor = Color.green;
    [SerializeField]  Color blueColor = Color.blue;

    
    [SerializeField]  ObstacleType obstacleType = ObstacleType.Static;
    [SerializeField]  float movementSpeed = 2f;
    [SerializeField]  float movementDistance = 2f;
    [SerializeField]  float rotationSpeed = 45f;
    
    

     SpriteRenderer spriteRenderer;
     Vector3 startPosition;
     Vector3 startScale;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startPosition = transform.position;
        startScale = transform.localScale;
        UpdateObstacleColor();
    }

    void Update()
    {
        HandleObstacleBehavior();
    }

    private void HandleObstacleBehavior()
    {
        switch (obstacleType)
        {
            case ObstacleType.MovingHorizontal:
                
                float xMovement = Mathf.Sin(Time.time * movementSpeed) * movementDistance;
                transform.position = startPosition + new Vector3(xMovement, 0, 0);
                break;

            case ObstacleType.MovingVertical:
                
                float yMovement = Mathf.Sin(Time.time * movementSpeed) * movementDistance;
                transform.position = startPosition + new Vector3(0, yMovement, 0);
                break;

            case ObstacleType.Rotating:
               
                transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
                break;

            

            case ObstacleType.Static:
            default:
                
                break;
        }
    }

    private void UpdateObstacleColor()
    {
        switch (obstacleColor)
        {
            case ColorChanger.PlayerColor.Red:
                spriteRenderer.color = redColor;
                break;
            case ColorChanger.PlayerColor.Green:
                spriteRenderer.color = greenColor;
                break;
            case ColorChanger.PlayerColor.Blue:
                spriteRenderer.color = blueColor;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandlePlayerCollision(collision.gameObject);
    }

    private void HandlePlayerCollision(GameObject playerObject)
    {
        
        if (GameManager.instance != null && !GameManager.instance.IsGameActive())
            return;

        ColorChanger player = playerObject.GetComponent<ColorChanger>();
        if (player != null && player.CurrentColor != obstacleColor)
        {
            Debug.Log("Wrong color!");

            
            if (GameManager.instance != null)
            {
                GameManager.instance.GameOver();
            }
        }
    }

    
    public void ChangeColor(ColorChanger.PlayerColor newColor)
    {
        obstacleColor = newColor;
        UpdateObstacleColor();
    }
}