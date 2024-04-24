using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerHealthSystem : MonoBehaviour
{
    [field: Header("Health parameters")]
    public float m_PlayerHealth { get; private set; } = 100;
    public float m_PlayerMaxHealth = 100;
    private float m_playerHealTime = 5f;
    private float m_currentHealTime;

    private bool m_isDead;

    private float m_quickHealModifier = 1f;

    public UnityEvent UE_PlayerDead;
    public UnityEvent UE_GameOver;
    public UnityEvent UE_PlayerRespawn;
    public UnityEvent UE_PlayerHeal;
    public UnityEvent UE_PlayerDamage;

    private gameManager m_gameManagerREF;

    private void Awake()
    {
        m_gameManagerREF = GameObject.FindObjectOfType<gameManager>();

        UE_GameOver.AddListener(GameObject.FindObjectOfType<UIController>().openAndCloseGamePanel);
        UE_GameOver.AddListener(GameObject.FindObjectOfType<UIController>().openAndCloseGameOverStatsPanel);
        UE_GameOver.AddListener(GameObject.FindObjectOfType<GameStats>().InitialiseStatsUI);
        UE_GameOver.AddListener(GameObject.FindObjectOfType<AISpawnerController>().DestroyAllEnemy);
        UE_GameOver.AddListener(delegate { Destroy(gameObject); });

        UE_PlayerRespawn.AddListener(GameObject.FindObjectOfType<gameManager>().RespawnPlayer);
        UE_PlayerRespawn.AddListener(delegate { Destroy(gameObject); });

        UE_PlayerDead.AddListener(delegate { GameObject.FindObjectOfType<gameManager>().m_PlayerLives--; });
        UE_PlayerDead.AddListener(delegate { GameObject.FindObjectOfType<UIController>().UpdateLivesUI(GameObject.FindObjectOfType<gameManager>().m_PlayerLives); });
        UE_PlayerDead.AddListener(Death);
        UE_PlayerDead.AddListener(delegate { GameObject.FindObjectOfType<GameStats>().addPlayerDeaths(1); });

        UE_PlayerHeal.AddListener(PlayerHeal);
        UE_PlayerHeal.AddListener(GameObject.FindObjectOfType<UIController>().UpdateHealthUI);

        UE_PlayerDamage.AddListener(GameObject.FindObjectOfType<UIController>().UpdateHealthUI);
    }
    private void Start()
    {
        m_PlayerHealth = m_PlayerMaxHealth;
        m_currentHealTime = m_playerHealTime;
    }
    private void Update()
    {
        m_currentHealTime -= Time.deltaTime;
        // Check if the player can heal
        if (m_currentHealTime <= 0)
        {
            UE_PlayerHeal.Invoke(); // player can heal        
        }
    }
    /// <summary> When called checks if the player has lives to respawn, or whether game is over. </summary>
    private void Death()
    {
        m_isDead = true;
        // players dead
        if (m_gameManagerREF.m_PlayerLives == 0)
        {
            UE_GameOver.Invoke(); // load gameover scene and gameover UI
        }
        else if (m_gameManagerREF.m_PlayerLives >= 1)
        {
            UE_PlayerRespawn.Invoke(); // respawn player and continue gameplay  
        }
    }
    /// <summary> When called it resets player health to max, the player has healed. </summary>
    private void PlayerHeal()
    {
        m_PlayerHealth = m_PlayerMaxHealth;
    }
    /// <summary> When called deals damage to the player, damage is specified by the passed float parameter. </summary>
    public void DealDamage(float l_Damage)
    {
        if (!m_isDead)
        {
            AudioManager.instance.PlayAudioClip(2);
            m_currentHealTime = m_playerHealTime;
            m_PlayerHealth = m_PlayerHealth - l_Damage; // deal damage to player
                                                        // check if player dead
            if (m_PlayerHealth <= 0)
            {
                UE_PlayerDead.Invoke();
            }
        }
    }
    public void MaxHealthPerkChanged(int newLevel)
    {
        m_PlayerMaxHealth = 100 + (20 * newLevel);
    }
    public void QuickHealPerkChanged(int newLevel)
    {
        m_quickHealModifier = 1 - (0.1f * newLevel);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet")) 
        {
            DealDamage(collision.GetComponent<Bullet>().damageInflict);
            UE_PlayerDamage.Invoke();
            Destroy(collision.gameObject);
        }
    }
}
