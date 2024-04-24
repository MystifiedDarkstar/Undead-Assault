using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private ObjectiveUI[] m_objectiveUIS;
    [SerializeField] private TMPro.TextMeshProUGUI m_questTitleText;

    private int m_currentActiveObjectiveCount;

    [SerializeField] private GameObject m_completeQuestButton;
    [SerializeField] private GameObject m_startQuestButton;

    private RectTransform m_QuestUIPanelRT;
    private void Start()
    {
        m_QuestUIPanelRT = gameObject.GetComponent<RectTransform>();
    }
    /// <summary>
    /// Update the current quantity in a specific objective UI, Usually called after an enemy is killed or something collected that is required for a current objective.
    /// </summary>
    /// <param name="objectiveIndex">The index of the objective your attempting to update, this can be passed from the objective array on the quest.</param>
    /// <param name="objectiveInfo">This is a reference to the objectiveType for the objective you are updating.</param>
    public void UpdateObjectiveUI(int objectiveIndex, ObjectiveType objectiveInfo)
    {
        m_objectiveUIS[objectiveIndex].UpdateCurrentQuantity(objectiveInfo.m_currentQuantity);
    }
    public void UpdateObjectiveComplete(int objectiveIndex)
    {
        m_objectiveUIS[objectiveIndex].ToggleComplete();
    }
    /// <summary>
    /// Updates the UI for a new quest once previous is complete, or sets the UI up for the first quest of the game.
    /// </summary>
    /// <param name="questInfo">Pass a reference to the new quest you wish to display on the UI.</param>
    public void SetQuestUI(QuestType questInfo)
    {
        ResetQuestUI();
        // set UI for every objective and turn it on
        for (int i = 0; i < questInfo.m_objectives.Length; i++)
        {
            m_objectiveUIS[i].InitialiseObjectiveUI(questInfo.m_objectives[i]);
        }
        // set quest title
        m_questTitleText.text = questInfo.m_questName;

        m_currentActiveObjectiveCount = questInfo.m_objectives.Length;
        SetQuestUIPanelHeight(m_currentActiveObjectiveCount);

        gameObject.SetActive(true);
    }
    public void ResetQuestUI()
    {
        for (int i = 0; i < m_objectiveUIS.Length; i++)
        {
            m_objectiveUIS[i].ResetObjectiveUI();
        }
        m_questTitleText.text = "";
    }
    public void ShowCompleteButton()
    {
        m_completeQuestButton.transform.SetSiblingIndex(m_currentActiveObjectiveCount); // sets correct heirachy position for complete button.
        SetQuestUIPanelHeight(m_currentActiveObjectiveCount + 1);
        m_completeQuestButton.SetActive(true);
    }
    public void HideCompleteButton()
    {
        m_completeQuestButton.transform.SetAsLastSibling(); // sets correct heirachy position for complete button.
        m_completeQuestButton.SetActive(false);
    }
    public void ShowStartQuestButton()
    {
        m_startQuestButton.transform.SetAsFirstSibling();
        SetQuestUIPanelHeight(1);
        m_questTitleText.text = "No Quest Started!";
        m_startQuestButton.SetActive(true);
    }
    public void HideStartButton()
    {
        m_startQuestButton.transform.SetAsLastSibling(); // sets correct heirachy position for complete button.
        m_startQuestButton.SetActive(false);
    }
    public void HideQuestUI() 
    { 
        gameObject.SetActive(false);  
    }
    private void SetQuestUIPanelHeight (int numberElements)
    {
        Vector2 currentPanelSize = new Vector2();
        currentPanelSize = m_QuestUIPanelRT.sizeDelta;
        currentPanelSize.y = (numberElements * 50) + 70;
        m_QuestUIPanelRT.sizeDelta = new Vector2(currentPanelSize.x, currentPanelSize.y);
    }
}
