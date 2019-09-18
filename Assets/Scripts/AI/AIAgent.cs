using System;
using FluidHTN;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class AIAgent : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The domain definition for this agent")]
    private AIDomainDefinition _domainDefinition;

    [SerializeField]
    [Tooltip("The sensing capabilities of the agent")]
    private AISenses _senses;


    private Planner<AIContext> _planner;
    private Domain<AIContext> _domain;
    private AIContext _context;
    private SensorySystem _sensory;

    // Start is called before the first frame update
    void Awake()
    {
        if (_domainDefinition == null) {
            Logger.Debug($"Missing domain definition in {name}!");
            gameObject.SetActive(false);
            return;
        }

        _planner = new Planner<AIContext>();
        // Create context here
        // Create sensory system here

        _domain = _domainDefinition.Create();
    }

    // Update is called once per frame
    void Update()
    {
        if (_planner == null || _domain == null || _context == null) // Add sensory system here
            return;




        _planner.Tick(_domain, _context);
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
