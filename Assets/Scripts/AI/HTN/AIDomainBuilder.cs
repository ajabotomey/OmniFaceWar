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
        //var condition = 
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
}
