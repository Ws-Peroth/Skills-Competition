using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    private const int MaxPower = 5;
    private const float amagedUnbreakableTime = 1.5f;
    
    private Color _defaultColor = Color.white;
    private Color _unbreakableColor = new Color(1, 1, 1, 0.3f);
    [SerializeField] private SpriteRenderer playerRenderer;
    
    private bool isDamageBuff;
    private Coroutine _damageUpCoroutine;

    [SerializeField] private bool isUnbreakable;
    private Coroutine _unbreakableCoroutine;

    private void Start()
    {
        StartCoroutine(StartGame());
    }

    private IEnumerator StartGame()
    {
        StartCoroutine(FadeIn(4   ));
        InitializeBaseData();
        Attack();
        yield return new WaitForSeconds(2);
    }

    private IEnumerator FadeIn(float time)
    {
        var loopCount = (time / 0.04f);
        var alphaDelta = 1 / loopCount;
        playerRenderer.color = new Color(1, 1, 1, 0);
        
        while (loopCount --> 0)
        {
            var color = playerRenderer.color;
            color.a += alphaDelta;
            playerRenderer.color = color;
            yield return new WaitForSeconds(0.04f);
        }
    }
    
    private void Update()
    {
        if(GameManager.Instance.isGameEnd) gameObject.SetActive(false);
        
        if (!GameManager.Instance._isChangedForcedUnbreakable) return;
        
        GameManager.Instance._isChangedForcedUnbreakable = false;

        if (_unbreakableCoroutine != null)
        {
            StopCoroutine(_unbreakableCoroutine);
        }

        isUnbreakable = GameManager.Instance.IsForcedUnbreakable;
        playerRenderer.color = isUnbreakable ? _unbreakableColor : _defaultColor;

    }

    public override void InitializeBaseData()
    {
        AttackDelay = 0.1f;
        GameManager.Instance.Power = 1;
        Speed = 0.2f;
        Damage = 1;
        BulletType = PoolCode.PlayerBullet;
    }

    #region Item

    public void GetPowerUpItem()
    {
        var power = GameManager.Instance.Power + 1;
        print($"[GetPowerUpItem] power : {power}");
        GameManager.Instance.Power = power > MaxPower ? MaxPower : power;
    }

    public void GetDamageUpItem(int damage)
    {
        Damage = Damage + damage > MaxPower ? MaxPower : Damage + damage;
    }

    public void SetUnbreakableMode(float time)
    {
        if (GameManager.Instance.IsForcedUnbreakable)
        {
            return;
        }

        UnbreakableRoutine(time);
    }

    private void UnbreakableRoutine(float time)
    {
        if (isUnbreakable)
        {
            StopCoroutine(_unbreakableCoroutine);
        }

        _unbreakableCoroutine = StartCoroutine(UnbreakableBuff(time));
    }

    private IEnumerator UnbreakableBuff(float time)
    {
        isUnbreakable = true;
        playerRenderer.color = _unbreakableColor;
        yield return new WaitForSeconds(time - 0.5f);

        playerRenderer.color = _defaultColor;
        yield return new WaitForSeconds(0.5f);

        isUnbreakable = false;
    }

    #endregion

    protected override void Killed()
    {
        throw new System.NotImplementedException();
    }

    public override void Damaged(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void Damaged(float damage, DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Hp:
                if (GameManager.Instance.IsForcedUnbreakable) return;
                if (isUnbreakable) return;
                
                SoundManager.Instance.SfxPlay(SFX.PlayerHit);
                GameManager.Instance.GetHpDamage(damage);
                SetUnbreakableMode(amagedUnbreakableTime);
                break;
            
            case DamageType.Pain:
                GameManager.Instance.GetPainDamage(damage);
                break;
        }
    }

    protected override void OnBecameInvisible()
    {
        return;
    }

    protected override void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(Vector3.right * Speed);
            playerRenderer.flipX = true;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector3.left * Speed);
            playerRenderer.flipX = false;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(Vector3.up * Speed);
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(Vector3.down * Speed);
        }
    }

    #region AttackRoutine

    protected override void Attack()
    {
        StartCoroutine(AttackRoutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (!Input.GetKey(KeyCode.Space))
            {
                yield return null;
                continue;
            }
            Shoot();
            yield return new WaitForSeconds(AttackDelay);
        }
    }

    private void Shoot()
    {
        SoundManager.Instance.SfxPlay(SFX.PlayerAttack);
        var N = 8f;
        var d = 2f;

        var startPosition = -(GameManager.Instance.Power - 1f) / N;
        for (var i = 0; i < GameManager.Instance.Power; i++)
        {
            var position = transform.position + new Vector3(startPosition, 0, 0);
            var bullet = PoolManager.Instance.CreatPrefab(PoolCode.PlayerBullet);
            bullet.transform.position = position;
            InitBullet(bullet);
            startPosition += d / N;
        }
    }

    private void InitBullet(GameObject bullet)
    {
        var bulletScript = bullet.GetComponent<PlayerBullet>();
        bulletScript.BulletDamage = Damage;
        bulletScript.InitializeBaseData();
        bullet.SetActive(true);
    }

    #endregion
}