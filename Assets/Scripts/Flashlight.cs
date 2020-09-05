using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flashlight : BaseObject
{
    private const int _maxCharge = 10;

    [SerializeField] private Light FlashLight;

    private KeyCode control = KeyCode.F;

    [SerializeField] private Slider slider;

    private float _charge;
    public float Charge 
    { 
        get => _charge; 
        set
        {
            _charge = value;
            slider.value = value;
        }
    }

    private bool _flashActive;
    public bool FlashActive 
    { 
        get => _flashActive; 
        set
        {
            _flashActive = value;
            FlashLight.enabled = value;
            StopAllCoroutines();
            StartCoroutine(Timer(value));
        }
    }


    protected override void Awake()
    {
        base.Awake();
        FlashLight = GetComponentInChildren<Light>();
        slider.minValue = 0;
        slider.maxValue = _maxCharge;
        Charge = _maxCharge;
        ActiveFlashLight(true);
    }

    private void ActiveFlashLight(bool val)
    {
        FlashActive = val;
    }
    void Update()
    {
        if (Input.GetKeyDown(control))
        {
            ActiveFlashLight(!FlashLight.enabled);
        }

    }

    private IEnumerator Timer(bool isDischarge)
    {
        if (isDischarge)
        {
            while (Charge > 0)
            {
                yield return new WaitForSeconds(1);
                Charge--;
                Debug.Log(Charge);
            }
            ActiveFlashLight(false);
        }
        else
        {
            while (Charge < _maxCharge)
            {
                yield return new WaitForSeconds(1);
                Charge++;
                Debug.Log(Charge);
            }
        }
    }
}
