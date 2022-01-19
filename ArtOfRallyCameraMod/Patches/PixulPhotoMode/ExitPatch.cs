using ArtOfRallyCameraMod.Camera;
using HarmonyLib;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global


namespace ArtOfRallyCameraMod.Patches.PixulPhotoMode
{
    [HarmonyPatch(typeof(global::Pixul.PixulPhotoMode), nameof(Pixul.PixulPhotoMode.ExitPhotoMode))]
    public class ExitPatch
    {
        public static void Prefix()
        {
            CameraHandler.IsInPhotoMode = false;
        }
    }
}