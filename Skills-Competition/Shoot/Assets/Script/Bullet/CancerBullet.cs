using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CancerBullet : Bullet
{
    private float time;
    public override void InitializeBaseData()
    {
        time = 0;
        BulletSpeed = 0.1f;
        BulletTarget = "HitBox";
        BulletDirection = Vector3.down;
        BulletType = PoolCode.CancelBullet;
    }

    protected override void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        time += Time.deltaTime;

        transform.Translate(BulletDirection * (time - 0.7f) * BulletSpeed);
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag(BulletTarget))
        {
            col.GetComponentInParent<Player>().Damaged(BulletDamage, DamageType.Hp);
            PoolManager.Instance.DestroyPrefab(gameObject, BulletType);
        }
        base.OnTriggerEnter2D(col);
    }
}
