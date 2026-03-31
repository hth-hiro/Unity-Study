

public interface IInteractable
{
    bool CanInteract { get; }

    string InteractionName { get; }

    void Interact();
}

