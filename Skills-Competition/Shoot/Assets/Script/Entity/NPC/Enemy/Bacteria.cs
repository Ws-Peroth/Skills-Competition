using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;

public class Bacteria : Entity
{
    public override void InitializeBaseData()
    {
        IsDestroyed = false;
        Score = 100;
        Speed = 0.1f;
        Damage = 2;
        Hp = GameManager.Instance.Stage == 1 ? 5 : 25;
        PrefabType = PoolCode.Bacteria;
        // BulletType;
        isGetPain = false;
    }
    
    public bool isGetPain;

    protected override void OnBecameInvisible()
    {
        
    }

    protected override void FixedUpdate()
    {
        Move();
    }

    protected override void Killed()
    {
        if (IsDestroyed)
        {
            return;
        }

        IsDestroyed = true;
        var particle = ParticleManager.Instance.CreatPrefab(ParticlePool.Enemy);
        particle.SetPosition(transform.position);
        particle.Play();
        SoundManager.Instance.SfxPlay(SFX.EnemyDestroy);
        GameManager.Instance.GetScore(Score);
        PoolManager.Instance.DestroyPrefab(gameObject, PrefabType);
    }

    protected override void Move()
    {
        transform.Translate(Vector2.down * Speed);
    }

    public override void Damaged(float damage)
    {
        if(Hp < 0){return;}
        Hp -= damage;

        if (Hp < 0)
        {
            Killed();
        }

    }

    protected override void Attack() { return; }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("HitBox"))
        {
            col.GetComponentInParent<Player>().Damaged(Damage / 2, DamageType.Hp);
        }
        
        if (col.CompareTag("DestroyBorder"))
        {
            GameManager.Instance.GetPainDamage(Damage / 2);
        }
        
        base.OnTriggerEnter2D(col);
    }
}
