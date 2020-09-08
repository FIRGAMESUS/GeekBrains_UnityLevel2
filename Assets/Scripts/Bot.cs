using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : Unit
{
    private void Start()
    {
        Health = 100;
        Dead = false;
    }
    private void Update()
    {
        if (Dead)
        {
            GORigidbody.isKinematic = true;
            //Animation Die
            Destroy(gameObject, 5f);
            return;
        }
    }
}
