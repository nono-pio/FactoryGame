using UnityEngine;

public class ItemOnGround : MonoBehaviour
{

    [HideInInspector] public Item item;
    [HideInInspector] public int count;

    [SerializeField] private BoxCollider2D boxCollider2D;
    [SerializeField] private SpriteRenderer sprite; 

    [SerializeField] private int nbEntiteCheckForFindItem = 10;

    public void InstantiateItem(Item _item, int _count = 1)
    {
        item = _item;
        count = _count;
        sprite.sprite = item.image;

        CheckCollisionWithItem();
    }
    
    private void CheckCollisionWithItem()
    {
        Collider2D[] colliders = new Collider2D[nbEntiteCheckForFindItem];
        ContactFilter2D contactFilter = new ContactFilter2D();
        contactFilter.useTriggers = true;
        contactFilter.useLayerMask = true;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));

        int nbCollision = boxCollider2D.OverlapCollider(contactFilter, colliders);

        foreach (var collision in colliders)
        {
            if (collision == null) return;
            if (collision.CompareTag("Item"))
            {
                ItemOnGround collisionItem = collision.GetComponentInChildren<ItemOnGround>();
                if (item = collisionItem.item)
                {
                    count += collisionItem.count;
                    Destroy(collisionItem.gameObject);
                }
            }
        }
    }
}
