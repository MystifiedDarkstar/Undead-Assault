using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Script References")]
    private PlayerWeaponSystem m_playerWeaponSystemREF;
    private RoomController m_roomControllerREF;
    private GameStats m_gameStatsREF;
    private PlayerHealthSystem m_PlayerHealthSystemREF;

    [Header("PlayerHealthUI")]
    [SerializeField] Slider m_healthBar;    
    [SerializeField] GameObject[] Lives;

    [Header("PlayerRoomUI")]
    [SerializeField]TMPro.TextMeshProUGUI m_roomLabel;

    [Header("ScoreUI")]
    [SerializeField] TMPro.TextMeshProUGUI m_scoreLabel;

    [Header("WaveUI")]
    [SerializeField] TMPro.TextMeshProUGUI m_waveLabel;

    [Header("Ammo UI")]
    [SerializeField] TMPro.TextMeshProUGUI m_currentAmmoLabel;
    [SerializeField] TMPro.TextMeshProUGUI m_maxAmmoLabel;
    [SerializeField] TMPro.TextMeshProUGUI m_weaponStateLabel;

    [Header("GameUI References")]
    [SerializeField] GameObject m_controlsPanel;
    bool m_isControlsPanelOpen = false;

    [SerializeField] GameObject m_menuPanel;
    bool m_isMenuPanelOpen = false;

    [SerializeField] GameObject m_gamePanel;
    bool m_isGamePanelOpen = true;

    [SerializeField] GameObject m_gameOverStatsPanel;
    bool m_isGameOverStatsPanelOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && m_isGamePanelOpen)
        {

            //player pressed escape and the game panel is up, we want to load Menu
            openAndCloseGamePanel();
            openAndCloseMenuPanel();
        }
    }
    public void InitialiseUI()
    {
        m_playerWeaponSystemREF = GameObject.FindObjectOfType<PlayerWeaponSystem>();
        m_PlayerHealthSystemREF = GameObject.FindObjectOfType<PlayerHealthSystem>();
        m_roomControllerREF = GameObject.FindObjectOfType<RoomController>();
        m_gameStatsREF = GameObject.FindObjectOfType<GameStats>();
        UpdateHealthUI();
        UpdateAmmoCount();
        UpdateLivesUI(GameObject.FindObjectOfType<gameManager>().m_PlayerLives);
    }

    public void UpdateHealthUI() 
    {
        m_healthBar.value = m_PlayerHealthSystemREF.m_PlayerHealth / m_PlayerHealthSystemREF.m_PlayerMaxHealth;
    }

    public void UpdateLivesUI(int newLivesCount)
    {
        for (int i = 0; i < Lives.Length; i++)
        {
            Lives[i].SetActive(false);
            if (i < newLivesCount)
            {
                Lives[i].SetActive(true);
            }
        }
    }
    public void UpdateScoreUI() 
    {
        m_scoreLabel.text = "$" + m_gameStatsREF.m_score;
    }

    public void UpdateRoomUI() 
    {
        switch (m_roomControllerREF.m_currentRoom)
        {
            case RoomController.m_rooms.Landing:
                m_roomLabel.text = "Landing";
                break;
            case RoomController.m_rooms.Camp:
                m_roomLabel.text = "Camp";
                break;
            case RoomController.m_rooms.Pond:
                m_roomLabel.text = "Pond";
                break;
            default:
                m_roomLabel.text = "None";
                break;
        }
    }

    public void UpdateWaveUI (int l_waveCount)
    {
        m_waveLabel.text = l_waveCount.ToString();
    }

    public void UpdateAmmoCount() 
    { 
        m_currentAmmoLabel.text = m_playerWeaponSystemREF.currentClip.ToString();
        m_maxAmmoLabel.text = " / " + m_playerWeaponSystemREF.m_maxClipSize.ToString();  
    }
    public void UpdateWeaponState(string currentState)
    {
        m_weaponStateLabel.text = currentState;
    }
    public void openAndCloseGamePanel()
    {
        m_isGamePanelOpen = !m_isGamePanelOpen; // set the bool to opposite value, if true set false, if false set true.
        m_gamePanel.SetActive(m_isGamePanelOpen); // set the panel to active/deactive depending on value of bool. true = active, false = deactive.
    }

    public void openAndCloseMenuPanel()
    {
        m_isMenuPanelOpen = !m_isMenuPanelOpen; // set the bool to opposite value, if true set false, if false set true.
        m_menuPanel.SetActive(m_isMenuPanelOpen); // set the panel to active/deactive depending on value of bool. true = active, false = deactive.
    }
    public void openAndCloseControlsPanel()
    {
        m_isControlsPanelOpen = !m_isControlsPanelOpen; // set the bool to opposite value, if true set false, if false set true.
        m_controlsPanel.SetActive(m_isControlsPanelOpen); // set the panel to active/deactive depending on value of bool. true = active, false = deactive.
    }
    public void openAndCloseGameOverStatsPanel()
    {
        m_isGameOverStatsPanelOpen = !m_isGameOverStatsPanelOpen; // set the bool to opposite value, if true set false, if false set true.
        m_gameOverStatsPanel.SetActive(m_isGameOverStatsPanelOpen); // set the panel to active/deactive depending on value of bool. true = active, false = deactive.
    }
}
