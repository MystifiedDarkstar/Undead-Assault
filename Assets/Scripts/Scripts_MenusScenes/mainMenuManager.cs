using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mainMenuManager : MonoBehaviour
{

    public GameObject m_controlsPanel;
    bool m_isControlsPanelOpen = false;

    public GameObject m_menuPanel;
    bool m_isMenuPanelOpen = true;

    public void openAndCloseMenuPanel() {

        m_isMenuPanelOpen = !m_isMenuPanelOpen; // set the bool to opposite value, if true set false, if false set true.
        m_menuPanel.SetActive(m_isMenuPanelOpen); // set the panel to active/deactive depending on value of bool. true = active, false = deactive.
    
    }
    public void openAndCloseControlsPanel() {

        m_isControlsPanelOpen = !m_isControlsPanelOpen; // set the bool to opposite value, if true set false, if false set true.
        m_controlsPanel.SetActive(m_isControlsPanelOpen); // set the panel to active/deactive depending on value of bool. true = active, false = deactive.

    }
}
