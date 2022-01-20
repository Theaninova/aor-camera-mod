using System.Collections.Generic;
using UnityModManagerNet;

namespace ArtOfRallyCameraMod.Camera
{
    public static class CameraHandler
    {
        public static List<CameraAngle> CameraAngles;
        public static List<CameraAngle> CameraAnglesOriginals;
        public static List<float> YawResetSpeeds;
        public static SceneryManager SceneryManager;
        public static CarCameras CarCamera;

        public static bool IsInitialized;
        public static bool IsInPhotoMode;
        public static bool IsCameraEditor;

        public const int CustomCamera1 = (int)CameraAngle.CameraAngles.CAMERA8 + 1;
        public const int CustomCamera2 = CustomCamera1 + 1;

        public static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (SceneryManager == null || IsInitialized) return;

            if (!IsInPhotoMode && Input.GetKeyUp(Main.Settings.CameraEditor.keyCode))
                IsCameraEditor = !IsCameraEditor;
            if (IsCameraEditor) CameraEditor.Edit(modEntry);
        }

        public static void UpdateCamera(int camIndex)
        {
            CarCamera.distance = CameraAngles[camIndex].distance;
            CarCamera.height = CameraAngles[camIndex].height;
            CarCamera.initialPitchAngle = CameraAngles[camIndex].initialPitchAngle;
            CarCamera.yawResetSpeed = YawResetSpeeds[camIndex];
            CarCamera.SetToWantedPositionImmediate();
        }

        public static void LoadCamerasFromSettings()
        {
            CameraAngles[CustomCamera1].distance = Main.Settings.Camera8Distance;
            CameraAngles[CustomCamera1].height = Main.Settings.Camera8Height;
            CameraAngles[CustomCamera1].initialPitchAngle = Main.Settings.Camera8Angle;

            CameraAngles[CustomCamera2].distance = Main.Settings.Camera9Distance;
            CameraAngles[CustomCamera2].height = Main.Settings.Camera9Height;
            CameraAngles[CustomCamera2].initialPitchAngle = Main.Settings.Camera9Angle;
        }
    }
}