using UnityEngine;

public class FireRatePowerUp : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    [SerializeField] private float destroyX = -13f;

    private PlayerManager playerManager;

    private void Start()
    {
        playerManager = FindObjectOfType<PlayerManager>();
    }

    void Update()
    {
        transform.position += Vector3.left * speed * Time.deltaTime;

        if (transform.position.x <= destroyX)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {

        if (other.CompareTag("Player"))
        {

            if (playerManager != null)
            {

                playerManager.ActivatePowerUp();

                Destroy(gameObject);
            }
        }
    }
}