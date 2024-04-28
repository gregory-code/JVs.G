using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class titleScreen : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject controlMenu;

    [SerializeField] TextMeshProUGUI[] keybindTexts;
    int currentSelection;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    [SerializeField] GameObject rebindScreen;

    public void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Fight()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenControls()
    {
        mainMenu.SetActive(false);
        controlMenu.SetActive(true);
    }

    public void CloseControls()
    {
        mainMenu.SetActive(true);
        controlMenu.SetActive(false);
    }

    public void SelectCurrent(int id)
    {
        currentSelection = id;
    }

    public void Rebind(InputActionReference inputRef)
    {
        rebindScreen.SetActive(true);

        Debug.Log(inputRef.action.actionMap);

        inputRef.action.Disable();
        rebindingOperation = inputRef.action.PerformInteractiveRebinding()
            .WithControlsExcluding("Mouse")
            .WithCancelingThrough("<Keyboard>/escape")
            .OnMatchWaitForAnother(0.1f)
            .OnCancel(operation => RebindFailed(inputRef))
            .OnComplete(operation => RebindComplete(inputRef))
            .Start();
    }

    private void RebindFailed(InputActionReference inputRef)
    {
        rebindScreen.SetActive(false);

        inputRef.action.Enable();

        rebindingOperation.Dispose();
    }

    private void RebindComplete(InputActionReference inputRef)
    {
        int bindingIndex = inputRef.action.GetBindingIndexForControl(inputRef.action.controls[0]);

        string keybind = (InputControlPath.ToHumanReadableString(inputRef.action.bindings[bindingIndex].effectivePath,
            InputControlPath.HumanReadableStringOptions.OmitDevice));

        keybindTexts[currentSelection].text = keybind;

        rebindScreen.SetActive(false);
        inputRef.action.Enable();
        rebindingOperation.Dispose();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
