using ArtOfRallyCameraMod.Camera;
using ArtOfRallyCameraMod.State;
using HarmonyLib;

namespace ArtOfRallyCameraMod.Patches.SceneryManager
{
    [HarmonyPatch(typeof(global::SceneryManager), nameof(global::SceneryManager.Init))]
    public class SceneryManagerInitPatch
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(global::SceneryManager __instance)
        {
            ModState.IsInitialized = false;
            ModState.IsCameraEditor = false;

            CameraHandler.SceneryManager = __instance;

            CameraHandler.InitCams();
        }
    }
}