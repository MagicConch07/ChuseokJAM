using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "SO/Input")]
public class InputReaderSO : ScriptableObject, Contorls.IPlayerActions
{
    private Contorls _contorls;
    
    public event Action<Vector2> MovementEvent;
    public event Action<bool> HookEvent;
    public event Action<Vector2> MousePosEvent;

    private void OnEnable()
    {
        if (_contorls == null)
        {
            _contorls = new Contorls();
        }
        _contorls.Player.SetCallbacks(this);
        _contorls.Player.Enable();
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        MovementEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnHook(InputAction.CallbackContext context)
    {
        if (context.performed)
            HookEvent?.Invoke(true);
        else if (context.canceled)
            HookEvent?.Invoke(false);
    }

    public void OnMousePosition(InputAction.CallbackContext context)
    {
        MousePosEvent?.Invoke(context.ReadValue<Vector2>());
    }
}
