using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LifeHeart : MonoBehaviour
{
    public UnityEvent UE_GivePlayerLife;
    private gameManager m_gameManagerREF;
    [SerializeField] private string m_objectiveID;

    private float m_lifespan = 10f;
    private void Awake()
    {
        m_gameManagerREF = GameObject.FindObjectOfType<gameManager>();

        UE_GivePlayerLife.AddListener(delegate { m_gameManagerREF.m_PlayerLives++; });
        UE_GivePlayerLife.AddListener(delegate { GameObject.FindObjectOfType<UIController>().UpdateLivesUI(m_gameManagerREF.m_PlayerLives); });

        UE_GivePlayerLife.AddListener(delegate { GameObject.FindObjectOfType<QuestController>().UpdateActiveObjective(m_objectiveID, 1); });
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
            // give life to player
            AudioManager.instance.PlayAudioClip(6);
            if (m_gameManagerREF.m_PlayerLives != m_gameManagerREF.m_PlayerMaxLives) 
            {
                UE_GivePlayerLife.Invoke();
            }
            Destroy(gameObject);// destroy
        }
    }
}
