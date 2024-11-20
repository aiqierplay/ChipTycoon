using Aya.Events;

[EventEnum]
public enum GameEvent
{
    // Game Phase
    LoadData = 0,
    LoadLevel = 1,
    NextLevel = 3,
    RetryLevel = 4,

    // Game Phase
    Ready = 10,
    Start = 20,
    Pause = 30,
    Endless = 40,
    Reward = 50,
    Win = 60,
    Lose = 70,

    // Game Func
    Upgrade = 1000,

    // Frame Func
    CompleteGuide = 10000,

    /// <summary>
    /// Key, Follow, LookAt
    /// </summary>
    SwitchCamera = 20000,

    NoAds = 100000,

    Launch = 999999,
}