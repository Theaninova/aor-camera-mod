using ArtOfRallyCameraMod.State;
using HarmonyLib;

namespace ArtOfRallyCameraMod.Patches.PixulPhotoMode
{
    public class PixulPhotoModeExitPatch
    {
        [HarmonyPatch(typeof(global::Pixul.PixulPhotoMode), nameof(Pixul.PixulPhotoMode.ExitPhotoMode))]
        public class PixulPhotoModeEnterPatch
        {
            public static void Prefix()
            {
                GameState.IsInPhotoMode = false;
            }
        }
    }
}