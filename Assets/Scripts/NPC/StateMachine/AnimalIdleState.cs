using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalIdleState : AnimalBaseState
{
    public AnimalIdleState(AnimalStateMachine animalstateMachine) : base(animalstateMachine)
    {
    }

    public override void Enter()
    {
        Animal animal = stateMachine.animal;

        animal.SetAgentMoveSpeed(stateMachine.animal.data.walkSpeed, true);
        if (stateMachine.AnimationCoroutine != null)
        {
            animal.StopCoroutine(stateMachine.AnimationCoroutine);
            stateMachine.AnimationCoroutine = null;
        }
        stateMachine.AnimationCoroutine = animal.WanderToNewLocation();
        animal.StartCoroutine(stateMachine.AnimationCoroutine);
        base.Enter();
        StartAnimation(stateMachine.animal.animationData.IdleParameterHash);
    }
    public override void Update()
    {
        base.Update();
        PlayerSearch();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.animal.animationData.IdleParameterHash);
    }

    
}
