using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 爆発エフェクトを制御するコンポーネント
/// </summary>
public class ExplosionEffectController : MonoBehaviour
{
    /// <summary>爆発エフェクトが表示される時間</summary>
    [SerializeField] float m_lifeTime = 1f;
    
    void Start()
    {
        // m_lifeTime 秒後に消す
        Destroy(this.gameObject, m_lifeTime);
    }
}
