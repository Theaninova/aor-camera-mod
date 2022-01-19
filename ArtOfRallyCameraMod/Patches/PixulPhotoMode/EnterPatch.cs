using ArtOfRallyCameraMod.Camera;
using HarmonyLib;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global


namespace ArtOfRallyCameraMod.Patches.PixulPhotoMode
{
    [HarmonyPatch(typeof(global::Pixul.PixulPhotoMode), nameof(Pixul.PixulPhotoMode.EnterPhotoMode))]
    [HarmonyPatch(typeof(global::Pixul.PixulPhotoMode), nameof(Pixul.PixulPhotoMode.EnterPhotoModeForView))]
    public class EnterPatch
    {
        public static void Prefix()
        {
            CameraHandler.IsInPhotoMode = true;
            CameraHandler.IsCameraEditor = false;
        }
    }
}