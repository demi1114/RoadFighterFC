using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("通常速度")]
    public float normalBaseSpeed = 50f;

    [Header("スリップ時速度")]
    public float slipBaseSpeed = 25f;

    private float baseSpeed;

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

    [Header("スリップ設定")]
    public float slipDuration = 2f;

    [Header("クラッシュ設定")]
    public float crashDuration = 3f;

    private bool isCrashed = false;
    private float crashTimer = 0f;

    private bool isSlipping = false;
    private float slipTimer = 0f;

    private float slipTiltDirection = 0f; // -1 or 1

    public enum PlayerState
    {
        Normal,
        Slip,
        Crash
    }

    private void Start()
    {
        baseSpeed = normalBaseSpeed;
        forwardSpeed = baseSpeed;
    }

    private void Update()
    {
        if (isCrashed)
        {
            crashTimer -= Time.deltaTime;

            if (crashTimer <= 0f)
            {
                isCrashed = false;

                // 復帰時に通常速度へ
                forwardSpeed = baseSpeed;
            }

            return;
        }

        if (isSlipping)
        {
            float input = 0f;

            if (Keyboard.current != null)
            {
                if (Keyboard.current.aKey.isPressed) input = -1f;
                if (Keyboard.current.dKey.isPressed) input = 1f;
            }

            if (Gamepad.current != null)
            {
                if (Gamepad.current.dpad.left.isPressed) input = -1f;
                if (Gamepad.current.dpad.right.isPressed) input = 1f;
                if (Gamepad.current.leftStick.left.isPressed) input = -1f;
                if (Gamepad.current.leftStick.right.isPressed) input = 1f;
            }

            if (input == -slipTiltDirection)
            {
                RecoverFromSlip();
                return;
            }

            slipTimer -= Time.deltaTime;

            if (slipTimer <= 0f)
            {
                RecoverFromSlip();
                return;
            }

            transform.position += transform.forward * forwardSpeed * Time.deltaTime;
            return;
        }

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

    private void RecoverFromSlip()
    {
        isSlipping = false;

        transform.rotation = Quaternion.identity;

        // 通常速度に戻す
        baseSpeed = normalBaseSpeed;

        forwardSpeed = baseSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("SlipObstacle"))
        {
            StartSlip();
        }

        if (other.CompareTag("CrashObstacle"))
        {
            StartCrash();
        }
    }

    private void StartSlip()
    {
        isSlipping = true;
        slipTimer = slipDuration;

        // スリップ中は低速
        baseSpeed = slipBaseSpeed;

        forwardSpeed = baseSpeed;

        slipTiltDirection = Random.value < 0.5f ? -1f : 1f;

        transform.rotation = Quaternion.Euler(0f, slipTiltDirection * 45f, 0f);
    }

    private void StartCrash()
    {
        if (isCrashed)
            return;

        isCrashed = true;
        crashTimer = crashDuration;

        // 即停止
        forwardSpeed = 0f;
    }
}