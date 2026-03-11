using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRB;
    private Vector3 inputDirection;

    private bool isInventoryOpened = false;

    [SerializeField] private GameObject inventory;
    [SerializeField] private float moveSpeed = 5f;

    void Awake()
    {
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
    }
    void Start()
    {
        inventory.gameObject.SetActive(false);
        isInventoryOpened = false;
    }

    void Update()
    {
        PlayerInput();

        if (!isInventoryOpened)
            PlayerMove();
    }

    void FixedUpdate()
    {
        playerRB.linearVelocity = new Vector3(inputDirection.x * moveSpeed, playerRB.linearVelocity.y, inputDirection.z * moveSpeed);
    }

    void PlayerMove()
    {
        inputDirection = Vector3.zero;

        if (Keyboard.current.wKey.isPressed)
            inputDirection += Vector3.forward;

        if (Keyboard.current.sKey.isPressed)
            inputDirection += Vector3.back;

        if (Keyboard.current.aKey.isPressed)
            inputDirection += Vector3.left;

        if (Keyboard.current.dKey.isPressed)
            inputDirection += Vector3.right;
    }

    void PlayerInput()
    {
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            isInventoryOpened = !isInventoryOpened;
            inventory.SetActive(isInventoryOpened);
        }
    }
}