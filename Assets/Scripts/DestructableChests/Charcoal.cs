using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Charcoal : MonoBehaviour
{
    private float m_CharcoalCooldown = 30f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            SetCharcoalInactive();
        }
    }
    private void Update()
    {
        m_CharcoalCooldown -= Time.deltaTime;
        if (m_CharcoalCooldown <= 0)
        {
            SetCharcoalActive();
        }
    }
    private void SetCharcoalInactive()
    {
        GameObject.FindObjectOfType<QuestController>().UpdateActiveObjective( "CharcoalCollect" , 1);
        AudioManager.instance.PlayAudioClip(4);
        m_CharcoalCooldown = 30f;
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = false;
    }
    private void SetCharcoalActive()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }
}
