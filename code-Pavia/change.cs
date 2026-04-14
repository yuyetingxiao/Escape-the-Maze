using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class change : MonoBehaviour
{
    // Start is called before the first frame update

    public void game1()
    {
        SceneManager.LoadScene("game1");
    }

    public void game2()
    {
        SceneManager.LoadScene("game2");
    }

    public void game3()
    {
        SceneManager.LoadScene("game3");
    }

    public void game4()
    {
        SceneManager.LoadScene("game4");
    }

    public void start()
    {
        SceneManager.LoadScene("start");
    }

    public void end()
    {
        SceneManager.LoadScene("end");
    }

    // 退出游戏方法
    public void ExitGame()
    {
#if UNITY_EDITOR
        // 在编辑器中退出播放模式
        UnityEditor.EditorApplication.isPlaying = false;
#else
            // 在独立应用程序中退出游戏
            Application.Quit();
#endif

    }
}