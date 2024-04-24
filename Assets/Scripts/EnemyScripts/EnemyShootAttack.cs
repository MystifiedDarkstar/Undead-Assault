using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShootAttack : MonoBehaviour
{
    private float m_projectileSpeed = 20f; // Speed of the bullet when spawned
    private float m_projectileDamage = 20f;
    private float m_fireRate = 1f; // the speed at which the player can shoot
    private int m_maxClipSize = 12; // the amount of ammo in a singular clip before reload
    private int m_currentClip;// stores amount of ammo in current clip.
    private float m_reloadTime = 6f;
    private bool m_isEnemyReloading = false;

    [SerializeField] GameObject bulletPrefab; // reference to the bullet we want to spawn
    [SerializeField] GameObject m_bulletHolder;
    private GameObject m_target;

    private Vector3 m_playerLocation;
    private void Start()
    {
        m_currentClip = m_maxClipSize;
    }
    // Update is called once per frame
    void Update()
    {
        if (m_target == null)
        {
            m_target = GameObject.FindGameObjectWithTag("Player");
        }
        m_fireRate -= Time.deltaTime;

        m_playerLocation = m_target.transform.position - transform.position;

        // cehck if firerate cooldown is over, and if we have bullets to shoot
        if (m_fireRate <= 0 && m_currentClip > 0 && !m_isEnemyReloading)
        {
            //Shoot
            Fire();
            //cehck if we need to reload
            if (m_currentClip <= 0)
            {
                //reloading
                Reload();
            }
            else
            {
                //dont need to reload, reset standard firerate timer
                m_fireRate = 1f;
            }
        }
    }
    /// <summary> When called checks if the player can fire, if so it fires a bullet in mouse direction. Then proceeds to check if player needs to reload. </summary>
    private void Fire()
    {
        GameObject bulletToSpawn = Instantiate(bulletPrefab, transform.position, Quaternion.identity, m_bulletHolder.transform);

        if (bulletToSpawn.GetComponent<Rigidbody2D>() != null)
        {
            bulletToSpawn.GetComponent<Rigidbody2D>().AddForce(m_playerLocation.normalized * m_projectileSpeed, ForceMode2D.Impulse);
            bulletToSpawn.GetComponent<Bullet>().damageInflict = m_projectileDamage;
        }
        m_currentClip--;
    }
    /// <summary> When called reloads the players weapon. Prevents firing while reloading. </summary>
    private void Reload()
    {
        m_isEnemyReloading = true;
        StartCoroutine(EnemyReload());
    }
    /// <summary> Coroutine for player reload, will reload the weapon and then wait for the assigned reload time. </summary>
    IEnumerator EnemyReload()
    {
        m_currentClip = m_maxClipSize;
        yield return new WaitForSeconds(m_reloadTime);
        m_isEnemyReloading = false;
    }
}
