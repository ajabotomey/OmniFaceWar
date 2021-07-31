using FluidHTN;
using FluidHTN.Operators;
using UnityEngine;

public class FireWeaponOperator : IOperator
{
    public void Stop(IContext ctx)
    {
        if (ctx is AIContext c) {
            
        }
    }

    public TaskStatus Update(IContext ctx)
    {
        // Fire bullet.
        // TODO: Add fire rate

        if (ctx is AIContext c) {

            if (c.WeaponFireTimer <= 0f) {
                RotateTowardsTarget(c.CurrentEnemy.transform, c);

                Bullet firedBullet = c.BulletFactory.Create();

                int damage = ((BulletTypeGun)c.CurrentWeapon).Damage;
                firedBullet.SetDamage(damage);

                // Incorporate spread into the aim
                var spreadFactor = c.SpreadFactor;
                var aim = c.Gun.up;
                aim.x += Random.Range(-spreadFactor, spreadFactor);

                firedBullet.transform.position = c.BulletSpawnPoint.position;
                firedBullet.transform.rotation = Quaternion.identity;
                firedBullet.GetComponent<Rigidbody2D>().velocity = aim * 50.0f;
                firedBullet.transform.Rotate(0, 0, Mathf.Atan2(aim.y, aim.x) * Mathf.Rad2Deg);

                c.CurrentWeapon.Fire();

                c.WeaponFireTimer = c.CurrentWeapon.FireRate;

                return TaskStatus.Success;
            }

            c.WeaponFireTimer -= Time.deltaTime;
            return TaskStatus.Continue;
        }

        return TaskStatus.Failure;
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
