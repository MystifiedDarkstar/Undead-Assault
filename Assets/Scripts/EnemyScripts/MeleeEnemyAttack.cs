using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class MeleeEnemyAttack : MonoBehaviour
{
    public UnityEvent playerDamage;

    private float m_damageRange = 3;
    [SerializeField] private float m_damage = 10;
    [SerializeField] private float m_fireRate = 1;

    private GameObject m_playerREF;
    private void Awake()
    {
        m_playerREF = GameObject.FindGameObjectWithTag("Player");
        playerDamage.AddListener(delegate { m_playerREF.GetComponent<PlayerHealthSystem>().DealDamage(m_damage); });
        playerDamage.AddListener(GameObject.FindFirstObjectByType<UIController>().UpdateHealthUI);
    }

    // Update is called once per frame
    void Update()
    {
        m_fireRate -= Time.deltaTime;
        if (m_playerREF == null)
        {
            m_playerREF = GameObject.FindGameObjectWithTag("Player");
        }
        //ATTACK
        if (Vector3.Distance(m_playerREF.transform.position, gameObject.transform.localPosition) <= m_damageRange && m_fireRate <= 0)
        {
            playerDamage.Invoke();
            m_fireRate = 1;
        }
    }
}
