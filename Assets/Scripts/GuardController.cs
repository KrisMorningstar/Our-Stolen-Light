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

    public bool lapPatrol;
    public bool canSeePlayer;
    public int rayLength = 10;
    // Start is called before the first frame update
    void Start()
    {
        canSeePlayer = false;
        agent = GetComponent<NavMeshAgent>();
        currentState = patrolState;
        StartCoroutine(ScanForPlayer());
    }

    // Update is called once per frame
    void Update()
    {
        currentState = currentState.StateTask(this);
        currentStateName = currentState.ToString();

    }

    IEnumerator ScanForPlayer()
    {
        while (!canSeePlayer)
        {
            Debug.DrawRay(transform.position, transform.forward * rayLength, Color.red);
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.forward, out hit, rayLength))
            {
                if (hit.collider.gameObject.layer == 7)
                {
                    Debug.Log("hit Player");
                    canSeePlayer = true;
                }
            }
            if (canSeePlayer)
            {
                Debug.Log("Chasing!");
                yield return chaseState;
            }

            yield return new WaitForSecondsRealtime(0.2f);
        }
        
    }
}
public class IdleState : IGuardState
{
    public IGuardState StateTask(GuardController _guard)
    {
        //ScanForPlayer(_guard);

        if (_guard.canSeePlayer)
        {
            Debug.Log("Chasing!");
            return _guard.chaseState;
        }
        else return _guard.idleState;
    }

    /*private void ScanForPlayer(GuardController _guard)
    {
        Debug.DrawRay(_guard.transform.position, _guard.transform.forward * _guard.rayLength, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(_guard.transform.position, _guard.transform.forward, out hit, _guard.rayLength))
        {
            if (hit.collider.gameObject.layer == 7)
            {
                Debug.Log("hit Player");
                _guard.canSeePlayer = true;
            }
        }
    }*/
}
public class PatrolState : IGuardState
{
    private int patrolPoint = 0;
    private bool scanning = false;
    private bool reversed = false;

    public IGuardState StateTask(GuardController _guard)
    {
        _guard.agent.SetDestination(_guard.patrolTargets[patrolPoint].transform.position);
        if (_guard.agent.remainingDistance < 0.001f)
        {
            Debug.Log(patrolPoint);
            Debug.Log("I'm here");
            if ((_guard.patrolTargets.Length-1) == patrolPoint)
            {
                Debug.Log("oobiiedoobie");
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
        return _guard.patrolState;
    }


    
    
}
public class ChaseState : IGuardState
{
    public IGuardState StateTask(GuardController _guard)
    {
        return _guard.chaseState;
    }
}
public class StunState : IGuardState
{
    public IGuardState StateTask(GuardController _guard)
    {
        return _guard.stunState;
    }
}


