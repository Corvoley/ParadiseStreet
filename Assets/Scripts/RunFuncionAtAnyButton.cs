using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RunFuncionAtAnyButton : MonoBehaviour
{
    public UnityEvent OnButtonClick;
    public bool canBeInvoked;


    private void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            if (canBeInvoked)
            {
                OnButtonClick?.Invoke();

            }
        }
    }

    private async void OnEnable()
    {
        await SetInvokeBool();
    }
    private void OnDisable()
    {
        canBeInvoked = false;
    }

    private async Task SetInvokeBool()
    {
        await Awaitable.WaitForSecondsAsync(1);
        canBeInvoked = true;
    }

}
