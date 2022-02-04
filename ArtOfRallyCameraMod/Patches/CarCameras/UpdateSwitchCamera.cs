using System.Reflection;
using HarmonyLib;

// ReSharper disable InconsistentNaming
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ArtOfRallyCameraMod.Patches.CarCameras
{
    internal static class CarCamerasSwitchState
    {
        public static readonly MethodInfo SetCameraFromSave = typeof(global::CarCameras).GetMethod("SetCameraFromSave",
            BindingFlags.Instance | BindingFlags.NonPublic);
    }

    [HarmonyPatch(typeof(global::CarCameras), "UpdateSwitchCamera")]
    public class UpdateSwitchCamera
    {
        public static void Prefix(ref int __state)
        {
            if (Main.Settings.NoOnFlyCameraSwitch && PadManager.GetPlayer().GetButtonDown(61))
            {
                __state = SaveGame.GetInt("SETTINGS_CAMERA_PRESET", 3);
            }
        }

        public static void Postfix(global::CarCameras __instance, ref int __state)
        {
            if (Main.Settings.NoOnFlyCameraSwitch && PadManager.GetPlayer().GetButtonDown(61))
            {
                SaveGame.SetInt("SETTINGS_CAMERA_PRESET", __state);
                CarCamerasSwitchState.SetCameraFromSave.Invoke(__instance, new object[] { false });
            }
        }
    }
}