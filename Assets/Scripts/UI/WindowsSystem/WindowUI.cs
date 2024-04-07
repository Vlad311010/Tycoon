using UnityEngine;
using UnityEngine.UI;

public class WindowUI : MonoBehaviour
{
    
    public bool closableByEsc = true;
    public bool pauseGame = true;


    public void OpenWindow()
    {
        if (pauseGame)
            SceneController.Pause();

        gameObject.SetActive(true);
        SetInteractable(true);
    }

    public void CloseWindow(bool escPress)
    {
        if (!escPress)
            CloseWindow();
        else if (escPress && closableByEsc)
            CloseWindow();
    }

    private void CloseWindow()
    {
       SetActive(false);
    }

    public void SetInteractable(bool interactable)
    {
        foreach (var item in GetComponentsInChildren<Selectable>())
        {
            item.interactable = interactable;
        }
    }

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
