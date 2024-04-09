using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsControllerGame : WindowsController
{
    [SerializeField] private GeneralPopupWindow popupWindow;

    
    public void LoadMainMenu()
    {
        SceneController.Resume();
        SceneManager.LoadScene("MainMenu");
    }
}
