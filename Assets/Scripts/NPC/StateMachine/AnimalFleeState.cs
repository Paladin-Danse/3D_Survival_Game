using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class AnimalFleeState : AnimalBaseState
{
    public AnimalFleeState(AnimalStateMachine animalstateMachine) : base(animalstateMachine)
    {
    }
    public override void Enter()
    {
        stateMachine.animal.SetAgentMoveSpeed(stateMachine.animal.data.runSpeed, false);
        base.Enter();

    }
    public override void Update()
    {
        base.Update();
        FleeingUpdate();
    }
    public void FleeingUpdate()
    {
        if (stateMachine.animal.agent.remainingDistance < 0.1f)
        {
            stateMachine.animal.agent.SetDestination(GetFleeLocation());
        }
        else
        {
            stateMachine.ChangeState(stateMachine.wanderState);
            //SetState(AIState.Wandering);
        }
    }
    Vector3 GetFleeLocation()
    {
        NavMeshHit hit;
        Animal animal = stateMachine.animal;

        NavMesh.SamplePosition(animal.transform.position + (UnityEngine.Random.onUnitSphere * animal.data.safeDistance), out hit, animal.data.maxWanderDistance, NavMesh.AllAreas);

        int i = 0;
        while (GetDestinationAngle(hit.position) > 90 || animal.playerDistance < animal.data.safeDistance)
        {

            NavMesh.SamplePosition(animal.transform.position + (UnityEngine.Random.onUnitSphere * animal.data.safeDistance), out hit, animal.data.maxWanderDistance, NavMesh.AllAreas);
            i++;
            if (i == 30)
                break;
        }

        return hit.position;
    }
    float GetDestinationAngle(Vector3 targetPos)
    {
        Animal animal = stateMachine.animal;
        return Vector3.Angle(animal.transform.position - animal.playerPos, animal.transform.position + targetPos);
    }
}
