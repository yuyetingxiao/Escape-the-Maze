using UnityEngine;
using System.Collections.Generic;

public class LevelProgressManager : MonoBehaviour
{
    public static LevelProgressManager Instance;

    private List<LevelData> levelDatas = new List<LevelData>();
    private int totalLevels = 4;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitLevelData();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitLevelData()
    {
        levelDatas.Clear();
        for (int i = 0; i < totalLevels; i++)
        {
            levelDatas.Add(new LevelData
            {
                levelIndex = i,
                currentBeans = 0,
                targetBeans = 0,
                remainingTime = 0,
                isCompleted = false
            });
        }
    }

    public void SetLevelTarget(int levelIndex, int targetBeans, float maxTime)
    {
        if (levelIndex >= 0 && levelIndex < totalLevels)
        {
            levelDatas[levelIndex].targetBeans = targetBeans;
            levelDatas[levelIndex].maxTime = maxTime;
        }
    }

    public void UpdateLevelProgress(int levelIndex, int currentBeans, float remainingTime)
    {
        if (levelIndex >= 0 && levelIndex < totalLevels)
        {
            levelDatas[levelIndex].currentBeans = currentBeans;
            levelDatas[levelIndex].remainingTime = remainingTime;
            levelDatas[levelIndex].isCompleted = currentBeans >= levelDatas[levelIndex].targetBeans;
        }
    }

    public LevelData GetLevelData(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < totalLevels)
            return levelDatas[levelIndex];
        return null;
    }

    public void ResetAllProgress()
    {
        InitLevelData();
    }

    public class LevelData
    {
        public int levelIndex;
        public int currentBeans;
        public int targetBeans;
        public float maxTime;
        public float remainingTime;
        public bool isCompleted;
    }
}