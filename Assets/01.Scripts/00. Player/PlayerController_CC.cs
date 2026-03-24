using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController_CC : MonoBehaviour
{
    private CharacterController m_controller;
    private Vector3 m_inputDirection;
    private Vector3 m_velocity;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float gravity = -25f;
    [SerializeField] private float jumpHeight = 2f;

    [Header("FPS Look Settings")]
    [SerializeField] private InputActionReference m_lookAction;
    [SerializeField] private Transform m_cameraTransform;
    [SerializeField] private float m_mouseSensitivity = 0.2f;
    private float m_xRotation = 0f;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference m_moveAction;
    [SerializeField] private InputActionReference m_jumpAction;

    void Awake()
    {
        m_controller = GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        m_moveAction.action.Enable();
        m_jumpAction.action.Enable();
        m_lookAction.action.Enable();

        m_jumpAction.action.performed += OnJump;
    }

    void OnJump(InputAction.CallbackContext ctx)
    {

    }

    void Update()
    {
        
    }
}
