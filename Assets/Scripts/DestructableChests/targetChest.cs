using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Random = UnityEngine.Random; // this is used because "random" is used in multiple namespaces, this fully qualifies this specific the random case.

public class targetChest : MonoBehaviour
{
    private GameStats m_gameStatsREF;
    public UnityEvent chestHit;

    public GameObject heartPrefab;
    public GameObject coinPrefab;
    public GameObject crystalPrefab;

    private bool m_isActive = true;
    private float m_ChestCooldown = 120f;
    private void Awake()
    {
        m_gameStatsREF = GameObject.FindObjectOfType<GameStats>();
        chestHit.AddListener(delegate { VFXManager.CreateExplosion(transform.position); });
        chestHit.AddListener(SpawnLoot);
        chestHit.AddListener(SetChestInactive);
    }
    private void Update()
    {
        m_ChestCooldown -= Time.deltaTime;

        if (m_ChestCooldown <= 0)
        {
            SetChestActive();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet") && m_isActive)
        {
            Destroy(collision.gameObject); // destroy the bullet that collided
            chestHit.Invoke();
        }
    }

    private void SpawnLoot() 
    {
        int lootChance =  Random.Range(0, 100);

        if (lootChance <= 10) 
        {
            GameObject LiveLoot = Instantiate(heartPrefab , transform.position, quaternion.identity);
        }
        else if (lootChance > 10 && lootChance <= 70) 
        {
            GameObject CoinLoot = Instantiate(coinPrefab , transform.position, quaternion.identity);
        }
        else if (lootChance > 70 && lootChance <= 100)
        {
            GameObject CrystalLoot = Instantiate(crystalPrefab, transform.position, quaternion.identity);
        }
    }
    private void SetChestInactive()
    {
        m_ChestCooldown = 120f;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
        m_isActive = false;
    } 
    private void SetChestActive()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        m_isActive = true;
    }
}
