using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float speed = 9f;
    [SerializeField] private float angle = 125.0f;
    [SerializeField] private float rotationSpeed = 500.0f;

    private Rigidbody2D rb;

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 85);

        Collider2D weaponCollider = GetComponent<Collider2D>();
        foreach (Collider2D col in FindObjectsOfType<Collider2D>())
        {
            if (!col.CompareTag("Player"))
            {
                Physics2D.IgnoreCollision(weaponCollider, col);
            }
        }

        rb = GetComponent<Rigidbody2D>();

        rb.gravityScale = 0.5f;

        Vector2 velocity = Quaternion.Euler(0, 0, angle) * Vector2.right * speed;

        rb.velocity = velocity;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
    }

    void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}