using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalWanderState : AnimalBaseState
{
    public AnimalWanderState(AnimalStateMachine animalstateMachine) : base(animalstateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.animal.SetAgentMoveSpeed(stateMachine.animal.data.walkSpeed, false);
        base.Enter();
        StartAnimation(stateMachine.animal.animationData.WalkParameterHash);

    }
    public override void Update()
    {
        base.Update();
        PassiveUpdate();
    }
    public override void Exit()
    {
        base.Exit();
        StopAnimation(stateMachine.animal.animationData.WalkParameterHash);
    }
}
