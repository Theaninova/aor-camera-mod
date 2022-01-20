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
            // ReSharper disable once SwitchStatementMissingSomeEnumCasesNoDefault
            switch (__instance.settingsType)
            {
                case global::SettingsSelectable.SettingsType.CameraPreset:
                    __instance.stringList.Add("Custom Camera 1");
                    __instance.stringList.Add("Custom Camera 2");
                    break;
                case global::SettingsSelectable.SettingsType.CameraRotationDamping:
                    __instance.stringList = SelectableStringLists.PercentageList_StartAtZeroPercentDouble;
                    break;
            }
        }
    }
}