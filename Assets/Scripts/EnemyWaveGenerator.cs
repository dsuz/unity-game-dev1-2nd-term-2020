using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵を生成するコンポーネント
/// m_enemyPrefabs にアサインされたプレハブを m_spawnTimesInWave 回ずつ生成し、全部生成したら最後に m_bossPrefab にアサインされたプレハブを生成する
/// Wave の仕様は以下の通り。
/// 1. m_enemyPrefabs 配列の要素としてアサインされているプレハブを、m_spawnIntervalInWave 秒ごとに m_spawnTimesInWave 回生成する。これを１ウェーブとする。
/// 2. ウエーブが終わったら、画面（シーン）から敵がいなくなるまで待つ
/// 3. m_enemyPrefabs 配列から次の要素のプレハブを、１ウェーブぶん生成する
/// 4. m_enemyPrefabs 配列の全ての要素に対して１ウェーブずつの生成が終わったら、シーンから敵がいなくなるのを待つ
/// 5. m_bossPrefab にアサインされたプレハブを生成する
/// </summary>
public class EnemyWaveGenerator : MonoBehaviour
{
    /// <summary>ウエーブとして生成するプレハブの配列</summary>
    [SerializeField] GameObject[] m_enemyPrefabs = null;
    /// <summary>ボスとして生成するプレハブの配列</summary>
    [SerializeField] GameObject m_bossPrefab = null;
    /// <summary>敵を生成する位置として設定するオブジェクト</summary>
    [SerializeField] Transform m_spawnPoint = null;
    /// <summary>１ウェーブ内での敵プレハブの生成間隔（秒）</summary>
    [SerializeField] float m_spawnIntervalInWave = 2f;
    /// <summary>１ウェーブ内での敵プレハブを生成する回数</summary>
    [SerializeField] int m_spawnTimesInWave = 5;
    /// <summary>m_enemyPrefabs の添字</summary>
    int m_index;
    /// <summary>１ウェーブ内で敵プレハブを生成した回数を数えるためのカウンタ変数</summary>
    int m_spawnCounter;
    /// <summary>１ウェーブ内での m_spawnIntervalInWave をカウントするためのカウンター</summary>
    float m_timer;
    /// <summary>ボスが生成済みか判定するフラ部</summary>
    bool m_isBossSpawned;

    void Update()
    {
        // ボスを生成した後は何もしない
        if (m_isBossSpawned)
        {
            return; // ここで関数を抜ける
        }

        // ウェーブが切り替わった後は、敵が一体もいなくなったら次の敵を生成する
        if (m_spawnCounter == 0)
        {
            int enemyCount = GameObject.FindObjectsOfType<EnemyController>().Length;    // タグで検索してもよい
            // 敵がまだ残っていたら何もしない
            if (enemyCount > 0)
            {
                return;
            }
        }

        // ウェーブ内で敵の生成間隔を待つ
        m_timer += Time.deltaTime;
        if (m_timer > m_spawnIntervalInWave)
        {
            // m_enemyPrefabs 配列の全ての要素に対してウェーブが終わったら
            if (m_index > m_enemyPrefabs.Length - 1)
            {
                // ボスを生成する
                m_isBossSpawned = true;
                Debug.Log("Spawn Boss");
                GameObject boss = Instantiate(m_bossPrefab);
                boss.transform.position = m_spawnPoint.position;
                return;
            }

            // まだウェーブを生成し、タイマーをリセットして m_spawnCounter をカウントアップする
            m_timer = 0;
            m_spawnCounter++;
            Debug.LogFormat("m_spawnCounter: {0}", m_spawnCounter);

            // 敵を生成する
            GameObject go = Instantiate(m_enemyPrefabs[m_index]);
            go.transform.position = m_spawnPoint.position;

            // ウェーブが終わったら、次のウェーブに映る
            if (m_spawnCounter > m_spawnTimesInWave - 1)
            {
                m_spawnCounter = 0;
                m_index++;
                Debug.LogFormat("m_index: {0}", m_index);
            }
        }
    }
}
