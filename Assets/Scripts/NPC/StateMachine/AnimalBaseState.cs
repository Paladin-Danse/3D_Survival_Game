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
        Debug.Log(animationHash);
        stateMachine.animal.animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        Debug.Log(animationHash);
        stateMachine.animal.animator.SetBool(animationHash, false);
    }

    public void PassiveUpdate()
    {
        if (stateMachine.animal.agent.remainingDistance < 0.1f)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            if (stateMachine.AnimationCoroutine == null)
            {
                stateMachine.AnimationCoroutine = stateMachine.animal.WanderToNewLocation();
                stateMachine.animal.StartCoroutine(stateMachine.AnimationCoroutine);
            }
        }
        
        if (stateMachine.animal.playerDistance < stateMachine.animal.data.detectDistance)
        {
            if (stateMachine.animal.data.isHostile)
            {
                stateMachine.animal.animator.SetTrigger(stateMachine.animal.animationData.AlertParameterHash);

                if (stateMachine.AnimationCoroutine == null || stateMachine.AnimationCoroutine != BarkAnimation())
                {
                    if(stateMachine.AnimationCoroutine != null) stateMachine.animal.StopCoroutine(stateMachine.AnimationCoroutine);
                    stateMachine.AnimationCoroutine = BarkAnimation();
                    stateMachine.animal.StartCoroutine(stateMachine.AnimationCoroutine);
                }
                
            }
        }
    }

    public IEnumerator BarkAnimation()
    {
        stateMachine.animal.agent.isStopped = true;
        stateMachine.animal.animator.SetTrigger(stateMachine.animal.animationData.AlertParameterHash);

        AnimatorStateInfo info = stateMachine.animal.animator.GetCurrentAnimatorStateInfo(0);
        while ((info.normalizedTime < 1.0f && info.normalizedTime > 0.0f) || info.IsName("Alert"))
        {
            yield return null;
        }
        stateMachine.animal.agent.isStopped = false;
        stateMachine.ChangeState(stateMachine.attackState);
    }
}
