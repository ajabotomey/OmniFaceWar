using FluidHTN;
using UnityEngine;

[CreateAssetMenu(fileName = "TrooperDomain", menuName = "AI/Domains/Trooper")]
public class TrooperDomainDefinition : AIDomainDefinition
{
    public override Domain<AIContext> Create()
    {
        return new AIDomainBuilder("Trooper")
            .Sequence("Engage enemy")
                .HasState(AIWorldState.EnemyFound)
                .FireWeapon()
                .Select("Attack or pursue?")
                    .PursuePlayer()
                    .AttackPlayer()
                    .AttackPlayerAndAlertOthers()
                    //.MoveToPlayer()
                    //.FireWeapon()
                    //.Sequence("Pursue Enemy")
                    //    .MoveToPlayer()
                    //    .FireWeapon()
                    //.End()
                .End()
            .End()
            .Select("Move to Patrol Point")
                .MoveToPatrolPoint()
            .End()
            .Build();
    }
}

            //.Sequence("Received Damage")
            //    .TakenDamage()
            //    .RecievedDamage()
            //.End()
            //.Sequence("Found Enemy")
            //    .HasState(AIWorldState.EnemyFound)
            //.End()