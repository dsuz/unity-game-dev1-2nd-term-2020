using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // シーン遷移を行うために追加している

/// <summary>
/// このオブジェクトが破棄されたことをもってゲームクリアとして、シーンをロードする
/// </summary>
public class GameClearDetector : MonoBehaviour
{
    [SerializeField] string m_sceneNameToBeLoaded = "SceneNameToBeLoaded";

    void OnDestroy()
    {
        Debug.Log("Clear");
        SceneManager.LoadScene(m_sceneNameToBeLoaded);
    }
}
