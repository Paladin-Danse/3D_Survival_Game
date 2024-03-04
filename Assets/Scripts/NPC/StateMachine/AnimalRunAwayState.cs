using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalRunAwayState : AnimalBaseState
{
    Animal animal;
    public AnimalRunAwayState(AnimalStateMachine animalstateMachine) : base(animalstateMachine)
    {
    }

    public override void Enter()
    {
        animal = stateMachine.animal;
        animal.SetAgentMoveSpeed(animal.data.runSpeed, false);
        base.Enter();
        StartAnimation(animal.animationData.RunParameterHash);
    }
    public override void Update()
    {
        base.Update();
        RunAwayUpdate();
    }
    public override void Exit() 
    {
        base.Exit();
        StopAnimation(animal.animationData.RunParameterHash);
    }

    public void RunAwayUpdate()
    {
        if(animal.playerDistance < animal.data.safeDistance && stateMachine.AnimationCoroutine == null)
        {
            stateMachine.AnimationCoroutine = RunAway();
            animal.StartCoroutine(stateMachine.AnimationCoroutine);
        }
        else if(animal.playerDistance > animal.data.safeDistance && stateMachine.AnimationCoroutine == null)
        {
            stateMachine.ChangeState(stateMachine.idleState);
        }
    }

    private IEnumerator RunAway()
    {
        Vector3 playerReverseDirection = animal.transform.position - animal.playerPos;
        animal.agent.SetDestination(playerReverseDirection * animal.data.safeDistance);
        yield return new WaitUntil(() => animal.agent.remainingDistance < 0.1f);
        stateMachine.AnimationCoroutine = null;
    }
}
