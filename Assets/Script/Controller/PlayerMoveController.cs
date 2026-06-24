using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMoveController : MonoBehaviour
{
    [Header("이동 입력 데드존")]
    [SerializeField] private float deadzone = 0.2f;

    [Header("Character")]
    [SerializeField] private Character player;

    public void MoveInput(InputAction.CallbackContext context)
    {
        Vector2 raw = context.ReadValue<Vector2>();

        Vector2 input =
            raw.magnitude < deadzone
            ? Vector2.zero
            : raw.normalized;

        if (context.canceled)
            input = Vector2.zero;

        input.y = 0;

        player.SetMoveInput(input);
    }
}