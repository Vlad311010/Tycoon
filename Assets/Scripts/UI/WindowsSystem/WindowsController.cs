using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class WindowsController : MonoBehaviour
{
    [SerializeField] WindowUI mainWindow;
    [SerializeField] bool mainWindowStartActive = false;

    private DefaultControls control;

    // state
    private Stack<WindowUI> windowsStack = new Stack<WindowUI>();

    protected virtual void Awake()
    {
        control = new DefaultControls();
        control.Enable();

        control.Menu.BackBtn.performed += BackButtonPressed;
        if (mainWindowStartActive)
            OpenWindow(mainWindow);

    }

    protected virtual void OnEnable()
    {
        control.Enable();
    }

    protected virtual void OnDisable()
    {
        control.Disable();
    }

    private void OnDestroy()
    {
        control.Menu.BackBtn.performed -= BackButtonPressed;
    }

    public void OpenWindow(WindowUI window)
    {
        if (window == null) return;

        if (windowsStack.Count > 0)
            windowsStack.Peek()?.SetActive(false);
        
        windowsStack.Push(window);
        window.OpenWindow();
    }

    public void CloseWindow(bool bckButton = false)
    {
        windowsStack.Pop().CloseWindow(false);
        if (windowsStack.Count > 0)
            windowsStack.Peek().SetActive(true);
        else
            SceneController.Resume();

    }

    private void BackButtonPressed(InputAction.CallbackContext obj)
    {
        if (!obj.performed) return;

        if (windowsStack.Count > 0)
            CloseWindow(true);
        else
            OpenWindow(mainWindow);
    }

    public void Exit()
    {
        Application.Quit();
    }
}
