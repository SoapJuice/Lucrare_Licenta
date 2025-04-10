using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamagedInvincibility : MonoBehaviour
{
    [SerializeField]
    private float duration;

    private ImmunityFrames immunityController;

    private void Awake()
    {
        immunityController = GetComponent<ImmunityFrames>();
    }
    public void startInvincibility()
    {
        immunityController.startImmunityFrames(duration);
    }
}
