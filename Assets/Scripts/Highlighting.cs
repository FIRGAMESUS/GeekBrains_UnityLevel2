using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Highlighting : MonoBehaviour
{
    [SerializeField] private Material HighlightingMaterial;
    private Transform currentObject;
    private Material currentMaterial;

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (currentObject != null && currentMaterial != null)
            {
                currentObject.GetComponent<MeshRenderer>().material = currentMaterial;
            }
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider)
                {
                    currentObject = hit.collider.transform;
                    currentMaterial = currentObject.GetComponent<MeshRenderer>().material;
                    currentObject.GetComponent<MeshRenderer>().material = HighlightingMaterial;
                }
            }
        }
    }
}
