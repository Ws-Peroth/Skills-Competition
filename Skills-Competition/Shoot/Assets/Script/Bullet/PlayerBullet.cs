using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : Bullet
{
    [SerializeField] private Sprite defaultSprite;
    [SerializeField] private Sprite enhanceSprite;
    [SerializeField] private SpriteRenderer bulletRenderer;
    public override void InitializeBaseData()
    {
        // Bullet Damage
        BulletSpeed = 0.2f;
        BulletTarget = "Enemy";
        BulletDirection = Vector3.up;
        BulletType = PoolCode.PlayerBullet;

        var sprite = BulletDamage > 1 ? enhanceSprite : defaultSprite;
        bulletRenderer.sprite = sprite;
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
            col.GetComponent<Entity>().Damaged(BulletDamage);
            PoolManager.Instance.DestroyPrefab(gameObject, BulletType);
        }
        base.OnTriggerEnter2D(col);
    }
}
