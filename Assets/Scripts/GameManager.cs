using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // UI を操作するために追加している

/// <summary>
/// ゲーム全体を管理するクラス。
/// EnemyWaveGenerator と同じ GameObject にアタッチする必要がある。
/// </summary>
[RequireComponent(typeof(EnemyWaveGenerator), typeof(PlayerCounter))]
public class GameManager : MonoBehaviour
{
    /// <summary>シーンをロードするコンポーネント</summary>
    [SerializeField] SceneLoader m_sceneLoader = null;
    /// <summary>残機数</summary>
    [SerializeField] int m_life = 3;
    /// <summary>得点</summary>
    int m_score;
    /// <summary>自機のプレハブを指定する</summary>
    [SerializeField] GameObject m_playerPrefab = null;
    /// <summary>ゲームの初期化が終わってからゲームが始まるまでの待ち時間</summary>
    [SerializeField] float m_waitTimeUntilGameStarts = 5f;
    /// <summary>自機がやられてからゲームの再初期化をするまでの待ち時間</summary>
    [SerializeField] float m_waitTimeAfterPlayerDeath = 5f;
    /// <summary>EnemyWaveGenerator を保持しておく変数</summary>
    EnemyWaveGenerator m_enemyGenerator;
    /// <summary>残機表示をする PlayerCounter を保持しておく変数</summary>
    PlayerCounter m_playerCounter;
    /// <summary>スコア表示用 Text</summary>
    [SerializeField] Text m_scoreText = null;
    /// <summary>GameOver 表示用 Text</summary>
    [SerializeField] Text m_gameoverText = null;
    /// <summary>タイマー</summary>
    float m_timer;
    /// <summary>ゲームの状態</summary>
    GameState m_status = GameState.NonInitialized;

    void Start()
    {
        // ゲームオーバーの表示を消す
        if (m_gameoverText)
        {
            m_gameoverText.enabled = false;
        }

        // EnemyGenerator を取得しておき、まずは敵の生成を止めておく
        m_enemyGenerator = GetComponent<EnemyWaveGenerator>();
        m_enemyGenerator.StopGeneration();

        m_playerCounter = GetComponent<PlayerCounter>();
        AddScore(0);    // 得点を初期化する
    }

    void Update()
    {
        switch (m_status)   // ゲームの状態によって処理を分ける
        {
            case GameState.NonInitialized:
                Debug.Log("Initialize.");
                Instantiate(m_playerPrefab);    // プレイヤーを生成する
                m_status = GameState.Initialized;   // ステータスを初期化済みにする
                m_playerCounter.Refresh(m_life);    // 残機表示を更新する
                break;
            case GameState.Initialized:
                m_timer += Time.deltaTime;
                if (m_timer > m_waitTimeUntilGameStarts)    // 待つ
                {
                    Debug.Log("Game Start.");
                    m_timer = 0f;   // タイマーをリセットする
                    m_status = GameState.InGame;   // ステータスをゲーム中にする
                    m_enemyGenerator.StartGeneration(); // 敵の生成を開始する
                }
                break;
            case GameState.PlayerDead:
                // 残機がなかったらゲームオーバーを表示する
                if (m_gameoverText && m_life < 1)
                {
                    m_gameoverText.enabled = true;
                }

                m_timer += Time.deltaTime;
                if (m_timer > m_waitTimeAfterPlayerDeath)   // 待つ
                {
                    if (m_life > 0) // 残機がまだある場合
                    {
                        Debug.Log("Restart Game.");
                        m_timer = 0f;   // タイマーをリセットする
                        m_status = GameState.NonInitialized;   // 初期化するためにステータスを更新する
                        ClearScene();
                    }
                    else
                    {
                        GameOver(); // 残機がもうない場合はゲームオーバーにする
                    }
                }
                break;
        }
    }

    /// <summary>
    /// スコアを加算する
    /// </summary>
    /// <param name="score"></param>
    public void AddScore(int score)
    {
        m_score += score;
        if (m_scoreText)
        {
            m_scoreText.text = "Score: " + m_score.ToString("d10"); // 10桁でゼロ埋め (zero padding) する
        }
    }

    /// <summary>
    /// プレイヤーがやられた時、外部から呼ばれる関数
    /// </summary>
    public void PlayerDead()
    {
        Debug.Log("Player Dead.");
        m_enemyGenerator.StopGeneration();  // 敵の生成を止める
        m_life -= 1;    // 残機を減らす
        m_status = GameState.PlayerDead;   // ステータスをプレイヤーがやられた状態に更新する
    }

    /// <summary>
    /// シーン上にある敵と敵の弾を消す
    /// </summary>
    void ClearScene()
    {
        // 敵を消す
        GameObject[] goArray = GameObject.FindGameObjectsWithTag("EnemyTag");
        foreach (var go in goArray)
        {
            Destroy(go);
        }

        // 敵の弾を消す
        goArray = GameObject.FindGameObjectsWithTag("EnemyBulletTag");
        foreach (var go in goArray)
        {
            Destroy(go);
        }
    }

    /// <summary>
    /// ゲームオーバー時に呼び出す
    /// </summary>
    void GameOver()
    {
        Debug.Log("Game over. Load scene.");
        if (m_sceneLoader)
        {
            m_sceneLoader.LoadScene();
        }
    }
}

/// <summary>
/// ゲームの状態を表す列挙型
/// </summary>
enum GameState
{
    /// <summary>ゲーム初期化前</summary>
    NonInitialized,
    /// <summary>ゲーム初期化済み、ゲーム開始前</summary>
    Initialized,
    /// <summary>ゲーム中</summary>
    InGame,
    /// <summary>プレイヤーがやられた</summary>
    PlayerDead,
}