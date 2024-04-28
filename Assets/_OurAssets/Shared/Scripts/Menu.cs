using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Menu : MonoBehaviour
{
    bool menuState;
    [SerializeField] CanvasGroup myGroup;
    [SerializeField] GameObject Selector;
    [SerializeField] CanvasGroup[] otherGroups;
    int button = 0;

    [SerializeField] TextMeshProUGUI[] keybindTexts;
    [SerializeField] InputActionReference[] actions;
    int currentSelection;

    [SerializeField] GameObject rebindScreen;

    CharacterBase P1;
    CharacterBase P2;

    private InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    public void ChangeMenuState()
    {
        menuState = !menuState;

        myGroup.blocksRaycasts = menuState;
        myGroup.interactable = menuState;

        Cursor.visible = menuState;
        Cursor.lockState = (menuState) ? CursorLockMode.None : CursorLockMode.Locked;
        Time.timeScale = (menuState) ? 0 : 1;
    }

    public void GetPlayers(CharacterBase p1, CharacterBase p2)
    {
        P1 = p1;
        P2 = p2;
    }

    public void MoveUp()
    {
        if (menuState)
        {
            button--;
            button = Mathf.Clamp(button, 0, 3);
        }
    }

    public void MoveDown()
    {
        if (menuState)
        {
            button++;
            button = Mathf.Clamp(button, 0, 3);

        }
    }

    public void SelectCurrent(int id)
    {
        currentSelection = id;
    }

    public void Start()
    {
        for(int i = 0; i < actions.Length; i++)
        {
            int bindingIndex = actions[i].action.GetBindingIndexForControl(actions[i].action.controls[0]);

            string keybind = (InputControlPath.ToHumanReadableString(actions[i].action.bindings[bindingIndex].effectivePath,
                InputControlPath.HumanReadableStringOptions.OmitDevice));

            keybindTexts[i].text = keybind;
        }

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

    private void Update()
    {
        float value = (menuState) ? 1 : 0 ;
        myGroup.alpha = Mathf.Lerp(myGroup.alpha, value, 1);

        if (menuState == false)
            return;

        for(int i = 0; i < otherGroups.Length; i++)
        {
            float groupValue = (button == i) ? 1 : 0 ;
            otherGroups[i].interactable = (button == i);
            otherGroups[i].blocksRaycasts = (button == i);
            otherGroups[i].alpha = Mathf.Lerp(otherGroups[i].alpha, groupValue, .7f);
        }

        Vector3 selectorValue = new Vector3(-650, 375, 0);
        selectorValue.y = 375 - (button * 250);
        Selector.transform.localPosition = Vector3.Lerp(Selector.transform.localPosition, selectorValue, 0.5f);
    }
}
