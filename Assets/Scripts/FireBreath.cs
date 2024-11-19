using UnityEngine;

public class FireBreath : MonoBehaviour
{
    [SerializeField] private ParticleSystem fireParticleSystem;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float fireDuration = 0.6f;
    [SerializeField] private float minPitch = 0.7f;
    [SerializeField] private float maxPitch = 0.9f;

    private float fireTimer;
    private bool isFiring;
    private AudioSource audioSource;
    private FireBarManager fireBarManager;

    public bool IsFiring => isFiring;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        fireBarManager = FindObjectOfType<FireBarManager>();
    }

    private void Update()
    {
        if (isFiring)
        {
            fireTimer -= Time.deltaTime;
            if (fireTimer <= 0f)
            {
                StopFire();
            }
        }

        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetMouseButton(0) && !isFiring && fireBarManager.CanFire())
        {
            Vector3 mousePosition = Input.mousePosition;
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, Camera.main.nearClipPlane));
            Vector2 direction = (worldMousePosition - firePoint.position).normalized;

            if (worldMousePosition.x > firePoint.position.x)
            {
                StartFire(direction);
            }
        }
    }

    private void StartFire(Vector2 direction)
    {
        isFiring = true;
        fireTimer = fireDuration;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        firePoint.rotation = Quaternion.Euler(0, 0, angle);

        var main = fireParticleSystem.main;
        main.startRotation = -angle * Mathf.Deg2Rad;
        fireParticleSystem.Play();

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.Play();

        fireBarManager.UseFirePoints();
    }

    private void StopFire()
    {
        isFiring = false;
        fireParticleSystem.Stop();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Enemy"))
        {
            Destroy(other.gameObject);
        }
    }
}