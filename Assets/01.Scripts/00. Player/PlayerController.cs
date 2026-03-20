using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    private Rigidbody m_playerRB;
    private Vector3 m_inputDirection;

    private bool m_isInputBlocked = false;
    private bool m_isShopOpened = false;

    [SerializeField] private GameObject m_shop;

    [SerializeField] private float moveSpeed = 5f;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference toggleInventoryAction;
    [SerializeField] private InputActionReference toggleShopAction;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        m_playerRB = GetComponent<Rigidbody>();
    }

    public void SetInputBlock(bool isBlock)
    {
        m_isInputBlocked = isBlock;

        if (isBlock)
        {
            m_inputDirection = Vector3.zero;
        }
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
        InventoryUI.Instance?.gameObject.SetActive(false);

        if (m_shop != null)
        {
            m_shop.SetActive(false);
            m_isShopOpened = false;
        }
    }

    void Update()
    {
        if (m_isInputBlocked)
        {
            m_inputDirection = Vector3.zero;
            return;
        }

        Vector3 moveInput = moveAction.action.ReadValue<Vector2>();

        m_inputDirection = new Vector3(moveInput.x, 0f, moveInput.y);
    }

    void FixedUpdate()
    {
        m_playerRB.linearVelocity = new Vector3(m_inputDirection.x * moveSpeed, m_playerRB.linearVelocity.y, m_inputDirection.z * moveSpeed);
    }

    void OnToggleInventory(InputAction.CallbackContext ctx)
    {
        InventoryManager.Instance?.OnToggleInventory(ctx);
    }

    void OnToggleShop(InputAction.CallbackContext ctx)
    {
        m_isShopOpened = !m_isShopOpened;
        m_shop.SetActive(m_isShopOpened);
    }
}