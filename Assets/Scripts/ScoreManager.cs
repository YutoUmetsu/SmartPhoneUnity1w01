using UnityEngine;
using TMPro; // TextMeshProを使うために必要

public class ScoreManager : MonoBehaviour
{
    [Header("表示用のTextMeshProコンポーネント")]
    [SerializeField] private TMP_Text scoreText;

    [Header("スコアの設定")]
    [SerializeField] private int pointsPerSecond = 100; // 1秒間でもらえるスコア

    private float scoreTimer = 0f;  // 0.1秒を数えるためのタイマー
    private float currentScore = 0f; // 現在のスコア（細かく加算するためfloatで管理）
    private bool isGameActive = true; // ゲーム中かどうかのフラグ

    void Start()
    {
        // 初期表示を「0」にする
        UpdateScoreText();
    }

    void Update()
    {
        if (!isGameActive) return;

        // タイマーを進める
        scoreTimer += Time.deltaTime;

        // 0.1秒経つごとに処理
        if (scoreTimer >= 0.1f)
        {
            // 経過した時間分のスコアを計算して加算
            // 例：1秒で100点なら、0.1秒で10点加算されるようにする
            currentScore += pointsPerSecond * scoreTimer;

            // タイマーをリセット（余剰分を引き継ぐ）
            scoreTimer -= 0.1f;

            // 画面のテキストを更新
            UpdateScoreText();
        }
    }

    /// <summary>
    /// スコアの数値を整数（int）に変換してTMPに反映する
    /// </summary>
    void UpdateScoreText()
    {
        if (scoreText != null)
        {
            // 数値だけをそのまま表示（ドットや文字を含めない）
            // 小数点を切り捨てて、きれいな整数にする
            int displayScore = Mathf.FloorToInt(currentScore);
            scoreText.text = displayScore.ToString();
        }
    }

    /// <summary>
    /// ゲームオーバー時に外部（プレイヤーの衝突判定など）から呼び出す関数
    /// </summary>
    public void StopScore()
    {
        isGameActive = false;
        Debug.Log($"スコア確定: {Mathf.FloorToInt(currentScore)} 点");
    }
}