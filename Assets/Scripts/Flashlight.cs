using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    [SerializeField]private Light FlashLight;

    [SerializeField]private float timeout = 10;
    [SerializeField]private float currTime;
    private float currReloadTime;

    private KeyCode control = KeyCode.F;


    private void Awake()
    {
          FlashLight = GetComponentInChildren<Light>();
    }

    private void ActiveFlashLight(bool val)
    {
        FlashLight.enabled = val;
    }

    void Update()
    {
        if(Input.GetKeyDown(control) && !FlashLight.enabled)
        {
            ActiveFlashLight(true);
        }
        else if(Input.GetKeyDown(control) && FlashLight.enabled)
        {
            ActiveFlashLight(false);
        }

        if(FlashLight.enabled)
        {
            currTime += Time.deltaTime;
            if(currTime > timeout)
            {
                currTime = 0;
                ActiveFlashLight(false);
            }
        }

    }
}
