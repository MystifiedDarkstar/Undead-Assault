using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PerkController : MonoBehaviour
{
    private GameStats m_gameStatsREF;

    private int m_maxPerkLevel = 5;
    private int m_perkCost = 2500;

    public int m_maxHealthLevel = 0;
    public int m_quickHealLevel = 0;
    public int m_movementSpeedLevel = 0;
    public int m_reloadSpeedLevel = 0;
    public int m_bulletDamageLevel = 0;

    private int m_currentPerkIndex;

    public UnityEvent UE_UpdateMaxHealth;
    public UnityEvent UE_UpdateQuickHeal;
    public UnityEvent UE_UpdateMovementSpeed;
    public UnityEvent UE_UpdateReloadSpeed;
    public UnityEvent UE_UpdateBulletDamage;
    public UnityEvent UE_MaxPerkReached;
    public UnityEvent UE_UpdateAllPerks;

    private void Awake()
    {
        m_gameStatsREF = GameObject.FindObjectOfType<GameStats>();

        UE_UpdateMaxHealth.AddListener(delegate { GameObject.FindObjectOfType<PerkUIController>().UpdatePerkLevel(m_currentPerkIndex, m_maxHealthLevel); });
        UE_UpdateMaxHealth.AddListener(delegate { m_gameStatsREF.removeScore(m_perkCost); });
        UE_UpdateMaxHealth.AddListener(GameObject.FindObjectOfType<UIController>().UpdateScoreUI);
        UE_UpdateMaxHealth.AddListener(delegate { GameObject.FindObjectOfType<PlayerHealthSystem>().MaxHealthPerkChanged(m_maxHealthLevel); });
        UE_UpdateMaxHealth.AddListener(delegate { AudioManager.instance.PlayAudioClip(5); });

        UE_UpdateQuickHeal.AddListener(delegate { GameObject.FindObjectOfType<PerkUIController>().UpdatePerkLevel(m_currentPerkIndex, m_quickHealLevel); });
        UE_UpdateQuickHeal.AddListener(delegate { m_gameStatsREF.removeScore(m_perkCost); });
        UE_UpdateQuickHeal.AddListener(GameObject.FindObjectOfType<UIController>().UpdateScoreUI);
        UE_UpdateQuickHeal.AddListener(delegate { GameObject.FindObjectOfType<PlayerHealthSystem>().QuickHealPerkChanged(m_quickHealLevel); });
        UE_UpdateQuickHeal.AddListener(delegate { AudioManager.instance.PlayAudioClip(5); });

        UE_UpdateMovementSpeed.AddListener(delegate { GameObject.FindObjectOfType<PerkUIController>().UpdatePerkLevel(m_currentPerkIndex, m_movementSpeedLevel); });
        UE_UpdateMovementSpeed.AddListener(delegate { m_gameStatsREF.removeScore(m_perkCost); });
        UE_UpdateMovementSpeed.AddListener(GameObject.FindObjectOfType<UIController>().UpdateScoreUI);
        UE_UpdateMovementSpeed.AddListener(delegate { GameObject.FindObjectOfType<PlayerMovementController>().SpeedPerkChanged(m_movementSpeedLevel); });
        UE_UpdateMovementSpeed.AddListener(delegate { AudioManager.instance.PlayAudioClip(5); });

        UE_UpdateReloadSpeed.AddListener(delegate { GameObject.FindObjectOfType<PerkUIController>().UpdatePerkLevel(m_currentPerkIndex, m_reloadSpeedLevel); });
        UE_UpdateReloadSpeed.AddListener(delegate { m_gameStatsREF.removeScore(m_perkCost); });
        UE_UpdateReloadSpeed.AddListener(GameObject.FindObjectOfType<UIController>().UpdateScoreUI);
        UE_UpdateReloadSpeed.AddListener(delegate { GameObject.FindObjectOfType<PlayerWeaponSystem>().ReloadPerkChanged(m_reloadSpeedLevel); });
        UE_UpdateReloadSpeed.AddListener(delegate { AudioManager.instance.PlayAudioClip(5); });

        UE_UpdateBulletDamage.AddListener(delegate { GameObject.FindObjectOfType<PerkUIController>().UpdatePerkLevel(m_currentPerkIndex, m_bulletDamageLevel); });
        UE_UpdateBulletDamage.AddListener(delegate { m_gameStatsREF.removeScore(m_perkCost); });
        UE_UpdateBulletDamage.AddListener(GameObject.FindObjectOfType<UIController>().UpdateScoreUI);
        UE_UpdateBulletDamage.AddListener(delegate { GameObject.FindObjectOfType<PlayerWeaponSystem>().DamagePerkChanged(m_bulletDamageLevel); });
        UE_UpdateBulletDamage.AddListener(delegate { AudioManager.instance.PlayAudioClip(5); });

        UE_MaxPerkReached.AddListener(delegate { GameObject.FindObjectOfType<PerkUIController>().DisableBuyButton(m_currentPerkIndex); });
        UE_MaxPerkReached.AddListener(delegate { GameObject.FindObjectOfType<PerkUIController>().UpdatePerkNameText(m_currentPerkIndex); });

        UE_UpdateAllPerks.AddListener(delegate { GameObject.FindObjectOfType<PerkUIController>().UpdatePerkLevel(0, m_maxHealthLevel); });
        UE_UpdateAllPerks.AddListener(delegate { GameObject.FindObjectOfType<PlayerHealthSystem>().MaxHealthPerkChanged(m_maxHealthLevel); });
        UE_UpdateAllPerks.AddListener(delegate { CheckMaxPerks(0); });
        UE_UpdateAllPerks.AddListener(delegate { GameObject.FindObjectOfType<PerkUIController>().UpdatePerkLevel(1, m_quickHealLevel); });
        UE_UpdateAllPerks.AddListener(delegate { GameObject.FindObjectOfType<PlayerHealthSystem>().QuickHealPerkChanged(m_quickHealLevel); });
        UE_UpdateAllPerks.AddListener(delegate { CheckMaxPerks(1); });
        UE_UpdateAllPerks.AddListener(delegate { GameObject.FindObjectOfType<PerkUIController>().UpdatePerkLevel(2, m_movementSpeedLevel); });
        UE_UpdateAllPerks.AddListener(delegate { GameObject.FindObjectOfType<PlayerMovementController>().SpeedPerkChanged(m_movementSpeedLevel); });
        UE_UpdateAllPerks.AddListener(delegate { CheckMaxPerks(2); });
        UE_UpdateAllPerks.AddListener(delegate { GameObject.FindObjectOfType<PerkUIController>().UpdatePerkLevel(3, m_reloadSpeedLevel); });
        UE_UpdateAllPerks.AddListener(delegate { GameObject.FindObjectOfType<PlayerWeaponSystem>().ReloadPerkChanged(m_reloadSpeedLevel); });
        UE_UpdateAllPerks.AddListener(delegate { CheckMaxPerks(3); });
        UE_UpdateAllPerks.AddListener(delegate { GameObject.FindObjectOfType<PerkUIController>().UpdatePerkLevel(4, m_bulletDamageLevel); });
        UE_UpdateAllPerks.AddListener(delegate { GameObject.FindObjectOfType<PlayerWeaponSystem>().DamagePerkChanged(m_bulletDamageLevel); });
        UE_UpdateAllPerks.AddListener(delegate { CheckMaxPerks(4); });
    }

    public void IncreasePerkLevel(int index) 
    { 
        switch (index)
        {
            case 0:
                if (m_maxHealthLevel != m_maxPerkLevel && m_gameStatsREF.m_score >= m_perkCost)
                {
                    m_maxHealthLevel++;
                    m_currentPerkIndex = index;
                    UE_UpdateMaxHealth.Invoke();
                    if (m_maxHealthLevel == m_maxPerkLevel)
                    {
                        UE_MaxPerkReached.Invoke();
                    }
                }
                break;
            case 1:
                if (m_quickHealLevel != m_maxPerkLevel && m_gameStatsREF.m_score >= m_perkCost)
                {
                    m_quickHealLevel++;
                    m_currentPerkIndex = index;
                    UE_UpdateQuickHeal.Invoke();
                    if (m_quickHealLevel == m_maxPerkLevel)
                    {
                        UE_MaxPerkReached.Invoke();
                    }
                }
                break; 
            case 2:
                if (m_movementSpeedLevel != m_maxPerkLevel && m_gameStatsREF.m_score >= m_perkCost)
                {
                    m_movementSpeedLevel++;
                    m_currentPerkIndex = index;
                    UE_UpdateMovementSpeed.Invoke();
                    if (m_movementSpeedLevel == m_maxPerkLevel)
                    {
                        UE_MaxPerkReached.Invoke();
                    }
                }
                break; 
            case 3:
                if (m_reloadSpeedLevel != m_maxPerkLevel && m_gameStatsREF.m_score >= m_perkCost)
                {
                    m_reloadSpeedLevel++;
                    m_currentPerkIndex = index;
                    UE_UpdateReloadSpeed.Invoke();
                    if (m_reloadSpeedLevel == m_maxPerkLevel)
                    {
                        UE_MaxPerkReached.Invoke();
                    }
                }
                break;
            case 4:
                if (m_bulletDamageLevel != m_maxPerkLevel && m_gameStatsREF.m_score >= m_perkCost)
                {
                    m_bulletDamageLevel++;
                    m_currentPerkIndex = index;
                    UE_UpdateBulletDamage.Invoke();
                    if (m_bulletDamageLevel == m_maxPerkLevel)
                    {
                        UE_MaxPerkReached.Invoke();
                    }
                }
                break;
            default:
                break;
        }
    }
    private void CheckMaxPerks(int index) 
    {
        switch (index) 
        {
            case 0:
                if (m_maxHealthLevel == m_maxPerkLevel)
                {
                    m_currentPerkIndex = index;
                    UE_MaxPerkReached.Invoke();
                }
                break;
            case 1:
                if (m_quickHealLevel == m_maxPerkLevel)
                {
                    m_currentPerkIndex = index;
                    UE_MaxPerkReached.Invoke();
                }
                break;
            case 2:
                if (m_movementSpeedLevel == m_maxPerkLevel)
                {
                    m_currentPerkIndex = index;
                    UE_MaxPerkReached.Invoke();
                }
                break;
            case 3:
                if (m_reloadSpeedLevel == m_maxPerkLevel)
                {
                    m_currentPerkIndex = index;
                    UE_MaxPerkReached.Invoke();
                }
                break;
            case 4:
                if (m_bulletDamageLevel == m_maxPerkLevel)
                {
                    m_currentPerkIndex = index;
                    UE_MaxPerkReached.Invoke();
                }
                break;   
            default : 
                break;
        
        }
    }
}
