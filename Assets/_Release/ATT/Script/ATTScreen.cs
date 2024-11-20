using System;
using UnityEngine;

namespace Unity.Advertisement.IosSupport.Components
{
    /// <summary>
    /// This component controls an iOS App Tracking Transparency context screen.
    /// You should only have one of these in your app.
    /// </summary>
    public sealed class ATTScreen : MonoBehaviour
    {
        /// <summary>
        /// This event will be invoked after the ContinueButton is clicked
        /// and after the tracking authorization request has been sent.
        /// It's a good idea to subscribe to this event so you can destroy
        /// this GameObject to free up memory after it's no longer needed.
        /// Once the tracking authorization request has been sent, there's no
        /// need for this popup again until the app is uninstalled and reinstalled.
        /// </summary>
#if UNITY_IOS
        public event Action sentTrackingAuthorizationRequest;
#endif

        public void RequestAuthorizationTracking()
        {
#if UNITY_IOS && SuperSonic
            Debug.Log("Unity iOS Support: Requesting iOS App Tracking Transparency native dialog.");

            ATTrackingStatusBinding.RequestAuthorizationTracking();

            sentTrackingAuthorizationRequest?.Invoke();
#else
            Debug.LogWarning("Unity iOS Support: Tried to request iOS App Tracking Transparency native dialog, " +
                             "but the current platform is not iOS.");
#endif
        }
    }
}
