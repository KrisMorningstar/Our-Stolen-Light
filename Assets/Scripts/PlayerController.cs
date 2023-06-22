using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private float speed = 9f;
    public bool isStealthed;
    public Material stealthMaterial;
    public Material normalMaterial;

    private PlayerInputActions playerInputActions;
    private InputAction movement;
    private InputAction move;
    private InputAction jump;
    private InputAction strike;
    private InputAction interact;
    private InputAction shoot;
    private InputAction draw;   
    private InputAction turning;

    public GameObject lightDetector;

    private GameObject mainCamera;
    public GameObject arms;

    private Vector2 mValue;
    private Vector2 tValue;
    private Rigidbody rb;
    private Vector3 localVelocity;
    public float jumpForce;

    private Animation punch;
    public GameObject animObject;

    public LayerMask enemyMask;

    private bool isGrounded;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        movement = playerInputActions.Player.Movement;
        movement.Enable();
        move = playerInputActions.Player.Move;
        move.Enable();
        jump = playerInputActions.Player.Jump;
        jump.Enable();
        strike = playerInputActions.Player.Strike;
        strike.Enable();
        interact = playerInputActions.Player.Interact;
        interact.Enable();
        shoot = playerInputActions.Player.Shoot;
        shoot.Enable();
        draw = playerInputActions.Player.Draw;
        draw.Enable();
        turning = playerInputActions.Player.Turning;
        turning.Enable();

        rb = GetComponent<Rigidbody>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        //Cursor.lockState = CursorLockMode.Confined;
        //Cursor.visible = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        punch = animObject.GetComponent<Animation>();
        isStealthed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (jump.WasPressedThisFrame())
        {
            if (isGrounded)
            {
                rb.AddForce(Vector3.up * jumpForce);
            }
        }
        if (strike.WasPressedThisFrame())
        {
            punch.Play("PunchClip");
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                if (hit.collider.gameObject.layer == 9)
                {
                    hit.collider.gameObject.GetComponent<GuardController>().isStunned = true;
                }
            }
        }
        if (interact.WasPressedThisFrame())
        {
            punch.Play("PunchClip");
            RaycastHit hit;
            if(Physics.Raycast(transform.position, transform.forward, out hit, 2f))
            {
                Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.layer == 10)
                {
                    Debug.Log("hit it?");
                    //hit.collider.gameObject.GetComponent<GuardController>().isTaken = true;   
                    hit.collider.gameObject.SetActive(false);
                }
            }
        }

        if(!isStealthed && (lightDetector.GetComponent<DetectLight>().lightLevel <= 2))
        {
            isStealthed = true;
            arms.GetComponent<Renderer>().material.Lerp(arms.GetComponent<Renderer>().material, stealthMaterial,10);
        }
        else if(isStealthed && (lightDetector.GetComponent<DetectLight>().lightLevel >= 3))
        {
            isStealthed = false;
            arms.GetComponent<Renderer>().material.Lerp(arms.GetComponent<Renderer>().material, normalMaterial, 10);
        }
    }

    private void LateUpdate()
    {
        mValue = move.ReadValue<Vector2>().normalized;
        localVelocity = new Vector3(mValue.x * speed, rb.velocity.y, mValue.y * speed);
        if (isGrounded)
        {
            rb.velocity = transform.TransformDirection(localVelocity);
        }

        tValue = turning.ReadValue<Vector2>();
        rb.rotation = Quaternion.Euler(0, tValue.x/2, 0);

        mainCamera.transform.localRotation = Quaternion.Euler(-tValue.y/2, mainCamera.transform.localRotation.y, mainCamera.transform.localRotation.z);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Floor"))
        {
            isGrounded = false;
        }
    }
}
