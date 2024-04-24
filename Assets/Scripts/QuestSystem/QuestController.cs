using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class QuestController : MonoBehaviour
{
    [SerializeField] private List<QuestType> m_playerQuests;
    
    private List<QuestType> m_completedQuests = new List<QuestType>();
    private bool m_isQuestActive = false;
    private QuestType m_currentQuest;
    private int m_objectiveIndex = 1;

    public UnityEvent E_noQuestsRemaining;
    public UnityEvent E_setQuestUI;
    public UnityEvent E_resetQuestUI;
    public UnityEvent E_updateCurrentObjectiveUI;
    public UnityEvent E_updateObjectiveFinishedUI;
    public UnityEvent E_questComplete;

    public bool m_allQuestsCompleted = false;
    public bool m_finishGamePermission = false;
    private void Start()
    {
        E_setQuestUI.AddListener(delegate { GameObject.FindObjectOfType<QuestUI>().SetQuestUI(m_currentQuest); });

        E_updateCurrentObjectiveUI.AddListener(delegate { GameObject.FindObjectOfType<QuestUI>().UpdateObjectiveUI(m_objectiveIndex, m_currentQuest.m_objectives[m_objectiveIndex]); });
        E_updateCurrentObjectiveUI.AddListener(delegate { CheckObjectiveComplete(m_objectiveIndex); });

        E_updateObjectiveFinishedUI.AddListener(delegate { GameObject.FindObjectOfType<QuestUI>().UpdateObjectiveComplete(m_objectiveIndex); });
        E_updateObjectiveFinishedUI.AddListener(CheckQuestComplete);
    }
    public void StartNewQuest()
    {
        if (m_playerQuests.Count > 0)
        {
            // There is at least 1 quest remaining.
            m_currentQuest = m_playerQuests[0];
            m_isQuestActive = true;
            m_playerQuests.RemoveAt(0);
            E_setQuestUI.Invoke();
            m_currentQuest.m_onStartEvent.Invoke();
        }
        else
        {
            // No Quests Left, Close the UI Fully.
            E_noQuestsRemaining.Invoke();
        }
    }
    public void CompleteQuest()
    {
        AudioManager.instance.PlayAudioClip(6);
        m_completedQuests.Add(m_currentQuest);
        m_currentQuest = new QuestType();
        m_isQuestActive = false;
        
        if (m_playerQuests.Count > 0)
        {
            E_resetQuestUI.Invoke();
        }
        else
        {
            // No Quests Left, Close the UI Fully.
            if (m_finishGamePermission)
            {
                GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerHealthSystem>().UE_GameOver.Invoke();
            }
            m_allQuestsCompleted = true;
            E_noQuestsRemaining.Invoke();
        }
    }
    public void UpdateActiveObjective(string objectiveID , int amountToAdd)
    {
        // check if the ID is needed in any objective
        if (QueryObjectiveID(objectiveID))
        {
            // cycle through all objectives it is needed.
            for (int i = 0; i < m_currentQuest.m_objectives.Length; i++)
            {
                // check if its the correct objective that is needed
                if (m_currentQuest.m_objectives[i].m_objectiveID == objectiveID && !m_currentQuest.m_objectives[i].m_isComplete)
                {
                    m_objectiveIndex = i;
                    // increment the current quantity for quest
                    m_currentQuest.m_objectives[m_objectiveIndex].m_currentQuantity += amountToAdd;
                    E_updateCurrentObjectiveUI.Invoke();
                    break;
                }
            }
        }
    }
    public void CheckObjectiveComplete(int objectiveIndex)
    {
        if (m_currentQuest.m_objectives[objectiveIndex].m_currentQuantity >= m_currentQuest.m_objectives[objectiveIndex].m_maxQuantity)
        {
            m_currentQuest.m_objectives[objectiveIndex].m_isComplete = true;
            m_currentQuest.m_completedObjectives++;
            m_objectiveIndex = objectiveIndex;
            E_updateObjectiveFinishedUI.Invoke();
        }
    }
    void CheckQuestComplete()
    {
        if (m_currentQuest.m_completedObjectives == m_currentQuest.m_objectives.Length)
        {
            m_currentQuest.m_isCompleted = true;
            E_questComplete.Invoke();
        }
    }
    bool QueryObjectiveID(string objectiveID)
    {
        if (m_isQuestActive)
        {
            foreach (ObjectiveType objective in m_currentQuest.m_objectives)
            {
                if (objectiveID == objective.m_objectiveID)
                {
                    return true;
                }
            }
            return false;
        }
        return false;
    } 
}
