using System;
using Cinemachine;

[Serializable]
public class CameraReference
{
    public string Camera;

    public CameraData CameraData
    {
        get
        {
            var cameraData = CameraManager.Ins.GetCamera(Camera);
            return cameraData;
        }
    }

    public CinemachineVirtualCamera VirtualCamera
    {
        get
        {
            var cameraData = CameraData;
            if (cameraData == null) return default;
            return cameraData.Camera;
        }
    }

    public CameraReference(string camera)
    {
        Camera = camera;
    }

    #region Override Operator

    public static implicit operator CameraData(CameraReference cameraReference) => cameraReference.CameraData;
    public static implicit operator CameraReference(CameraData cameraData) => new CameraReference(cameraData.Key);

    public static implicit operator string(CameraReference cameraReference) => cameraReference.Camera;
    public static implicit operator CameraReference(string cameraKey) => new CameraReference(cameraKey);

    public static implicit operator CinemachineVirtualCamera(CameraReference cameraReference) => cameraReference.VirtualCamera;

    #endregion
}