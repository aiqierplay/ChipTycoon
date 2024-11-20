#if SuperSonic
using System;
using Aya.Extension;
using SupersonicWisdomSDK;
using UnityEngine;

namespace Aya.Analysis
{
    public class AnalysicSuperSonic : AnalysisBase
    {
        public override void LevelStart(string level)
        {
            try
            {
                SupersonicWisdom.Api.NotifyLevelStarted(level.AsLong(), null);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public override void LevelCompleted(string level)
        {
            try
            {
                SupersonicWisdom.Api.NotifyLevelCompleted(level.AsLong(), null);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public override void LevelFailed(string level)
        {
            try
            {
                SupersonicWisdom.Api.NotifyLevelFailed(level.AsLong(), null);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public override void LevelSkipped(string level)
        {
            try
            {
                SupersonicWisdom.Api.NotifyLevelSkipped(level.AsLong(), null);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }

        public override void LevelRevived(string level)
        {
            try
            {
                SupersonicWisdom.Api.NotifyLevelRevived(level.AsLong(), null);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
        }
    }
}
#endif