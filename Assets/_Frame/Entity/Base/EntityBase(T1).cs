using UnityEngine;

public abstract class EntityBase<T> : EntityBase where T : EntityBase<T>
{
    public static T Ins
    {
        get
        {
            if (Application.isPlaying && ApplicationIsQuitting)
            {
                return null;
            }

            if (Instance != null) return Instance;
            Instance = FindObjectOfType<T>();
            if (Instance != null)
            {
                return Instance;
            }

            var hideFlag = HideFlags.None;
            var insName = typeof(T).Name;
            if (!Application.isPlaying)
            {
                insName += " (Editor)";
            }

            var obj = new GameObject
            {
                name = insName,
                hideFlags = hideFlag,
            };

            Instance = obj.AddComponent<T>();
            if (Application.isPlaying) DontDestroyOnLoad(Instance);
            return Instance;
        }
    }

    internal static bool ApplicationIsQuitting = false;
    internal static T Instance;

    protected override void Awake()
    {
        base.Awake();
        if (Instance != this)
        {
            Instance = this as T;
        }
    }
}