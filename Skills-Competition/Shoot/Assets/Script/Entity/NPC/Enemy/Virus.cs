using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Virus : Entity
{
    public override void InitializeBaseData()
    {
        AttackDelay =3f;
        IsDestroyed = false;
        Score = 100;
        Speed = 0.03f;
        Damage = 6;
        Hp = GameManager.Instance.Stage == 1 ? 30 : 50;
        PrefabType = PoolCode.Virus;
        BulletType = PoolCode.VirusBullet;

        StartCoroutine(AttackRoutine());
    }
    private void InitBullet(GameObject bullet)
    {
        var bulletScript = bullet.GetComponent<VirusBullet>();
        bulletScript.InitializeBaseData();
        bulletScript.BulletDamage = Damage;
        bullet.SetActive(true);
    }    
    protected override void Attack()
    {
        SoundManager.Instance.SfxPlay(SFX.EnemyAttack);
        for (var i = 0; i < 5; i++)
        {
            var position = transform.position;
            var bullet = PoolManager.Instance.CreatPrefab(BulletType);
            bullet.transform.position = position;
            bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, (i - 2) * 10));
            InitBullet(bullet);
        }
    }

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
        if(Hp <= 0){return;}
        Hp -= damage;

        if (Hp <= 0)
        {
            Killed();
        }

    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            Attack();
            yield return new WaitForSeconds(AttackDelay);
        }
    }

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
