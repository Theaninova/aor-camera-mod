using System.Reflection;
using ArtOfRallyCameraMod.Camera;
using HarmonyLib;
using UnityModManagerNet;

namespace ArtOfRallyCameraMod
{
    public static class Main
    {
        public static Settings.Settings Settings;

        // ReSharper disable once UnusedMember.Local
        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = new Harmony(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Settings = UnityModManager.ModSettings.Load<Settings.Settings>(modEntry);
            modEntry.OnUpdate = CameraHandler.OnUpdate;
            modEntry.OnFixedGUI = EditorGUI.OnFixedGUI;
            modEntry.OnGUI = entry =>  Settings.Draw(entry);;
            modEntry.OnSaveGUI = entry => Settings.Save(entry);;
            modEntry.Logger.Log("Camera Mod loaded but not initialized");

            return true;
        }

        public static void ApplySettings()
        {
            CameraHandler.LoadCamerasFromSettings();

            // if current camera is a custom camera -> update current camera
            var camIndex = (int) CameraHandler.CarCamera.CurrentCameraAngle.cameraType;
            if (camIndex > 7)
            {
                CameraHandler.UpdateCamera(camIndex);
            }
        }
    }
}