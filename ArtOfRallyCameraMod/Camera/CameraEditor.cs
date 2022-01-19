using UnityModManagerNet;

namespace ArtOfRallyCameraMod.Camera
{
    public static class CameraEditor
    {
        private const float MoveDelta = 0.5f;
        
        public static void Edit(UnityModManager.ModEntry modEntry)
        {
            var isKeyPressed = false;
            var camIndex = (int) CameraHandler.CarCamera.CurrentCameraAngle.cameraType;

            if (Input.GetKeyUp(Main.Settings.HeightMinus.keyCode))
            {
                CameraHandler.CameraAngles[camIndex].height -= MoveDelta;
                isKeyPressed = true;
            }

            if (Input.GetKeyUp(Main.Settings.HeightPlus.keyCode))
            {
                CameraHandler.CameraAngles[camIndex].height += MoveDelta;
                isKeyPressed = true;
            }

            if (Input.GetKeyUp(Main.Settings.DistanceMinus.keyCode))
            {
                CameraHandler.CameraAngles[camIndex].distance -= MoveDelta;
                isKeyPressed = true;
            }

            if (Input.GetKeyUp(Main.Settings.DistancePlus.keyCode))
            {
                CameraHandler.CameraAngles[camIndex].distance += MoveDelta;
                isKeyPressed = true;
            }

            if (Input.GetKeyUp(Main.Settings.AngleMinus.keyCode))
            {
                CameraHandler.CameraAngles[camIndex].initialPitchAngle -= MoveDelta;
                isKeyPressed = true;
            }

            if (Input.GetKeyUp(Main.Settings.AnglePlus.keyCode))
            {
                CameraHandler.CameraAngles[camIndex].initialPitchAngle += MoveDelta;
                isKeyPressed = true;
            }

            if (Input.GetKeyUp(Main.Settings.ResetCamera.keyCode))
            {
                CameraHandler.CameraAngles[camIndex].distance = CameraHandler.CameraAnglesOriginals[camIndex].distance;
                CameraHandler.CameraAngles[camIndex].height = CameraHandler.CameraAnglesOriginals[camIndex].height;
                CameraHandler.CameraAngles[camIndex].initialPitchAngle = CameraHandler.CameraAnglesOriginals[camIndex].initialPitchAngle;
                isKeyPressed = true;
            }

            if (isKeyPressed)
            {
                CameraHandler.UpdateCamera(camIndex);
                // if custom camera is active -> save changed settings to mod settings
                switch (camIndex)
                {
                    case 8:
                        Main.Settings.Camera8Distance = CameraHandler.CameraAngles[camIndex].distance;
                        Main.Settings.Camera8Height = CameraHandler.CameraAngles[camIndex].height;
                        Main.Settings.Camera8Angle = CameraHandler.CameraAngles[camIndex].initialPitchAngle;
                        Main.Settings.Camera8YawResetSpeed = CameraHandler.YawResetSpeeds[camIndex];
                        Main.Settings.Save(modEntry);
                        break;
                    case 9:
                        Main.Settings.Camera9Distance = CameraHandler.CameraAngles[camIndex].distance;
                        Main.Settings.Camera9Height = CameraHandler.CameraAngles[camIndex].height;
                        Main.Settings.Camera9Angle = CameraHandler.CameraAngles[camIndex].initialPitchAngle;
                        Main.Settings.Camera9YawResetSpeed = CameraHandler.YawResetSpeeds[camIndex];
                        Main.Settings.Save(modEntry);
                        break;
                }
            }
        }
    }
}