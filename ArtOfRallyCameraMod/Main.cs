using ArtOfRallyCameraMod.Camera;
using UnityModManagerNet;

namespace ArtOfRallyCameraMod
{
    public static class Main
    {
        private static UnityModManager.ModEntry _mod;
        public static Settings.Settings Settings;

        // Send a response to the mod manager about the launch status, success or not.
        // ReSharper disable once UnusedMember.Local
        private static bool Load(UnityModManager.ModEntry modEntry)
        {
            _mod = modEntry;
            Settings = UnityModManager.ModSettings.Load<Settings.Settings>(modEntry);
            modEntry.OnUpdate = CameraHandler.OnUpdate;
            modEntry.OnFixedGUI = EditorGUI.OnFixedGUI;
            modEntry.OnGUI = entry =>  Settings.Draw(entry);;
            modEntry.OnSaveGUI = entry => Settings.Save(entry);;
            modEntry.Logger.Log("Camera Mod loaded but not initialized");

            return true; // If false the mod will show an error.
        }

        public static void ApplySettings()
        {
            // load changed camera settings from mod settings
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