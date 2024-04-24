using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ObjectiveType
{

    public string m_objectiveName;
    public string m_objectiveTrackDescription;
    public string m_objectiveID;
    public int m_currentQuantity;
    public int m_maxQuantity;
    public bool m_currencyObjective;
    public bool m_isComplete = false;

}
