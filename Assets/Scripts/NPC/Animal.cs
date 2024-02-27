using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIState
{
    Idle,
    Wandering,
    Attacking,
    RunAway,
    Fleeing
}

public class Animal : MonoBehaviour, IDamagable
{
    [field: Header("References")]
    [field: SerializeField] public AnimalData data { get; private set; }

    [field: Header("Animations")]
    [field: SerializeField] public AnimalAnimationData animationData { get; private set; }

    public Rigidbody rigidbody { get; private set; }
    public Animator animator { get; private set; }

    private AnimalStateMachine stateMachine;
    public NavMeshAgent agent;
    private SkinnedMeshRenderer[] meshRenderers;

    protected AIState aiState;
    private float lastAttackTime;
    public float playerDistance { get; private set; }
    private Vector3 playerPos;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        stateMachine = new AnimalStateMachine(this);
    }

    private void Start()
    {
        playerPos = PlayerController.instance ? PlayerController.instance.transform.position : new Vector3(0, 0, 0);
        stateMachine.ChangeState(stateMachine.idleState);
    }

    private void Update()
    {
        playerDistance = Vector3.Distance(transform.position, playerPos);

        //stateMachine.HandleInput();
        stateMachine.Update();
    }
    /*
    protected void SetState(AIState newState)
    {
        aiState = newState;
        switch (aiState)
        {
            case AIState.Idle:
                {
                    agent.speed = data.walkSpeed;
                    agent.isStopped = true;
                }
                break;
            case AIState.Wandering:
                {
                    agent.speed = data.walkSpeed;
                    agent.isStopped = false;
                }
                break;

            case AIState.Attacking:
                {
                    agent.speed = data.runSpeed;
                    agent.isStopped = false;
                }
                break;
            case AIState.RunAway:
                {
                    agent.speed = data.runSpeed;
                    agent.isStopped = false;
                }
                break;
            case AIState.Fleeing:
                {
                    agent.speed = data.runSpeed;
                    agent.isStopped = false;
                }
                break;
        }

        animator.speed = agent.speed / data.walkSpeed;
    }
    *///SetState();
    public void FleeingUpdate()
    {
        if (agent.remainingDistance < 0.1f)
        {
            agent.SetDestination(GetFleeLocation());
        }
        else
        {
            stateMachine.ChangeState(stateMachine.wanderState);
            //SetState(AIState.Wandering);
        }
    }

    public void AttackingUpdate()
    {
        if (playerDistance > data.attackDistance || !IsPlaterInFireldOfView())
        {
            agent.isStopped = false;
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(playerPos, path))
            {
                agent.SetDestination(playerPos);
            }
            else
            {
                stateMachine.ChangeState(stateMachine.wanderState);
                //SetState(AIState.Fleeing);
            }
        }
        else
        {
            agent.isStopped = true;
            if (Time.time - lastAttackTime > data.attackRate)
            {
                lastAttackTime = Time.time;
                if (PlayerController.instance) PlayerController.instance.GetComponent<IDamagable>().TakePhysicalDamage(data.damage);
                animator.speed = 1;
                animator.SetTrigger("Attack");
            }
        }
    }

    //public void PassiveUpdate()
    //{
    //    if (agent.remainingDistance < 0.1f)
    //    {
    //        stateMachine.ChangeState(stateMachine.idleState);
    //        //SetState(AIState.Idle);
    //        Invoke("WanderToNewLocation", Random.Range(data.minWanderWaitTime, data.maxWanderWaitTime));
    //    }

    //    if (playerDistance < data.detectDistance)
    //    {
    //        stateMachine.ChangeState(stateMachine.attackState);
    //        //SetState(AIState.Attacking);
    //    }
    //}

    bool IsPlaterInFireldOfView()
    {
        Vector3 directionToPlayer = playerPos - transform.position;
        float angle = Vector3.Angle(transform.forward, directionToPlayer);
        return angle < data.fieldOfView * 0.5f;
    }

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(data.minWanderDistance, data.maxWanderDistance)), out hit, data.maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < data.detectDistance)
        {
            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * Random.Range(data.minWanderDistance, data.maxWanderDistance)), out hit, data.maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    Vector3 GetFleeLocation()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * data.safeDistance), out hit, data.maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (GetDestinationAngle(hit.position) > 90 || playerDistance < data.safeDistance)
        {

            NavMesh.SamplePosition(transform.position + (Random.onUnitSphere * data.safeDistance), out hit, data.maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }

    float GetDestinationAngle(Vector3 targetPos)
    {
        return Vector3.Angle(transform.position - playerPos, transform.position + targetPos);
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        data.maxHealth -= damageAmount;
        if (data.maxHealth <= 0)
            Die();

        StartCoroutine(DamageFlash());
    }

    void Die()
    {
        for (int x = 0; x < data.dropOnDeath.Length; x++)
        {
            Instantiate(data.dropOnDeath[x].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }

        Destroy(gameObject);
    }

    IEnumerator DamageFlash()
    {
        for (int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.1f);
        for (int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = Color.white;
    }

    public void SetAgentMoveSpeed(float newSpeed, bool isStop)
    {
        agent.speed = newSpeed;
        agent.isStopped = isStop;
    }

    public IEnumerator WanderToNewLocation()
    {
        yield return new WaitForSeconds(Random.Range(data.minWanderWaitTime, data.maxWanderWaitTime));
        stateMachine.ChangeState(stateMachine.wanderState);
        agent.SetDestination(GetWanderLocation());
    }
}
