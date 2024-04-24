using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CoinLoot : MonoBehaviour
{
    public UnityEvent UE_GivePlayerScore;

    [SerializeField] private string m_objectiveID;
    private float m_lifespan = 10f;
    private void Awake()
    {
        UE_GivePlayerScore.AddListener(delegate { GameObject.FindObjectOfType<GameStats>().addScore(100); });
        UE_GivePlayerScore.AddListener(GameObject.FindObjectOfType<UIController>().UpdateScoreUI);
        UE_GivePlayerScore.AddListener(delegate { GameObject.FindObjectOfType<QuestController>().UpdateActiveObjective(m_objectiveID, 1); });
    }
    private void Update()
    {
        m_lifespan -= Time.deltaTime;
        if (m_lifespan <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            AudioManager.instance.PlayAudioClip(6);
            UE_GivePlayerScore.Invoke();
            Destroy(gameObject);// destroy
        }
    }
}
