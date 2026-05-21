using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Header("生成する障害物のプレハブ")]
    [SerializeField] private GameObject obstaclePrefab;

    [Header("3つの生成位置を表す空オブジェクト（左、中央、右の順）")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("生成間隔の設定")]
    [SerializeField] private float initialSpawnInterval = 2.0f;
    [SerializeField] private float minimumSpawnInterval = 0.5f;
    [SerializeField] private float speedUpAmount = 0.15f;

    // オーディオ関連の変数
    [Header("オーディオ設定")]
    [SerializeField] private AudioSource audioSource; // 用意したAudioSourceをドラッグ＆ドロップ
    [SerializeField] private AudioClip levelUpSound;  // レベルアップ時の効果音(SE)

    private float currentSpawnInterval;
    private float spawnTimer;
    private float difficultyTimer;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        spawnTimer = 0f;
        difficultyTimer = 0f;
    }

    void Update()
    {
        // 1. 10秒経つごとに難易度アップ
        difficultyTimer += Time.deltaTime;
        if (difficultyTimer >= 10f)
        {
            difficultyTimer = 0f;
            LevelUpDifficulty();
        }

        // 2. 障害物の自動生成タイマー
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= currentSpawnInterval)
        {
            spawnTimer = 0f;
            SpawnRandomObstacle();
        }
    }

    void SpawnRandomObstacle()
    {
        if (spawnPoints == null || spawnPoints.Length == 0 || obstaclePrefab == null) return;

        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform targetSpawnPoint = spawnPoints[randomIndex];

        if (targetSpawnPoint != null)
        {
            Instantiate(obstaclePrefab, targetSpawnPoint.position, targetSpawnPoint.rotation);
        }
    }

    /// <summary>
    /// 10秒ごとに呼び出され、徐々に生成スピードを上げる
    /// </summary>
    void LevelUpDifficulty()
    {
        currentSpawnInterval -= speedUpAmount;

        if (currentSpawnInterval < minimumSpawnInterval)
        {
            currentSpawnInterval = minimumSpawnInterval;
        }

        Debug.Log($"【レベルアップ】次の生成まで: {currentSpawnInterval:F2} 秒");

        // ==== 【追加】レベルアップの音を鳴らす処理 ====
        if (audioSource != null && levelUpSound != null)
        {
            // PlayOneShotを使えば、音が重なっても途切れずに綺麗に鳴り響きます
            audioSource.PlayOneShot(levelUpSound);
        }
    }
}