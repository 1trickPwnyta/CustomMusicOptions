using CustomMusic;
using HarmonyLib;
using RimWorld;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace CustomMusicOptions
{
    [HarmonyPatch(typeof(MusicManagerPlay))]
    [HarmonyPatch("AppropriateNow")]
    public static class Patch_MusicManagerPlay
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator il)
        {
            bool ignoring = false;
            bool finished = false;
            Label checkMapLabel = il.DefineLabel();

            foreach (CodeInstruction instruction in instructions)
            {
                if (!finished && instruction.opcode == OpCodes.Ldarg_0)
                {
                    instruction.opcode = OpCodes.Ldarg_1;
                    yield return instruction;
                    yield return new CodeInstruction(OpCodes.Ldarg_0);
                    yield return new CodeInstruction(OpCodes.Call, typeof(SongUtility).Method(nameof(SongUtility.AppropriateNowForCombatStatus)));
                    yield return new CodeInstruction(OpCodes.Brtrue, checkMapLabel);
                    yield return new CodeInstruction(OpCodes.Ldc_I4_0);
                    yield return new CodeInstruction(OpCodes.Ret);
                    ignoring = true;
                }

                if (ignoring && instruction.opcode == OpCodes.Call && (MethodInfo)instruction.operand == typeof(Find).Method("get_AnyPlayerHomeMap"))
                {
                    instruction.labels.Add(checkMapLabel);
                    ignoring = false;
                    finished = true;
                }

                if (!ignoring)
                {
                    yield return instruction;
                }
            }
        }

        public static void Postfix(SongDef song, ref bool __result)
        {
            if (CustomMusicOptionsSettings.DisableVanillaMusic)
            {
                if (!(song is CustomSongDef))
                {
                    __result = false;
                }
            }
        }
    }
}
