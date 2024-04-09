using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GeneralPopupWindow : MonoBehaviour
{
    [SerializeField] TMP_Text text;
    [SerializeField] Button okButton;
    [SerializeField] Button cancelButton;
    [SerializeField] Button singleModButton;

    private WindowUI windowUI;
    private WindowsController windowsController;

    private void Awake()
    {
        windowUI = GetComponent<WindowUI>();
        windowsController = GetComponentInParent<WindowsController>();
        GameEvents.current.onPopupWindowCall += CallPopupWindow;
        gameObject.SetActive(false);
        
        singleModButton.onClick.AddListener(() => windowsController.CloseWindow());
    }

    private void OnDisable()
    {
        okButton.onClick.RemoveAllListeners();
        cancelButton.onClick.RemoveAllListeners();
    }

    public void CallPopupWindow(string windowText, bool textAlingCenter, bool confirmationWindow, Action okBtnAction)
    {
        if (gameObject.activeSelf) return;

        GetComponentInParent<WindowsController>().OpenWindow(windowUI);
        
        text.alignment = textAlingCenter ? TextAlignmentOptions.CenterGeoAligned : TextAlignmentOptions.TopLeft;

        SetText(windowText);
        if (confirmationWindow)
        {
            okButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);
            singleModButton.gameObject.SetActive(false);

            SetOkBtnFunction(okBtnAction);
            SetCancelBtnDefaultFunction();
        }
        else
        {
            okButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
            singleModButton.gameObject.SetActive(true);
        }
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
