using FluidHTN;
using UnityEngine;
using System;

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
            .Sequence("Engage enemy")
                .HasState(AIWorldState.EnemyFound)
                //.FireWeapon()
                .Select("Warn, Attack or pursue?")
                    .WarnPlayer()
                    .PursuePlayer()
                    .AttackPlayer()
                    .AttackPlayerAndAlertOthers()
                .End()
            .End()
            .Sequence("Lost Enemy")
                .HasState(AIWorldState.EnemyFound, Convert.ToByte(false))
                .Select("Return to Last Position")         
                    .PauseForMoment() // Pause movement
                    .MoveToLastRecordedPosition() // Move to last recorded position
                    // Continue assault on player
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