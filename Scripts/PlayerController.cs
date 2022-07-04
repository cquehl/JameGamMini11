/* Copyright by: Cory Wolf */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerController : MonoBehaviour
{

    private float yaw, pitch;
    private bool isGrounded;
    private bool isCrouching;
    private float defaultScale;
    private float defaultmoveSpeed;
    [HideInInspector]
    public Rigidbody rb;
    private KeyCode crouchKey;
    private KeyCode jumpKey;

    [SerializeField] private float moveSpeed = 5f, sens = 2f;
    [SerializeField] private float maxY = 90f, minY = -90f;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private Transform orientation;
    public bool isFps = true;

    [Header("Crouching")]
    [SerializeField] private float crouchScale;
    [SerializeField] private float crouchSpeed;
    [SerializeField] private float airMoveSpeed;
    [SerializeField] private bool canCrouch;
    [SerializeField] private bool canJump;
    [HideInInspector]
    public bool stopCamera;


    public static PlayerController instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        Settings();
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        defaultScale = transform.localScale.y;
        defaultmoveSpeed = moveSpeed;
        rb.freezeRotation = true;
    }

    void Settings()
    {
        sens = SettingsManager.instance.mouseSens;
        this.crouchKey = SettingsManager.instance.crouchKey;
        this.jumpKey = SettingsManager.instance.jumpKey;
    }


    void Update()
    {
        if(!stopCamera)
        {
            Look();
            if (canJump)
                Jumping();
            if (canCrouch)
                Crouching();
            SpeedCheck();
        }
    }

    private void FixedUpdate()
    {
        if (isFps && !stopCamera)
            Movement();
    }

    void Look()
    {
        pitch -= Input.GetAxisRaw("Mouse Y") * sens;
        pitch = Mathf.Clamp(pitch, minY, maxY);
        yaw += Input.GetAxisRaw("Mouse X") * sens;
        Camera.main.transform.localRotation = Quaternion.Euler(pitch, yaw, 0);
        orientation.localRotation = Quaternion.Euler(0, yaw, 0);
    }

    void Movement()
    {
        //GetInputs
        Vector2 axis = new Vector2(Input.GetAxis("Vertical"), Input.GetAxis("Horizontal")) * moveSpeed;
        Vector3 forward = new Vector3(-Camera.main.transform.right.z, 0f, Camera.main.transform.right.x);
        //Move

        Vector3 wishDirection = (forward * axis.x + Camera.main.transform.right * axis.y + Vector3.up * rb.velocity.y);
        rb.velocity = wishDirection;
    }

    void Jumping()
    {
        if (Physics.Raycast(rb.transform.position, Vector3.down, 1 + 0.001f))
        {
            isGrounded = true;
            if (Input.GetKey(jumpKey))
            {
                rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            }
        }
        else isGrounded = false;
    }

    void Crouching()
    {
        if (Input.GetKey(crouchKey))
        {
            //Start Crouch
            isCrouching = true;
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, crouchScale, transform.localScale.z), crouchSpeed * Time.deltaTime);
        }
        else
        {
            //EndCrouch
            isCrouching = false;
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(transform.localScale.x, defaultScale, transform.localScale.z), crouchSpeed * Time.deltaTime);
        }
    }

    void SpeedCheck()
    {
        if (!isGrounded || isCrouching)
        {
            moveSpeed = airMoveSpeed;
        }
        else moveSpeed = defaultmoveSpeed;
    }

}