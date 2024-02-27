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
        stateMachine.animal.animator.SetBool(animationHash, true);
    }

    protected void StopAnimation(int animationHash)
    {
        stateMachine.animal.animator.SetBool(animationHash, false);
    }
}
