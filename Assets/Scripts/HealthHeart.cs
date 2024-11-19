using UnityEngine;

public class HealthHeart : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float destroyX = -13f;

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerManager playerHealth = other.GetComponent<PlayerManager>();
            if (playerHealth != null)
            {
                playerHealth.Heal(1);
            }
            Destroy(gameObject);
        }
    }
}
