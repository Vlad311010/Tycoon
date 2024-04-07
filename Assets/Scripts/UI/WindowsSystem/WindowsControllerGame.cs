using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WindowsControllerGame : WindowsController
{
    [SerializeField] private WindowUI respawnWindow;
    [SerializeField] private GeneralPopupWindow popupWindow;

    
    /*protected override void OnEnable()
    {
        base.OnEnable();
    }

    protected override void OnDisable()
    {
        base.OnDisable();
    }*/

    
    
    public void LoadMainMenu()
    {
        SceneController.Resume();
        SceneManager.LoadScene("MainMenu");
    }
}
