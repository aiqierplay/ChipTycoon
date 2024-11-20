using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Aya.TweenPro
{
    [AddComponentMenu("UTween Pro/UTween Manager")]
    public partial class UTweenManager : MonoBehaviour
    {
        #region Singleton

        internal static bool ApplicationIsQuitting = false;
        internal static UTweenManager Instance;

        public static UTweenManager Ins
        {
            get
            {
                if (Application.isPlaying && ApplicationIsQuitting)
                {
                    return null;
                }

                if (Instance != null) return Instance;
                Instance = FindObjectOfType<UTweenManager>();
                if (Instance != null)
                {
                    if (Application.isPlaying) DontDestroyOnLoad(Instance);
                    return Instance;
                }

                var hideFlag = UTweenSetting.Ins.ShowManagerInHierarchy ? HideFlags.None : HideFlags.HideAndDontSave;
                var insName = nameof(UTween);
                if (!Application.isPlaying)
                {
                    insName += " (Editor)";
                }

                var obj = new GameObject
                {
                    name = insName,
                    hideFlags = hideFlag,
                };

                Instance = obj.AddComponent<UTweenManager>();
                if (Application.isPlaying) DontDestroyOnLoad(Instance);
                return Instance;
            }
        }

        #endregion

        #region Cache

        internal HashSet<TweenAnimation> AddList = new HashSet<TweenAnimation>();
        internal HashSet<TweenAnimation> RemoveList = new HashSet<TweenAnimation>();
        internal HashSet<TweenAnimation> PlayingList = new HashSet<TweenAnimation>();

        public int Count => PlayingList.Count + AddList.Count + RemoveList.Count;

        internal Dictionary<UpdateMode, HashSet<TweenAnimation>> UpdateListDic
        {
            get
            {
                if (_updateListDic != null) return _updateListDic;
                _updateListDic = new Dictionary<UpdateMode, HashSet<TweenAnimation>>()
                {
                    {UpdateMode.Update, UpdateList},
                    {UpdateMode.LateUpdate, LateUpdateList},
                    {UpdateMode.FixedUpdate, FixedUpdateList},
                    {UpdateMode.WaitForEndOfFrame, WaitForFixedUpdateList},
                    {UpdateMode.WaitForFixedUpdate, WaitForEndOfFrameList}
                };

                return _updateListDic;
            }
        }

        private Dictionary<UpdateMode, HashSet<TweenAnimation>> _updateListDic;

        internal HashSet<TweenAnimation> UpdateList = new HashSet<TweenAnimation>();
        internal HashSet<TweenAnimation> LateUpdateList = new HashSet<TweenAnimation>();
        internal HashSet<TweenAnimation> FixedUpdateList = new HashSet<TweenAnimation>();
        internal HashSet<TweenAnimation> WaitForFixedUpdateList = new HashSet<TweenAnimation>();
        internal HashSet<TweenAnimation> WaitForEndOfFrameList = new HashSet<TweenAnimation>();

        #endregion

        #region MonoBehaviour

        protected void Awake()
        {
            if (!Application.isPlaying) return;
            DontDestroyOnLoad(gameObject);
            StartCoroutine(WaitForFixedUpdate());
            StartCoroutine(WaitForEndOfFrame());
        }

        protected void OnEnable()
        {

        }

        protected void OnDisable()
        {

        }

        protected IEnumerator WaitForFixedUpdate()
        {
            var wait = new WaitForFixedUpdate();
            while (true)
            {
                yield return wait;
                if (WaitForFixedUpdateList.Count == 0) yield return null;
                var scaledDeltaTime = Time.fixedDeltaTime;
                var unscaledDeltaTime = Time.fixedUnscaledDeltaTime;
                var smoothDeltaTime = Time.fixedDeltaTime;
                UpdateImpl(WaitForFixedUpdateList, scaledDeltaTime, unscaledDeltaTime, smoothDeltaTime);
            }
        }

        protected IEnumerator WaitForEndOfFrame()
        {
            var wait = new WaitForEndOfFrame();
            while (true)
            {
                yield return wait;
                if (WaitForEndOfFrameList.Count == 0) yield return null;
                var scaledDeltaTime = Time.deltaTime;
                var unscaledDeltaTime = Time.unscaledDeltaTime;
                var smoothDeltaTime = Time.smoothDeltaTime;
                UpdateImpl(WaitForEndOfFrameList, scaledDeltaTime, unscaledDeltaTime, smoothDeltaTime);
            }
        }

        protected void Update()
        {
            SyncPlayingList();
            if (UpdateList.Count == 0) return;
            var scaledDeltaTime = Time.deltaTime;
            var unscaledDeltaTime = Time.unscaledDeltaTime;
            var smoothDeltaTime = Time.smoothDeltaTime;
            UpdateImpl(UpdateList, scaledDeltaTime, unscaledDeltaTime, smoothDeltaTime);
            // TODO.. 13 - 15 ms
        }

        protected void LateUpdate()
        {
            SyncPlayingList();
            if (LateUpdateList.Count == 0) return;
            var scaledDeltaTime = Time.deltaTime;
            var unscaledDeltaTime = Time.unscaledDeltaTime;
            var smoothDeltaTime = Time.smoothDeltaTime;
            UpdateImpl(LateUpdateList, scaledDeltaTime, unscaledDeltaTime, smoothDeltaTime);
        }

        protected void FixedUpdate()
        {
            if (FixedUpdateList.Count == 0) return;
            var scaledDeltaTime = Time.fixedDeltaTime;
            var unscaledDeltaTime = Time.fixedUnscaledDeltaTime;
            var smoothDeltaTime = Time.smoothDeltaTime;
            UpdateImpl(FixedUpdateList, scaledDeltaTime, unscaledDeltaTime, smoothDeltaTime);
        }

        protected void OnDestroy()
        {
            if (Application.isPlaying)
            {
                ApplicationIsQuitting = true;
            }

            if (Instance != null)
            {
                Destroy(Instance);
                Instance = null;
            }
        }

        #endregion

        #region Add / Remove TweenAnimation

        public void AddTweenAnimation(TweenAnimation tweenAnimation)
        {
            if (AddList.Contains(tweenAnimation)) return;
            if (RemoveList.Contains(tweenAnimation))
            {
                RemoveList.Remove(tweenAnimation);
            }

            AddList.Add(tweenAnimation);
            UTweenCallback.OnAddAnimation(tweenAnimation);
        }

        public void RemoveTweenAnimation(TweenAnimation tweenAnimation)
        {
            if (RemoveList.Contains(tweenAnimation)) return;
            if (AddList.Contains(tweenAnimation))
            {
                AddList.Remove(tweenAnimation);
            }

            RemoveList.Add(tweenAnimation);
            UTweenCallback.OnRemoveAnimation(tweenAnimation);
        }

        public void AddTweener(Tweener tweener)
        {
            AddTweenAnimation(tweener.Animation);
        }

        public void RemoveTweener(Tweener tweener)
        {
            RemoveTweenAnimation(tweener.Animation);
        }

        internal void SyncPlayingList()
        {
            if (RemoveList.Count > 0)
            {
                foreach (var tweenAnimation in RemoveList)
                {
                    if (PlayingList.Contains(tweenAnimation)) PlayingList.Remove(tweenAnimation);
                    var updateList = UpdateListDic[tweenAnimation.UpdateMode];
                    updateList.Remove(tweenAnimation);
                    if (!AddList.Contains(tweenAnimation))
                    {
                        tweenAnimation.DeSpawn();
                    }
                }

                RemoveList.Clear();
            }

            if (AddList.Count > 0)
            {
                foreach (var tweenAnimation in AddList)
                {
                    if (!PlayingList.Contains(tweenAnimation)) PlayingList.Add(tweenAnimation);
                    var updateList = UpdateListDic[tweenAnimation.UpdateMode];
                    updateList.Add(tweenAnimation);
                    tweenAnimation.AfterAddToManager();
                }

                AddList.Clear();
            }
        }

        #endregion

        internal void UpdateImpl(HashSet<TweenAnimation> updateList, float scaledDeltaTime, float unscaledDeltaTime, float smoothDeltaTime)
        {
            foreach (var tweenAnimation in updateList)
            {
                try
                {
                    var deltaTime = 0f;
                    if (tweenAnimation.TimeMode == TimeMode.Normal) deltaTime = scaledDeltaTime;
                    else if (tweenAnimation.TimeMode == TimeMode.UnScaled) deltaTime = unscaledDeltaTime;
                    else if (tweenAnimation.TimeMode == TimeMode.Smooth) deltaTime = smoothDeltaTime;
                    tweenAnimation.Update(deltaTime);
                }
                catch (Exception exception)
                {
                    UTweenCallback.OnException(exception);
                    RemoveTweenAnimation(tweenAnimation);
                }
            }
        }

        internal void PerformanceTest(Action action)
        {
            var start = Time.realtimeSinceStartup;
            action();
            var end = Time.realtimeSinceStartup;
            var total = end - start;
            var time = Math.Round(total * 1000f, 2);
            Debug.Log("Time : " + time);
        }
    }
}