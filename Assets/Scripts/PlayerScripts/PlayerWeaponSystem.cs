using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerWeaponSystem : MonoBehaviour
{
    private float m_reloadModifier = 1f;
    public float m_damageModifier { get; private set; } = 1f;

    [Header("Projectile parameters")]
    private float projectileSpeed = 25f; // Speed of the bullet when spawned4
    [SerializeField] public float projectileDamage { get; private set; } = 10f;
    [SerializeField] GameObject bulletPrefab; // reference to the bullet we want to spawn
    private Vector3 mouseLocation; // the current mouse direction compared to the player.


    [Header("Weapon parameters")]
    private float m_fireRate = 0.25f; // the speed at which the player can shoot
    [SerializeField] public int m_maxClipSize { get; private set; } = 25; // the amount of ammo in a singular clip before reload
    public int currentClip { get; private set; } // stores amount of ammo in current clip.
    [SerializeField] GameObject m_bulletHolder;
    private float m_reloadTime = 3f;
    private bool m_isPlayerReloading = false;

    [Header("Unity Events")]
    public UnityEvent UE_PlayerReload;
    public UnityEvent UE_PlayerFire;
    public UnityEvent UE_PlayerReloadUI;

    private string m_currentStateString;

    [SerializeField] private ParticleSystem m_weaponFired;

    private void Awake()
    {
        UE_PlayerReload.AddListener(Reload);
        UE_PlayerReload.AddListener(GameObject.FindObjectOfType<UIController>().UpdateAmmoCount);
        UE_PlayerReloadUI.AddListener(delegate { GameObject.FindObjectOfType<UIController>().UpdateWeaponState(m_currentStateString); });
        UE_PlayerReloadUI.AddListener(delegate { AudioManager.instance.PlayAudioClip(8); });

        UE_PlayerFire.AddListener(Fire);
        UE_PlayerFire.AddListener(GameObject.FindObjectOfType<UIController>().UpdateAmmoCount);
        UE_PlayerFire.AddListener(delegate { AudioManager.instance.PlayAudioClip(0); });
    }
    private void Start()
    {
        UE_PlayerReload.Invoke();
    }
    private void Update()
    {
        Vector3 mousePointOnScreen = Camera.main.ScreenToWorldPoint(Input.mousePosition); // get the world location of the mouse point and store it for later.
        mouseLocation = mousePointOnScreen - transform.position; // set the current mouse location

        m_fireRate -= Time.deltaTime; // reduce amount of time on firerate timer

        // Was the fire button pressed (mapped to Left mouse button or gamepad trigger)
        if (Input.GetButton("Fire1"))
        {
            // cehck if firerate cooldown is over, and if we have bullets to shoot
            if (m_fireRate <= 0 && currentClip > 0 && !m_isPlayerReloading)
            {
                //Shoot
                UE_PlayerFire.Invoke();
                //cehck if we need to reload
                if (currentClip <= 0)
                {
                    //reloading
                    UE_PlayerReload.Invoke();
                }
                else
                {
                    //dont need to reload, reset standard firerate timer
                    m_fireRate = 0.25f;
                }
            }
        }
        // Check if player requested to reload
        if (Input.GetKeyDown(KeyCode.R))
        {
            UE_PlayerReload.Invoke();
        }
    }
    /// <summary> When called checks if the player can fire, if so it fires a bullet in mouse direction. Then proceeds to check if player needs to reload. </summary>
    private void Fire()
    {
        GameObject bulletToSpawn = Instantiate(bulletPrefab, transform.position, Quaternion.identity, m_bulletHolder.transform);
        m_weaponFired.Play();

        if (bulletToSpawn.GetComponent<Rigidbody2D>() != null)
        {
            bulletToSpawn.GetComponent<Rigidbody2D>().AddForce(mouseLocation.normalized * projectileSpeed, ForceMode2D.Impulse);
            bulletToSpawn.GetComponent<Bullet>().damageInflict = projectileDamage * m_damageModifier;
        }
        currentClip--;
    }
    /// <summary> When called reloads the players weapon. Prevents firing while reloading. </summary>
    private void Reload()
    {
        m_isPlayerReloading = true;
        m_currentStateString = "Reloading";
        UE_PlayerReloadUI.Invoke();
        StartCoroutine(PlayerReload());
    }


    /// <summary> Coroutine for player reload, will reload the weapon and then wait for the assigned reload time. </summary>
    IEnumerator PlayerReload()
    {
        currentClip = m_maxClipSize;
        yield return new WaitForSeconds(m_reloadTime);
        m_isPlayerReloading = false;
        m_currentStateString = "Ready To Fire!";
        UE_PlayerReloadUI.Invoke();
    }
    public void ReloadPerkChanged(int newLevel)
    {
        m_reloadModifier = 1 - (0.1f * newLevel);
    }
    public void DamagePerkChanged(int newLevel)
    {
        m_damageModifier = 1 + (0.2f * newLevel);
    }
}
