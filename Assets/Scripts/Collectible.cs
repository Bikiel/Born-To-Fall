using UnityEngine;

public class Collectible : MonoBehaviour
{
    [SerializeField] private Sprite[] gemSprites;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        SetRandomSprite();
    }

    private void SetRandomSprite()
    {
        if (spriteRenderer == null || gemSprites == null || gemSprites.Length == 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, gemSprites.Length);
        spriteRenderer.sprite = gemSprites[randomIndex];
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddScore(1);
            }

            Destroy(gameObject);
        }
    }
}
