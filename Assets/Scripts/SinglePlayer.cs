using UnityEngine;

public class SinglePlayer : Unit
{
    RaycastHit hit;
    Ray ray;
    Transform McamT;
    Transform target;
    private bool Grab;
    [SerializeField] LayerMask Layer;

    void Start()
    {
        Health = 100;
        Dead = false;
        McamT = Camera.main.transform;
    }


    void Update()
    {

        ray = new Ray(McamT.position, McamT.forward);

        if(Input.GetKeyDown(KeyCode.E))
        {
            Grab = !Grab;
        }

        if(Grab)
        {
            if (Physics.Raycast(ray, out hit, 50, Layer))
            {
                if (hit.collider.tag == "Pickup")
                {
                   hit.transform.GetComponent<Rigidbody>().isKinematic = true;
                   hit.transform.parent = McamT;
                }
            }
        }
        else
        {
            if(hit.transform)
            {
                hit.transform.parent = null;
                hit.transform.GetComponent<Rigidbody>().isKinematic = false;
                hit.transform.GetComponent<Rigidbody>().AddForce(Vector3.forward*0.5f, ForceMode.Impulse);
            }
        }
    }
}
