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

    }
    public override void Update()
    {
        base.Update();
        PassiveUpdate();
    }

    public void PassiveUpdate()
    {
        if (stateMachine.animal.agent.remainingDistance < 0.1f)
        {
            stateMachine.ChangeState(stateMachine.idleState);
            //SetState(AIState.Idle);
            stateMachine.animal.StartCoroutine("WanderToNewLocation");
        }

        if (stateMachine.animal.playerDistance < stateMachine.animal.data.detectDistance)
        {
            //stateMachine.ChangeState(stateMachine.attackState);
            //SetState(AIState.Attacking);
        }
    }
}
