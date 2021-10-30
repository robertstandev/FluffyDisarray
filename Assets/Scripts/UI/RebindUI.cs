using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RebindUI : MonoBehaviour
{
    [SerializeField]private InputActionReference inputActionReference;
    [SerializeField]private bool excludeMouse = true;
    [SerializeField][Range(0,10)]private int selectedBinding;
    [SerializeField]private InputBinding.DisplayStringOptions displayStringOptions;
    [SerializeField]private InputBinding inputBinding;
    private int bindingIndex;
    private string actionName;
    [SerializeField]private Text actionText;
    [SerializeField]private Button rebindButton;
    [SerializeField]private Text rebindText;
    [SerializeField]private Button resetButton;

    private void OnEnable()
    {
        rebindButton.onClick.AddListener(() => doRebind());
        resetButton.onClick.AddListener(() => resetBinding());

        InputManager.rebindComplete += updateUI;
    }

    private void OnDisable()
    {
        InputManager.rebindComplete -= updateUI;
    }

    private void OnValidate()
    {
        if(inputActionReference == null) { return; }

        getBindingInfo();
        updateUI();
    }

    private void getBindingInfo()
    {
        if(inputActionReference.action != null)
        {
            actionName = inputActionReference.action.name;
        }

        if(inputActionReference.action.bindings.Count > selectedBinding)
        {
            inputBinding = inputActionReference.action.bindings[selectedBinding];
            bindingIndex = selectedBinding;
        }
    }

    private void updateUI()
    {
        if(actionText != null)
        {
            actionText.text = actionName;
        }

        if(rebindText != null)
        {
            if(Application.isPlaying)
            {
                rebindText.text = InputManager.getBindingName(actionName, bindingIndex);
            }
            else
            {
                rebindText.text = inputActionReference.action.GetBindingDisplayString(bindingIndex);
            }
        }
    }

    private void doRebind()
    {
        InputManager.startRebind(actionName, bindingIndex, rebindText);
    }

    private void resetBinding()
    {

    }

}
