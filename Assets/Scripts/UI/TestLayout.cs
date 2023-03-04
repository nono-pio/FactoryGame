using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

[AddComponentMenu("Layout/Test Layout")]
[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
[DisallowMultipleComponent]
public class TestLayout : UIBehaviour, ILayoutSelfController, ILayoutElement
{
    //Fields in the inspector used to manipulate the RectTransform
    public Vector3 m_Position;
    public Vector3 m_Rotation;
    public Vector2 m_Scale;

    float ILayoutElement.minWidth {get;}
    float ILayoutElement.preferredWidth {get;}
    float ILayoutElement.flexibleWidth {get;}
    float ILayoutElement.minHeight {get;}
    float ILayoutElement.preferredHeight {get;}
    float ILayoutElement.flexibleHeight {get;}
    int ILayoutElement.layoutPriority {get;}

    //This handles horizontal aspects of the layout (derived from ILayoutController)
    public virtual void SetLayoutHorizontal()
    {
        Debug.Log("SetLayoutHor");
        //Move and Rotate the RectTransform appropriately
        UpdateRectTransform();
    }

    //This handles vertical aspects of the layout
    public virtual void SetLayoutVertical()
    {
        Debug.Log("SetLayoutVer");
        //Move and Rotate the RectTransform appropriately
        UpdateRectTransform();
    }

    //This tells when there is a change in the inspector
    #if UNITY_EDITOR
    protected override void OnValidate()
    {
        Debug.Log("Validate");
        //Update the RectTransform position, rotation and scale
        UpdateRectTransform();
    }

    #endif

    //This tells when there has been a change to the RectTransform's settings in the inspector
    protected override void OnRectTransformDimensionsChange()
    {
        Debug.Log("OnRectTransformDimensionsChange");
        //Update the RectTransform position, rotation and scale
        UpdateRectTransform();
    }

    void UpdateRectTransform()
    {
        
    }

    void ILayoutElement.CalculateLayoutInputHorizontal()
    {
        Debug.Log("CalculateLayoutInputHorizontal");
    }

    void ILayoutElement.CalculateLayoutInputVertical()
    {
        Debug.Log("CalculateLayoutInputVertical");
    }
}
