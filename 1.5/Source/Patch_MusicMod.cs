using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;
using Verse;

namespace CustomMusicOptions
{
    // Patched manually in mod constructor
    public static class Patch_MusicMod
    {
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (CodeInstruction instruction in instructions)
            {
                if (instruction.opcode == OpCodes.Call && instruction.operand is MethodInfo && (MethodInfo)instruction.operand == typeof(Widgets).Method(nameof(Widgets.Checkbox), new[] { typeof(Vector2), typeof(bool).MakeByRefType(), typeof(float), typeof(bool), typeof(bool), typeof(Texture2D), typeof(Texture2D) }))
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_S, 16);
                    yield return new CodeInstruction(OpCodes.Call, typeof(SongUtility).Method(nameof(SongUtility.DoSongTypeWidget)));
                    continue;
                }

                yield return instruction;
            }
        }
    }
}
