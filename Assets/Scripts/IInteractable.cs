using Components;

namespace DefaultNamespace
{
    public interface IInteractable
    {
        bool CanInteract(InteractionComponent instigator);
        
        void Interact(InteractionComponent instigator);
        
        string GetActionName();
    }
}