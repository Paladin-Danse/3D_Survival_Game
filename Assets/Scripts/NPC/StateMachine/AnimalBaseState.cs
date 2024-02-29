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

        while (!stateMachine.animal.animator.GetCurrentAnimatorStateInfo(0).IsTag("Run"))
        {
            yield return null;
        }

        stateMachine.animal.agent.isStopped = false;
        stateMachine.ChangeState(stateMachine.attackState);
    }
    //protected float GetNormalizedTime(Animator animator, string tag)
    //{
    //    AnimatorStateInfo currentInfo = animator.GetCurrentAnimatorStateInfo(0);
    //    AnimatorStateInfo nextInfo = animator.GetNextAnimatorStateInfo(0);

    //    if (animator.IsInTransition(0) && nextInfo.IsTag(tag))
    //    {
    //        return nextInfo.normalizedTime;
    //    }
    //    else if (!animator.IsInTransition(0) && currentInfo.IsTag(tag))
    //    {
    //        return currentInfo.normalizedTime;
    //    }
    //    else
    //    {
    //        return 0f;
    //    }
    //}
}
