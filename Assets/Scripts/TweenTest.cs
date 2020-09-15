using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;  // DOTween を使うために追加している

/// <summary>
/// DOTween のテスト用スクリプト (2D, Sprite)
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class TweenTest : MonoBehaviour
{
    /// <summary>DOTween のシーケンス</summary>
    Sequence m_seq;
    SpriteRenderer m_sprite;

    void Start()
    {
        m_seq = DOTween.Sequence(); // シーケンスを初期化する
        m_sprite = GetComponent<SpriteRenderer>();
        BuildSequence();
        PlaySequence();
    }

    void BuildSequence()
    {
        // シーケンスに動作を追加していく
        m_seq.Append(transform.DOMove(new Vector2(0, 3), 5).SetRelative().SetEase(Ease.Linear))    // ５秒かけて上に３メートル動く
            .Append(transform.DOMove(new Vector2(3, 3), 2).SetEase(Ease.Linear))                   // ２秒かけて (3, 3) に移動する
            .Append(transform.DORotate(new Vector3(0, 0, 180), 1).SetRelative().SetLoops(4));      // １秒で180度回転するのを４回繰り返す

        // さらに動作を追加していく
        m_seq.Append(transform.DOScale(2 * transform.localScale, 2))    // ２秒かけて２倍の大きさになりながら
            .Join(m_sprite.DOColor(Color.red, 1))                       // １秒かけて色を赤に変えながら
            .Join(m_sprite.DOFade(0, 2))                                // ２秒かけて透明になる
            .Append(m_sprite.DOColor(Color.white, 2))                   // ２秒かけて白に色を変えながら
            .Join(m_sprite.DOFade(1, 2))                                // ２秒かけて完全に不透明になる
            .AppendInterval(1)                                          // １秒待つ
            .Append(transform.DOScale(transform.localScale, 1))         // １秒かけて元の大きさに戻りながら
            .Join(transform.DOMove(Vector2.zero, 3).OnComplete(() =>    // ３秒で原点に移動する
                {
                    Debug.Log("Animation Finished.");                   // 終わったらログを出力して
                    m_seq.Rewind();                                     // シーケンスを巻き戻す
                }));
    }

    public void PlaySequence()
    {
        m_seq.Play();
    }

    public void StopSequence()
    {
        m_seq.Rewind();
    }

    public void PauseSequence()
    {
        m_seq.Pause();
    }
}
