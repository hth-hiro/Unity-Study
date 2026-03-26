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
    private bool m_isGrounded;

    [SerializeField] private float m_moveSpeed = 10f;
    [SerializeField] private float m_jumpForce = 5f;

    [Header("FPS Look Settings")]
    [SerializeField] private InputActionReference m_lookAction;
    [SerializeField] private Transform m_cameraTransform;
    [SerializeField] private float m_mouseSensitivity = 0.2f;

    private float m_xRotation = 0f;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference m_moveAction;
    [SerializeField] private InputActionReference m_toggleInventoryAction;
    [SerializeField] private InputActionReference m_toggleShopAction;
    [SerializeField] private InputActionReference m_jumpAction;

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

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // 이벤트 등록
    void OnEnable()
    {
        m_moveAction.action.Enable();
        m_jumpAction.action.Enable();
        m_lookAction.action.Enable();

        m_jumpAction.action.performed += OnJump;

        m_skillEAction.action.Enable();
        m_skillShiftAction.action.Enable();

        m_skillEAction.action.performed += OnSkillE;
        m_skillShiftAction.action.performed += OnSkillShift;

        m_toggleInventoryAction.action.Enable();
        m_toggleInventoryAction.action.performed += OnToggleInventory;

        m_toggleShopAction.action.Enable();
        m_toggleShopAction.action.performed += OnToggleShop;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 이벤트 제거 (중복 방지)
    void OnDisable()
    {
        m_toggleInventoryAction.action.performed -= OnToggleInventory;
        m_toggleShopAction.action.performed -= OnToggleShop;
        m_skillEAction.action.performed -= OnSkillE;
        m_skillShiftAction.action.performed -= OnSkillShift;

        m_jumpAction.action.performed -= OnJump;

        m_moveAction.action.Disable();
        m_jumpAction.action.Disable();
        m_lookAction.action.Disable();

        m_toggleInventoryAction.action.Disable();
        m_toggleShopAction.action.Disable();
        m_skillEAction.action.Disable();
        m_skillShiftAction.action.Disable();
    }

    void Start()
    {
        //InventoryUI.Instance?.gameObject.SetActive(false);
        //ShopUI.Instance?.gameObject.SetActive(false);
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

        Vector2 lookInput = m_lookAction.action.ReadValue<Vector2>();

        float mouseX = lookInput.x * m_mouseSensitivity;
        float mouseY = lookInput.y * m_mouseSensitivity;

        m_xRotation -= mouseY;
        m_xRotation = Mathf.Clamp(m_xRotation, -90f, 90f);

        m_cameraTransform.localRotation = Quaternion.Euler(m_xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void FixedUpdate()
    {
        Vector3 moveDir = transform.TransformDirection(m_inputDirection);

        m_playerRB.linearVelocity = new Vector3(moveDir.x * m_moveSpeed, m_playerRB.linearVelocity.y, moveDir.z * m_moveSpeed);
    }

    private void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y > 0.7f)
            {
                m_isGrounded = true;
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        m_isGrounded = false;
    }

    public void OnJump(InputAction.CallbackContext ctx)
    {
        if (m_isInputBlocked) return;

        if (m_isGrounded)
        {
            m_playerRB.AddForce(Vector3.up * m_jumpForce, ForceMode.VelocityChange);
        }
    }

    void OnToggleInventory(InputAction.CallbackContext ctx)
    {
        //InventoryManager.Instance?.OnToggleInventory(ctx);
    }

    void OnToggleShop(InputAction.CallbackContext ctx)
    {
        if (!ctx.performed) return;

        UIManager.Instance?.ToggleShop();
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