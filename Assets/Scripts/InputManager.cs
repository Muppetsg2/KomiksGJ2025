using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    private bool nextPressed = false;

    private static InputManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one InputManager in the Scene");
        }

        instance = this;
    }

    public static InputManager Instance { get { return instance; } }

    public void Next(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            nextPressed = true;
        }
        else if (context.canceled)
        {
            nextPressed = false;
        }
    }

    public bool GetNextPressed()
    {
        bool result = nextPressed;
        nextPressed = false;
        return result;
    }
}
