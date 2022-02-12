using HarmonyLib;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items;
using NorthwoodLib.Pools;
using System.Collections.Generic;
using System.Reflection.Emit;
using static HarmonyLib.AccessTools;

namespace CustomisableKeycards
{
    [HarmonyPatch(typeof(DoorPermissions), nameof(DoorPermissions.CheckPermissions))]
    public class CheckPermissionPatch
    {
        public static bool HasValue { get; set; } = false;
        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, ILGenerator generator)
        {
            List<CodeInstruction> newInstructions = ListPool<CodeInstruction>.Shared.Rent(instructions);
            int index = 31;
            Label skipLabel = generator.DefineLabel();
            var inserted = new[]
            {
                new CodeInstruction(OpCodes.Pop),
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call, Method(typeof(CheckPermissionPatch), nameof(CheckPermissionPatch.NewCheck))),
                new CodeInstruction(OpCodes.Call, PropertyGetter(typeof(CheckPermissionPatch), nameof(HasValue))),
                new CodeInstruction(OpCodes.Brfalse_S, skipLabel),
                new CodeInstruction(OpCodes.Ret),
                new CodeInstruction(OpCodes.Pop).WithLabels(skipLabel),
                new CodeInstruction(OpCodes.Ldarg_1)
            };

            newInstructions.InsertRange(index, inserted);

            // newInstructions[index + inserted.Length].WithLabels(skipLabel);

            for (int z = 0; z < newInstructions.Count; z++)
                yield return newInstructions[z];

            ListPool<CodeInstruction>.Shared.Return(newInstructions);
        }

        private static bool NewCheck(DoorPermissions instance, ItemBase item)
        {
            if (Main.Instance.Config.KeycardPermission.TryGetValue(item.ItemTypeId, out var permission))
            {
                HasValue = true;
                return permission.HasFlagFast(instance.RequiredPermissions);
            }
            HasValue = false;
            return false;
        }
    }
}
