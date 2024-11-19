using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float minX = -5f;
    [SerializeField] private float maxX = 5f;
    [SerializeField] private float minY = -5f;
    [SerializeField] private float maxY = 5f;
    [SerializeField] private GameObject joystick;

    private Rigidbody2D rb;
    private Vector2 movement;

    private VirtualJoystick virtualJoystick;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

#if UNITY_ANDROID || UNITY_IOS
        EnableMobileControls();
#elif UNITY_WEBGL
        if (IsMobileBrowser())
        {
            EnableMobileControls();
        }
        else
        {
            DisableMobileControls();
        }
#else
        DisableMobileControls();
#endif
    }

    void Update()
    {
#if UNITY_STANDALONE || UNITY_WEBGL
        if (!IsMobileBrowser())
        {
            HandlePCInput();
        }
        else
        {
            HandleMobileInput();
        }
#elif UNITY_ANDROID || UNITY_IOS
        HandleMobileInput();
#endif
    }

    void HandlePCInput()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveY = Input.GetAxis("Vertical");

        movement = new Vector2(moveX, moveY);
    }

    void HandleMobileInput()
    {
        if (virtualJoystick != null)
        {
            float moveX = virtualJoystick.Horizontal();
            float moveY = virtualJoystick.Vertical();

            movement = new Vector2(moveX, moveY);
        }
    }

    void FixedUpdate()
    {
        Vector2 newPosition = rb.position + movement * moveSpeed * Time.fixedDeltaTime;

        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);

        rb.MovePosition(newPosition);
    }

    void EnableMobileControls()
    {
        virtualJoystick = FindObjectOfType<VirtualJoystick>();
        if (joystick != null)
        {
            joystick.SetActive(true);
        }
    }

    void DisableMobileControls()
    {
        if (joystick != null)
        {
            joystick.SetActive(false);
        }
    }

    bool IsMobileBrowser()
    {
        return SystemInfo.deviceType == DeviceType.Handheld || Application.isMobilePlatform ||
               (Application.platform == RuntimePlatform.WebGLPlayer &&
                (SystemInfo.operatingSystem.Contains("iPhone") ||
                 SystemInfo.operatingSystem.Contains("Android") ||
                 SystemInfo.operatingSystem.Contains("iPad")));
    }
}