using FluidHTN;
using System.Collections;
using System.Collections.Generic;
using FluidHTN.Factory;
using FluidHTN.PrimitiveTasks;
using UnityEngine;

public class AIDomainBuilder : BaseDomainBuilder<AIDomainBuilder, AIContext>
{
    public AIDomainBuilder(string domainName) : base(domainName, new DefaultFactory())
    {

    }

    public AIDomainBuilder HasState(AIWorldState state)
    {
        var condition = new HasWorldStateCondition(state);
        Pointer.AddCondition(condition);
        return this;
    }

    public AIDomainBuilder HasState(AIWorldState state, byte value)
    {
        var condition = new HasWorldStateCondition(state, value);
        Pointer.AddCondition(condition);
        return this;
    }

    public AIDomainBuilder SetState(AIWorldState state, byte value, EffectType type)
    {
        if (Pointer is IPrimitiveTask task) {
            var effect = new SetWorldStateEffect(state, value, type);
            task.AddEffect(effect);
        }
        return this;
    }

    public AIDomainBuilder TakenDamage()
    {
        var condition = new TakenDamageCondition();
        Pointer.AddCondition(condition);

        return this;
    }

    public AIDomainBuilder RecievedDamage()
    {
        Action("Recieved Damage");
        if (Pointer is IPrimitiveTask task) {
            task.SetOperator(new TakeDamageOperator());
        }
        End();

        return this;
    }

    public AIDomainBuilder MoveToPlayer()
    {
        Action("Move to player");
        if (Pointer is IPrimitiveTask task) {
            task.SetOperator(new MoveToOperator(AIDestinationTarget.Enemy));
        }
        End();

        return this;
    }

    public AIDomainBuilder MoveToPatrolPoint()
    {
        Action("Move to Patrol Point");
        if (Pointer is IPrimitiveTask task) {
            task.SetOperator(new MoveToOperator(AIDestinationTarget.PatrolPoint));
        }
        End();

        return this;
    }

    public AIDomainBuilder FireWeapon()
    {
        Action("Fire Weapon");
        if (Pointer is IPrimitiveTask task) {
            task.SetOperator(new FireWeaponOperator());
        }
        End();

        return this;
    }
}
