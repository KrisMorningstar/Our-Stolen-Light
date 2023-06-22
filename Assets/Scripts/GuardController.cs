using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardController : MonoBehaviour
{
    public string currentStateName;
    private IGuardState currentState;

    public IdleState idleState = new IdleState();
    public PatrolState patrolState = new PatrolState();
    public ChaseState chaseState = new ChaseState();
    public StunState stunState = new StunState();

    public NavMeshAgent agent;
    public GameObject[] patrolTargets;
    public GameObject playerTarget;

    public bool lapPatrol;
    public bool canSeePlayer;
    public bool isStunned;
    public int rayLength = 10;

    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject player;
    public GameObject heirloom;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    private float delay = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        canSeePlayer = false;
        isStunned = false;
        agent = GetComponent<NavMeshAgent>();
        currentState = patrolState;

        player = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(LoSCoroutine(delay));
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.StateTask(this);
        currentStateName = currentState.ToString();
    }

    private IEnumerator LoSCoroutine(float _delay)
    {
        WaitForSeconds wait = new WaitForSeconds(_delay);

        while (currentState != stunState)
        {
            yield return wait;
            LineOfSightChecker();
        }
    }

    private void LineOfSightChecker()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    canSeePlayer = true;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }
}
public class IdleState : IGuardState
{
    public IGuardState StateTask(GuardController _guard)
    {
        if (_guard.canSeePlayer)
        {
            Debug.Log("Chasing!");
            return _guard.chaseState;
        }
        else if (_guard.isStunned)
        {
            Debug.Log("Stunned!");
            return _guard.stunState;
        }
        else if (!_guard.heirloom.active)
        {
            Debug.Log("Taken!");
            return _guard.chaseState;
        }
        else return _guard.idleState;
    }
}
public class PatrolState : IGuardState
{
    private int patrolPoint = 0;
    private bool reversed = false;

    public IGuardState StateTask(GuardController _guard)
    {
        _guard.agent.SetDestination(_guard.patrolTargets[patrolPoint].transform.position);
        if (_guard.agent.remainingDistance < 0.001f)
        {
            if ((_guard.patrolTargets.Length-1) == patrolPoint)
            {
                switch (_guard.lapPatrol)
                {
                    case true:
                        patrolPoint = 0;
                        break;
                    case false:
                        reversed = true;
                        patrolPoint--;
                        break;
                }
                return _guard.patrolState;
            }
            else if(patrolPoint == 0)
            {
                reversed = false;
            }
            switch (reversed)
            {
                case true:
                    patrolPoint--;
                    break;
                case false:
                    patrolPoint++;
                    break;
            }
        }
        if (_guard.canSeePlayer)
        {
            Debug.Log("Chasing!");
            return _guard.chaseState;
        }
        else if (_guard.isStunned)
        {
            Debug.Log("Stunned!");
            return _guard.stunState;
        }
        else if (!_guard.heirloom.active)
        {
            Debug.Log("Taken!");
            return _guard.chaseState;
        }
        else return _guard.patrolState;
    }


    
    
}
public class ChaseState : IGuardState
{
    public IGuardState StateTask(GuardController _guard)
    {
        _guard.agent.SetDestination(_guard.player.transform.position);


        if (_guard.isStunned)
        {
            Debug.Log("Stunned!");
            return _guard.stunState;
        }
        else return _guard.chaseState;
    }
}
public class StunState : IGuardState
{
    public IGuardState StateTask(GuardController _guard)
    {
        _guard.gameObject.SetActive(false);
        return _guard.stunState;
    }
}


