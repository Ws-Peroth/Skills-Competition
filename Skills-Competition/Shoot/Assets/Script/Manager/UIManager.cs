using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlTypes;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;
    
    
    public Slider BossHp;
    public float BossMaxHp;
    public Entity CurrentBoss;
    public Text CurrentBossText;
    
    [SerializeField] private Slider playerHpSlider;
    [SerializeField] private Slider playerPainSlider;
    
    [SerializeField] private Slider setPlayerHpSlider;
    [SerializeField] private Slider setPlayerPainSlider;
    [SerializeField] private Text setPlayerHpText;
    [SerializeField] private Text setPlayerPainText;

    [SerializeField] private GameObject leaderBoardPage;
    [SerializeField] private GameObject scoreInputPage;

    [HideInInspector] public string inputName;
    [SerializeField] private Text inputPageScoreText;
    
    [SerializeField] private Text leaderBoardScoreText;
    [SerializeField] private Text leaderBoardNameText;

    public void OnCloseButton()
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        SoundManager.Instance.BgmPlay(BGM.Title);
        SceneManager.LoadScene(1);
    }
    public void OnUserNameSubmit(Text userName)
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        if(string.IsNullOrWhiteSpace(userName.text)) return;
        
        inputName = userName.text;
        ScoreManager.Instance.AddScore(inputName, GameManager.Instance.Score);
        Instance.OpenLeaderBoard();
    }

    public void OpenLeaderBoard()
    {
        print("Open");
        var scores = ScoreManager.Instance.LeaderBoardData;
        leaderBoardScoreText.text = "";
        leaderBoardNameText.text = "";
        
        var range = math.min(ScoreManager.MaxRank, ScoreManager.Instance.LeaderBoardData.Count);
        for (var i = 0; i < range; i++)
        {
            var (userName, score) = scores[i];
            leaderBoardScoreText.text += $"{score}\n";
            leaderBoardNameText.text += $"{i + 1}. {userName}\n";
        }
        print("Open");
        leaderBoardPage.SetActive(true);
    }

    public void OpenScoreInputPage()
    {
        inputPageScoreText.text = $"Score : {GameManager.Instance.Score}";
        scoreInputPage.SetActive(true);
    }
    
    #region CheatUI
    public void SetStage(int stage)
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        switch (stage)
        {
            case 1:
                SceneManager.LoadScene(2);
                break;
            
            case 2:
                GameManager.Instance.Stage1End();
                break;
        }
    }

    public void SetPower(int power)
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        GameManager.Instance.Power = power;
    }

    public void ActiveUnbreakable(bool status)
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        GameManager.Instance.IsForcedUnbreakable = status;
    }

    public void SetPlayerHp()
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        GameManager.Instance.SetPlayerHp(setPlayerHpSlider.value);
    }
    public void SetPlayerPain()
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        GameManager.Instance.SetPlayerPain(setPlayerPainSlider.value);
    }

    public void SpawnWhiteCell()
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        GameManager.Instance.Spawn(PoolCode.WhiteCell);
    }

    public void SpawnRedCell()
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        GameManager.Instance.Spawn(PoolCode.RedCell);
    }

    public void AllKill()
    {
        SoundManager.Instance.SfxPlay(SFX.ButtonClick);
        PoolManager.Instance.DestroyAllPrefabs(PoolCode.Bacteria);
        PoolManager.Instance.DestroyAllPrefabs(PoolCode.Germ);
        PoolManager.Instance.DestroyAllPrefabs(PoolCode.Virus);
        PoolManager.Instance.DestroyAllPrefabs(PoolCode.Cancer);
        PoolManager.Instance.DestroyAllPrefabs(PoolCode.CovidVirus);
        PoolManager.Instance.DestroyAllPrefabs(PoolCode.UpgradeCovidVirus);
    }
    
    #endregion
    
    [SerializeField] private Text hpText; 
    [SerializeField] private Text painText; 
    [SerializeField] private Text powerText; 
    [SerializeField] private Text scoreText; 
    [SerializeField] private Text stageText;

    private void Update()
    {
        var hp = GameManager.Instance.Hp;
        var pain = GameManager.Instance.Pain;
        var power = GameManager.Instance.Power;
        var stage = GameManager.Instance.Stage;
        var score = GameManager.Instance.Score;

        var setHp = setPlayerHpSlider.value;
        var setPain = setPlayerPainSlider.value;

        hpText.text = $"{hp}%";
        painText.text = $"{pain}%";
        powerText.text = $"Power : {power}";
        stageText.text = $"Stage {stage} : {(stage == 1 ? "Vein" : "Brain")}";
        scoreText.text = $"Score : {score}";

        setPlayerHpText.text = $"{setHp}%";
        setPlayerPainText.text = $"{setPain}%";

        playerHpSlider.value = GameManager.Instance.Hp;
        playerPainSlider.value = GameManager.Instance.Pain;

        if (BossHp.gameObject.activeSelf && CurrentBoss != null)
        {
            BossHp.value = CurrentBoss.Hp / BossMaxHp * 100;
            CurrentBossText.text = $"{CurrentBoss.Hp} / {BossMaxHp}";
        }
        else
        {
            BossHp.gameObject.SetActive(false);
        }
    }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }

        Instance = this;

        leaderBoardPage.SetActive(false);
        scoreInputPage.SetActive(false);
        BossHp.gameObject.SetActive(false);
    }
}
