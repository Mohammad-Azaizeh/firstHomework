using UnityEngine;

public class FinishLine : MonoBehaviour
{
    [SerializeField]  Color finishLineColor = Color.white;

     SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.color = finishLineColor;

        
        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player reached finish line!");

            if (GameManager.instance != null)
            {
                GameManager.instance.WinGame();
            }
        }
    }
}