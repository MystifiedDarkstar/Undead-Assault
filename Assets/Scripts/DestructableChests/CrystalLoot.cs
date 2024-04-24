using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CrystalLoot : MonoBehaviour
{
    public UnityEvent UE_GivePlayerCrystals;
    [SerializeField] private string m_objectiveID;
    private float m_lifespan = 10f;
    private void Awake()
    {
        UE_GivePlayerCrystals.AddListener(delegate { GameObject.FindObjectOfType<GameStats>().addCrystals(1); });
        UE_GivePlayerCrystals.AddListener(delegate { GameObject.FindObjectOfType<QuestController>().UpdateActiveObjective(m_objectiveID, 1); });
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
            AudioManager.instance.PlayAudioClip(4);
            UE_GivePlayerCrystals.Invoke();
            Destroy(gameObject);// destroy
        }
    }
}
