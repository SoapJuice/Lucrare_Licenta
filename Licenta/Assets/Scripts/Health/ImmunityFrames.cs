using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImmunityFrames : MonoBehaviour
{
    private HealthController healthController;

    private void Awake()
    {
        healthController = GetComponent<HealthController>();
    }

    public void startImmunityFrames(float duration)
    {
        StartCoroutine(InvincibilityCoroutine(duration));
    }

    private IEnumerator InvincibilityCoroutine(float duration)
    {
        healthController.immune = true;
        yield return new WaitForSeconds(duration);
        healthController.immune = false;
    }
}
