using FluidHTN;
using UnityEngine;

[CreateAssetMenu(fileName = "TrooperDomain", menuName = "AI/Domains/Trooper")]
public class TrooperDomainDefinition : AIDomainDefinition
{
    public override Domain<AIContext> Create()
    {
        return new AIDomainBuilder("Trooper")
            .Sequence("Received Damage")
                .TakenDamage()
                .RecievedDamage()
            .End()
            .Sequence("Found Enemy")
                .HasState(AIWorldState.EnemyFound)
            .End()
            .Build();
    }
}
