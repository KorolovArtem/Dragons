using UnityEngine;

public class SpearEnemy : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float throwRange = 20f;
    [SerializeField] private float minThrowCooldown = 3f;
    [SerializeField] private float maxThrowCooldown = 6f;
    [SerializeField] private GameObject spearPrefab;
    [SerializeField] private float destroyEnemyX = -13f;
    [SerializeField] private string deathAnimationTrigger = "Die";
    [SerializeField] private float deathAnimationDuration = 1f;

    private Transform player;
    private float throwTimer;
    private Animator animator;
    private bool isDying = false;
    private PlayerManager playerManager;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        ResetThrowTimer();
        playerManager = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        if (isDying) return;

        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);

            if (distance > throwRange || throwTimer > 0)
            {
                Vector2 direction = Vector2.left;
                transform.position += (Vector3)direction * speed * Time.deltaTime;
            }

            if (throwTimer > 0)
            {
                throwTimer -= Time.deltaTime;
            }

            if (distance <= throwRange && throwTimer <= 0)
            {
                ThrowSpear();
                ResetThrowTimer();
            }

            if (transform.position.x <= destroyEnemyX)
            {
                Destroy(gameObject);
            }
        }
    }

    void ThrowSpear()
    {
        if (player != null)
        {
            GameObject spear = Instantiate(spearPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = spear.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                Vector2 direction = (player.position - transform.position).normalized;
                rb.velocity = direction * speed;
            }
        }
    }

    void ResetThrowTimer()
    {
        throwTimer = Random.Range(minThrowCooldown, maxThrowCooldown);
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