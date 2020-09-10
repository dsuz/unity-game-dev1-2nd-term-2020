using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
