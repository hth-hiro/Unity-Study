using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private const float MIN_MOUSE_SENSITIVITY = 0.001f;
    private const float SENSITIVITY_CONVERSION_SCALE = 0.01739f;

    public static PlayerController Instance { get; private set; }

    private Rigidbody m_playerRB;
    private Vector3 m_inputDirection;

    private bool m_isInputBlocked = false;
    private bool m_isGrounded;

    public bool IsInputBlocked { get { return m_isInputBlocked; } }
    public float MouseSensitivity { get { return m_mouseSensitivity; } }

    [SerializeField] private float m_moveSpeed = 10f;
    [SerializeField] private float m_jumpForce = 5f;

    [Header("FPS Look Settings")]
    [SerializeField] private InputActionReference m_look;
    [SerializeField] private Transform m_cameraTransform;
    [SerializeField] private float m_mouseSensitivity = 11.50f;

    private float m_xRotation = 0f;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference m_move;
    [SerializeField] private InputActionReference m_Inventory;
    [SerializeField] private InputActionReference m_jump;

    [Header("Skill Inputs")]
    [SerializeField] private InputActionReference m_skillE;
    [SerializeField] private InputActionReference m_skillShift;

    [Header("OnGui")]
    [SerializeField] private bool m_showSensitivityDebug = true;

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
        InitializeMouseSensitivity();
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
        m_move.action.Enable();
        m_jump.action.Enable();
        m_look.action.Enable();

        m_jump.action.performed += OnJump;

        m_skillE.action.Enable();
        m_skillShift.action.Enable();

        m_skillE.action.performed += OnSkillE;
        m_skillShift.action.performed += OnSkillShift;

        m_Inventory.action.Enable();
        m_Inventory.action.performed += OnToggleInventory;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // 이벤트 제거 (중복 방지)
    void OnDisable()
    {
        m_Inventory.action.performed -= OnToggleInventory;

        m_skillE.action.performed -= OnSkillE;
        m_skillShift.action.performed -= OnSkillShift;

        m_jump.action.performed -= OnJump;

        m_move.action.Disable();
        m_jump.action.Disable();
        m_look.action.Disable();

        m_Inventory.action.Disable();
        m_skillE.action.Disable();
        m_skillShift.action.Disable();
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

        Vector3 moveInput = m_move.action.ReadValue<Vector2>();

        m_inputDirection = new Vector3(moveInput.x, 0f, moveInput.y);

        Vector2 lookInput = m_look.action.ReadValue<Vector2>();

        float mouseX = lookInput.x * m_mouseSensitivity;
        float mouseY = lookInput.y * m_mouseSensitivity;

        m_xRotation -= mouseY;
        m_xRotation = Mathf.Clamp(m_xRotation, -90f, 90f);

        if (m_cameraTransform != null)
        {
            m_cameraTransform.localRotation = Quaternion.Euler(m_xRotation, 0f, 0f);
        }

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

    public void SetMouseSensitivity(float value)
    {
         m_mouseSensitivity = CalculateAppliedSensitivity(value);
    }

    private void InitializeMouseSensitivity()
    {
        try
        {
            if (SettingsManager.Instance != null && SettingsManager.Instance.Play != null)
            {
                SetMouseSensitivity(SettingsManager.Instance.Play.MouseSensitivity);
            }
            else
            {
                SetMouseSensitivity(m_mouseSensitivity);
            }
        }
        catch
        {
            SetMouseSensitivity(m_mouseSensitivity);
        }
    }

    private float CalculateAppliedSensitivity(float settingsValue)
    {
        return Mathf.Max(MIN_MOUSE_SENSITIVITY, settingsValue * SENSITIVITY_CONVERSION_SCALE);
    }
}
