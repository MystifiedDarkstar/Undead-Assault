using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PerkUI
{
    public GameObject[] m_perkLevels;
    public TMPro.TextMeshProUGUI m_perkName;
    public Button m_perkBuyButton;
    public string m_perkNameString;
}
