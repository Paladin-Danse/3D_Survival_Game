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
        
    }

    private void OnEnable()
    {
        agent.enabled = true;
        stateMachine.ChangeState(stateMachine.idleState);
        health = data.maxHealth;
    }

    private void Start()
    {
        if (PlayerController.instance)
            playerController = PlayerController.instance;
        else if (GameObject.FindObjectOfType<PlayerController>())
            playerController = GameObject.FindObjectOfType<PlayerController>();
        else
            Debug.Log("Animal : PlayerController is Not Found!");

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

        //�״� �ִϸ��̼��� �ö����� ��ٸ���
        while (!animator.GetCurrentAnimatorStateInfo(0).IsTag("Death"))
        {
            yield return null;
        }
        //�״� �ִϸ��̼��� ������
        while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }
        //���� �� �Ҹ�ð����� ��ٷȴٰ�
        yield return new WaitForSeconds(data.deathToTime);
        //���ӿ��� ����
        gameObject.SetActive(false);
    }
}
