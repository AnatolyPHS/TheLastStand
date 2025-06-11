using GameSceneObjects;
using GameSceneObjects.StateBehaviour;
using GameSceneObjects.Units;
using UnityEngine;
using UnityEngine.AI;

public class MoveToTargetUnitState : BaseUnitState
{
    private const float TickPeriod = 1f;
    
    private IWithTarget unitWithTarget;
    private float attackRange = 0;
    private NavMeshAgent controllableAgent;
    
    private float nextTickTime = float.MinValue;
    
    public MoveToTargetUnitState(Unit unit, StationBehaviour enemyStationBehaviour) 
        : base(unit, enemyStationBehaviour)
    {
        unitWithTarget = unit as IWithTarget;
        attackRange = unit.GetAttackRange();
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
    
    private void ProcessTargetApproach()
    {
        if (unitWithTarget.HasTarget() == false || unitWithTarget.GetCurrentTarget().IsAlive() == false)
        {
            SwitchToNoTargetState();
            return;
        }

        IHittable target = unitWithTarget.GetCurrentTarget();
        Vector3 targetPosition = target.GetPosition();
        controllableAgent.SetDestination(targetPosition);

        if (Vector3.Distance(unitToControl.transform.position, targetPosition) <= attackRange)
        {
            SwitchToAttackState();
        }
    }

    protected virtual void SwitchToAttackState()
    {
        stateSwitcher.SwitchState<AttackTargetUnitState>();
    }
}
