using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ObjectiveUI
{

    [SerializeField] private GameObject m_objectiveParent;
    [SerializeField] private TMPro.TextMeshProUGUI m_objectiveText;
    [SerializeField] private Toggle m_objectiveCheck;
    private ObjectiveType m_objectiveInfo;

    /// <summary>
    /// This sets up the objective UI the first time, and stores the objective Info so less info needs to be passed to update quantity.
    /// </summary>
    /// <param name="objectiveInfo">This is the objectiveType of the objective you want to store in this UI.</param>
    public void InitialiseObjectiveUI(ObjectiveType objectiveInfo)
    {
        if (objectiveInfo.m_currencyObjective)
        {
            m_objectiveInfo = objectiveInfo;
            m_objectiveText.text = "$" + m_objectiveInfo.m_currentQuantity + " / $" + m_objectiveInfo.m_maxQuantity + " " + m_objectiveInfo.m_objectiveTrackDescription;
            m_objectiveParent.SetActive(true);
        }
        else
        {
            m_objectiveInfo = objectiveInfo;
            m_objectiveText.text = m_objectiveInfo.m_currentQuantity + " / " + m_objectiveInfo.m_maxQuantity + "  " + m_objectiveInfo.m_objectiveTrackDescription;
            m_objectiveParent.SetActive(true);
        }
    }
    /// <summary>
    /// Resets UI fully and hides it until it is required for a new UI.
    /// </summary>
    public void ResetObjectiveUI()
    {
        m_objectiveParent.SetActive(false);
        m_objectiveCheck.isOn = false;
        m_objectiveText.text = "";
    }
    /// <summary>
    /// Update the current quantity for a objective by passing the new quantity.
    /// </summary>
    /// <param name="currentQuantity">This is the value of the new quantity for the quest objective.</param>
    public void UpdateCurrentQuantity(int currentQuantity)
    {
        if (m_objectiveInfo.m_currencyObjective)
        {
            m_objectiveText.text = "$" + currentQuantity + " / $" + m_objectiveInfo.m_maxQuantity + "  " + m_objectiveInfo.m_objectiveTrackDescription;
        }
        else
        {
            m_objectiveText.text = currentQuantity + " / " + m_objectiveInfo.m_maxQuantity + "  " + m_objectiveInfo.m_objectiveTrackDescription;
        }
    }
    public void ToggleComplete()
    {
        m_objectiveCheck.isOn = true;
    }
}
