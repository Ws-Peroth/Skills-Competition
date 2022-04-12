using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class UpgradeCovidBoss : Entity
{
public override void InitializeBaseData()
    {
        AttackDelay = 2f;
        IsDestroyed = false;
        Score = 100;
        Speed = 0.03f;
        Damage = 6;
        Hp = 15000;
        PrefabType = PoolCode.UpgradeCovidVirus;
        BulletType = PoolCode.UpgradeCovidVirusBullet;

        UIManager.Instance.CurrentBoss = this;
        UIManager.Instance.BossHp.gameObject.SetActive(true);
        UIManager.Instance.BossMaxHp = Hp;
        StartCoroutine(AttackRoutine());
    }

    private void InitBullet(GameObject bullet, bool isReflect)
    {
        var bulletScript = bullet.GetComponent<UpgradeCovidBullet>();
        bulletScript.InitializeBaseData();
        bulletScript.BulletDamage = Damage;
        bulletScript.isReflect = isReflect;
        bullet.SetActive(true);
    }

    protected override void Attack()
    {
        new NotImplementedException();
    }

    private void Attack(float rotation, bool isReflect)
    {
        var position = transform.position;
        var bullet = PoolManager.Instance.CreatPrefab(BulletType);
        bullet.transform.position = position;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        InitBullet(bullet, isReflect);
    }

    protected override void OnBecameInvisible()
    {
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
        UIManager.Instance.CurrentBoss = null;
        UIManager.Instance.BossHp.gameObject.SetActive(false);
        
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
        GameManager.Instance.Stage2End();
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
    {var rot = 0f;
        while (true)
        {
            var patternNumber = Random.Range(0, 3);

            print($"pattern : {patternNumber}");
            
            
            switch (patternNumber)
            {
                case 0:
                    for (var i = 0; i < 5; i++)
                    {
                        ShootPattern(rot + i, true);
                        yield return new WaitForSeconds(1f);
                    }
                    break;
                case 1:
                    for (var loop = 0; loop < 3; loop++)
                    {
                        for (var i = 0; i < 3; i++)
                        {
                            ShootPattern(i * 3 + rot, false);
                            yield return new WaitForSeconds(0.2f);
                        }

                        yield return new WaitForSeconds(2f);
                        
                        for (var i = 3; i >= 0; i--)
                        {
                            ShootPattern(i * 3 + rot, false);
                            yield return new WaitForSeconds(0.2f);
                        }
                        
                        yield return new WaitForSeconds(2f);
                    }

                    break;
                case 2:
                    Pattern3();
                    break;

            }

            rot = (rot + 1) % 360;
            yield return new WaitForSeconds(AttackDelay);
        }
    }
    private void ShootPattern(float rotation, bool isReflect)
    {
        SoundManager.Instance.SfxPlay(SFX.BossAttack);
        for (var i = 0; i < 36; i++)
        {
            Attack(i * 10 + rotation, isReflect);
        }
    }

    private void Pattern3()
    {
        for (var i = 0; i < 3; i++)
        {
            GameManager.Instance.Spawn(PoolCode.Cancer);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            col.GetComponent<Player>().Damaged(Damage / 2, DamageType.Hp);
        }

        base.OnTriggerEnter2D(col);
    }
}
