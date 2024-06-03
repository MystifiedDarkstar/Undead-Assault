using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyHealthSystem : MonoBehaviour
{
    public UnityEvent UE_enemyDead;
    [SerializeField] private float m_currentHealth = 20f;
    [SerializeField] private int m_rewardScore;
    [SerializeField] private string m_objectiveID;
    [SerializeField] private GameObject m_hitPS;
    private void Awake()
    {

        UE_enemyDead.AddListener(delegate { GameObject.FindFirstObjectByType<GameStats>().addScore(m_rewardScore); });
        UE_enemyDead.AddListener(delegate { GameObject.FindFirstObjectByType<GameStats>().addEnemyKills(1); });
        UE_enemyDead.AddListener(GameObject.FindFirstObjectByType<AISpawnerController>().minusEnemyCountToKill);
        UE_enemyDead.AddListener(delegate { GameObject.FindFirstObjectByType<QuestController>().UpdateActiveObjective(m_objectiveID, 1); });
        UE_enemyDead.AddListener(GameObject.FindFirstObjectByType<UIController>().UpdateScoreUI);
        UE_enemyDead.AddListener(delegate {AudioManager.instance.PlayAudioClip(3); });
        UE_enemyDead.AddListener(delegate { VFXManager.CreateExplosion(gameObject.transform.position); });
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            GameObject newHitPS = Instantiate(m_hitPS, transform.position , Quaternion.identity, gameObject.transform);
            Destroy(collision.gameObject); // destroy the bullet that collided

            // deal damage to enemy
            m_currentHealth = m_currentHealth - (collision.gameObject.GetComponent<Bullet>().damageInflict);

            // check if ded
            if (m_currentHealth <= 0)
            {
                // if ded call death
                Death();
            }
        }
    }
    private void Death()
    {
        UE_enemyDead.Invoke();

        Destroy(gameObject); // destroy itself from the game
    }
}
