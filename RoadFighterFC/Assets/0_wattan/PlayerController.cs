using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("通常速度")]
    public float baseSpeed = 50f;

    [Header("現在速度")]
    public float forwardSpeed;

    [Header("左右移動速度")]
    public float sideSpeed = 5f;

    [Header("加速度")]
    public float fastAcceleration = 100f;
    public float slowAcceleration = 50f;

    [Header("最高速度")]
    public float fastMaxSpeed = 400f;
    public float slowMaxSpeed = 200f;

    [Header("減速度")]
    public float deceleration = 150f;

    private void Start()
    {
        forwardSpeed = baseSpeed;
    }

    private void Update()
    {
        bool fastBoost =
            (Mouse.current != null && Mouse.current.leftButton.isPressed) ||
            (Gamepad.current != null && Gamepad.current.buttonSouth.isPressed);

        bool slowBoost =
            (Mouse.current != null && Mouse.current.rightButton.isPressed) ||
            (Gamepad.current != null && Gamepad.current.buttonEast.isPressed);

        if (fastBoost)
        {
            forwardSpeed += fastAcceleration * Time.deltaTime;
            forwardSpeed = Mathf.Min(forwardSpeed, fastMaxSpeed);
        }
        else if (slowBoost)
        {
            forwardSpeed += slowAcceleration * Time.deltaTime;
            forwardSpeed = Mathf.Min(forwardSpeed, slowMaxSpeed);
        }
        else
        {
            // ボタンを離したら通常速度まで減速
            forwardSpeed = Mathf.MoveTowards(
                forwardSpeed,
                baseSpeed,
                deceleration * Time.deltaTime
            );
        }

        // 常に前進
        Vector3 move = transform.forward * forwardSpeed * Time.deltaTime;

        float horizontal = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed)
                horizontal = -1f;
            else if (Keyboard.current.dKey.isPressed)
                horizontal = 1f;
        }

        if (Gamepad.current != null)
        {
            if (Gamepad.current.dpad.left.isPressed|Gamepad.current.leftStick.left.isPressed)
                horizontal = -1f;
            else if (Gamepad.current.dpad.right.isPressed|Gamepad.current.leftStick.right.isPressed)
                horizontal = 1f;
        }

        move += transform.right * horizontal * sideSpeed * Time.deltaTime;

        transform.position += move;
    }
}