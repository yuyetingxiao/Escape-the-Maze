using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuProgressPanel : MonoBehaviour
{
    [Header("4个关卡的进度文本")]
    public Text[] levelProgressTexts; // 按顺序拖入4个关卡的Text
    [Header("4个关卡的跳转按钮")]
    public Button[] levelJumpButtons; // 按顺序拖入4个关卡的按钮

    void OnEnable()
    {
        // 面板打开时，刷新所有关卡进度
        RefreshAllProgress();
    }

    // 刷新所有关卡进度
    void RefreshAllProgress()
    {
        if (LevelProgressManager.Instance == null) return;

        for (int i = 0; i < levelProgressTexts.Length; i++)
        {
            var data = LevelProgressManager.Instance.GetLevelData(i);
            if (data == null || levelProgressTexts[i] == null) continue;

            // 显示进度：已收集/目标
            levelProgressTexts[i].text = $"{data.currentBeans}/{data.targetBeans}";

            // 按钮绑定：点击跳转到对应关卡
            int levelIndex = i; // 闭包捕获
            levelJumpButtons[i].onClick.RemoveAllListeners();
            levelJumpButtons[i].onClick.AddListener(() => JumpToLevel(levelIndex));
        }
    }

    // 跳转到指定关卡
    void JumpToLevel(int levelIndex)
    {
        SceneManager.LoadScene($"game{levelIndex + 1}");
    }

    // 关闭面板按钮
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}