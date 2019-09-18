using FluidHTN;
using FluidHTN.Conditions;

public class TakenDamageCondition : ICondition
{
    public string Name { get; } = "Taken Damage";

    public bool IsValid(IContext ctx)
    {
        if (ctx is AIContext c) {
            return c.WasRecentlyAttacked();
        }

        throw new System.Exception("Unexpected context type");
    }
}
