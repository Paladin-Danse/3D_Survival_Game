using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalStateMachine : StateMachine
{
    public Animal animal;

    public IDamagable Target;

    public AnimalIdleState idleState;
    public AnimalAttackState attackState;
    public AnimalRunAwayState runAwayState;
    public AnimalWanderState wanderState;
    public AnimalFleeState fleeState;
    public AnimalStateMachine(Animal animal)
    {
        this.animal = animal;

        idleState = new AnimalIdleState(this);
        attackState = new AnimalAttackState(this);
        runAwayState = new AnimalRunAwayState(this);
        wanderState = new AnimalWanderState(this);
        fleeState = new AnimalFleeState(this);
    }
}
