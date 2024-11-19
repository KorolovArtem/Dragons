using UnityEngine;

public class DragonHeadController : MonoBehaviour
{
    [SerializeField] private Transform dragonHead;
    [SerializeField] private float rotationSpeed = 5f;
    [SerializeField] private Animator dragonAnimator;
    [SerializeField] private FireBreath fireBreath;

    private void Update()
    {
        Vector3 targetPosition = GetTargetPosition();
        RotateDragonHead(targetPosition);

        if (fireBreath != null)
        {
            bool isFiring = fireBreath.IsFiring;
            dragonAnimator.SetBool("IsBreathingFire", isFiring);
        }
    }

    Vector3 GetTargetPosition()
    {
        Vector3 targetPosition = Vector3.zero;

#if UNITY_STANDALONE || UNITY_WEBGL
        targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0;
#elif UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            targetPosition = Camera.main.ScreenToWorldPoint(touch.position);
            targetPosition.z = 0;
        }
#endif

        return targetPosition;
    }

    void RotateDragonHead(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - dragonHead.position;
        direction.z = 0;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle = Mathf.Clamp(angle, -45f, 60f);

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        dragonHead.rotation = Quaternion.Lerp(dragonHead.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}