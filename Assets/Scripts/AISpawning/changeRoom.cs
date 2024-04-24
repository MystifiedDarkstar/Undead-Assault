using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class changeRoom : MonoBehaviour
{
    private UIController UIREF;
    private RoomController m_roomControllerREF;
    public RoomController.m_rooms changeRoomTo;

    void Start()
    {
        m_roomControllerREF = GameObject.FindObjectOfType<RoomController>();
        UIREF = GameObject.FindObjectOfType<UIController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            m_roomControllerREF.m_currentRoom = changeRoomTo;
            UIREF.UpdateRoomUI();

        }
    }
}
