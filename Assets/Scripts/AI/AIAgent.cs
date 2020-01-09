using System;
using FluidHTN;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class AIAgent : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The domain definition for this agent")]
    private AIDomainDefinition _domainDefinition;

    [SerializeField]
    [Tooltip("The sensing capabilities of the agent")]
    private AISenses _senses;

    [SerializeField]
    private EntityHealth health;

    [SerializeField]
    private AStarPathfinding aStar;

    [SerializeField]
    private Transform[] patrolPoints;

    [SerializeField]
    private Transform fovSensor;

    [SerializeField]
    private Transform gun;

    [SerializeField]
    private Transform bulletSpawnPoint;

    [SerializeField]
    private float spreadFactor;

    [SerializeField]
    private Weapon weapon;

    [SerializeField]
    private VoidEvent alertAllEnemiesEvent;

    public LayerMask Layer { get; set; }

    [Inject]
    private Bullet.Factory bulletFactory;

    private Planner<AIContext> _planner;
    private Domain<AIContext> _domain;
    private AIContext _context;
    private SensorySystem _sensory;

    private Vector3 destinationPos;

    // Start is called before the first frame update
    void Awake()
    {
        if (_domainDefinition == null) {
            Logger.Debug($"Missing domain definition in {name}!");
            gameObject.SetActive(false);
            return;
        }

        _planner = new Planner<AIContext>();
        _context = new AIContext(this, _senses, health, aStar, patrolPoints, fovSensor, gun, bulletSpawnPoint, spreadFactor, weapon, bulletFactory);
        _sensory = new SensorySystem(this);

        _domain = _domainDefinition.Create();

        Layer = gameObject.layer;
    }

    // Update is called once per frame
    void Update()
    {
        if (_planner == null || _domain == null || _context == null || _sensory == null) // Add sensory system here
            return;

        _context.Time = Time.time;
        _context.DeltaTime = Time.deltaTime;

        if (_context.CanSense) {
            _sensory.Tick(_context);
        }

        _planner.Tick(_domain, _context);

        if (_context.LogDecomposition) {
            Debug.Log("---------------------- DECOMP LOG --------------------------");
            while (_context.DecompositionLog?.Count > 0) {
                var entry = _context.DecompositionLog.Dequeue();
                var depth = FluidHTN.Debug.Debug.DepthToString(entry.Depth);
                //Console.ForegroundColor = entry.Color;
                Debug.Log(depth + " " + entry.Name + ": " + entry.Description);
            }
            //Console.ResetColor();
            Debug.Log("-------------------------------------------------------------");
        }
    }

    public void AlertAllEnemies()
    {
        alertAllEnemiesEvent.Raise();
    }

    public void SetAgentToFullAlert()
    {
        _context.SetState(AIWorldState.AlertLevel, 100, EffectType.PlanAndExecute);
    }

    private void OnDrawGizmos()
    {
        if (_context == null)
            return;

        _senses?.DrawGizmos(_context);
        _sensory?.DrawGizmos(_context);

#if UNITY_EDITOR
        var task = _planner.GetCurrentTask();
        if (task != null) {
            // Create a handle
            Handles.Label(transform.position + Vector3.up, task.Name);
        }
#endif
    }
}
