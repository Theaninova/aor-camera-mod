using System.Collections.Generic;
using ArtOfRallyCameraMod.Camera;
using HarmonyLib;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global


namespace ArtOfRallyCameraMod.Patches.CarCameras
{
    [HarmonyPatch(typeof(global::CarCameras), "Start")]
    public class StartPatch
    {
        // ReSharper disable twice InconsistentNaming
        public static void Postfix(global::CarCameras __instance, ref List<CameraAngle> ___CameraAnglesList)
        {
            CameraHandler.CameraAngles = ___CameraAnglesList;
            CameraHandler.CarCamera = __instance;

            //TODO: hood cam
            ___CameraAnglesList.Add(new CameraAngle(7f, 2f, -1f,
                (CameraAngle.CameraAngles)CameraHandler.CustomCamera1));
            ___CameraAnglesList.Add(new CameraAngle(10f, 3f, -1.5f,
                (CameraAngle.CameraAngles)CameraHandler.CustomCamera2));

            CameraHandler.YawResetSpeeds = ___CameraAnglesList.ConvertAll(angle => 10f);
            CameraHandler.CameraAnglesOriginals = ___CameraAnglesList.ConvertAll(camera =>
                new CameraAngle(camera.distance, camera.height, camera.initialPitchAngle, camera.cameraType));

            CameraHandler.LoadCamerasFromSettings();

            CameraHandler.IsInitialized = true;
        }
    }
}