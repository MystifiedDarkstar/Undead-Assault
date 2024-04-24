using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    [SerializeField] GameObject m_PlayerRespawn;
    [SerializeField] GameObject m_PlayerPrefab;
    public int m_PlayerLives = 5;
    public int m_PlayerMaxLives { get; private set; } = 5;

    private bool m_firstSpawn = true;

    private GameStats m_gameStatsREF;

    private void Awake()
    {
        m_gameStatsREF = GameObject.FindObjectOfType<GameStats>();
    }
    private void Start()
    {
        RespawnPlayer();
    } 

    public void RespawnPlayer()
    {
        GameObject playerToSpawn = Instantiate(m_PlayerPrefab, m_PlayerRespawn.transform.localPosition, Quaternion.identity);

        // This is triggered at the start of the game, check if it is the first spawn, if not increment the revive counter.
        if (!m_firstSpawn)
        {
            m_gameStatsREF.addPlayerRevives(1);
        }
        GameObject.FindObjectOfType<RoomController>().m_currentRoom = RoomController.m_rooms.Landing;
        m_firstSpawn = false;
    }
}
