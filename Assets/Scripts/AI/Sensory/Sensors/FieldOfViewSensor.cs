using FluidHTN;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfViewSensor : MonoBehaviour, ISensor
{

    [Header("Sensor Information")]
    [SerializeField]
    [Tooltip("How often should we update the information from the sensor")]
    private float _tickRate = 1f;

    public float TickRate => _tickRate;
    public float NextTickTime { get; set; }

    [Header("FOV Information")]
    [SerializeField] private float viewRadius;
    [SerializeField] [Range(0, 360)] private float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public LayerMask smokeBombMask;

    [HideInInspector] public List<Transform> visibleTargets = new List<Transform>();

    public float meshResolution;
    public int edgeResolveIterations;
    public float edgeDstThreshold;

    [SerializeField] private MeshFilter viewMeshFilter;
    [SerializeField] private MeshRenderer viewMeshRenderer;
    private Mesh viewMesh;

    public bool VisualisationOn {
        get; set;
    }

    public float ViewRadius {
        get { return viewRadius; }
        set { viewRadius = value; }
    }

    public float ViewAngle {
        get { return viewAngle; }
        set { viewAngle = value; }
    }

    public void Tick(AIContext context)
    {
        FindVisibleTargets();

        if (FOVDetect()) {
            context.SetState(AIWorldState.EnemyFound, true, EffectType.PlanAndExecute);
        }
    }

    public void DrawGizmos(AIContext context)
    {
        
    }

    void LateUpdate()
    {

    }

    void FindVisibleTargets()
    {
        visibleTargets.Clear();

        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector2 dirToTarget = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
            float angle = Vector2.Angle(dirToTarget, transform.up);
            if (angle < viewAngle / 2) {
                float dstToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, (obstacleMask | smokeBombMask))) {
                    visibleTargets.Add(target);
                }
            }
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal) {
            angleInDegrees -= transform.eulerAngles.z;
        }

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
    }

    // Detect Player
    public bool FOVDetect()
    {
        Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll(transform.position, viewRadius, targetMask);

        int layerMask = obstacleMask | smokeBombMask;

        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius[i].transform;
            Vector2 dirToTarget = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
            float angle = Vector2.Angle(dirToTarget, transform.up);
            if (angle < viewAngle / 2) {
                float dstToTarget = Vector2.Distance(transform.position, target.position);

                if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, layerMask)) {
                    return true;
                }
            }
        }

        return false;
    }
}
