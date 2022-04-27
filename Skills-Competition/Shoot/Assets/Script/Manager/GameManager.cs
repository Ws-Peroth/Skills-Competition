using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public enum DamageType
{
    Hp,
    Pain
}
public class GameManager : MonoBehaviour
{
    [SerializeField] private Text resultText;
    public bool isGameEnd;
    private const float Stage1Time = 1;
    public const float Stage2Time = 1;

    private Coroutine[] _spawnCoroutines;
    
    public static GameManager Instance;
    [field:SerializeField] public float Hp { get; private set; }
    [field:SerializeField] public float Pain { get; private set; }
    [field: SerializeField] public int Power { get; set; }
    [field:SerializeField] public int Score { get; set; }
    [field:SerializeField] public int Stage { get; set; }
    [field:SerializeField] public bool _isChangedForcedUnbreakable { get; set; }
    
    [field:SerializeField] private bool _isForcedUnbreakable;

    public bool stageChange = true;
    public bool IsForcedUnbreakable
    {
        get => _isForcedUnbreakable;
        set
        {
            _isChangedForcedUnbreakable = true;
            _isForcedUnbreakable = value;
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

        Hp = 100;
        Pain = 0;
        Stage = 1;
        
        resultText.gameObject.SetActive(false);
    }

    private void Start()
    {
        PoolManager.Instance.DestroyEntirePrefabs();
        StopAllCoroutines();
        StartCoroutine(StartStage1());
        
        // ShowStage.Instance.ShowStageTitle("Stage 1 : Vein");
    }

    public void FixDict(PoolCode code, SpawnData data)
    {
        _spawnData[code] = data;
    }
    
    private Dictionary<PoolCode, SpawnData> _spawnData = new Dictionary<PoolCode, SpawnData>()
    {
        {PoolCode.Bacteria,          new SpawnData(5.5f, (-2.7f, 2.7f), (1, 3))},
        {PoolCode.Germ,              new SpawnData(5.5f, (-2.7f, 2.7f), (3, 5))},
        {PoolCode.Virus,             new SpawnData(5.5f, (-2.7f, 2.7f), (7, 10))},
        {PoolCode.Cancer,            new SpawnData(5.5f, (-2.7f, 2.7f), (8, 11))},
        {PoolCode.WhiteCell,         new SpawnData(5.5f, (-2.7f, 2.7f), (1, 2))},
        {PoolCode.RedCell,           new SpawnData(5.5f, (-2.7f, 2.7f), (5, 10))},
        
        {PoolCode.CovidVirus,        new SpawnData(6.3f, (0f, 0f), (0, 0))},
        {PoolCode.UpgradeCovidVirus, new SpawnData(6.3f, (0f, 0f), (0, 0))},
    };

    private IEnumerator StartStage1()
    {
        SoundManager.Instance.BgmPlay(BGM.Stage1);
        BackGroundManager.Instance.ChangeStage(1);
        FadeEffect.Instance.FadeOut(2);
        yield return new WaitForSeconds(2);

        print("Stage 1 Start");
        Stage = 1;
        Hp = 100;
        Pain = 10;
        yield return new WaitForSeconds(2f);
        _spawnCoroutines = new[]
        {
            StartCoroutine(SpawnEnemy(PoolCode.Bacteria, 1)),
            StartCoroutine(SpawnEnemy(PoolCode.Germ, 10)),
            StartCoroutine(SpawnEnemy(PoolCode.Virus, 30)),
            StartCoroutine(SpawnEnemy(PoolCode.Cancer, 50)),
            StartCoroutine(SpawnEnemy(PoolCode.WhiteCell, 0)),
            StartCoroutine(SpawnEnemy(PoolCode.RedCell, 25)),
        };

        yield return new WaitForSeconds(Stage1Time);

        foreach (var coroutine in _spawnCoroutines)
        {
            StopCoroutine(coroutine);
        }
        
        SoundManager.Instance.BgmPlay(BGM.Stage1Boss);
        yield return new WaitForSeconds(3);
        
        Spawn(PoolCode.CovidVirus);

        yield break;
    }

    public void Stage1End()
    {
        print("Stage 1 End");
        Score += (int) ((100 - Pain) * 100);
        Score += (int) (Hp * 100);
        Pain = 0;
        PoolManager.Instance.DestroyEntirePrefabs();
        StopAllCoroutines();
        StartCoroutine(StartStage2());
    }
    
    private IEnumerator StartStage2()
    {
        stageChange = true;
        FadeEffect.Instance.FadeIn(2);
        yield return new WaitForSeconds(3);
        SoundManager.Instance.BgmPlay(BGM.Stage2);
        BackGroundManager.Instance.ChangeStage(2);
        FadeEffect.Instance.FadeOut(2);
        yield return new WaitForSeconds(2);
        
        // ShowStage.Instance.ShowStageTitle("Stage 2 : Brain");
        
        Stage = 2;
        Hp = 100;
        Pain = 30;
        print("Stage 2 Start");
        yield return new WaitForSeconds(2f);
        
        BossBG.Instance.BossAppend();
        _spawnCoroutines = new[]
        {
            StartCoroutine(SpawnEnemy(PoolCode.Bacteria, 1)),
            StartCoroutine(SpawnEnemy(PoolCode.Germ, 1)),
            StartCoroutine(SpawnEnemy(PoolCode.Virus, 3)),
            StartCoroutine(SpawnEnemy(PoolCode.Cancer, 5)),
            StartCoroutine(SpawnEnemy(PoolCode.WhiteCell, 0)),
            StartCoroutine(SpawnEnemy(PoolCode.RedCell, 10)),
        };

        yield return new WaitForSeconds(Stage2Time);

        foreach (var coroutine in _spawnCoroutines)
        {
            StopCoroutine(coroutine);
        }
        
        SoundManager.Instance.BgmPlay(BGM.Stage2Boss);
        yield return new WaitForSeconds(2f);
        
        
        Spawn(PoolCode.UpgradeCovidVirus);
        
        yield break;
    }
    
    public void Stage2End()
    {
        BossBG.Instance.BossDestroyed();
        resultText.text = "Game Clear";
        print("Stage 2 End");
        Score += (int) ((100 - Pain) * 100);
        Score += (int) (Hp * 100);
        Pain = 0;
        PoolManager.Instance.DestroyEntirePrefabs();
        StartCoroutine(OpenEnding());
    }

    private IEnumerator OpenEnding()
    {
        var score = Score;
        isGameEnd = true;
        resultText.gameObject.SetActive(true);
        yield return new WaitForSeconds(3);
        FadeEffect.Instance.FadeIn(2);
        yield return new WaitForSeconds(3);
        SoundManager.Instance.BgmPlay(BGM.Ending);

        
        
        if (ScoreManager.Instance.IsNewRecord(score))
        {
            UIManager.Instance.OpenScoreInputPage();
        }
        else
        {
            UIManager.Instance.OpenLeaderBoard();
        }
        
        FadeEffect.Instance.FadeOut(2);
        yield return new WaitForSeconds(2);
    }

    #region Itme

    private const float MaxHp = 100;
    private const float MinHp = 0;
    public void GetHpHealItem(float hp)
    {
        Hp = Hp + hp > MaxHp ? MaxHp : Hp + hp;
    }

    public void GetPainHealItem(float pain)
    {
        Pain = Pain - pain < 0 ? 0 : Pain - pain;
    }
    
    public void GetPainDamage(float damage)
    {
        var pain = Pain + damage;
        Pain = pain > 100 ? 100 : pain;
        print($"[GameManager] Get Pain damage : {damage}");
        IsGameOver();
    }

    public void GetHpDamage(float damage)
    {
        var hp = Hp - damage;
        Hp = hp < MinHp ? MinHp : hp;
        IsGameOver();
    }

    private void IsGameOver()
    {
        if (Hp <= 0 || Pain >= 100)
        {
            print("Game Over");
            resultText.text = "Game Over";
            StopAllCoroutines();
            PoolManager.Instance.DestroyEntirePrefabs();
            StartCoroutine(OpenEnding());
        }
    }

    public void GetScore(int score)
    {
        Score += score;
    }
    #endregion

    #region Cheat
    
    public void SetPlayerHp(float hp)
    {
        Hp = hp;
        IsGameOver();
    }

    public void SetPlayerPain(float pain)
    {
        Pain = pain;
        IsGameOver();
    }
    #endregion
    
    #region Spawn routine

    private IEnumerator SpawnEnemy(PoolCode code, float startDelay)
    {
        yield return new WaitForSeconds(startDelay);
        
        while (true)
        {
            var delay = Spawn(code);
            yield return new WaitForSeconds(delay);
        }
    }

    public class SpawnData
    {
        public float Y { get; set; }
        public (float start, float end) X { get; set; }
        public (float start, float emd) Delay { get; set; }

        public SpawnData(float y, (float start, float end) x, (float start, float end) delay)
        {
            Y = y;
            X = x;
            Delay = delay;
        }
    }

    private static float GetRandNum((float start, float end) data) => Random.Range(data.start, data.end);
    
    public float Spawn(PoolCode code)
    {
        var data = _spawnData[code];
        var mob = PoolManager.Instance.CreatPrefab(code);
        mob.transform.position = new Vector3(GetRandNum(data.X), data.Y);
        var mobScript = mob.GetComponent<Entity>();
        mob.SetActive(true);
        mobScript.InitializeBaseData();
        return GetRandNum(data.Delay);
    }
    
    #endregion
}
