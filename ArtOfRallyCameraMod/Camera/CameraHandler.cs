using System.Collections.Generic;
using ArtOfRallyCameraMod.State;
using UnityEngine;
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

        public static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            if (SceneryManager == null || ModState.IsInitialized) return;

            if (!GameState.IsInPhotoMode && Input.GetKeyUp(Main.Settings.CameraEditor.keyCode))
                ModState.IsCameraEditor = !ModState.IsCameraEditor;
            if (ModState.IsCameraEditor) CameraEditor.Edit(modEntry);
        }

        public static void InitCams()
        {
            CarCamera = Object.FindObjectOfType<CarCameras>();
            if (CarCamera == null) return;

            ModState.IsInitialized = true;
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
            CameraAngles[8].distance = Main.Settings.Camera8Distance;
            CameraAngles[8].height = Main.Settings.Camera8Height;
            CameraAngles[8].initialPitchAngle = Main.Settings.Camera8Angle;
            YawResetSpeeds[8] = Main.Settings.Camera8YawResetSpeed;

            CameraAngles[9].distance = Main.Settings.Camera9Distance;
            CameraAngles[9].height = Main.Settings.Camera9Height;
            CameraAngles[9].initialPitchAngle = Main.Settings.Camera9Angle;
            YawResetSpeeds[9] = Main.Settings.Camera9YawResetSpeed;
        }
    }
}