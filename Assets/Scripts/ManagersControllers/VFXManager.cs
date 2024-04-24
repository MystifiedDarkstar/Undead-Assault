using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private GameObject m_ExplosionPrefab;

    public static VFXManager instance;
    private void Awake()
    {
        instance = this;

       
    }
    public static GameObject CreateExplosion(Vector3 position, float deathTime = 0.5f) {

        if (instance == null) {

            Debug.LogError("Tried to spawn explosion, But instance hasn't been set!");
            return null;
        }

        GameObject explosion = Instantiate(instance.m_ExplosionPrefab, position, Quaternion.identity);

        Destroy(explosion , deathTime);

        return explosion;
    }
}
