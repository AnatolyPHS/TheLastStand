using GameSceneObjects;
using GameSceneObjects.StateBehaviour;
using GameSceneObjects.Units;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTargetUnitState : BaseUnitState
{
    private const float TickPeriod = 1f;

    protected IWithTarget unitWithTarget;
    protected NavMeshAgent controllableAgent;
    
    private float nextTickTime = float.MinValue;
    
    public MoveToTargetUnitState(GameUnit unit, StationBehaviour enemyStationBehaviour) 
        : base(unit, enemyStationBehaviour)
    {
        unitWithTarget = unit as IWithTarget;
        controllableAgent = unit.GetNavMeshAgent();
    }

    public override void OnStateEnter()
    {
        ProcessTargetApproach();
        nextTickTime = Time.time + TickPeriod;
    }

    public override void OnStateExit()
    {
        nextTickTime = float.MaxValue;
        controllableAgent.ResetPath();
    }

    public override void OnStateUpdate(float deltaTime)
    {
        if (Time.time < nextTickTime)
        {
            return;
        }
        
        nextTickTime = Time.time + TickPeriod;
        ProcessTargetApproach();
    }

    protected virtual void SwitchToNoTargetState()
    {
        stateSwitcher.SwitchState<SearchForTargetUnitState>();
    }
    
    protected virtual void ProcessTargetApproach()
    {
        if (unitWithTarget.HasTarget() == false || unitWithTarget.GetCurrentTarget().CanBeAttacked() == false)
        {
            SwitchToNoTargetState();
            return;
        }

        IHittable target = unitWithTarget.GetCurrentTarget();
        Vector3 targetPosition = target.GetPosition();
        controllableAgent.SetDestination(targetPosition);

        if (Vector3.Distance(unitToControl.transform.position, targetPosition) <= unitToControl.GetAttackRange())
        {
            SwitchToAttackState();
        }
    }

    protected virtual void SwitchToAttackState()
    {
        stateSwitcher.SwitchState<AttackTargetUnitState>();
    }
}
