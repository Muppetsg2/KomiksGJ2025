using UnityEngine;
using UnityEngine.InputSystem;

public delegate void NextPressedEventHandler();
public delegate void InteractPressedEventHandler();
public delegate void JumpPressedEventHandler();
public delegate void AttackPressedEventHandler();
public delegate void MoveEventHandler(Vector2 move);

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private static InputManager instance;

    public event NextPressedEventHandler OnNextPressed;
    public event InteractPressedEventHandler OnInteractPressed;
    public event JumpPressedEventHandler OnJumpPressed;
    public event AttackPressedEventHandler OnAttackPressed;
    public event MoveEventHandler OnMove;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one InputManager in the Scene");
        }

        instance = this;
    }

    public static InputManager Instance { get { return instance; } }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnInteractPressed?.Invoke();
        }
    }

    public void Attack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnAttackPressed?.Invoke();
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnJumpPressed?.Invoke();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        OnMove?.Invoke(context.ReadValue<Vector2>());
    }

    public void Next(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            OnNextPressed?.Invoke();
        }
    }
}
