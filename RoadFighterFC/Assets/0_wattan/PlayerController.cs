using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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

    private float slipTiltDirection = 0f;

    private FuelManager fuelManager;
    //追加
    [Header("飛行機設定")]
    public GameObject airplanePrefab;
    public float airplaneSpawnTime = 25f;
    //プレイヤーの近くに生成
    public Vector3 airplaneSpawnOffset = new Vector3(0f, 15f, 80f);
    public float randomSideRange = 20f;

    private float noHitTimer = 0f;
    private bool airplaneSpawned = false;

    [Header("速度UI")]
    public TMP_Text speedText;

    private bool IsHitWall()
    {
        return transform.position.x <= -25f || transform.position.x >= 25f;
    }

    private bool IsAtMaxSpeed()
    {
        return forwardSpeed >= fastMaxSpeed - 0.1f;
    }

    private bool CanCrashFromWall()
    {
        // スリップ中 or 最高速度中ならクラッシュ
        if (isSlipping) return true;
        if (IsAtMaxSpeed()) return true;
        return false;
    }

    private void Start()
    {
        baseSpeed = normalBaseSpeed;
        forwardSpeed = baseSpeed;

        // FuelManagerを取得
        fuelManager = FindFirstObjectByType<FuelManager>();

        UpdateSpeedUI();
    }

    private void Update()
    {
        //追加  25秒間障害物に当たらなかったら飛行機を出現
        if (!airplaneSpawned)
        {
            noHitTimer += Time.deltaTime;

            if (noHitTimer >= airplaneSpawnTime)
            {
                float randomX = Random.Range(-randomSideRange, randomSideRange);

                Vector3 spawnPos =transform.position +
                transform.forward * airplaneSpawnOffset.z +
                transform.up * airplaneSpawnOffset.y +transform.right * (airplaneSpawnOffset.x + randomX);

                Instantiate(airplanePrefab,spawnPos,transform.rotation);
                noHitTimer = 0f;　//←25秒ごとに何度も出す　1度のみにするなら airplaneSpawned = true;に変える
            }
        }

        // =====================
        // クラッシュ中
        // =====================
        if (isCrashed)
        {
            crashTimer -= Time.deltaTime;

            if (crashTimer <= 0f)
            {
                isCrashed = false;

                isSlipping = false;
                slipTimer = 0f;

                baseSpeed = normalBaseSpeed;
                forwardSpeed = baseSpeed;

                transform.rotation = Quaternion.identity;
            }

            return;
        }

        // =====================
        // スリップ中
        // =====================
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
            }

            // 逆入力で回復
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

            // ★スリップ中：壁で即クラッシュ
            if (IsHitWall())
            {
                StartCrash();
                UpdateSpeedUI();
                return;

            }

            return;
        }

        // =====================
        // 通常時
        // =====================
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
            forwardSpeed = Mathf.MoveTowards(
                forwardSpeed,
                baseSpeed,
                deceleration * Time.deltaTime
            );
        }

        Vector3 move = transform.forward * forwardSpeed * Time.deltaTime;

        float horizontal = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed) horizontal = -1f;
            else if (Keyboard.current.dKey.isPressed) horizontal = 1f;
        }

        if (Gamepad.current != null)
        {
            if (Gamepad.current.dpad.left.isPressed || Gamepad.current.leftStick.left.isPressed)
                horizontal = -1f;
            else if (Gamepad.current.dpad.right.isPressed || Gamepad.current.leftStick.right.isPressed)
                horizontal = 1f;
        }

        move += transform.right * horizontal * sideSpeed * Time.deltaTime;

        transform.position += move;

        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -26f, 26f);
        transform.position = pos;

        // ★通常時：最高速度時のみ壁クラッシュ
        if (IsHitWall() && IsAtMaxSpeed())
        {
            StartCrash();
            UpdateSpeedUI();
            return;
        }
        UpdateSpeedUI();
    }

    private void RecoverFromSlip()
    {
        isSlipping = false;
        transform.rotation = Quaternion.identity;

        baseSpeed = normalBaseSpeed;
        forwardSpeed = baseSpeed;
        UpdateSpeedUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        //追加  障害物に当たったらタイマーをリセット
        if (other.CompareTag("SlipObstacle") || other.CompareTag("CrashObstacle"))
        {
            noHitTimer = 0f;
        }

        if (other.CompareTag("SlipObstacle"))
        {
            StartSlip();
        }

        if (other.CompareTag("CrashObstacle"))
        {
            StartCrash();
        }

        if (other.CompareTag("FuelTruck"))
        {
            if (fuelManager != null)
            {
                // 燃料を7回復
                fuelManager.AddFuel(7f);

                // スコアを1000加算
                fuelManager.AddScore(1000);
            }

            Destroy(other.gameObject);
        }
    }

    private void StartSlip()
    {
        isSlipping = true;
        slipTimer = slipDuration;

        baseSpeed = slipBaseSpeed;
        forwardSpeed = baseSpeed;

        slipTiltDirection = Random.value < 0.5f ? -1f : 1f;

        transform.rotation = Quaternion.Euler(0f, slipTiltDirection * 45f, 0f);
    }

    private void StartCrash()
    {
        if (isCrashed) return;

        isCrashed = true;
        crashTimer = crashDuration;

        // クラッシュ時に燃料を5減らす
        if (fuelManager != null)
        {
            fuelManager.CrashPenalty(5f);
        }

        forwardSpeed = 0f;
    }
    // 速度UI更新
    private void UpdateSpeedUI()
    {
        if (speedText == null) return;

        // 通常速度(10)を0km/h、最高速度(35)を400km/hに変換
        float t = (forwardSpeed - normalBaseSpeed) / (fastMaxSpeed - normalBaseSpeed);
        t = Mathf.Clamp01(t);

        int displaySpeed = Mathf.RoundToInt(t * 400f);

        speedText.text = $"{displaySpeed} km/h";
    }
}