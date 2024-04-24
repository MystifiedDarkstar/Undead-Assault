using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStats : MonoBehaviour
{
    [Header("PlayerStats")]
    [SerializeField] private int m_playerDeaths;
    [SerializeField] private int m_playerRevives;

    [SerializeField] private int m_enemyKills;
    [SerializeField] public int m_score { get; private set;}
    [SerializeField] private int m_totalScore;
    [SerializeField] private int m_totalScoreSpent;

    [SerializeField] public int m_crystals { get; private set;}

    [Header("GameOverStatUI References")]
    [SerializeField] TMPro.TextMeshProUGUI  m_playerDeathStat;
    [SerializeField] TMPro.TextMeshProUGUI  m_playerReviveStat;
    [SerializeField] TMPro.TextMeshProUGUI  m_enemyKillsStat;
    [SerializeField] TMPro.TextMeshProUGUI  m_environmentKillsStat;
    [SerializeField] TMPro.TextMeshProUGUI m_playerTotalScoreStat;
    [SerializeField] TMPro.TextMeshProUGUI m_playerTotalSpentStat;

    private UIController m_UIControllerREF;
    private QuestController m_QuestControllerREF;

    [SerializeField] Camera m_camera;

    private void Awake()
    {
        m_UIControllerREF = GameObject.FindObjectOfType<UIController>();
        m_QuestControllerREF = GameObject.FindObjectOfType<QuestController>();  
    }
    public void checkTotalScoreObjectiveStart()
    {
        m_QuestControllerREF.UpdateActiveObjective("TotalScoreIncreased" , m_totalScore);
    }
    public void checkTotalSpentObjectiveStart()
    {
        m_QuestControllerREF.UpdateActiveObjective("ScoreSpent", m_totalScoreSpent);
    }
    public void checkTotalKillObjectiveStart()
    {
        m_QuestControllerREF.UpdateActiveObjective("TotalKill", m_enemyKills);
    }
    public void InitialiseStatsUI()
    {

        m_playerDeathStat.text = m_playerDeaths.ToString();
        m_playerReviveStat.text = m_playerRevives.ToString();
        m_enemyKillsStat.text = m_enemyKills.ToString();    
        m_environmentKillsStat.text = m_crystals.ToString();
        m_playerTotalScoreStat.text = m_totalScore.ToString();
        m_playerTotalSpentStat.text = m_totalScoreSpent.ToString();
        m_camera.GetComponent<Camera>().enabled = true; 
    }

    public void addPlayerDeaths(int l_amountToAdd)
    {
        m_playerDeaths += l_amountToAdd;
    }
    public void addPlayerRevives(int l_amountToAdd)
    {
        m_playerRevives += l_amountToAdd;
    }
    public void addEnemyKills(int l_amountToAdd)
    {
        m_enemyKills += l_amountToAdd;
        m_QuestControllerREF.UpdateActiveObjective("TotalKill", m_enemyKills);
    }
    public void addScore(int l_amountToAdd)
    {
        m_score += l_amountToAdd;
        UpdateTotalScore(l_amountToAdd);
        m_UIControllerREF.UpdateScoreUI();
    }
    public void removeScore(int l_amountToRemove)
    {
        m_score -= l_amountToRemove;
        addToTotalSpent(l_amountToRemove);
        m_UIControllerREF.UpdateScoreUI();
    }
    public void addCrystals(int l_amountToAdd)
    {
        m_crystals += l_amountToAdd;
    }
    private void UpdateTotalScore(int l_amountToAdd)
    {
        m_totalScore += l_amountToAdd;
        m_QuestControllerREF.UpdateActiveObjective("TotalScoreIncreased", l_amountToAdd);
    }
    public void addToTotalSpent(int l_amountToAdd)
    {
        m_totalScoreSpent += l_amountToAdd;
        m_QuestControllerREF.UpdateActiveObjective("ScoreSpent", l_amountToAdd);
    }
}
