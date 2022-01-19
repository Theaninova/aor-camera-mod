using ArtOfRallyCameraMod.Camera;
using HarmonyLib;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global


namespace ArtOfRallyCameraMod.Patches.SceneryManager
{
    [HarmonyPatch(typeof(global::SceneryManager), nameof(global::SceneryManager.Init))]
    public class InitPatch
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(global::SceneryManager __instance)
        {
            CameraHandler.IsInitialized = false;
            CameraHandler.IsCameraEditor = false;

            CameraHandler.SceneryManager = __instance;
        }
    }
}