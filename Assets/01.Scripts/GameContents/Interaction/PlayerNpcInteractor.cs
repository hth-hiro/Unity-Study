using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerNpcInteractor : MonoBehaviour
{
    [SerializeField] private InputActionReference m_interaction;

    private readonly List<Collider> m_npcColliders = new List<Collider>();
    private InputAction m_activeInteractionAction;
    private int m_lastInteractFrame = -1;

    private void OnEnable()
    {
        m_activeInteractionAction = m_interaction != null ? m_interaction.action : null;
        if (m_activeInteractionAction == null)
        {
            Debug.LogWarning("PlayerNpcInteractor: Interaction Action Reference is not assigned.");
            return;
        }

        m_activeInteractionAction.started += OnInteractionPerformed;
        m_activeInteractionAction.performed += OnInteractionPerformed;
        m_activeInteractionAction.Enable();
    }

    private void OnDisable()
    {
        if (m_activeInteractionAction == null)
        {
            return;
        }

        m_activeInteractionAction.started -= OnInteractionPerformed;
        m_activeInteractionAction.performed -= OnInteractionPerformed;
        m_activeInteractionAction.Disable();
        m_activeInteractionAction = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsValidNpc(other))
        {
            return;
        }

        if (!m_npcColliders.Contains(other))
        {
            m_npcColliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == null)
        {
            return;
        }

        m_npcColliders.Remove(other);
    }

    private void OnInteractionPerformed(InputAction.CallbackContext context)
    {
        if (m_lastInteractFrame == Time.frameCount)
        {
            return;
        }

        m_lastInteractFrame = Time.frameCount;

        IInteractable interactable = GetClosestInteractable();
        if (interactable == null || !interactable.CanInteract)
        {
            return;
        }

        Debug.Log($"PlayerNpcInteractor: Interact -> {((MonoBehaviour)interactable).name}");
        interactable.Interact();
    }

    private IInteractable GetClosestInteractable()
    {
        float closestDistanceSqr = float.MaxValue;
        IInteractable closestInteractable = null;

        for (int i = m_npcColliders.Count - 1; i >= 0; i--)
        {
            Collider npcCollider = m_npcColliders[i];
            if (npcCollider == null)
            {
                m_npcColliders.RemoveAt(i);
                continue;
            }

            IInteractable interactable = npcCollider.GetComponentInParent<IInteractable>();
            if (interactable == null)
            {
                m_npcColliders.RemoveAt(i);
                continue;
            }

            float distanceSqr = (npcCollider.bounds.ClosestPoint(transform.position) - transform.position).sqrMagnitude;
            if (distanceSqr < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqr;
                closestInteractable = interactable;
            }
        }

        return closestInteractable;
    }

    private bool IsValidNpc(Collider col)
    {
        if (col == null)
        {
            return false;
        }

        Transform rootTransform = col.transform.root;
        if (rootTransform == null || !rootTransform.CompareTag("NPC"))
        {
            return false;
        }

        return col.GetComponentInParent<IInteractable>() != null;
    }
}
