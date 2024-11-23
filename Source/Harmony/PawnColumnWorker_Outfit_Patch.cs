using System;
using System.Collections.Generic;
using HarmonyLib;
using RimWorld;
using Verse;
using UnityEngine;

namespace LemonSkin.OBR
{
    [HarmonyPatch(typeof(PawnColumnWorker_Outfit))]
    class PawnColumnWorker_Outfit_Patch
    {
        [HarmonyPrefix]
        [HarmonyPatch("DoCell")]
        static bool Prefix(Rect rect, Pawn pawn, PawnTable table, PawnColumnWorker_Outfit __instance)
        {
            if (pawn.outfits.forcedHandler.SomethingIsForced)
            {
                if (pawn.outfits == null)
                {
                    return true;
                }

                float rect_x = rect.x;
                float rect_y = rect.y + 2f;
                float rect_width_long = Mathf.FloorToInt((rect.width * 0.5f) - 4f);
                float rect_width_short = Mathf.FloorToInt((rect.width * 0.25f) - 4f);
                float rect_height = rect.height - 4f;
               
                bool somethingIsForced = pawn.outfits.forcedHandler.SomethingIsForced;
                Rect outfit_rect = new Rect(rect_x, rect_y, rect_width_long, rect_height);
                
                if (pawn.IsQuestLodger())
                {
                    Text.Anchor = TextAnchor.MiddleCenter;
                    Widgets.Label(outfit_rect, "Unchangeable".Translate().Truncate(outfit_rect.width, null));
                    TooltipHandler.TipRegionByKey(outfit_rect, "QuestRelated_Outfit");
                    Text.Anchor = TextAnchor.UpperLeft;
                }
                else
                {
                    Widgets.Dropdown<Pawn, ApparelPolicy>(outfit_rect,
                                                    pawn,
                                                    (Pawn p) => p.outfits.CurrentApparelPolicy,
                                                    new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<ApparelPolicy>>>(__instance.Button_GenerateMenu),
                                                    pawn.outfits.CurrentApparelPolicy.label.Truncate(outfit_rect.width, null),
                                                    null,
                                                    pawn.outfits.CurrentApparelPolicy.label,
                                                    null,
                                                    null,
                                                    true);
                }

                rect_x += outfit_rect.width + 4f;
                Rect clear_forced_rect = new Rect(rect_x, rect_y, rect_width_short + 3f, rect_height);


                if (Widgets.ButtonText(clear_forced_rect, "OBR.ClearForcedApparel".Translate().Truncate(clear_forced_rect.width), true, true, true))
                {
                    pawn.outfits.forcedHandler.Reset();
                }

                if (Mouse.IsOver(clear_forced_rect))
                {
                    TooltipHandler.TipRegion(clear_forced_rect, new TipSignal(delegate ()
                    {
                        string text = "ForcedApparel".Translate() + ":\n";
                        foreach (Apparel apparel in pawn.outfits.forcedHandler.ForcedApparel)
                        {
                            text = text + "\n   " + apparel.LabelCap;
                        }
                        return text;
                    }, pawn.GetHashCode() * 612));
                }

                rect_x += clear_forced_rect.width + 4f;
                Rect save_overwrite_rect = new Rect(rect_x, rect_y, rect_width_short + 2f, rect_height);

                if (Widgets.ButtonText(save_overwrite_rect, "OBR.Save".Translate().Truncate(save_overwrite_rect.width), true, true, true))
                {
                    if(Event.current.button == 0)
                    {
                        OutfitBuilderRedux.OutfitBuilderRedux_Do(pawn, true);
                    }
                    else if(Event.current.button == 1)
                    {
                        OutfitBuilderRedux.OutfitBuilderRedux_Do(pawn, false);
                    }
                }

                if (Mouse.IsOver(save_overwrite_rect))
                {
                    TooltipHandler.TipRegion(save_overwrite_rect, new TipSignal(delegate ()
                    {
                        string text = "Left click: Save outfit (overwrites)\nRight click: Update outfit";
                        return text;
                    }, pawn.GetHashCode() * 612));
                }

                return false;
            }

            return true;
        }
    }

}
