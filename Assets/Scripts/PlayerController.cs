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

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference toggleInventoryAction;

    void Awake()
    {
        playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    // 이벤트 등록
    void OnEnable()
    {
        moveAction.action.Enable();
        toggleInventoryAction.action.Enable();

        toggleInventoryAction.action.performed += OnToggleInventory;
    }

    // 이벤트 제거 (중복 방지)
    void OnDisable()
    {
        toggleInventoryAction.action.performed -= OnToggleInventory;

        moveAction.action.Disable();
        toggleInventoryAction.action.Disable();
    }

    void Start()
    {
        inventory.gameObject.SetActive(false);
        isInventoryOpened = false;
    }

    void Update()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        if (!isInventoryOpened)
            inputDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        else
            inputDirection = Vector3.zero;
    }

    void FixedUpdate()
    {
        playerRB.linearVelocity = new Vector3(inputDirection.x * moveSpeed, playerRB.linearVelocity.y, inputDirection.z * moveSpeed);
    }

    void OnToggleInventory(InputAction.CallbackContext ctx)
    {
        isInventoryOpened = !isInventoryOpened;
        inventory.SetActive(isInventoryOpened);
    }
}