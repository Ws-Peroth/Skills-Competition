using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirusBullet : Bullet
{
    public override void InitializeBaseData()
    {
        BulletSpeed = 0.1f;
        BulletTarget = "HitBox";
        BulletDirection = Vector3.down;
        BulletType = PoolCode.VirusBullet;
    }

    protected override void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        transform.Translate(BulletDirection * BulletSpeed);
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
