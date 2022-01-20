using System.Reflection;
using ArtOfRallyCameraMod.Camera;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace ArtOfRallyCameraMod
{
    public static class Main
    {
        public static Settings.Settings Settings;
        public static UnityModManager.ModEntry.ModLogger Logger;

        // ReSharper disable once UnusedMember.Local
        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            Logger = modEntry.Logger;
            Settings = UnityModManager.ModSettings.Load<Settings.Settings>(modEntry);

            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            modEntry.OnUpdate = CameraHandler.OnUpdate;
            modEntry.OnFixedGUI = EditorGUI.OnFixedGUI;
            modEntry.OnGUI = entry => Settings.Draw(entry);
            modEntry.OnSaveGUI = entry => Settings.Save(entry);

            return true;
        }

        public static void ApplySettings()
        {
            CameraHandler.LoadCamerasFromSettings();

            // if current camera is a custom camera -> update current camera
            var camIndex = (int)CameraHandler.CarCamera.CurrentCameraAngle.cameraType;
            if (camIndex > 7)
            {
                CameraHandler.UpdateCamera(camIndex);
            }
        }
    }
}