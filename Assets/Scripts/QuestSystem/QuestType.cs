using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[Serializable]
public class QuestType
{
    public string m_questName;
    public ObjectiveType[] m_objectives;
    public int m_completedObjectives;
    public bool m_isCompleted = false;
    public UnityEvent m_onStartEvent;
}
