using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace LemonSkin.OBR
{
    [HarmonyPatch(typeof(PawnColumnWorker_Outfit))]
    class PawnColumnWorker_Outfit_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch("DoCell")]
        static bool Prefix(
            Rect rect,
            Pawn pawn,
            PawnTable table,
            PawnColumnWorker_Outfit __instance
        )
        {
            if (pawn.outfits == null)
            {
                return true;
            }
            Rect rect2 = rect.ContractedBy(0f, 2f);
            bool somethingIsForced = pawn.outfits.forcedHandler.SomethingIsForced;
            Rect left = rect2;
            Rect right = default(Rect);
            if (somethingIsForced)
            {
                rect2.SplitVerticallyWithMargin(out left, out right, 4f);
            }

            if (pawn.IsQuestLodger())
            {
                Text.Anchor = TextAnchor.MiddleCenter;
                Widgets.Label(left, "Unchangeable".Translate().Truncate(left.width));
                TooltipHandler.TipRegionByKey(left, "QuestRelated_Outfit");
                Text.Anchor = TextAnchor.UpperLeft;
            }
            else
            {
                Widgets.Dropdown(
                    left,
                    pawn,
                    (Pawn p) => p.outfits.CurrentApparelPolicy,
                    __instance.Button_GenerateMenu,
                    pawn.outfits.CurrentApparelPolicy.label.Truncate(left.width),
                    null,
                    pawn.outfits.CurrentApparelPolicy.label,
                    null,
                    null,
                    true
                );
            }

            if (!somethingIsForced)
            {
                return true;
            }

            if (Mouse.IsOver(right) && Event.current.type == EventType.MouseDown && Event.current.button == 1)
            {
                List<FloatMenuOption> options =
                [
                    new FloatMenuOption(
                        "Save Outfit",
                        delegate
                        {
                            OutfitBuilderRedux.OutfitBuilderRedux_CreateNewOutfit(pawn);
                        }
                    ),

                    //new FloatMenuOption(
                    //    "Save Outfit As...",
                    //    delegate {
                    //        Find.WindowStack.Add(new Dialog_GiveName(
                    //            pawn.Name.ToStringShort, 
                    //            delegate(string newName)
                    //            {
                    //            OutfitBuilderRedux.OutfitBuilderRedux_SaveOutfitAs(pawn, newName);
                    //            }
                    //        ));
                    //    }
                    //),

                    new FloatMenuOption(
                        "Update Outfit",
                        delegate
                        {
                            OutfitBuilderRedux.OutfitBuilderRedux_UpdateOutfit(pawn);
                        }
                    ),
                    new FloatMenuOption(
                        "Clear Forced Apparel",
                        delegate
                        {
                            pawn.outfits.forcedHandler.Reset();
                        }
                    ),
                ];

                Find.WindowStack.Add(new FloatMenu(options));
                Event.current.Use();
                return false;
            }

            if (Widgets.ButtonText(right, "ClearForcedApparel".Translate()))
            {
                pawn.outfits.forcedHandler.Reset();
            }

            if (Mouse.IsOver(right))
            {
                TooltipHandler.TipRegion(
                    right,
                    new TipSignal(
                        delegate
                        {
                            string text = "ForcedApparel".Translate() + ":\n";
                            foreach (Apparel item in pawn.outfits.forcedHandler.ForcedApparel)
                            {
                                text = text + "\n   " + item.LabelCap;
                            }
                            return text;
                        },
                        pawn.GetHashCode() * 612
                    )
                );
            }
            return false;
        }
    }
}