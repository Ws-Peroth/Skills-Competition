using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeCovidBullet : Bullet
{
    public bool isReflect { get; set; }
    public override void InitializeBaseData()
    {
        BulletSpeed = 0.07f;
        BulletTarget = "HitBox";
        BulletDirection = Vector3.down;
        BulletType = PoolCode.UpgradeCovidVirusBullet;
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
        if ( col.CompareTag("MoveBorder") && isReflect)
        {
            transform.Rotate(new Vector3(0, 0, 180));
            isReflect = false;
        }
        if (col.CompareTag(BulletTarget))
        {
            col.GetComponentInParent<Player>().Damaged(BulletDamage, DamageType.Hp);
            PoolManager.Instance.DestroyPrefab(gameObject, BulletType);
        }
        base.OnTriggerEnter2D(col);
    }
}
