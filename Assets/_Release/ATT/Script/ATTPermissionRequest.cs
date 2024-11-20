using UnityEngine;
#if UNITY_IOS && SuperSonic
// Include the IosSupport namespace if running on iOS:
using Unity.Advertisement.IosSupport.Components;
using Unity.Advertisement.IosSupport;
#endif

public class ATTPermissionRequest : MonoBehaviour
{
#if UNITY_IOS && SuperSonic
    public ATTScreen ScreenPrefab;

    public void Awake()
    {
        // Check the user's consent status.
                    var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

            if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
            {
                var contextScreen = Instantiate(ScreenPrefab).GetComponent<ATTScreen>();

                // after the Continue button is pressed, and the tracking request
                // has been sent, automatically destroy the popup to conserve memory
                contextScreen.sentTrackingAuthorizationRequest += () => Destroy(contextScreen.gameObject);
            }
    }
#endif
}