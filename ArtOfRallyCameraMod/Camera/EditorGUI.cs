using ArtOfRallyCameraMod.State;
using UnityEngine;
using UnityModManagerNet;

namespace ArtOfRallyCameraMod.Camera
{
    public static class EditorGUI
    {
        private static readonly Rect LabelRect = new Rect(70, 40, 200, 200);

        public static void OnFixedGUI(UnityModManager.ModEntry modEntry)
        {
            if (!ModState.IsCameraEditor) return;

            GUI.Label(LabelRect, $"Camera Editor\n" +
                                 $"Current Camera: {(int)CameraHandler.CarCamera.CurrentCameraAngle.cameraType}\n" +
                                 $"Height: {CameraHandler.CarCamera.height}\n" +
                                 $"Distance: {CameraHandler.CarCamera.distance}\n" +
                                 $"Pitch Angle: {CameraHandler.CarCamera.initialPitchAngle}\n" +
                                 $"\n" +
                                 $"{Main.Settings.HeightMinus.keyCode}: Height -0.5\n" +
                                 $"{Main.Settings.HeightPlus.keyCode}: Height +0.5\n" +
                                 $"{Main.Settings.DistanceMinus.keyCode}: Distance -0.5\n" +
                                 $"{Main.Settings.DistancePlus.keyCode}: Distance +0.5\n" +
                                 $"{Main.Settings.AngleMinus.keyCode}: Pitch Angle -0.5\n" +
                                 $"{Main.Settings.AnglePlus.keyCode}: Pitch Angle +0.5\n" +
                                 "\n" +
                                 $"{Main.Settings.ResetCamera.keyCode}: Reset Camera");
        }
    }
}