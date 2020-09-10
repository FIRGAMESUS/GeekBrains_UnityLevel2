using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePlayer : Unit
{
    RaycastHit hit;
    Ray ray;
    Transform McamT;
    Transform target;
    bool grab;

    void Start()
    {
        Health = 100;
        Dead = false;
        McamT = Camera.main.transform;
    }

    void Update()
    {
        ray = new Ray(McamT.position, McamT.forward);
        if (Input.GetKeyDown(KeyCode.E))
        {
            grab = !grab;
        }
        if (grab)
        {
            if (Physics.Raycast(ray, out hit, 50))
            {
                if (hit.collider.tag == "PickUp")
                {
                    hit.transform.GetComponent<Rigidbody>().isKinematic = true;
                    target = hit.transform.parent;
                    target.parent = McamT;
                }
            }
        }
        else
        {
            target.parent = null;
            hit.transform.GetComponent<Rigidbody>().isKinematic = false;
        }

    }
}
