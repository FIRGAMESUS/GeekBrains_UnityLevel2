using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Bot : Unit
{
    private NavMeshAgent _agent;
    private Transform _playerT;
    private int stoppingDistance = 3;

    [SerializeField] private List<Vector3> targetPos;
    private int currentInt;

    protected override void Awake()
    {
        base.Awake();

        _agent = GetComponent<NavMeshAgent>();
        _playerT = GameObject.FindObjectOfType<SinglePlayer>().transform;
        Health = 100;
        Dead = false;

        _agent.stoppingDistance = 1f;
        currentInt = 0;
        Debug.Log(currentInt);
    }

    private void Update()
    {
        _agent.stoppingDistance = stoppingDistance;
        //_agent.SetDestination(_playerT.position);

        _agent.SetDestination(targetPos[currentInt]);

        if (_agent.remainingDistance > _agent.stoppingDistance)
        {
            GOAnimator.SetBool("move", true);
            
        }
        else
        {
            GOAnimator.SetBool("move", false);
            currentInt++;
            if (currentInt > targetPos.Count - 1) currentInt = 0;
            Debug.Log(currentInt);
        }

        if (Dead)
        {
            _agent.ResetPath();
            GORigidbody.isKinematic = true;
            GOAnimator.SetBool("die", true);
            Destroy(gameObject, 5f);
            return;
        }
    }
}
