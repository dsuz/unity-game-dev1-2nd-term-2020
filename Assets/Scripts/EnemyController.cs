using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の基本的な機能を制御するコンポーネント
/// </summary>
public class EnemyController : MonoBehaviour
{
    /// <summary>敵の弾のプレハブ</summary>
    [SerializeField] GameObject m_enemyBulletPrefab = null;
    /// <summary>敵が弾を発射する間隔（秒）</summary>
    [SerializeField] float m_fireInterval = 1f;
    /// <summary>爆発エフェクトのプレハブ</summary>
    [SerializeField] GameObject m_explosionPrefab = null;
    float m_timer;

    void Update()
    {
        if (m_enemyBulletPrefab)
        {
            // 一定間隔で弾を発射する
            m_timer += Time.deltaTime;
            if (m_timer > m_fireInterval)
            {
                m_timer = 0f;
                Instantiate(m_enemyBulletPrefab, this.transform.position, Quaternion.identity);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerBulletController>())  // 衝突相手が 弾 だったら
        {
            Destroy(collision.gameObject);  // 弾のオブジェクトを破棄する
            // 爆発エフェクトを生成する
            if (m_explosionPrefab)
            {
                Instantiate(m_explosionPrefab, this.transform.position, m_explosionPrefab.transform.rotation);
            }
            Destroy(this.gameObject);       // そして自分も破棄する
        }
    }
}