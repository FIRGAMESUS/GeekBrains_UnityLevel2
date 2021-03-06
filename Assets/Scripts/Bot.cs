﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Bot : Unit
{

    private NavMeshAgent _agent;
    private Transform _playerT;
    private Transform target;

    public List<Vector3> _wayPoints = new List<Vector3>();
    private int pointCounter;
   // [SerializeField] private GameObject wayPointMain; //change it to load from file

    [SerializeField] private float DelayFindTargets = 0.1f;

    [Header("Дистанции остановки: ")]
    [SerializeField] private float _seekDistance = 3f;
    [SerializeField] private float _stopDistance = 0.4f;
    [SerializeField] private float _attackDistance = 7f;


    private float timeWait = 3f;
    private float timeOut = 0;


    //Shooting
    [SerializeField] private int bulletCount = 30;
    private int currentBullCount = 0;
    [SerializeField] private float shootDistance = 1000f;
    [SerializeField] private int damage = 20;
    [Header("Настройки для оружия: ")]
    [Tooltip("Объект добавляется автоматически, должен находиться на дуле оружия")][SerializeField] protected Transform gunT;
    [Tooltip("Объект добавляется автоматически")] [SerializeField] protected ParticleSystem muzzleFlash;
    [Tooltip("Объект добавляется автоматически")] [SerializeField] protected GameObject hitParticle;


    [Header("Состояния бота: ")]
    [SerializeField] private bool patrol;
    [SerializeField] private bool shooting;

    //Target
    [Header("Списки целей: ")]
    [SerializeField] private Collider[] targetInViewRadius;
    [SerializeField] private List<Transform> visibleTargets = new List<Transform>();

    [Header("Настройки зоны видимости: ")]
    [SerializeField] [Range(30, 90)]private float maxAngle = 30;//60
    [SerializeField] [Range(10, 40)] private float maxRadius = 10;

    [SerializeField] private LayerMask targetMask;
    [SerializeField] private LayerMask obstacleMask;
    [TextArea(3,5)][SerializeField] private string Test;

    public List<Vector3> WayPoints { get => _wayPoints; set => _wayPoints = value; }

#if UNITY_EDITOR

    [ContextMenu("Tools/Значения по умолчанию")]
    public void Default()
    {
        bulletCount = 30;
        shootDistance = 1000f;
        damage = 20;
        maxAngle = 30;
        maxRadius = 20;
        patrol = true;
    }

    [ContextMenu("Tools/Рандомные значения зоны видимости")]
    public void Randomization()
    {
        maxRadius = Random.Range(30, 90);
        maxAngle = Random.Range(10, 40);
    }

    private void OnDrawGizmos()
    {
        Vector3 pos = transform.position + Vector3.up;
        Handles.color = new Color(1, 0, 1, 0.1f);

        Handles.DrawSolidArc(pos, transform.up, transform.forward, maxAngle, maxRadius);
        Handles.DrawSolidArc(pos, transform.up, transform.forward, -maxAngle, maxRadius);
    }


#endif

    IEnumerator Shoot(RaycastHit playerHit)
    {
        yield return new WaitForSeconds(0.5f);
        muzzleFlash.Play();
        playerHit.collider.GetComponent<ISetDamage>().SetDamage(damage);
        GameObject temp = Instantiate(hitParticle, playerHit.point, Quaternion.identity);
        temp.transform.parent = playerHit.transform;
        Destroy(temp, 0.8f);
        shooting = false;
    }

    IEnumerator FindTargets(float delay)
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            FindVisibleTargets();
        }
    }


    protected override void Awake()
    {
        base.Awake();
        _agent = GetComponent<NavMeshAgent>();

        _playerT = GameObject.FindObjectOfType<SinglePlayer>().transform;

        _agent.stoppingDistance = _stopDistance;
        _agent.updatePosition = true;
        _agent.updateRotation = true;
        patrol = true;

        Health = 100;
        Dead = false;

        //if(wayPointMain)
        //{
        //    foreach (Transform item in wayPointMain.transform)
        //    {
        //        WayPoints.Add(item.position);
        //    }
        //}


        StartCoroutine(FindTargets(DelayFindTargets));

        gunT = GameObject.FindGameObjectWithTag("GunT").transform;
        muzzleFlash = GetComponentInChildren<ParticleSystem>();
        hitParticle = Resources.Load<GameObject>("Prefabs/Flare");
    }

    private void FindVisibleTargets()
    {
        targetInViewRadius = Physics.OverlapSphere(transform.position + Vector3.up, maxRadius, targetMask);
        for (int i = 0; i < targetInViewRadius.Length; i++)
        {
            Transform tempTarget = targetInViewRadius[i].transform;

            Vector3 dirToTarget = (tempTarget.position - transform.position).normalized;
            float targetAngle = Vector3.Angle(transform.forward, dirToTarget);

            if ((-maxAngle)< targetAngle && targetAngle < maxAngle)
            {
                if(!Physics.Raycast(transform.position + Vector3.up, dirToTarget, obstacleMask))
                {
                    if(!visibleTargets.Contains(tempTarget))
                    {
                        visibleTargets.Add(tempTarget);
                    }
                }
            }
        }
    }


    void Update()
    {
       

        if (visibleTargets.Count > 0)
        {
            patrol = false;
            target = visibleTargets[0];
            float DistToTarget = Vector3.Distance(transform.position, target.position);


            if (DistToTarget > maxRadius)
            {
                visibleTargets.Clear();
            }
        }
        else
        {
            patrol = true;
        }


        if(patrol)
        {
            if(WayPoints.Count > 1)
            {
                _agent.stoppingDistance = _stopDistance;
                _agent.SetDestination(WayPoints[pointCounter]);

                if(!_agent.hasPath)
                {
                    timeOut += 0.1f;
                    if(timeOut>timeWait)
                    {
                        timeOut = 0;
                        if(pointCounter < WayPoints.Count-1)
                        {
                            pointCounter++;
                        }
                        else
                        {
                            pointCounter = 0;
                        }
                    }
                }
            }
            else
            {
                _agent.stoppingDistance = _seekDistance;
                _agent.SetDestination(_playerT.position);
            }
        }
        else
        {
            _agent.stoppingDistance = _attackDistance;
            _agent.SetDestination(target.position);

            if (!Dead)
            {
                transform.LookAt(new Vector3(target.position.x, 0, target.position.z));

                RaycastHit hit;

                Ray ray = new Ray(transform.position + Vector3.up, transform.forward);

                if (Physics.Raycast(ray, out hit, 500f, targetMask))
                {
                    if (!shooting)
                    {
                        _agent.ResetPath();
                        GOAnimator.SetBool("shoot", true);
                        shooting = true;
                        StartCoroutine(Shoot(hit));
                    }
                    else
                    {
                        GOAnimator.SetBool("shoot", false);
                    }
                }
                else
                {
                    _agent.stoppingDistance = _seekDistance;
                    _agent.SetDestination(target.position);
                }
            }
        }


        if(_agent.remainingDistance > _agent.stoppingDistance)
        {
            GOAnimator.SetBool("move", true);
        }
        else
        {
            GOAnimator.SetBool("move", false);
        }

        if(Dead)
        {
            _agent.ResetPath();
            GORigibody.isKinematic = true;
            GOAnimator.SetBool("die", true);
            Destroy(gameObject, 5f);
            return;
        }
    }
}
