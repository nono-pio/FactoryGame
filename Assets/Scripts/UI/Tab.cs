using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class Tab : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public CraftType type;
    [SerializeField] private TabsGroup tabsGroup;

    [SerializeField] public Image background;

    #region PointerSystem
    
    public void OnPointerExit(PointerEventData eventData)
    {
        tabsGroup.onTabExit(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tabsGroup.onTabEnter(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        tabsGroup.onTabSelected(this);
    }

    #endregion
}
