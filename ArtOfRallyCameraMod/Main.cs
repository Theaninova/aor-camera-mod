using System.Collections.Generic;
using System.Reflection;
using ArtOfRallyCameraMod.Classes;
using UnityEngine;
using UnityModManagerNet;

namespace ArtOfRallyCameraMod
{
    public static class Main
    {
        private static UnityModManager.ModEntry _mod;
        private static CarCameras _camera;
        private static readonly Rect LabelRect = new Rect(70, 40, 200, 200);
        private static List<CameraAngle> _cameraAngles;
        private static List<CameraAngle> _cameraAnglesOriginals;
        private static SceneryManager _sceneryManager;
        private static Settings _settings;

        // Send a response to the mod manager about the launch status, success or not.
        // ReSharper disable once UnusedMember.Local
        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            _mod = modEntry;
            _settings = UnityModManager.ModSettings.Load<Settings>(modEntry);
            modEntry.OnUpdate = OnUpdate;
            modEntry.OnFixedGUI = OnFixedGUI;
            modEntry.OnGUI = OnGUI;
            modEntry.OnSaveGUI = OnSaveGUI;
            modEntry.Logger.Log("Camera Mod loaded but not initialized");

            return true; // If false the mod will show an error.
        }

        private static void OnUpdate(UnityModManager.ModEntry modEntry, float dt)
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
                if (Input.GetKeyUp(_settings.CameraEditor.keyCode) && !GameState.IsInPhotoMode)
                {
                    ModState.IsCameraEditor = !ModState.IsCameraEditor;
                }
            }

            if (ModState.IsCameraEditor)
            {
                bool isKeyPressed = false;
                int camIndex = (int)_camera.CurrentCameraAngle.cameraType;
                // -Height
                if (Input.GetKeyUp(_settings.HeightMinus.keyCode))
                {
                    _cameraAngles[camIndex].height -= 0.5f;
                    isKeyPressed = true;
                }

                // +Height
                if (Input.GetKeyUp(_settings.HeightPlus.keyCode))
                {
                    _cameraAngles[camIndex].height += 0.5f;
                    isKeyPressed = true;
                }

                // -Distance
                if (Input.GetKeyUp(_settings.DistanceMinus.keyCode))
                {
                    _cameraAngles[camIndex].distance -= 0.5f;
                    isKeyPressed = true;
                }

                // +Distance
                if (Input.GetKeyUp(_settings.DistancePlus.keyCode))
                {
                    _cameraAngles[camIndex].distance += 0.5f;
                    isKeyPressed = true;
                }

                // -Angle
                if (Input.GetKeyUp(_settings.AngleMinus.keyCode))
                {
                    _cameraAngles[camIndex].initialPitchAngle -= 0.5f;
                    isKeyPressed = true;
                }

                // +Angle
                if (Input.GetKeyUp(_settings.AnglePlus.keyCode))
                {
                    _cameraAngles[camIndex].initialPitchAngle += 0.5f;
                    isKeyPressed = true;
                }

                // Reset Camera
                if (Input.GetKeyUp(_settings.ResetCamera.keyCode))
                {
                    _cameraAngles[camIndex].distance = _cameraAnglesOriginals[camIndex].distance;
                    _cameraAngles[camIndex].height = _cameraAnglesOriginals[camIndex].height;
                    _cameraAngles[camIndex].initialPitchAngle = _cameraAnglesOriginals[camIndex].initialPitchAngle;
                    isKeyPressed = true;
                }

                // Update Current Cam
                if (isKeyPressed)
                {
                    Main.UpdateCamera(camIndex);
                    // if custom camera is active -> save changed settings to mod settings
                    switch (camIndex)
                    {
                        case 8:
                            _settings.Camera8Distance = _cameraAngles[camIndex].distance;
                            _settings.Camera8Height = _cameraAngles[camIndex].height;
                            _settings.Camera8Angle = _cameraAngles[camIndex].initialPitchAngle;
                            _settings.Save(modEntry);
                            break;
                        case 9:
                            _settings.Camera9Distance = _cameraAngles[camIndex].distance;
                            _settings.Camera9Height = _cameraAngles[camIndex].height;
                            _settings.Camera9Angle = _cameraAngles[camIndex].initialPitchAngle;
                            _settings.Save(modEntry);
                            break;
                    }
                }
            }
        }

        static void OnFixedGUI(UnityModManager.ModEntry modEntry)
        {
            // show camera values when editor is enabled
            if (ModState.IsCameraEditor)
            {
                GUI.Label(LabelRect, $"Camera Editor\n" +
                                     $"Current Camera: {(int)_camera.CurrentCameraAngle.cameraType}\n" +
                                     $"Height: {_camera.height}\n" +
                                     $"Distance: {_camera.distance}\n" +
                                     $"Pitch Angle: {_camera.initialPitchAngle}\n" +
                                     $"\n" +
                                     $"{_settings.HeightMinus.keyCode}: Height -0.5\n" +
                                     $"{_settings.HeightPlus.keyCode}: Height +0.5\n" +
                                     $"{_settings.DistanceMinus.keyCode}: Distance -0.5\n" +
                                     $"{_settings.DistancePlus.keyCode}: Distance +0.5\n" +
                                     $"{_settings.AngleMinus.keyCode}: Pitch Angle -0.5\n" +
                                     $"{_settings.AnglePlus.keyCode}: Pitch Angle +0.5\n" +
                                     $"\n" +
                                     $"{_settings.ResetCamera.keyCode}: Reset Camera");
            }
        }

        /// <summary>
        /// Initialize new cameras and add it to the existing game cameras
        /// </summary>
        /// <param name="modEntry">UnityModManager.ModEntry</param>
        private static void InitCams(UnityModManager.ModEntry modEntry)
        {
            _camera = Object.FindObjectOfType<CarCameras>();
            if (_camera == null)
            {
                return;
            }

            var prop = _camera.GetType().GetField("CameraAnglesList",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (_cameraAngles == null && prop != null)
            {
                _cameraAngles = (List<CameraAngle>)prop.GetValue(_camera);
                //TODO: hood cam
                _cameraAngles.Add(new CameraAngle(7f, 2f, -1f, (CameraAngle.CameraAngles)8));
                _cameraAngles.Add(new CameraAngle(10f, 3f, -1.5f, (CameraAngle.CameraAngles)9));

                //Deep Copy original cameras
                _cameraAnglesOriginals = _cameraAngles.ConvertAll(camera =>
                    new CameraAngle(camera.distance, camera.height, camera.initialPitchAngle, camera.cameraType));
                Main.LoadCamerasFromSettings();
                modEntry.Logger.Log("Initialized cameras and loaded cameras from settings");
            }

            prop?.SetValue(_camera, _cameraAngles);

            modEntry.Logger.Log("Camera Mod initialized");

            ModState.IsInitialized = true;
        }

        /// <summary>
        /// Update camera to a given index
        /// </summary>
        /// <param name="camIndex"></param>
        private static void UpdateCamera(int camIndex)
        {
            _camera.distance = _cameraAngles[camIndex].distance;
            _camera.height = _cameraAngles[camIndex].height;
            _camera.initialPitchAngle = _cameraAngles[camIndex].initialPitchAngle;
            _camera.SetToWantedPositionImmediate();
        }

        /// <summary>
        /// Apply saved mod settings to mod
        /// </summary>
        public static void ApplySettings()
        {
            // load changed camera settings from mod settings
            Main.LoadCamerasFromSettings();
            // if current camera is a custom camera -> update current camera
            var camIndex = (int)_camera.CurrentCameraAngle.cameraType;
            if (camIndex > 7)
            {
                UpdateCamera(camIndex);
            }
        }

        /// <summary>
        /// Mod menu GUI
        /// </summary>
        /// <param name="modEntry"></param>
        private static void OnGUI(UnityModManager.ModEntry modEntry)
        {
            _settings.Draw(modEntry);
        }

        /// <summary>
        /// Save Mod Menu settings
        /// </summary>
        /// <param name="modEntry"></param>
        static void OnSaveGUI(UnityModManager.ModEntry modEntry)
        {
            _settings.Save(modEntry);
        }

        /// <summary>
        /// Load camera settings from saved mod settings
        /// </summary>
        private static void LoadCamerasFromSettings()
        {
            _cameraAngles[8].distance = _settings.Camera8Distance;
            _cameraAngles[8].height = _settings.Camera8Height;
            _cameraAngles[8].initialPitchAngle = _settings.Camera8Angle;

            _cameraAngles[9].distance = _settings.Camera9Distance;
            _cameraAngles[9].height = _settings.Camera9Height;
            _cameraAngles[9].initialPitchAngle = _settings.Camera9Angle;
        }
    }
}