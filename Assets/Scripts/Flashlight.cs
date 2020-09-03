using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : BaseObject
{
    [SerializeField] private Light FlashLight;
    [SerializeField] private float timeout = 10;
    private float currTime;
    private float currReloadTime;

    private KeyCode control = KeyCode.F;

    protected override void Awake()
    {
        base.Awake();
        FlashLight = GetComponentInChildren<Light>();
    }

    private void ActiveFlashLight(bool val)
    {
        FlashLight.enabled = val;
    }
    void Update()
    {
        if (Input.GetKeyDown(control))
        {
            ActiveFlashLight(!FlashLight.enabled);
        }

        if (FlashLight.enabled)
        {
            currTime += Time.deltaTime;
            if (currTime > timeout)
            {
                currTime = 0;
                ActiveFlashLight(false);
            }
        }
    }
}
