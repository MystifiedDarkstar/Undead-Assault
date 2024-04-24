using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PerkUIController : MonoBehaviour
{
    [SerializeField] PerkUI[] PerksUI;

    public void DisableBuyButton(int perkIndex) 
    {
        PerksUI[perkIndex].m_perkBuyButton.interactable = false; //prevent interaction
        PerksUI[perkIndex].m_perkBuyButton.gameObject.GetComponent<Image>().color = new Color(0,0,0); // set image black
    }
    public void UpdatePerkNameText(int perkIndex)
    {
        PerksUI[perkIndex].m_perkName.text = PerksUI[perkIndex].m_perkNameString;
    }
    public void UpdatePerkLevel(int perkIndex, int newPerkLevel)
    {
        for (int i = 0; i < PerksUI[perkIndex].m_perkLevels.Length; i++)
        {
            if (i < newPerkLevel)
            {
                PerksUI[perkIndex].m_perkLevels[i].SetActive(true);
            }
            else
            {
                PerksUI[perkIndex].m_perkLevels[i].SetActive(false);
            }
        }
    }
    public void InitialiseAllPerks()
    {
        for (int i = 0; i < PerksUI.Length; i++)
        {
            PerksUI[i].m_perkName.text = PerksUI[i].m_perkNameString + " ($2500)";
            for (int x = 0; x < PerksUI[i].m_perkLevels.Length; x++)
            {
                PerksUI[i].m_perkLevels[x].SetActive(false);
            }
        }
        GameObject.FindObjectOfType<PerkController>().UE_UpdateAllPerks.Invoke();
    }
}
