using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 背景をスクロールするためのコンポーネント。
/// 適当なオブジェクトにアタッチして使う。
/// 背景を指定すると、それをもう一つ複製する。元と複製したものを下にスクロールし、画面下に消えると上から出てくるようになる。
/// 従って、背景画像は上下に繋げてもよいものでなくてはならない。
/// </summary>
public class BackgroundController : MonoBehaviour
{
    /// <summary>背景</summary>
    [SerializeField] SpriteRenderer m_backgroundSprite = null;
    /// <summary>背景のスクロール速度</summary>
    [SerializeField] float m_scrollSpeedY = -1f;
    /// <summary>背景をクローンしたものを入れておく変数</summary>
    SpriteRenderer m_backgroundSpriteClone;
    /// <summary>背景座標の初期値</summary>
    float m_initialPositionY;

    void Start()
    {
        m_initialPositionY = m_backgroundSprite.transform.position.y;   // 座標の初期値を保存しておく

        // 背景画像を複製して上に並べる
        m_backgroundSpriteClone = Instantiate(m_backgroundSprite);
        m_backgroundSpriteClone.transform.Translate(0f, m_backgroundSprite.bounds.size.y, 0f);
    }

    void Update()
    {
        // 背景画像をスクロールする
        m_backgroundSprite.transform.Translate(0f, m_scrollSpeedY * Time.deltaTime, 0f);
        m_backgroundSpriteClone.transform.Translate(0f, m_scrollSpeedY * Time.deltaTime, 0f);

        // 背景画像がある程度下に降りたら、上に戻す
        if (m_backgroundSprite.transform.position.y < m_initialPositionY - m_backgroundSprite.bounds.size.y)
        {
            m_backgroundSprite.transform.Translate(0, m_backgroundSprite.bounds.size.y * 2, 0f);
        }

        // 背景画像のクローンがある程度下に降りたら、上に戻す
        if (m_backgroundSpriteClone.transform.position.y < m_initialPositionY - m_backgroundSpriteClone.bounds.size.y)
        {
            m_backgroundSpriteClone.transform.Translate(0, m_backgroundSpriteClone.bounds.size.y * 2, 0f);
        }
    }
}