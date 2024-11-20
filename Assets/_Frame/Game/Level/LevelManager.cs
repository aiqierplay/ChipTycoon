using System;
using Aya.Util;
using Sirenix.OdinInspector;
using UnityEngine;

public class LevelManager : EntityBase<LevelManager>
{
    public int TestLevelIndex = 1;
    
    public ABTestValueReference<LevelListData> LevelAbTest;

    [BoxGroup("Rand Level"), HideLabel]
    public LevelListData LevelListData;
    public int MaxLevel = -1;

    [NonSerialized] public new Level Level;
    [NonSerialized] public GameObject Background;

    public virtual void NextLevel()
    {
        Save.LevelIndex++;
        if (Save.LevelIndex > MaxLevel && MaxLevel > 0) Save.LevelIndex.Value = MaxLevel;

        if (LevelAbTest.EnableAbTest)
        {
            LevelListData = LevelAbTest.GetValue();
        }

        if (Save.LevelIndex >= LevelListData.StartRandIndex && LevelListData.EnableRandLevel)
        {
            Save.RandLevelIndex.Value = RandUtil.RandInt(0, LevelListData.Count);
        }

        LevelStart();
    }

    public void LevelStart()
    {
        var levelIndex = Save.LevelIndex.Value;
        if (TestLevelIndex > 0)
        {
            levelIndex = TestLevelIndex;
        }

        LevelStart(levelIndex);
    }

    public void LevelRetry()
    {
        CurrentLevel.Retry();
        LevelStart();
    }


    public void LevelStart(int levelIndex)
    {
        if (Level != null)
        {
            GamePool.DeSpawn(Level);
            Level = null;
        }

        App.StopAllCoroutines();
        UI.HideAllPage();
        UIPool.DeSpawnAll();
        GamePool.DeSpawnAll();
        EffectPool.DeSpawnAll();

        Level levelPrefab = null;

        if (LevelAbTest.EnableAbTest)
        {
            LevelListData = LevelAbTest.GetValue();
        }

        if (levelIndex >= LevelListData.StartRandIndex && LevelListData.EnableRandLevel)
        {
            var randLevelIndex = Save.RandLevelIndex.Value;
            levelPrefab = LevelListData.RandList[randLevelIndex];
        }

        if (levelPrefab == null)
        {
            var levelPrefabName = "Level_" + levelIndex.ToString("D2");
            if (LevelAbTest.EnableAbTest)
            {
                var path = ABTestSetting.Ins.GetValue(LevelAbTest.GroupKey) + levelPrefabName;
                levelPrefab = Resources.Load<Level>(path);
            }
            else
            {
                var path = $"Level/{levelPrefabName}";
                levelPrefab = Resources.Load<Level>(path);
            }
        }

        if (levelPrefab == null)
        {
            Log(levelIndex, "Level Load Failed!");
            return;
        }

        Level = GamePool.Spawn(levelPrefab);
        Level.Parent = null;
        Level.Init(levelIndex);
        InitEnvironment();

        App.Init();
        App.Enter(App.LaunchPhase);
    }

    public void InitEnvironment()
    {
        if (Background != null)
        {
            GamePool.DeSpawn(Background.gameObject);
            Background = null;
        }

        var environmentData = GetSetting<LevelEnvironmentSetting>().CurrentLevelEnvironment;
        if (environmentData == null) return;
        RenderSettings.fogColor = environmentData.FogColor;
        RenderSettings.skybox = environmentData.SkyBox;
        var roads = GameObject.FindGameObjectsWithTag(Tag.Road);
        foreach (var road in roads)
        {
            road.GetComponent<Renderer>().material = environmentData.RoadMat;
        }

        if (environmentData.Background != null)
        {
            Background = Instantiate(environmentData.Background);
        }
    }
}
