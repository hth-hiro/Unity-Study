using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    private Rigidbody m_playerRB;
    private Vector3 m_inputDirection;

    private bool m_isInventoryOpened = false;
    private bool m_isShopOpened = false;

    [SerializeField] private GameObject m_inventory;
    [SerializeField] private GameObject m_shop;

    [SerializeField] private float moveSpeed = 5f;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference toggleInventoryAction;
    [SerializeField] private InputActionReference toggleShopAction;

    void Awake()
    {
        m_playerRB = GameObject.Find("Player").GetComponent<Rigidbody>();
    }

    // 이벤트 등록
    void OnEnable()
    {
        moveAction.action.Enable();
        toggleInventoryAction.action.Enable();
        toggleShopAction.action.Enable();

        toggleInventoryAction.action.performed += OnToggleInventory;
        toggleShopAction.action.performed += OnToggleShop;
    }

    // 이벤트 제거 (중복 방지)
    void OnDisable()
    {
        toggleInventoryAction.action.performed -= OnToggleInventory;
        toggleShopAction.action.performed -= OnToggleShop;

        moveAction.action.Disable();
        toggleInventoryAction.action.Disable();
        toggleShopAction.action.Disable();
    }

    void Start()
    {
        if (m_inventory != null)
        {
            m_inventory.SetActive(false);
            m_isInventoryOpened = false;
        }

        if (m_shop != null)
        {
            m_shop.SetActive(false);
            m_isShopOpened = false;
        }
    }

    void Update()
    {
        Vector2 moveInput = moveAction.action.ReadValue<Vector2>();

        if (!m_isInventoryOpened)
            m_inputDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        else
            m_inputDirection = Vector3.zero;
    }

    void FixedUpdate()
    {
        m_playerRB.linearVelocity = new Vector3(m_inputDirection.x * moveSpeed, m_playerRB.linearVelocity.y, m_inputDirection.z * moveSpeed);
    }

    void OnToggleInventory(InputAction.CallbackContext ctx)
    {
        m_isInventoryOpened = !m_isInventoryOpened;
        m_inventory.SetActive(m_isInventoryOpened);
    }

    void OnToggleShop(InputAction.CallbackContext ctx)
    {
        m_isShopOpened = !m_isShopOpened;
        m_shop.SetActive(m_isShopOpened);
    }
}