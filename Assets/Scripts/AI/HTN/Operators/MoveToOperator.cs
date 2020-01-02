using FluidHTN;
using FluidHTN.Operators;
using System.Collections.Generic;
using UnityEngine;

public class MoveToOperator : IOperator
{
    private AIDestinationTarget DestinationTarget;

    private bool isNavigating = false;
    private List<AStarNode> nodePath;

    private Vector3 currentTargetPos;

    public MoveToOperator(AIDestinationTarget target)
    {
        DestinationTarget = target;
    }

    public TaskStatus StartNavigation(AIContext c)
    {
        //nodePath = c.AStar.FindPath(c.Agent.transform.position, DestinationPos);
        //if (nodePath.Count == 0)
        //    return TaskStatus.Failure;

        if (DestinationTarget == AIDestinationTarget.PatrolPoint) {
            if (c.patrolPoints == null || c.patrolPoints.Length == 0) {
                return TaskStatus.Failure;
            }

            // Move the the next waypoint
            FindNewPath(c, c.patrolPoints[c.currentWaypoint].position);

            isNavigating = true;
            return TaskStatus.Continue;
        } else if (DestinationTarget == AIDestinationTarget.Enemy) {
            if (c.CurrentEnemy == null)
                return TaskStatus.Failure;

            FindNewPath(c, c.CurrentEnemy.transform.position);

            isNavigating = true;
            return TaskStatus.Continue;
        }
        
        return TaskStatus.Failure;
    }

    public TaskStatus UpdateNavigation(AIContext c)
    {
        if (DestinationTarget == AIDestinationTarget.PatrolPoint) {
            if (c.patrolPoints == null || c.patrolPoints.Length == 0) {
                return TaskStatus.Failure;
            }

            AIAgent agent = c.Agent;

            // collision detection
            if (Physics2D.Raycast(c.Agent.transform.position, currentTargetPos, 5.0f, c.Agent.Layer)) {
                // Recreate the path
                FindNewPath(c, c.patrolPoints[c.currentWaypoint].position);
            }

            if (Vector2.Distance(agent.transform.position, currentTargetPos) < 0.05f) {

                nodePath[0].isSolid = false;
                nodePath.RemoveAt(0); // Remove first node each time
                

                if (nodePath.Count == 0) {

                    if (c.currentWaypoint == c.patrolPoints.Length - 1)
                        c.currentWaypoint = 0;
                    else
                        c.currentWaypoint++;

                    isNavigating = false;
                    return TaskStatus.Success;
                }

                nodePath[0].isSolid = true; // Prevents multiple AI units from selecting the tile that an AI unit is on.
                currentTargetPos = c.AStar.WorldPointFromNode(nodePath[0]);
            }

            RotateFOVSensor(c);

            agent.transform.position = Vector2.MoveTowards(agent.transform.position, currentTargetPos, Time.deltaTime);

            return TaskStatus.Continue;
        } else if (DestinationTarget == AIDestinationTarget.Enemy) {
            if (c.CurrentEnemy == null)
                return TaskStatus.Failure;

            AIAgent agent = c.Agent;

            if (Vector2.Distance(agent.transform.position, currentTargetPos) < 5f) { // TODO: Custom stop range for if in range of the player

                nodePath[0].isSolid = false;
                nodePath.RemoveAt(0); // Remove first node each time
                
                if (nodePath.Count == 0) {
                    isNavigating = false;
                    return TaskStatus.Success;
                }

                nodePath[0].isSolid = true; // Prevents multiple AI units from selecting the tile that an AI unit is on.
                currentTargetPos = c.AStar.WorldPointFromNode(nodePath[0]);
            }

            RotateFOVSensor(c);

            agent.transform.position = Vector2.MoveTowards(agent.transform.position, currentTargetPos, Time.deltaTime);

            return TaskStatus.Continue;
        }

        return TaskStatus.Failure;
    }

    public void Stop(IContext ctx)
    {
        if (ctx is AIContext c) {
            isNavigating = false;
        }
    }

    public TaskStatus Update(IContext ctx)
    {
        if (ctx is AIContext c) {
            if (isNavigating) {
                return UpdateNavigation(c);
            } else {
                return StartNavigation(c);
            }
        }

        return TaskStatus.Failure;
    }

    public TaskStatus FindNewPath(AIContext c, Vector3 newDestination)
    {
        nodePath = c.AStar.FindPath(c.Agent.transform.position, newDestination);
        if (nodePath.Count == 0)
            return TaskStatus.Failure;

        currentTargetPos = c.AStar.WorldPointFromNode(nodePath[0]);
        return TaskStatus.Continue;
    }

    public void RotateFOVSensor(AIContext c)
    {
        // Temporary
        var rotateSpeed = 180;
        var step = rotateSpeed * Time.deltaTime;

        Vector2 targetPos = currentTargetPos;
        Vector2 sensorPos = c.FOVSensor.position;
        targetPos.x -= sensorPos.x;
        targetPos.y -= sensorPos.y;

        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        angle -= 90;
        var newRotation = Quaternion.Euler(new Vector3(0, 0, angle));
        c.FOVSensor.rotation = Quaternion.RotateTowards(c.FOVSensor.rotation, newRotation, step);
    }
}
