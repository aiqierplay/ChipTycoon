using Aya.Extension;
using UnityEngine;

public abstract partial class EntityBase
{
    #region Camera

    public CameraManager Camera => CameraManager.Ins;
    public Camera MainCamera => Camera.Camera;
    public Camera UICamera => UI.Camera;

    public void CameraFollowThis()
    {
        Camera.Current.Camera.Follow = Trans;
    }

    public void CameraLookAtThis()
    {
        Camera.Current.Camera.LookAt = Trans;
    }

    public void CameraFollowAndLookAtThis()
    {
        CameraFollowThis();
        CameraLookAtThis();
    }

    public bool IsInView()
    {
        return Trans.IsInView(MainCamera);
    }

    public bool IsVisibleInMainCamera()
    {
        return Renderer != null && GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(MainCamera), Renderer.bounds);
    }

    #endregion
}
