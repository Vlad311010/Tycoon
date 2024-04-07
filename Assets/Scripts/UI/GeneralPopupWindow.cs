using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneralPopupWindow : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Button okButton;
    [SerializeField] Button cancelButton;

    private WindowUI windowUI;
    private WindowsController windowsController;

    private void Awake()
    {
        windowUI = GetComponent<WindowUI>();
        windowsController = GetComponentInParent<WindowsController>();
        GameEvents.current.onPopupWindowCall += CallPopupWindow;
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
    }

    public void CallPopupWindow(string text, Action okBtnAction)
    {
        GetComponentInParent<WindowsController>().OpenWindow(windowUI);
        SetText(text);
        SetOkBtnFunction(okBtnAction);
        SetCancelBtnDefaultFunction();
    }

    public void SetText(string txt)
    {
        text.text = txt;
    }

    public void SetOkBtnFunction(Action action)
    {
        okButton.onClick.RemoveAllListeners();
        okButton.onClick.AddListener(() => action());
        okButton.onClick.AddListener(() => windowsController.CloseWindow());
    }

    public void SetCancelBtnFunction(Action action)
    {
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => action());
    }

    public void SetCancelBtnDefaultFunction()
    {
        cancelButton.onClick.RemoveAllListeners();
        cancelButton.onClick.AddListener(() => windowsController.CloseWindow());
    }

}
