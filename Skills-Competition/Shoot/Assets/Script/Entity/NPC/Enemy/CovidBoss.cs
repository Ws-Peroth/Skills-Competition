using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CovidBoss : Entity
{
    public override void InitializeBaseData()
    {
        AttackDelay = 2f;
        IsDestroyed = false;
        Score = 100;
        Speed = 0.03f;
        Damage = 6;
        Hp = 10000;
        PrefabType = PoolCode.CovidVirus;
        BulletType = PoolCode.CovidVirusBullet;

        UIManager.Instance.CurrentBoss = this;
        UIManager.Instance.BossHp.gameObject.SetActive(true);
        UIManager.Instance.BossMaxHp = Hp;
        StartCoroutine(AttackRoutine());
    }

    private void InitBullet(GameObject bullet)
    {
        var bulletScript = bullet.GetComponent<CovidBossBullet>();
        bulletScript.InitializeBaseData();
        bulletScript.BulletDamage = Damage;
        bullet.SetActive(true);
    }

    protected override void Attack()
    {
        new NotImplementedException();
    }

    private void Attack(float rotation)
    {
        var position = transform.position;
        var bullet = PoolManager.Instance.CreatPrefab(BulletType);
        bullet.transform.position = position;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        InitBullet(bullet);
    }

    protected override void OnBecameInvisible()
    {
        UIManager.Instance.CurrentBoss = null;
        UIManager.Instance.BossHp.gameObject.SetActive(false);
        
        if (IsDestroyed)
        {
            return;
        }

        Killed();
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

        if(GameManager.Instance.isGameEnd) return;
        
        IsDestroyed = true;
        var particle = ParticleManager.Instance.CreatPrefab(ParticlePool.Boss);
        particle.SetPosition(transform.position);
        particle.Play();
        SoundManager.Instance.SfxPlay(SFX.EnemyDestroy);
        GameManager.Instance.GetScore(Score);
        GameManager.Instance.Stage1End();
        PoolManager.Instance.DestroyPrefab(gameObject, PrefabType);
    }

    protected override void Move()
    {
        if(transform.position.y < 3) return;
        
        transform.Translate(Vector2.down * Speed);
    }

    public override void Damaged(float damage)
    {
        if (Hp <= 0)
        {
            return;
        }

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
            var patternNumber = Random.Range(0, 3);

            print($"pattern : {patternNumber}");
            
            switch (patternNumber)
            {
                case 0:
                    for (var i = 0; i < 5; i++)
                    {
                        SoundManager.Instance.SfxPlay(SFX.BossAttack);
                        Pattern1();
                        yield return new WaitForSeconds(2.5f);
                    }
                    break;
                case 1:
                    for (var loop = 0; loop < 3; loop++)
                    {
                        for (var i = 0; i < 3; i++)
                        {
                            Pattern2(i * 3);
                            yield return new WaitForSeconds(0.5f);
                        }

                        yield return new WaitForSeconds(2f);
                        
                        for (var i = 3; i >= 0; i--)
                        {
                            Pattern2(i * 3);
                            yield return new WaitForSeconds(0.5f);
                        }
                        
                        yield return new WaitForSeconds(2f);
                    }

                    break;
                case 2:
                    Pattern3();
                    break;

            }
            yield return new WaitForSeconds(AttackDelay);
        }
    }
    private void Pattern1()
    {
        SoundManager.Instance.SfxPlay(SFX.BossAttack);
        for (var i = 0; i < 24; i++)
        {
            Attack(i * 15);
        }
    }
    private void Pattern2(float rotation)
    {
        SoundManager.Instance.SfxPlay(SFX.BossAttack);
        for (var i = 0; i < 24; i++)
        {
            Attack(i * 15 + rotation);
        }
    }
    
    private void Pattern3()
    {
        for (var i = 0; i < 2; i++)
        {
            GameManager.Instance.Spawn(PoolCode.Virus);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("HitBox"))
        {
            col.GetComponentInParent<Player>().Damaged(Damage / 2, DamageType.Hp);
        }

        base.OnTriggerEnter2D(col);
    }
}
