using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random; // this is used because "random" is used in multiple namespaces, this fully qualifies this specific the random case.


public class AISpawnerController : MonoBehaviour
{

    private RoomController roomControlREF;
    private QuestController m_QuestControllerREF;
    private UIController UIREF;

    [SerializeField] private List<GameObject> m_enemyPrefabs;
    [SerializeField] private List<GameObject> m_spawnPool;
    [SerializeField] private List<GameObject> m_batSpawnPool;
    [SerializeField] private GameObject m_bossPrefab;

    [SerializeField] public GameObject m_EnemyHolder;

    [SerializeField] private AISpawnRoomData[] m_rooms;

    private int m_spawnCount = 5;
    private int m_countToSpawn = 5;
    private int m_countToKill = 5;
    private int m_waveCount = 1;

    private float m_spawnDelay = 1.5f;

    private float m_waveDelayTimer = 10;
    private float m_maxWaveDelayTimer = 10;
    private bool m_isWaveBreak = true;
    private bool m_isGameOver = false;

    private bool m_BossNextWave = false;
    private bool m_BossWaveStarted = false;

    private void Awake()
    {

        roomControlREF = GameObject.FindAnyObjectByType<RoomController>();
        UIREF = GameObject.FindAnyObjectByType<UIController>();
        m_QuestControllerREF = GameObject.FindAnyObjectByType<QuestController>();

    }
    private void Start()
    {
        UIREF.UpdateWaveUI(m_waveCount);// Update wave count UI;
    }
    // Update is called once per frame
    private void Update()
    {
        // if no enemies left to kill we can end the current wave.
        if (m_countToKill == 0)
        {

            endWave();

        }
        // if wave break is active start reducing the wave delay timer
        if (m_isWaveBreak)
        {

            m_waveDelayTimer -= Time.deltaTime;

        }
        // if wave timer is up call start wave
        if (m_waveDelayTimer < 0)
        {
            startWave();

        }
    }

    private void startWave()
    {
        AudioManager.instance.PlayAudioClip(7);
        // reset wave downtime delay 
        m_isWaveBreak = false;
        m_waveDelayTimer = m_maxWaveDelayTimer;

        if (m_BossNextWave)
        {
            m_BossWaveStarted = true;
            GameObject enemy = Instantiate(m_bossPrefab, selectSpawnPoint(), Quaternion.identity, m_EnemyHolder.transform); // spawn boss
            m_countToSpawn--; // reduce count to spawn by 1
            StartCoroutine(SpawnEnemies());
        }
        else if (m_waveCount % 5 == 0)
        {
            //bat round
            StartCoroutine(SpawnBats());
        }
        else
        {
            //normal wave
            StartCoroutine(SpawnEnemies());
        }
    }
    private void endWave()
    {
        if (m_BossWaveStarted) 
        {
            // boss is dead and killed, quest will be complete
            if (!m_QuestControllerREF.m_allQuestsCompleted)
            {
                m_QuestControllerREF.m_finishGamePermission = true;
            }
            else
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthSystem>().UE_GameOver.Invoke();
            }
        }
        m_isWaveBreak = true; // start counting down the delay timer between waves of 10 seconds

        m_waveCount++; // increase the wave count number by 1
        UIREF.UpdateWaveUI(m_waveCount);// Update wave count UI;
        m_QuestControllerREF.UpdateActiveObjective("RoundSurvived", 1);
        // Set the number of enemys to spawn for the wave, the number left to kill, and the number left to spawn.
        m_spawnCount = Mathf.RoundToInt(m_spawnCount * 1.15f); // set the number of enemies to spawn to 25% more than previous wave. so 4 enemies becomes 5. roudn this to integer.
        m_countToSpawn = m_spawnCount; // set the amount left to spawn
        m_countToKill = m_spawnCount; // set the amount left for player to kill

        if (m_waveCount % 10 == 0 && m_enemyPrefabs.Count > 0) // wave is multiple of 10 check if we have new classes to add to spawn poool
        {
            m_spawnPool.Add(m_enemyPrefabs[0]);
            m_enemyPrefabs.RemoveAt(0);
        }
        else if (m_waveCount % 2 == 0 && m_spawnDelay > 1f)
        {
            m_spawnDelay -= 0.1f;
        }
        else if (m_BossNextWave)
        {
            m_spawnCount++;
            m_countToSpawn++;
            m_countToKill++;
        }
     }
    IEnumerator SpawnEnemies()
    {
        // iterate code m_spawnCount times.
        for (int i = 0; i < m_spawnCount; i++)
        {
            if (!m_isGameOver)
            {
                GameObject enemy = Instantiate(selectEnemyPrefab(), selectSpawnPoint(), Quaternion.identity, m_EnemyHolder.transform); // create a new enemy and create a reference to it
                m_countToSpawn--; // reduce count to spawn by 1
            }
            yield return new WaitForSeconds(m_spawnDelay); //wait to cycle next iteration for specififed amount of time
        }
    }
    IEnumerator SpawnBats()
    {
        // iterate code m_spawnCount times.
        for (int i = 0; i < m_spawnCount; i++)
        {
            if (!m_isGameOver)
            {
                GameObject enemy = Instantiate(m_batSpawnPool[0], selectSpawnPoint(), Quaternion.identity, m_EnemyHolder.transform); // create a new enemy and create a reference to it
                m_countToSpawn--; // reduce count to spawn by 1
            }
            yield return new WaitForSeconds(m_spawnDelay / 2); //wait to cycle next iteration for specififed amount of time
        }
    }
    private GameObject selectEnemyPrefab()
    {
        GameObject l_selectedEnemyPrefab; // create gameobject variable instance to return later.

        // generate random number from 0 to spawn pool length
        int l_enemyPrefabIndex = Random.Range(0, m_spawnPool.Count);

        //set local gameobject reference to the selected prefab
        l_selectedEnemyPrefab = m_spawnPool[l_enemyPrefabIndex];

        // return the gameobject reference. 
        return l_selectedEnemyPrefab;

    }
    private Vector3 selectSpawnPoint()
    {
        Vector3 l_spawnPos = new Vector3(); // create variable instance to store vector locally to return

        // get the current room from room controller
        int l_currentRoomIndex = (int)roomControlREF.m_currentRoom; // this returns the index of the current room state, this will be the same position as the room in the rooms array. 

        //check if that room has spawn points, maybe its a safe room? 
        if (m_rooms[l_currentRoomIndex].m_spawnPoints.Length > 0) {

            // if there are spawn points in this room we want to run the following code

            // generate a random index between 0 and the last index of the spawn point array for that room
            int l_spawnPointIndex = Random.Range(0, m_rooms[l_currentRoomIndex].m_spawnPoints.Length);

            // locate that spawn point and store the location as a vector 3
            l_spawnPos = m_rooms[l_currentRoomIndex].m_spawnPoints[l_spawnPointIndex].transform.position; 
        }
        return l_spawnPos; // return location of spawn point
    }
    public void minusEnemyCountToKill()
    {
        // enemy died reduce count to kill by 1 everytime this is called by an enemy death event.
        m_countToKill--;
    }
    public void DestroyAllEnemy() 
    {
        m_isGameOver = true;
        Destroy(m_EnemyHolder);
    }
    public void CheckWaveNumberObjective()
    {   
        m_QuestControllerREF.UpdateActiveObjective("RoundSurvived", m_waveCount);
    }
    public void SetBossNextActive()
    {
        m_BossNextWave = true;
    }
}
