using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FireBarManager : MonoBehaviour
{
    [SerializeField] private Image fireBarImage;
    [SerializeField] private float maxFirePoints = 3f;
    [SerializeField] private float firePointUsage = 1f;
    [SerializeField] private float firePointRegenRate = 1f;
    [SerializeField] private float blinkInterval = 0.2f; 
    [SerializeField] private float blinkDuration = 7f;

    private float currentFirePoints;
    private bool isInfiniteFire = false;

    private void Start()
    {
        currentFirePoints = maxFirePoints;
        UpdateFireBar();
    }

    private void Update()
    {
        if (!isInfiniteFire)
        {
            RegenerateFirePoints();
        }
    }

    private void RegenerateFirePoints()
    {
        currentFirePoints = Mathf.Min(maxFirePoints, currentFirePoints + firePointRegenRate * Time.deltaTime);
        UpdateFireBar();
    }

    public bool CanFire()
    {
        return currentFirePoints >= firePointUsage || isInfiniteFire;
    }

    public void UseFirePoints()
    {
        if (!isInfiniteFire)
        {
            currentFirePoints -= firePointUsage;
            UpdateFireBar();
        }
    }

    private void UpdateFireBar()
    {
        fireBarImage.fillAmount = currentFirePoints / maxFirePoints;
    }

    public void RestoreAllFirePoints()
    {
        currentFirePoints = maxFirePoints;
        UpdateFireBar();
    }

    public void ActivateInfiniteFire(float duration)
    {
        isInfiniteFire = true;
        StartCoroutine(BlinkFireBar(blinkDuration));
        Invoke(nameof(DeactivateInfiniteFire), duration);
    }

    private void DeactivateInfiniteFire()
    {
        isInfiniteFire = false;
    }

    private IEnumerator BlinkFireBar(float duration)
    {
        float endTime = Time.time + duration;
        while (Time.time < endTime)
        {
            fireBarImage.enabled = !fireBarImage.enabled;
            yield return new WaitForSeconds(blinkInterval);
        }
        fireBarImage.enabled = true;
    }
}
