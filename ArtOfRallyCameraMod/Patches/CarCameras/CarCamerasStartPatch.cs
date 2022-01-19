using System.Collections.Generic;
using ArtOfRallyCameraMod.Camera;
using HarmonyLib;

namespace ArtOfRallyCameraMod.Patches.CarCameras
{
    [HarmonyPatch(typeof(global::CarCameras), "Start")]
    public class CarCamerasStartPatch
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(ref List<CameraAngle> ___CameraAnglesList)
        {
            CameraHandler.CameraAngles = ___CameraAnglesList;
            
            //TODO: hood cam
            ___CameraAnglesList.Add(new CameraAngle(7f, 2f, -1f, (CameraAngle.CameraAngles)8));
            ___CameraAnglesList.Add(new CameraAngle(10f, 3f, -1.5f, (CameraAngle.CameraAngles)9));

            CameraHandler.YawResetSpeeds = ___CameraAnglesList.ConvertAll(angle => 10f);
            CameraHandler.CameraAnglesOriginals = ___CameraAnglesList.ConvertAll(camera =>
                new CameraAngle(camera.distance, camera.height, camera.initialPitchAngle, camera.cameraType));
                
            CameraHandler.LoadCamerasFromSettings();
        }
    }
}