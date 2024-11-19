using UnityEngine;

public class SwordEnemy : MonoBehaviour
{
    [SerializeField] private float speed = 3f;
    [SerializeField] private float jumpForce = 7.5f;
    [SerializeField] private float destroyEnemyX = -13f;
    [SerializeField] private string deathAnimationTrigger = "Die";
    [SerializeField] private float deathAnimationDuration = 1f;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isDying = false;
    private PlayerManager playerManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        if (isDying) return;

        if (player != null)
        {
            Vector2 direction = Vector2.left;
            transform.position += (Vector3)direction * speed * Time.deltaTime;

            if (Mathf.Abs(player.position.x - transform.position.x) < 3.5f && rb.velocity.y == 0)
            {
                Jump();
            }

            if (transform.position.x <= destroyEnemyX)
            {
                Destroy(gameObject);
            }
        }
    }

    void Jump()
    {
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Die();
        }
    }

    void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("PlayerParticles"))
        {
            Die();
        }
    }

    void Die()
    {
        if (isDying) return;
        isDying = true;

        if (playerManager != null)
        {
            playerManager.IncrementTotalKills();
        }

        animator.SetTrigger(deathAnimationTrigger);

        Invoke("DisableEnemy", deathAnimationDuration);
    }

    void DisableEnemy()
    {
        gameObject.SetActive(false);
        isDying = false;
    }
}