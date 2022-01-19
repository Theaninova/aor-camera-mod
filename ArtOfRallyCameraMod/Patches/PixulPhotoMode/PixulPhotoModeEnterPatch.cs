using ArtOfRallyCameraMod.State;
using HarmonyLib;

namespace ArtOfRallyCameraMod.Patches.PixulPhotoMode
{
    [HarmonyPatch(typeof(global::Pixul.PixulPhotoMode), nameof(Pixul.PixulPhotoMode.EnterPhotoMode))]
    [HarmonyPatch(typeof(global::Pixul.PixulPhotoMode), nameof(Pixul.PixulPhotoMode.EnterPhotoModeForView))]
    public class PixulPhotoModeEnterPatch
    {
        public static void Prefix()
        {
            GameState.IsInPhotoMode = true;
            ModState.IsCameraEditor = false;
        }
    }
}