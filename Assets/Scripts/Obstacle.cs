using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.Rendering.VirtualTexturing.Debugging;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType { Static, MovingHorizontal, MovingVertical, Rotating }

    
    [SerializeField]  ColorChanger.PlayerColor obstacleColor = ColorChanger.PlayerColor.Red;
    [SerializeField]  Color redColor = Color.red;
    [SerializeField]  Color greenColor = Color.green;
    [SerializeField]  Color blueColor = Color.blue;

    
    [SerializeField]  ObstacleType obstacleType = ObstacleType.Static;
    [SerializeField]  float movementSpeed ;
    [SerializeField]  float movementDistance ;
    [SerializeField]  float rotationSpeed ;


    Rigidbody2D rb;
     SpriteRenderer spriteRenderer;
     Vector3 startPosition;
     Vector3 startScale;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        startScale = transform.localScale;
        UpdateObstacleColor();
    }

    private void FixedUpdate()
    {
        HandleObstacleBehavior();
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

                HandleHirozontalMovment();
                break;

            case ObstacleType.MovingVertical:

                HandleVerticalMovment();
                break;

            case ObstacleType.Rotating:

                HandleRotation();
                break;

            

            case ObstacleType.Static:
            default:
                
                break;
        }
    }

    private void HandleHirozontalMovment()
    {
        float targetX = startPosition.x + Mathf.Sin(Time.time * movementSpeed) * movementDistance;

        Vector2 targetPosition = new Vector2(targetX,rb.position.y);
        Vector2 direction = (targetPosition - rb.position);
        rb.AddForce(direction * movementSpeed);
    }

    private void HandleVerticalMovment()
    {
        float targetY = startPosition.y + Mathf.Sin(Time.time * movementSpeed) * movementDistance;
        Vector2 targetPosition = new Vector2(rb.position.x,targetY);
        Vector2 direction = (targetPosition - rb.position);
        rb.AddForce(direction * movementSpeed);
    }

    private void HandleRotation()
    {
        rb.AddTorque(rotationSpeed * Time.fixedDeltaTime);
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