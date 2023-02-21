using UnityEngine;

public class PopupManager : MonoBehaviour
{
    [HideInInspector] public static PopupManager instance;

    private bool isPopup = false;
    private Popup curPopup;

    private void Awake()
    {
        instance = this;
    }

    public void OpenPopup(Popup popup)
    {
        if (isPopup)
        {
            curPopup.Close();
        } else
        {
            isPopup = true;
            Player.instance.movePlayer.canMove = false;
        }
        popup.Open();
        curPopup = popup;
    }

    public void ClosePopup(Popup popup)
    {
        if (popup == curPopup)
        {
            isPopup = false;
            Player.instance.movePlayer.canMove = true;
            curPopup.Close();
        }
    }
}

public class Popup
{
    public GameObject popup;
    public delegate void CloseFunction();
    public CloseFunction closeFunction;

    public Popup(GameObject _popup)
    {
        popup = _popup;
    }

    public void Open()
    {
        popup.SetActive(true);
    }

    public void Close()
    {
        popup.SetActive(false);
        closeFunction();
    }
}
