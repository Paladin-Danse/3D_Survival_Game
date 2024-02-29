using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AnimalAttackState : AnimalBaseState
{
    public AnimalAttackState(AnimalStateMachine animalstateMachine) : base(animalstateMachine)
    {
    }

    public override void Enter()
    {
        stateMachine.animal.SetAgentMoveSpeed(stateMachine.animal.data.runSpeed, false);
        base.Enter();
        StartAnimation(stateMachine.animal.animationData.RunParameterHash);

    }
    public override void Update()
    {
        base.Update();
        AttackingUpdate();
        
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.animal.animationData.RunParameterHash);
    }

    public void AttackingUpdate()
    {
        Animal animal = stateMachine.animal;
        NavMeshAgent agent = animal.agent;

        if (animal.playerDistance > animal.data.attackDistance || !IsPlaterInFireldOfView())
        {
            agent.isStopped = false;
            NavMeshPath path = new NavMeshPath();
            if (agent.CalculatePath(animal.playerPos, path))
            {
                agent.SetDestination(animal.playerPos);
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
            if (Time.time - animal.lastAttackTime > animal.data.attackRate)
            {
                animal.lastAttackTime = Time.time;
                if (PlayerController.instance) PlayerController.instance.GetComponent<IDamagable>().TakePhysicalDamage(animal.data.damage);
                animal.animator.speed = 1;
                animal.animator.SetTrigger(animal.animationData.AttackParameterHash);
            }
        }
    }
    bool IsPlaterInFireldOfView()
    {
        Animal animal = stateMachine.animal;

        Vector3 directionToPlayer = animal.playerPos - animal.transform.position;
        float angle = Vector3.Angle(animal.transform.forward, directionToPlayer);
        return angle < animal.data.fieldOfView * 0.5f;
    }
}
