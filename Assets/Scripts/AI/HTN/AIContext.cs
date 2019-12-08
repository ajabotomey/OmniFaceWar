using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class AIContext
{
    public AIAgent Agent { get; }
    public AISenses Senses { get; }
    public AICharacter Enemy { get; }
    public EntityHealth Health { get; }
    public AStarPathfinding AStar { get; }
    public Transform FOVSensor { get; }
    public Transform Gun { get; }
    public Transform BulletSpawnPoint { get; }
    public Weapon CurrentWeapon { get; }
    public Bullet.Factory BulletFactory { get; }
    public Transform[] patrolPoints { get; }

    public float SpreadFactor { get; }

    public float Time { get; set; }
    public float DeltaTime { get; set; }
    public float GenericTimer { get; set; }

    // TODO: Add accessor and mutator
    private bool recentlyAttacked = false;

    public int currentWaypoint = 0;

    public TestPlayerControl CurrentEnemy { get; set; }

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

    public AIContext(AIAgent agent, AISenses senses, EntityHealth health, AStarPathfinding aStar, Transform[] waypoints, Transform fov, Transform gunObj, Transform bulletSpawn, float spreadFactor, Weapon weapon, Bullet.Factory bulletFactory)
    {
        Agent = agent;
        Senses = senses;
        Enemy = agent.GetComponent<AICharacter>();
        Enemy.Init(this);
        CanSense = true;
        Health = health;
        AStar = aStar;
        patrolPoints = waypoints;
        FOVSensor = fov;
        Gun = gunObj;
        BulletSpawnPoint = bulletSpawn;
        SpreadFactor = spreadFactor;
        BulletFactory = bulletFactory;
        CurrentWeapon = weapon;

        CurrentEnemy = GameObject.FindGameObjectWithTag("Player").GetComponent<TestPlayerControl>();

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
