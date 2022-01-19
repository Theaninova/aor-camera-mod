using HarmonyLib;
// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ArtOfRallyCameraMod.Patches.SettingsSelectable
{
    [HarmonyPatch(typeof(global::SettingsSelectable), "InitButtons")]
    public class InitButtons
    {
        // ReSharper disable once InconsistentNaming
        public static void Postfix(global::SettingsSelectable __instance)
        {
            if (__instance.settingsType != global::SettingsSelectable.SettingsType.CameraPreset) return;

            __instance.stringList.Add("Custom Camera 1");
            __instance.stringList.Add("Custom Camera 2");
        }
    }
}