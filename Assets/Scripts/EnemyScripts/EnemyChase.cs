using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyChase : MonoBehaviour
{
    private Transform m_target;
    NavMeshAgent m_agent;


    private void Awake()
    {
        m_target = GameObject.FindGameObjectWithTag("Player").transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //SET TARGET DESTINATION
        if (m_target != null)
        {
            m_agent.SetDestination(m_target.position);

        }
        else if (m_target == null) 
        {
            m_target = GameObject.FindGameObjectWithTag("Player").transform;
        }



    }
   


}
