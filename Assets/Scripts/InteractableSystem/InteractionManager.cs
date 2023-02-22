using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [HideInInspector] public static InteractionManager instance;
    private void Awake() { instance = this; }

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float interactRadius;

    public void onClick(Vector2 WorldPosition)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(WorldPosition, interactRadius, layerMask);
        foreach (var collider in colliders)
        {
            IInteractable interactable = collider.gameObject.GetComponent<IInteractable>();
            if (interactable != null)
            {
                if(interactable.messageInteract != null) MessageManager.instance.AddMessage(interactable.messageInteract);
                interactable.Open();
                return;
            }
        }
    }
}

public interface IInteractable
{
    public Message messageInteract { get; }
    public void Open();
}
