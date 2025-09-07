using UnityEngine;
using System.Collections;

public class Obstacle : MonoBehaviour
{
    public enum ObstacleType { Static, MovingHorizontal, MovingVertical, Rotating }

    [SerializeField] ColorChanger.PlayerColor obstacleColor = ColorChanger.PlayerColor.Red;
    [SerializeField] Color redColor = Color.red;
    [SerializeField] Color greenColor = Color.green;
    [SerializeField] Color blueColor = Color.blue;
    [SerializeField] ObstacleType obstacleType = ObstacleType.Static;
    [SerializeField] float movementSpeed = 2f;
    [SerializeField] float movementDistance = 2f;
    [SerializeField] float rotationSpeed = 45f;
    [SerializeField] float obstacleMass = 1000f;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb;
    Vector2 startPosition;
    Vector2 startScale;
    Coroutine movementCoroutine;
    Coroutine rotationCoroutine;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        startScale = transform.localScale;
        UpdateObstacleColor();

        rb.mass = obstacleMass;

        if (obstacleType != ObstacleType.Rotating)
        {
            rb.freezeRotation = true;
        }

        switch (obstacleType)
        {
            case ObstacleType.MovingHorizontal:
                movementCoroutine = StartCoroutine(MoveHorizontalCoroutine());
                break;

            case ObstacleType.MovingVertical:
                movementCoroutine = StartCoroutine(MoveVerticalCoroutine());
                break;

            case ObstacleType.Rotating:
                rotationCoroutine = StartCoroutine(RotateCoroutine());
                break;

            case ObstacleType.Static:
            default:
                break;
        }
    }
    void FixedUpdate()
    {
        if (obstacleType != ObstacleType.Static)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
    IEnumerator MoveHorizontalCoroutine()
    {
        float time = 0f;
        float leftBound = startPosition.x - movementDistance;
        float rightBound = startPosition.x + movementDistance;

        while (true)
        {
            float targetX = Mathf.PingPong(time * movementSpeed, movementDistance * 2) + leftBound;
            Vector2 targetPosition = new Vector2(targetX, startPosition.y);
            rb.MovePosition(targetPosition);

            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator MoveVerticalCoroutine()
    {
        float time = 0f;
        float bottomBound = startPosition.y - movementDistance;
        float topBound = startPosition.y + movementDistance;

        while (true)
        {
            float targetY = Mathf.PingPong(time * movementSpeed, movementDistance * 2) + bottomBound;
            Vector2 targetPosition = new Vector2(startPosition.x, targetY);
            rb.MovePosition(targetPosition);
            time += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }
    }
    IEnumerator RotateCoroutine()
    {
        while (true)
        {
            rb.MoveRotation(rb.rotation + rotationSpeed * Time.fixedDeltaTime);
            yield return new WaitForFixedUpdate();
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
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            Physics2D.IgnoreCollision(GetComponent<Collider2D>(), collision.collider);
            return;
        }
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            return;
        }
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
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
    void OnDisable()
    {
        if (movementCoroutine != null)
        {
            StopCoroutine(movementCoroutine);
        }
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }
    }
}
