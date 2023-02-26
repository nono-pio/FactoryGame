using UnityEngine;

public class InteractionManager : MonoBehaviour
{
    [HideInInspector] public static InteractionManager instance;
    private void Awake() { instance = this; }

    [SerializeField] private LayerMask layerMask;
    [SerializeField] private float interactRadius;
    [SerializeField] private Transform parentInteractable;

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

    public void InstantiateIteractable(GameObject prefab, Vector3 worldPos)
    {
        GameObject gameObject = Instantiate(prefab, parentInteractable);
        gameObject.transform.position = new Vector2(worldPos.x, worldPos.y + 0.55f);
    }
}

public interface IInteractable
{
    public Message messageInteract { get; }
    public void Open();
}
