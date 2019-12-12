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

    private bool foundPlayer = false;

    public bool VisualisationOn {
        get; set;
    }

    void Start()
    {
        viewMesh = new Mesh {
            name = "View Mesh"
        };
        viewMeshFilter.mesh = viewMesh;
        VisualisationOn = false;
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
            if (!foundPlayer) {
                context.SetState(AIWorldState.EnemyFound, true, EffectType.PlanAndExecute);
                foundPlayer = true;
            }
        } else {
            if (foundPlayer) {
                if (context.HasState(AIWorldState.EnemyFound, true)) {
                    context.SetState(AIWorldState.EnemyFound, false, EffectType.PlanAndExecute);
                }

                foundPlayer = false;
            }
        }
    }

    public void DrawGizmos(AIContext context)
    {
        
    }

    void LateUpdate()
    {
        if (VisualisationOn) {
            viewMeshRenderer.enabled = true;
            DrawFieldOfView();
        } else {
            viewMeshRenderer.enabled = false;
        }
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

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector2> viewPoints = new List<Vector2>();
        ViewCastInfo oldViewCast = new ViewCastInfo();
        for (int i = 0; i <= stepCount; i++) {
            float angle = -(transform.eulerAngles.z - (viewAngle / 2) + viewAngle) + stepAngleSize * i;
            ViewCastInfo newViewCast = ViewCast(angle);

            if (i > 0) {
                bool edgeDstThresholdExceeded = Mathf.Abs(oldViewCast.dst - newViewCast.dst) > edgeDstThreshold;
                if (oldViewCast.hit != newViewCast.hit || (oldViewCast.hit && newViewCast.hit && edgeDstThresholdExceeded)) {
                    EdgeInfo edge = FindEdge(oldViewCast, newViewCast);
                    if (edge.pointA != Vector2.zero) {
                        viewPoints.Add(edge.pointA);
                    }
                    if (edge.pointB != Vector2.zero) {
                        viewPoints.Add(edge.pointB);
                    }
                }

            }


            viewPoints.Add(newViewCast.point);
            oldViewCast = newViewCast;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++) {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);

            if (i < vertexCount - 2) {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }

        viewMesh.Clear();

        viewMesh.vertices = vertices;
        viewMesh.triangles = triangles;
        viewMesh.RecalculateNormals();
    }

    EdgeInfo FindEdge(ViewCastInfo minViewCast, ViewCastInfo maxViewCast)
    {
        float minAngle = minViewCast.angle;
        float maxAngle = maxViewCast.angle;
        Vector2 minPoint = Vector2.zero;
        Vector2 maxPoint = Vector2.zero;

        for (int i = 0; i < edgeResolveIterations; i++) {
            float angle = (minAngle + maxAngle) / 2;
            ViewCastInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.dst - newViewCast.dst) > edgeDstThreshold;
            if (newViewCast.hit == minViewCast.hit && !edgeDstThresholdExceeded) {
                minAngle = angle;
                minPoint = newViewCast.point;
            } else {
                maxAngle = angle;
                maxPoint = newViewCast.point;
            }
        }

        return new EdgeInfo(minPoint, maxPoint);
    }


    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, new Vector2(dir.x, dir.y), viewRadius, obstacleMask | smokeBombMask);

        if (hit) {
            return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        } else {
            return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
        }
    }

    public struct ViewCastInfo
    {
        public bool hit;
        public Vector2 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector2 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    public struct EdgeInfo
    {
        public Vector2 pointA;
        public Vector2 pointB;

        public EdgeInfo(Vector2 _pointA, Vector2 _pointB)
        {
            pointA = _pointA;
            pointB = _pointB;
        }
    }
}
