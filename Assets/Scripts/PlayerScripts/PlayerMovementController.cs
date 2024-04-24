using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovementController : MonoBehaviour
{
    #region Framework Stuff
    //Reference to attached animator
    private Animator animator;
    //Reference to attached rigidbody 2D
    private Rigidbody2D rb;
    //The direction the player is moving in
    private Vector2 playerDirection;
    //The speed at which they're moving
    private float playerSpeed = 1f;

    [Header("Movement parameters")]
    //The maximum speed the player can move
    [SerializeField] private float playerMaxSpeed = 100f;
    #endregion
    
    private float m_speedModifier = 1f;
    
    public UnityEvent UE_PlayerIntialise;


    /// <summary> When the script first initialises this gets called, use this for grabbing componenets </summary>
    private void Awake()
    {
        //Get the attached components so we can use them later
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        UE_PlayerIntialise.AddListener(GameObject.FindObjectOfType<UIController>().InitialiseUI);
        UE_PlayerIntialise.AddListener(GameObject.FindObjectOfType<PerkUIController>().InitialiseAllPerks);
    }
    /// <summary> Called after Awake(), and is used to initialize variables e.g. set values on the player </summary>
    private void Start()
    {
        UE_PlayerIntialise.Invoke();
    }
    /// <summary> When a fixed update loop is called, it runs at a constant rate, regardless of pc perfornamce so physics can be calculated properly </summary>
    private void FixedUpdate()
    { 
        //Set the velocity to the direction they're moving in, multiplied
        //by the speed they're moving
        rb.velocity = playerDirection.normalized * (playerSpeed * playerMaxSpeed * m_speedModifier) * Time.fixedDeltaTime;
    }
    /// <summary> When the update loop is called, it runs every frame, ca run more or less frequently depending on performance. Used to catch changes in variables or input. </summary>
    private void Update()
    {
        // read input from WASD keys
        playerDirection.x = Input.GetAxis("Horizontal");
        playerDirection.y = Input.GetAxis("Vertical");

        // check if there is some movement direction, if there is something, then set animator flags and make speed = 1
        if (playerDirection.magnitude != 0)
        {
            animator.SetFloat("Horizontal", playerDirection.x);
            animator.SetFloat("Vertical", playerDirection.y);
            animator.SetFloat("Speed", playerDirection.magnitude);

            //And set the speed to 1, so they move!
            playerSpeed = 1f;
        }
        else
        {
            //Was the input just cancelled (released)? If so, set
            //speed to 0
            playerSpeed = 0f;
            //Update the animator too, and return
            animator.SetFloat("Speed", 0);
        }
    }
    public void SpeedPerkChanged(int newLevel)
    {
        m_speedModifier = 1 + (0.15f * newLevel);
    }
}
