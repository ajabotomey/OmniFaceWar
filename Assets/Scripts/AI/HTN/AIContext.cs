using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AIContext
{
    public AIAgent Agent { get; }
    public AISenses Senses { get; }
    public AICharacter Enemy { get; }
    public EntityHealth Health { get; }

    public float Time { get; set; }
    public float DeltaTime { get; set; }
    public float GenericTimer { get; set; }

    // TODO: Add accessor and mutator
    private bool recentlyAttacked = false;

    public AICharacter CurrentEnemy { get; set; }

    /// <summary>
    /// We can use this to prevent sensory updates from causing an unwanted replan.
    /// This is used in the attack operator to ensure we always complete an attack
    /// once its started, so that its effect is played, ensuring our attacker gets
    /// tired.
    /// This still feels a bit hacky. I'd prefer something more deliberate, since
    /// so far in the example its fairly unique to attacks. So I'm not sure we
    /// need this kind of wide-spread blocking of sensory.
    /// </summary>
    public bool CanSense { get; set; }

    public AIContext(AIAgent agent, AISenses senses, EntityHealth health)
    {
        Agent = agent;
        Senses = senses;
        Enemy = agent.GetComponent<AICharacter>();
        Enemy.Init(this);
        CanSense = true;
        Health = health;
        base.Init();
    }

    public void TakeDamage(int damage)
    {
        Health.ApplyDamage(damage);
        recentlyAttacked = true;
    }

    public bool WasRecentlyAttacked()
    {
        if (recentlyAttacked == true) {
            recentlyAttacked = false;
            return true;
        }

        return false;
    }
}
