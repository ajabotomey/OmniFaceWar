using FluidHTN;
using FluidHTN.Operators;

public class TakeDamageOperator : IOperator
{
    public TaskStatus Update(IContext ctx)
    {
        if (ctx is AIContext c) {
            if (c.GenericTimer <= 0f) {


                // Run Animation here
            }

            c.GenericTimer = -1f;
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;
    }

    public void Stop(IContext ctx)
    {
        if (ctx is AIContext c) {
            c.GenericTimer = -1f;
        }
    }
}
