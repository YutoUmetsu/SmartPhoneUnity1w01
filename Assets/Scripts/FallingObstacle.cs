using UnityEngine;
using UnityEngine.SceneManagement;

public class FallingObstacle : MonoBehaviour
{
    [SerializeField] private float fallSpeed = 8f;   // 落ちる速度
    [SerializeField] private float destroyY = -10f;  // 画面外（下）に出て消えるY座標

    void Update()
    {
        // 【2D修正】真下（Vector3.down）に向かって進ませる
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime);

        // 【2D修正】画面の下に通り過ぎ去ったら自動で削除（Y座標で判定）
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }

    // 【2D修正】2D専用のトリガー判定
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("【衝突検知】プレイヤーに障害物が当たりました！");

            ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();
            if (scoreManager != null)
            {
                scoreManager.StopScore();
            }

            other.gameObject.SetActive(false); // プレイヤーを非表示
            Destroy(gameObject); // 障害物を消す
            SceneManager.LoadScene("GameOver");
        }
    }
}