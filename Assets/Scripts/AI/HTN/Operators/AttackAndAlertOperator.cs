using FluidHTN;
using FluidHTN.Operators;
using System.Collections.Generic;
using UnityEngine;

public class AttackAndAlertOperator : IOperator
{
    private bool isNavigating = false;
    private List<AStarNode> nodePath;

    private Vector3 currentTargetPos;
    public void Stop(IContext ctx)
    {
        throw new System.NotImplementedException();
    }

    public TaskStatus Update(IContext ctx)
    {
        if (ctx is AIContext c) {
            FireWeapon(c);

            if (isNavigating) {
                return UpdateNavigation(c);
            } else {
                return StartNavigation(c);
            }
        }

        return TaskStatus.Failure;
    }

    public TaskStatus StartNavigation(AIContext c)
    {
        if (c.CurrentEnemy == null)
            return TaskStatus.Failure;

        FindNewPath(c, c.CurrentEnemy.transform.position);
        c.Agent.AlertAllEnemies();

        isNavigating = true;
        return TaskStatus.Continue;
    }

    public TaskStatus UpdateNavigation(AIContext c)
    {
        if (c.CurrentEnemy == null)
            return TaskStatus.Failure;

        AIAgent agent = c.Agent;

        if (Vector2.Distance(agent.transform.position, currentTargetPos) < 5f) { // TODO: Custom stop range for if in range of the player

            nodePath.RemoveAt(0); // Remove first node each time

            if (nodePath.Count == 0) {
                isNavigating = false;
                return TaskStatus.Success;
            }

            currentTargetPos = c.AStar.WorldPointFromNode(nodePath[0]);
        }

        RotateFOVSensor(c);

        agent.transform.position = Vector2.MoveTowards(agent.transform.position, currentTargetPos, Time.deltaTime);

        return TaskStatus.Continue;
    }

    public TaskStatus FireWeapon(AIContext c)
    {
        if (c.WeaponFireTimer <= 0f) {
            RotateTowardsTarget(c.CurrentEnemy.transform, c);

            Bullet firedBullet = c.BulletFactory.Create();

            int damage = ((BulletTypeGun)c.CurrentWeapon).Damage;
            firedBullet.SetDamage(damage);

            // Incorporate spread into the aim
            var spreadFactor = c.SpreadFactor;
            var aim = c.Gun.up;
            //aim.x += Random.Range(-spreadFactor, spreadFactor);

            firedBullet.transform.position = c.BulletSpawnPoint.position;
            firedBullet.transform.rotation = Quaternion.identity;
            firedBullet.GetComponent<Rigidbody2D>().velocity = aim * 50.0f;
            firedBullet.transform.Rotate(0, 0, Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg);

            c.CurrentWeapon.Fire();

            c.WeaponFireTimer = c.CurrentWeapon.GetFireRate();

            return TaskStatus.Success;
        }

        c.WeaponFireTimer -= Time.deltaTime;
        return TaskStatus.Continue;
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

    public void RotateTowardsTarget(Transform target, AIContext c)
    {
        Vector2 targetPos = target.position;
        Vector2 gunPos = c.Gun.position;
        targetPos.x = targetPos.x - gunPos.x;
        targetPos.y = targetPos.y - gunPos.y;

        float angle = Mathf.Atan2(targetPos.y, targetPos.x) * Mathf.Rad2Deg;
        angle -= 90;
        c.Gun.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
