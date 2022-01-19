namespace ArtOfRallyCameraMod.Camera
{
    public class ModdedCameraAngle : CameraAngle
    {
        public float YawResetSpeed;

        public ModdedCameraAngle(
            float distance,
            float height,
            float initialPitchAngle,
            CameraAngles cameraType,
            float yawResetSpeed = 10f
        ) : base(distance, height, initialPitchAngle, cameraType)
        {
            this.YawResetSpeed = yawResetSpeed;
        }
    }
}