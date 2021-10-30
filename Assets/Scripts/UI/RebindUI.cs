using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class RebindUI : MonoBehaviour
{
    [SerializeField]private List<InputActionReference> allInputActions = new List<InputActionReference>();
    [SerializeField]private InputActionReference inputActionReference;
    [SerializeField][Range(0,10)]private int selectedBinding;
    [SerializeField]private InputBinding.DisplayStringOptions displayStringOptions;
    [SerializeField]private InputBinding inputBinding;
    private int bindingIndex;
    private string actionName;
    [SerializeField]private Dropdown referenceDropdown;
    [SerializeField]private Button rebindButton;
    [SerializeField]private Text rebindText;
    [SerializeField]private Button resetButton;

    private void Start()
    {
        populateDropDown();
        this.inputActionReference = (InputActionReference) ScriptableObject.CreateInstance(typeof(InputActionReference));
    }

    private void OnEnable()
    {
        this.rebindButton.onClick.AddListener(() => doRebind());
        this.resetButton.onClick.AddListener(() => resetBinding());
        this.referenceDropdown.onValueChanged.AddListener(delegate { DropdownValueChanged(this.referenceDropdown); });

        InputManager.rebindComplete += updateUI;
        InputManager.rebindCanceled += updateUI;
    }

    private void OnDisable()
    {
        InputManager.rebindComplete -= updateUI;
        InputManager.rebindCanceled -= updateUI;
    }

    private void OnValidate()
    {
        if(this.inputActionReference == null) { return; }

        getBindingInfo();
        updateUI();
    }

    private void getBindingInfo()
    {
        if(this.inputActionReference.action != null)
        {
            this.actionName = this.inputActionReference.action.name;
        }

        if(this.inputActionReference.action.bindings.Count > this.selectedBinding)
        {
            this.inputBinding = this.inputActionReference.action.bindings[selectedBinding];
            this.bindingIndex = this.selectedBinding;
        }
    }

    private void updateUI()
    {
        if(this.rebindText != null)
        {
            if(Application.isPlaying)
            {
                this.rebindText.text = InputManager.getBindingName(this.actionName, this.bindingIndex);
            }
            else
            {
                this.rebindText.text = this.inputActionReference.action.GetBindingDisplayString(this.bindingIndex);
            }
        }
    }

    private void doRebind()
    {
        InputManager.startRebind(this.actionName, this.bindingIndex, this.rebindText);
    }

    private void resetBinding()
    {
        InputManager.resetBinding(this.actionName, this.bindingIndex);
        updateUI();
    }

    private void DropdownValueChanged(Dropdown change)
    {
        //Debug.Log("New Value : " + change.value);
        //this.inputActionReference.Set(this.allInputActions[change.value]);
        //this.inputActionReference.Set(InputManager.inputActions.asset.FindAction(InputManager.inputActions.Gameplay + "/" + this.referenceDropdown.options[change.value].text));
    }

    private void populateDropDown()
    {
        List<string> inputCategoryList = new List<string>();

        foreach (var action in InputManager.inputActions)
        {
            inputCategoryList.Add(action.name);
        }

        this.referenceDropdown.AddOptions(inputCategoryList);
    }
}