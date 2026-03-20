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

    [SerializeField] private GameObject m_shop;

    [SerializeField] private float moveSpeed = 5f;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference m_moveAction;
    [SerializeField] private InputActionReference m_toggleInventoryAction;
    [SerializeField] private InputActionReference m_toggleShopAction;

    [Header("Skill Inputs")]
    [SerializeField] private InputActionReference m_skillEAction;
    [SerializeField] private InputActionReference m_skillShiftAction;

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
        m_moveAction.action.Enable();

        m_skillEAction.action.Enable();
        m_skillShiftAction.action.Enable();

        m_skillEAction.action.performed += OnSkillE;
        m_skillShiftAction.action.performed += OnSkillShift;

        m_toggleInventoryAction.action.Enable();
        m_toggleInventoryAction.action.performed += OnToggleInventory;

        m_toggleShopAction.action.Enable();
        m_toggleShopAction.action.performed += OnToggleShop;

    }

    // 이벤트 제거 (중복 방지)
    void OnDisable()
    {
        m_toggleInventoryAction.action.performed -= OnToggleInventory;
        m_toggleShopAction.action.performed -= OnToggleShop;
        m_skillEAction.action.performed -= OnSkillE;
        m_skillShiftAction.action.performed -= OnSkillShift;

        m_moveAction.action.Disable();
        m_toggleInventoryAction.action.Disable();
        m_toggleShopAction.action.Disable();
        m_skillEAction.action.Disable();
        m_skillShiftAction.action.Disable();
    }

    void Start()
    {
        InventoryUI.Instance?.gameObject.SetActive(false);

        if (m_shop != null)
        {
            m_shop.SetActive(false);
        }
    }

    void Update()
    {
        if (m_isInputBlocked)
        {
            m_inputDirection = Vector3.zero;
            return;
        }

        Vector3 moveInput = m_moveAction.action.ReadValue<Vector2>();

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
        ShopManager.Instance?.OnToggleShop(ctx);
    }

    public void OnSkillE(InputAction.CallbackContext ctx)
    {
        if (m_isInputBlocked) return;
        if (!ctx.performed) return;

        SkillManager.Instance?.UseSkill(0);
    }

    public void OnSkillShift(InputAction.CallbackContext ctx)
    {
        if (m_isInputBlocked) return;
        if (!ctx.performed) return;

        SkillManager.Instance?.UseSkill(1);
    }
}