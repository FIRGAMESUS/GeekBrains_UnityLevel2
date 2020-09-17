using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR

public class PathWaypoint : MonoBehaviour
{
    public List<Transform> nodes;
    public Vector3 currNode;
    public Vector3 prevNode;

    public int nodeCounter;

    //Saving path

    private void OnDrawGizmos()
    {
        if(transform.childCount != nodeCounter)
        {
            nodes.Clear();
            nodeCounter = 0;
        }

        if(transform.childCount > 0)
        {
            foreach (Transform item in transform)
            {
                if(!nodes.Contains(item))
                {
                    nodes.Add(item);
                }
                nodeCounter++;
            }
        }

        if(nodes.Count > 1)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                currNode = nodes[i].position;

                if(i > 0)
                {
                    prevNode = nodes[i - 1].position;
                }
                else if(i == 0 && nodes.Count > 1)
                {
                    prevNode = nodes[nodes.Count - 1].position;
                }


                Gizmos.color = Color.red;
                Gizmos.DrawLine(prevNode, currNode);

                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(currNode, 1);
            }
        }


    }
}

#endif
