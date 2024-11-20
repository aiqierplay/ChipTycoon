using Sirenix.OdinInspector;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[Searchable]
public class TestManager : EntityBase<TestManager>
{
    public void Update()
    {
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.W))
        {
            GameWin();
        }

        if (Input.GetKeyUp(KeyCode.L))
        {
            GameLose();
        }

        if (Input.GetKeyUp(KeyCode.N))
        {
            NextLevel();
        }

        if (Input.GetKeyUp(KeyCode.E))
        {
            GoToEndless();
        }
#endif
    }

    #region Game

    [BoxGroup("Game"), HorizontalGroup("Game/Control")]
    [Button("Win (W)")]
    [GUIColor(0.75f, 1f, 0.75f)]
    public void GameWin()
    {
        App.Enter<GameWin>();
    }

    [BoxGroup("Game"), HorizontalGroup("Game/Control")]
    [Button("Lose (L)")]
    [GUIColor(1f, 0.75f, 0.75f)]
    public void GameLose()
    {
        App.Enter<GameLose>();
    }

    [BoxGroup("Game"), HorizontalGroup("Game/Level")]
    [Button("Next Level (N)")]
    public void NextLevel()
    {
        Level.NextLevel();
    }

    [BoxGroup("Game"), HorizontalGroup("Game/Level")]
    [Button("Go To Endless (E)")]
    public void GoToEndless()
    {
        var block = CurrentLevel.GetItems<ItemChangeGamePhase>().Find(i => i.GamePhase == GamePhaseType.Win).GetComponentInParent<LevelBlock>();
        Player.Move.EnterBlock(CurrentLevel.RunnerBlockInsList.IndexOf(block));
        Player.Move.MovePath(0f);
    }

    #endregion

    #region Coin

    [BoxGroup("Coin"), HorizontalGroup("Coin/Add")]
    [Button("+100")]
    public void AddCoin100()
    {
        AddCoin(100);
    }

    [BoxGroup("Coin"), HorizontalGroup("Coin/Add")]
    [Button("+1000")]
    public void AddCoin1000()
    {
        AddCoin(1000);
    }

    [BoxGroup("Coin"), HorizontalGroup("Coin/Add")]
    [Button("+10000")]
    public void AddCoin10000()
    {
        AddCoin(10000);
    }

    [BoxGroup("Coin"), HorizontalGroup("Coin/Add")]
    [Button("+100000")]
    public void AddCoin100000()
    {
        AddCoin(100000);
    }

    [BoxGroup("Coin")]
    [GUIColor(1f, 0.75f, 0.75f)]
    [Button(SdfIconType.Trash, " Clear Coin")]
    public void ClearCoin()
    {
        Save.Coin.Value = 0;
    }

    public void AddCoin(int count)
    {
        Save.Coin.Value += count;
    }

    #endregion

    #region Diamond

    [BoxGroup("Diamond"), HorizontalGroup("Diamond/Add")]
    [Button("+10")]
    public void AddDiamond10()
    {
        AddDiamond(10);
    }

    [BoxGroup("Diamond"), HorizontalGroup("Diamond/Add")]
    [Button("+100 (D)")]
    public void AddDiamond100()
    {
        AddDiamond(100);
    }

    [BoxGroup("Diamond"), HorizontalGroup("Diamond/Add")]
    [Button("+1000")]
    public void AddDiamond1000()
    {
        AddDiamond(1000);
    }

    [BoxGroup("Diamond"), HorizontalGroup("Diamond/Add")]
    [Button("+10000")]
    public void AddDiamond10000()
    {
        AddDiamond(10000);
    }

    [BoxGroup("Diamond")]
    [GUIColor(1f, 0.75f, 0.75f)]
    [Button(SdfIconType.Trash, " Clear Diamond")]
    public void ClearDiamond()
    {
        Save.Diamond.Value = 0;
    }

    public void AddDiamond(int count)
    {
        Save.Diamond.Value += count;
    }

    #endregion

    #region Key

    [BoxGroup("Key"), HorizontalGroup("Key/Add")]
    [Button("+1")]
    public void AddKey()
    {
        AddKey(1);
    }

    [BoxGroup("Key"), HorizontalGroup("Key/Add")]
    [Button("+2")]
    public void AddKey2()
    {
        AddKey(2);
    }

    [BoxGroup("Key"), HorizontalGroup("Key/Add")]
    [Button("+3")]
    public void AddKey3()
    {
        AddKey(3);
    }

    public void AddKey(int count)
    {
        Save.Key.Value += count;
    }

    #endregion

    #region Save

    [BoxGroup("Save")]
    [Button(SdfIconType.Trash, " Clear Save Data"), GUIColor(1f, 0.5f, 0.5f)]
    public void ClearSaveData()
    {
        PlayerPrefs.DeleteAll();
    }

    #endregion

    #region Quick Select

#if UNITY_EDITOR

    [BoxGroup("Quick Select")]
    [Button("Player Animator")]
    public void SelectPlayerAnimator()
    {
        var animator = Player.Animator;
        if (animator == null) return;
        Selection.activeGameObject = animator.gameObject;
    }
#endif

    #endregion
}