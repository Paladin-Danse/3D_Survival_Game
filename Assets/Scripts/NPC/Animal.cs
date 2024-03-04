using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Properties;
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
[Serializable]
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
    private PlayerController playerController;

    protected AIState aiState;
    public float lastAttackTime { get; set; }
    public float playerDistance { get; private set; }
    [field: SerializeField] public Vector3 playerPos { get; private set; }
    private int health;
    protected event Action OnDie;
    public bool IsDead => health == 0;
    private void Awake()
    {
        animationData.Initialize();

        agent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        meshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        stateMachine = new AnimalStateMachine(this);
        playerController = PlayerController.instance;
    }

    private void OnEnable()
    {
        agent.enabled = true;
        stateMachine.ChangeState(stateMachine.idleState);
        health = data.maxHealth;
    }

    private void Start()
    {
        stateMachine.ChangeState(stateMachine.idleState);
        health = data.maxHealth;

        OnDie += Die;
    }

    private void Update()
    {
        playerPos = playerController ? playerController.transform.position : new Vector3(0, 0, 0);

        playerDistance = Vector3.Distance(transform.position, playerPos);

        if (!IsDead)
        {
            //stateMachine.HandleInput();
            stateMachine.Update();
        }
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

    Vector3 GetWanderLocation()
    {
        NavMeshHit hit;

        NavMesh.SamplePosition(transform.position + (UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(data.minWanderDistance, data.maxWanderDistance)), out hit, data.maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (Vector3.Distance(transform.position, hit.position) < data.detectDistance)
        {
            NavMesh.SamplePosition(transform.position + (UnityEngine.Random.onUnitSphere * UnityEngine.Random.Range(data.minWanderDistance, data.maxWanderDistance)), out hit, data.maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }
        
        return hit.position;
    }

    public void TakePhysicalDamage(int damageAmount)
    {
        if (health == 0) return;
        health = Mathf.Max(health -= damageAmount, 0);

        if (health == 0) OnDie?.Invoke();
        else animator.SetTrigger("OnHit");

        StartCoroutine(DamageFlash());
    }

    void Die()
    {
        if (!IsDead) return;

        agent.isStopped = true;
        agent.enabled = false;

        for (int x = 0; x < data.dropOnDeath.Length; x++)
        {
            Instantiate(data.dropOnDeath[x].dropPrefab, transform.position + Vector3.up * 2, Quaternion.identity);
        }

        if (stateMachine.AnimationCoroutine != DeathAnimation())
        {
            StopCoroutine(stateMachine.AnimationCoroutine);
            stateMachine.AnimationCoroutine = DeathAnimation();
            StartCoroutine(stateMachine.AnimationCoroutine);
        }
    }

    public void SetAgentMoveSpeed(float newSpeed, bool isStop)
    {
        agent.speed = newSpeed;
        agent.isStopped = isStop;
    }

    public IEnumerator WanderToNewLocation()
    {
        yield return new WaitForSeconds(UnityEngine.Random.Range(data.minWanderWaitTime, data.maxWanderWaitTime));
        stateMachine.ChangeState(stateMachine.wanderState);
        agent.SetDestination(GetWanderLocation());
        stateMachine.AnimationCoroutine = null;
    }
    IEnumerator DamageFlash()
    {
        for (int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = new Color(1.0f, 0.6f, 0.6f);

        yield return new WaitForSeconds(0.1f);
        for (int x = 0; x < meshRenderers.Length; x++)
            meshRenderers[x].material.color = Color.white;
    }

    IEnumerator DeathAnimation()
    {
        animator.SetTrigger("Death");

        //죽는 애니메이션이 올때까지 기다리고
        while (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Death"))
        {
            yield return null;
        }
        //죽는 애니메이션이 끝나면
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        //죽은 뒤 소멸시간까지 기다렸다가
        yield return new WaitForSeconds(data.deathToTime);
        //게임에서 제거
        gameObject.SetActive(false);
    }
}
