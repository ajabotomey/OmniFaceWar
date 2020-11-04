using System;
using FluidHTN;

public class DecrementWorldStateEffect : IEffect
{
    public string Name { get; }
    public EffectType Type { get; }
    public AIWorldState State { get; }
    public byte Value { get; }

    public DecrementWorldStateEffect(AIWorldState state, EffectType type)
    {
        Name = $"IncrementState({state})";
        Type = type;
        State = state;
        Value = 1;
    }

    public DecrementWorldStateEffect(AIWorldState state, byte value, EffectType type)
    {
        Name = $"IncrementState({state})";
        Type = type;
        State = state;
        Value = value;
    }

    public void Apply(IContext ctx)
    {
        if (ctx is AIContext c) {
            var currentValue = c.GetState(State);
            if (currentValue - Value > 0)
            {
                c.SetState(State, (byte)(currentValue - Value), Type);
            } else
            {
                c.SetState(State, (byte)0, Type);
            }
            
            if (ctx.LogDecomposition) ctx.Log(Name, $"IncrementWorldStateEffect.Apply({State}:{currentValue}-{Value}:{Type})", ctx.CurrentDecompositionDepth + 1, this);
            return;
        }

        throw new Exception("Unexpected context type");
    }
}
