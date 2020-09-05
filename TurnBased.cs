using Kingmaker.EntitySystem.Entities;
using Kingmaker.UnitLogic;
using Kingmaker.Visual.Animation.Kingmaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Kingmaker.UnitLogic.Commands.Base.UnitCommand;

namespace KingmakerAI
{
    class TurnBasedPatches
    {
        internal class TurnController_ContinueActing_Patch
        {
            static int attempts;
            internal static void Postfix(object __instance, ref bool __result)
            {
                if (__result)
                {
                    attempts = 0;
                    return;
                }

                var unit = CallOfTheWild.Helpers.GetField<UnitEntityData>(__instance, "Unit");
                if (!unit.Descriptor.State.CanAct || unit.IsDirectlyControllable)
                {
                    attempts = 0;
                    return;
                }
                attempts++;
                var tr = Harmony12.Traverse.Create(__instance);
                var t = tr.Property("TimeWaitedForIdleAI ").GetValue<float>();
                __result = !(unit.CombatState.HasCooldownForCommand(CommandType.Standard) || unit.CombatState.HasCooldownForCommand(CommandType.Swift)
                             || unit.CombatState.Cooldown.MoveAction > 1) && t < 3.0f && attempts < 100; //3s  max
                if (!__result)
                {
                    attempts = 0;
                }
            }
        }


        internal class TurnController_ContinueWaiting_Patch
        {
            internal static void Postfix(object __instance, ref bool __result)
            {
                if (__result)
                {
                    return;
                }

                var unit = CallOfTheWild.Helpers.GetField<UnitEntityData>(__instance, "Unit");
                if (!unit.Descriptor.State.CanAct || unit.IsDirectlyControllable)
                {
                    return;
                }

                var tr = Harmony12.Traverse.Create(__instance);
                float t = tr.Property("TimeWaitedToEndTurn").GetValue<float>();
                __result = !(unit.CombatState.HasCooldownForCommand(CommandType.Standard) || unit.CombatState.HasCooldownForCommand(CommandType.Swift) || 
                            unit.CombatState.Cooldown.MoveAction > 1) && t < 3.0f; //3s  max
            }
        }
    }
}
