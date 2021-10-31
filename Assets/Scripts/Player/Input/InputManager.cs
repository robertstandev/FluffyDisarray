using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class InputManager : MonoBehaviour
{
   public static PlayerInputManager inputActions;
   public static event Action rebindComplete;
   public static event Action rebindCanceled;
   public static event Action<InputAction, int> rebindStarted;

   private void Awake()
   {
       if(inputActions == null)
       {
           createInputsNewInstance();
       }
   }

   public static void createInputsNewInstance()
   {
       inputActions = new PlayerInputManager();
   }

   public static void startRebind(string actionName, int bindingIndex, Text statusText)
   {
       InputAction action = inputActions.asset.FindAction(actionName);
       if(action == null || action.bindings.Count <= bindingIndex) { return; }

        if(action.bindings[bindingIndex].isComposite)
        {
            var firstPartIndex = bindingIndex + 1;
            if(firstPartIndex < action.bindings.Count && action.bindings[firstPartIndex].isPartOfComposite)
            {
                doRebind(action, firstPartIndex, statusText, true);
            }
        }
        else
        {
            doRebind(action, bindingIndex, statusText, false);
        }

   }

   private static void doRebind(InputAction actionToRebind, int bindingIndex, Text statusText, bool allCompositeParts)
   {
       if(actionToRebind == null || bindingIndex < 0) { return; }

       statusText.text = $"Press a (actionToRebind.expectedControlType)";
       actionToRebind.Disable();

       var rebind = actionToRebind.PerformInteractiveRebinding(bindingIndex);
       rebind.OnComplete(operation =>
       {
           actionToRebind.Enable();
           operation.Dispose();

           if(allCompositeParts)
           {
               var nextBindingIndex = bindingIndex + 1;
               if(nextBindingIndex < actionToRebind.bindings.Count && actionToRebind.bindings[nextBindingIndex].isPartOfComposite)
               {
                   doRebind(actionToRebind, nextBindingIndex, statusText, allCompositeParts);
               }
           }

            rebindComplete?.Invoke();
       });
   
        rebind.OnCancel(operation =>
        {
            actionToRebind.Enable();
            operation.Dispose();

            rebindCanceled?.Invoke();
        });

        rebind.WithCancelingThrough("<Keyboard>/escape");
        //rebind.WithCancelingThrough("<Keyboard>/Z");
        //can add multiple (including controllers)

        rebindStarted?.Invoke(actionToRebind, bindingIndex);
        rebind.Start();
   }

   public static string getBindingName(string actionName, int bindingIndex)
   {
       if(inputActions == null)
       {
           createInputsNewInstance();
       }

       InputAction action = inputActions.asset.FindAction(actionName);
       return action.GetBindingDisplayString(bindingIndex);
   }

   public static void resetBinding(string actionName, int bindingIndex)
   {
       InputAction action = inputActions.asset.FindAction(actionName);

       if(action == null || action.bindings.Count <= bindingIndex) { return; }

       if(action.bindings[bindingIndex].isComposite)
        {
            action.RemoveAllBindingOverrides();
        }
        else
        {
            action.RemoveBindingOverride(bindingIndex);
        }
    }
}
