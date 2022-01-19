﻿using System.Collections.Generic;
using System.Reflection;
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

            if (ModState.IsCameraEditor) CameraEditor.Edit(modEntry);
        }

        private static void InitCams(UnityModManager.ModEntry modEntry)
        {
            CarCamera = Object.FindObjectOfType<CarCameras>();
            if (CarCamera == null) return;

            var prop = CarCamera.GetType().GetField("CameraAnglesList",
                BindingFlags.NonPublic | BindingFlags.Instance);
            if (CameraAngles == null && prop != null)
            {
                // swap cameras to use the modded class
                CameraAngles = ((List<CameraAngle>)prop.GetValue(CarCamera));
                prop.SetValue(CarCamera, CameraAngles);

                //TODO: hood cam
                CameraAngles.Add(new CameraAngle(7f, 2f, -1f, (CameraAngle.CameraAngles)8));
                CameraAngles.Add(new CameraAngle(10f, 3f, -1.5f, (CameraAngle.CameraAngles)9));
                YawResetSpeeds = CameraAngles.ConvertAll(angle => 10f);

                //Deep Copy original cameras
                CameraAnglesOriginals = CameraAngles.ConvertAll(camera =>
                    new CameraAngle(camera.distance, camera.height, camera.initialPitchAngle, camera.cameraType));
                
                LoadCamerasFromSettings();
                modEntry.Logger.Log("Initialized cameras and loaded cameras from settings");
            }

            prop?.SetValue(CarCamera, CameraAngles);

            modEntry.Logger.Log("Camera Mod initialized");

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