using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// シューティングゲームの自機を操作するためのコンポーネント
/// </summary>
[RequireComponent(typeof(Rigidbody2D))] // Rigidbody コンポーネントのアタッチを強制する
public class SpaceShipController : MonoBehaviour
{
    /// <summary>プレイヤーの移動速度</summary>
    [SerializeField] float m_moveSpeed = 5f;
    /// <summary>弾のプレハブ</summary>
    [SerializeField] GameObject m_bulletPrefab = null;
    /// <summary>弾の発射位置</summary>
    [SerializeField] Transform m_muzzle = null;
    /// <summary>一画面の最大段数 (0 = 無制限)</summary>
    [SerializeField, Range(0, 10)] int m_bulletLimit = 0;
    Rigidbody2D m_rb;

    void Start()
    {
        m_rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // 自機を移動させる
        float h = Input.GetAxisRaw("Horizontal");   // 垂直方向の入力を取得する
        float v = Input.GetAxisRaw("Vertical");     // 水平方向の入力を取得する
        Vector2 dir = new Vector2(h, v).normalized; // 進行方向の単位ベクトルを作る (dir = direction) 
        m_rb.velocity = dir * m_moveSpeed;        // 単位ベクトルにスピードをかけて速度ベクトルにして、それを Rigidbody の速度ベクトルとしてセットする

        // 左クリックまたは左 Ctrl で弾を発射する（単発）
        if (Input.GetButtonDown("Fire1"))
        {
            if (m_bulletLimit == 0 || this.GetComponentsInChildren<PlayerBulletController>().Length < m_bulletLimit)    // 画面内の弾数を制限する
            {
                Fire1();
            }
        }

        // 課題: ここに右クリックまたは左 Alt (Fire2) で弾を発射するようコードを追加せよ
    }

    /// <summary>
    /// 弾を発射する
    /// </summary>
    void Fire1()
    {
        if (m_bulletPrefab && m_muzzle) // m_bulletPrefab にプレハブが設定されている時 かつ m_muzzle に弾の発射位置が設定されている時
        {
            GameObject go = Instantiate(m_bulletPrefab, m_muzzle.position, m_bulletPrefab.transform.rotation);  // インスペクターから設定した m_bulletPrefab をインスタンス化する
            go.transform.SetParent(this.transform);
        }
    }
}
