using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 敵の基本的な機能を制御するコンポーネント
/// </summary>
public class EnemyController : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<PlayerBulletController>())  // 衝突相手が 弾 だったら
        {
            Destroy(collision.gameObject);  // 弾のオブジェクトを破棄する
            Destroy(this.gameObject);       // そして自分も破棄する
            /* ============
             * 注意: 上の２行の順番を入れ替えると挙動が変わる。
             * どのように挙動が変わるのかを確認し、なぜなのかを理解しておくこと。
             * ============ */
        }
    }
}