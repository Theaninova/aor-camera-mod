using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

// ReSharper disable UnusedType.Global
// ReSharper disable UnusedMember.Global

namespace ArtOfRallyCameraMod.Patches.CarCameras
{
    [HarmonyPatch(typeof(global::CarCameras), "Update")]
    public class UpdatePatch
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var method = SymbolExtensions.GetMethodInfo(() => SettingsManager.GetRotationDamping());
            var code = new List<CodeInstruction>(instructions);

            for (var i = 0; i < code.Count - 1; i++)
            {
                if (!code[i].Calls(method)) continue;

                Main.Logger.Error("Found");
                code[i + 1].operand = Main.Settings.MaximumStiffness;
                code[i + 2].operand = Main.Settings.MinimumStiffness;
                break;
            }

            return code.AsEnumerable();
        }

        // ReSharper disable twice InconsistentNaming
        /*public static void Postfix(ref float ___currentYawRotationAngle, ref float ___wantedYawRotationAngle)
        {
            ___currentYawRotationAngle = ___wantedYawRotationAngle;
        }*/
    }
}