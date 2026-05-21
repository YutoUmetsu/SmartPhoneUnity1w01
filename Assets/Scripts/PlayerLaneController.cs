using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class PlayerLaneController : MonoBehaviour
{
    [Header("動かすプレイヤー")]
    [SerializeField] private GameObject player;

    [Header("各レーンの位置となる空オブジェクト（左、中央、右の順）")]
    [SerializeField] private Transform[] laneTargets;

    // 現在いるレーンのインデックス（1 = 中央）
    private int currentLane = 1;

    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }

    void Update()
    {
        // 1. エディタ用：マウスの左クリックが押された瞬間
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            EvaluateLaneSelection(mousePos.x);
        }

        // 2. スマホ実機用：画面がタッチされた瞬間
        if (Touch.activeTouches.Count > 0)
        {
            Touch touch = Touch.activeTouches[0];
            if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
            {
                EvaluateLaneSelection(touch.screenPosition.x);
            }
        }
    }

    /// <summary>
    /// 押された画面の横幅（X座標）から、どのレーンかを判別する
    /// </summary>
    void EvaluateLaneSelection(float screenX)
    {
        // 確実に現在のゲーム画面の横幅を取得
        float screenWidth = Screen.width;

        int selectedLane = 1; // デフォルトは中央

        // 画面の横幅を3等分して判定（左・中央・右）
        if (screenX < screenWidth / 3.0f)
        {
            selectedLane = 0; // 左
        }
        else if (screenX < (screenWidth / 3.0f) * 2.0f)
        {
            selectedLane = 1; // 中央
        }
        else
        {
            selectedLane = 2; // 右
        }

        MoveToLane(selectedLane);
    }

    /// <summary>
    /// 指定されたインデックスのレーンへプレイヤーを移動
    /// </summary>
    void MoveToLane(int laneIndex)
    {
        if (laneTargets != null && laneIndex >= 0 && laneIndex < laneTargets.Length)
        {
            if (laneTargets[laneIndex] != null)
            {
                currentLane = laneIndex;
                SetPlayerPosition(currentLane);
            }
        }
    }

    /// <summary>
    /// 【2D版】プレイヤーの位置を対象のレーンのX座標に瞬間移動させる
    /// </summary>
    private void SetPlayerPosition(int laneIndex)
    {
        if (player == null) return;

        // 対象の空オブジェクトのX座標（左右の位置）を取得
        float targetX = laneTargets[laneIndex].position.x;

        // 2DなのでX座標だけを書き換えて、Y（高さ）やZはそのまま維持
        Vector3 currentPos = player.transform.position;
        currentPos.x = targetX;

        player.transform.position = currentPos;
    }
}