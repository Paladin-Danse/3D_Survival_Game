using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AnimalBaseState : IState
{
    protected AnimalStateMachine stateMachine;
    public AnimalBaseState(AnimalStateMachine animalstateMachine)
    {
        stateMachine = animalstateMachine;
    }
    public bool IsAlertEnd = false;

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {
        
    }

    public virtual void HandleInput()
    {
        
    }

    public virtual void PhysicsUpdate()
    {
        
    }

    public virtual void Update()
    {
        
    }
    protected void StartAnimation(int animationHash)
    {
        stateMachine.animal.animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.animal.animator.SetBool(animationHash, false);
    }

    public IEnumerator BarkAnimation()
    {
        stateMachine.animal.agent.isStopped = true;
        stateMachine.animal.animator.SetTrigger(stateMachine.animal.animationData.AlertParameterHash);

        while (!stateMachine.animal.animator.GetCurrentAnimatorStateInfo(0).IsTag("Run"))
        {
            yield return null;
        }

        stateMachine.animal.agent.isStopped = false;
        stateMachine.ChangeState(stateMachine.attackState);
    }

    public void PlayerSearch()
    {
        if(IsPlaterInFireldOfView() && stateMachine.animal.playerDistance < stateMachine.animal.data.detectDistance)
        {
            if (stateMachine.animal.data.isHostile)
            {
                stateMachine.animal.animator.SetTrigger(stateMachine.animal.animationData.AlertParameterHash);

                if (stateMachine.AnimationCoroutine == null || stateMachine.AnimationCoroutine != BarkAnimation())
                {
                    if (stateMachine.AnimationCoroutine != null) stateMachine.animal.StopCoroutine(stateMachine.AnimationCoroutine);
                    stateMachine.AnimationCoroutine = BarkAnimation();
                    stateMachine.animal.StartCoroutine(stateMachine.AnimationCoroutine);
                }
            }
            else
            {
                stateMachine.ChangeState(stateMachine.runAwayState);
            }
        }
    }
    protected bool IsPlaterInFireldOfView()
    {
        Animal animal = stateMachine.animal;

        Vector3 directionToPlayer = animal.playerPos - animal.transform.position;
        float angle = Vector3.Angle(animal.transform.forward, directionToPlayer);
        return angle < animal.data.fieldOfView * 0.5f;
    }
}
