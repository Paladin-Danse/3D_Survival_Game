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
        stateMachine.animal.SetAgentMoveSpeed(stateMachine.animal.data.walkSpeed, true);
        base.Enter();
        StartAnimation(stateMachine.animal.animationData.IdleParameterHash);
    }
    public override void Update()
    {
        base.Update();
        PassiveUpdate();
    }

    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.animal.animationData.IdleParameterHash);
    }

    
}
