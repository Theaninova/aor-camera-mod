using System.Collections.Generic;
using System.Reflection;
using ArtOfRallyCameraMod.State;
using UnityEngine;
using UnityModManagerNet;

namespace ArtOfRallyCameraMod.Camera
{
    public static class CameraHandler
    {
        private static List<CameraAngle> _cameraAngles;
        private static List<CameraAngle> _cameraAnglesOriginals;
        private static SceneryManager _sceneryManager;
        public static CarCameras CarCamera;

        public static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
        {
            // wait for sceneryManager
            if (_sceneryManager == null)
            {
                if (GameState.IsActiveRally)
                {
                    GameState.IsActiveRally = false;
                    modEntry.Logger.Log($"Active Rally State changed: {GameState.IsActiveRally}");
                    modEntry.Logger.Log("Camera Mod uninitialized");
                    ModState.IsInitialized = false;
                    ModState.IsCameraEditor = false;
                }

                _sceneryManager = Object.FindObjectOfType<SceneryManager>();
            }

            // check if ActiveRally state changes in the game
            if (_sceneryManager != null && !GameState.IsActiveRally)
            {
                GameState.IsActiveRally = true;
                modEntry.Logger.Log($"Active Rally State changed: {GameState.IsActiveRally}");
            }

            if (GameState.IsActiveRally && !ModState.IsInitialized)
            {
                InitCams(modEntry);
            }

            if (ControllerButtonDisplay.inPhotoMode != GameState.IsInPhotoMode)
            {
                GameState.IsInPhotoMode = ControllerButtonDisplay.inPhotoMode;
                modEntry.Logger.Log("Photo Mode active: " + GameState.IsInPhotoMode);
                if (GameState.IsInPhotoMode)
                {
                    ModState.IsCameraEditor = false;
                }
            }

            if (ModState.IsInitialized)
            {
                //Enable Camera Editor (only if not in Photo Mode)
                if (Input.GetKeyUp(Main.Settings.CameraEditor.keyCode) && !GameState.IsInPhotoMode)
                {
                    ModState.IsCameraEditor = !ModState.IsCameraEditor;
                }
            }

            if (ModState.IsCameraEditor)
            {
                bool isKeyPressed = false;
                int camIndex = (int)CarCamera.CurrentCameraAngle.cameraType;
                // -Height
                if (Input.GetKeyUp(Main.Settings.HeightMinus.keyCode))
                {
                    _cameraAngles[camIndex].height -= 0.5f;
                    isKeyPressed = true;
                }

                // +Height
                if (Input.GetKeyUp(Main.Settings.HeightPlus.keyCode))
                {
                    _cameraAngles[camIndex].height += 0.5f;
                    isKeyPressed = true;
                }

                // -Distance
                if (Input.GetKeyUp(Main.Settings.DistanceMinus.keyCode))
                {
                    _cameraAngles[camIndex].distance -= 0.5f;
                    isKeyPressed = true;
                }

                // +Distance
                if (Input.GetKeyUp(Main.Settings.DistancePlus.keyCode))
                {
                    _cameraAngles[camIndex].distance += 0.5f;
                    isKeyPressed = true;
                }

                // -Angle
                if (Input.GetKeyUp(Main.Settings.AngleMinus.keyCode))
                {
                    _cameraAngles[camIndex].initialPitchAngle -= 0.5f;
                    isKeyPressed = true;
                }

                // +Angle
                if (Input.GetKeyUp(Main.Settings.AnglePlus.keyCode))
                {
                    _cameraAngles[camIndex].initialPitchAngle += 0.5f;
                    isKeyPressed = true;
                }

                // Reset Camera
                if (Input.GetKeyUp(Main.Settings.ResetCamera.keyCode))
                {
                    _cameraAngles[camIndex].distance = _cameraAnglesOriginals[camIndex].distance;
                    _cameraAngles[camIndex].height = _cameraAnglesOriginals[camIndex].height;
                    _cameraAngles[camIndex].initialPitchAngle = _cameraAnglesOriginals[camIndex].initialPitchAngle;
                    isKeyPressed = true;
                }

                // Update Current Cam
                if (isKeyPressed)
                {
                    UpdateCamera(camIndex);
                    // if custom camera is active -> save changed settings to mod settings
                    switch (camIndex)
                    {
                        case 8:
                            Main.Settings.Camera8Distance = _cameraAngles[camIndex].distance;
                            Main.Settings.Camera8Height = _cameraAngles[camIndex].height;
                            Main.Settings.Camera8Angle = _cameraAngles[camIndex].initialPitchAngle;
                            Main.Settings.Save(modEntry);
                            break;
                        case 9:
                            Main.Settings.Camera9Distance = _cameraAngles[camIndex].distance;
                            Main.Settings.Camera9Height = _cameraAngles[camIndex].height;
                            Main.Settings.Camera9Angle = _cameraAngles[camIndex].initialPitchAngle;
                            Main.Settings.Save(modEntry);
                            break;
                    }
                }
            }
        }

        private static void InitCams(UnityModManager.ModEntry modEntry)
        {
            CarCamera = Object.FindObjectOfType<CarCameras>();
            if (CarCamera == null) return;

            var prop = CarCamera.GetType().GetField("CameraAnglesList",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (_cameraAngles == null && prop != null)
            {
                _cameraAngles = (List<CameraAngle>)prop.GetValue(CarCamera);
                //TODO: hood cam
                _cameraAngles.Add(new CameraAngle(7f, 2f, -1f, (CameraAngle.CameraAngles)8));
                _cameraAngles.Add(new CameraAngle(10f, 3f, -1.5f, (CameraAngle.CameraAngles)9));

                //Deep Copy original cameras
                _cameraAnglesOriginals = _cameraAngles.ConvertAll(camera =>
                    new CameraAngle(camera.distance, camera.height, camera.initialPitchAngle, camera.cameraType));
                LoadCamerasFromSettings();
                modEntry.Logger.Log("Initialized cameras and loaded cameras from settings");
            }

            prop?.SetValue(CarCamera, _cameraAngles);

            modEntry.Logger.Log("Camera Mod initialized");

            ModState.IsInitialized = true;
        }

        public static void UpdateCamera(int camIndex)
        {
            CarCamera.distance = _cameraAngles[camIndex].distance;
            CarCamera.height = _cameraAngles[camIndex].height;
            CarCamera.initialPitchAngle = _cameraAngles[camIndex].initialPitchAngle;
            CarCamera.SetToWantedPositionImmediate();
        }

        public static void LoadCamerasFromSettings()
        {
            _cameraAngles[8].distance = Main.Settings.Camera8Distance;
            _cameraAngles[8].height = Main.Settings.Camera8Height;
            _cameraAngles[8].initialPitchAngle = Main.Settings.Camera8Angle;

            _cameraAngles[9].distance = Main.Settings.Camera9Distance;
            _cameraAngles[9].height = Main.Settings.Camera9Height;
            _cameraAngles[9].initialPitchAngle = Main.Settings.Camera9Angle;
        }
    }
}