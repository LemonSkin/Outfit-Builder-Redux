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
                int num = Mathf.FloorToInt((rect.width - 4f) * 0.71428573f);
                int num2 = Mathf.FloorToInt((rect.width - 4f) * 0.2857143f);
                float num3 = rect.x;
                bool somethingIsForced = pawn.outfits.forcedHandler.SomethingIsForced;
                Rect rect2 = new Rect(num3, rect.y + 2f, (float)num, rect.height - 4f);
                //if (somethingIsForced)
                //{
                rect2.width -= 4f + (float)num2;
                //}
                if (pawn.IsQuestLodger())
                {
                    Text.Anchor = TextAnchor.MiddleCenter;
                    Widgets.Label(rect2, "Unchangeable".Translate().Truncate(rect2.width, null));
                    TooltipHandler.TipRegionByKey(rect2, "QuestRelated_Outfit");
                    Text.Anchor = TextAnchor.UpperLeft;
                }
                else
                {
                    //private static readonly Func<Pawn, bool, int> ticksMoveSpeed = (Func<Pawn, bool, int>)Delegate.CreateDelegate(typeof(Func<Pawn, bool, int>), 
                    //AccessTools.Method(typeof(Pawn), "TicksPerMove"));

                    Widgets.Dropdown<Pawn, Outfit>(rect2,
                                                    pawn,
                                                    (Pawn p) => p.outfits.CurrentOutfit,
                                                    new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<Outfit>>>(__instance.Button_GenerateMenu),
                                                    pawn.outfits.CurrentOutfit.label.Truncate(rect2.width, null),
                                                    null,
                                                    pawn.outfits.CurrentOutfit.label,
                                                    null,
                                                    null,
                                                    true);
                }
                num3 += rect2.width;
                num3 += 4f;
                Rect rect3 = new Rect(num3, rect.y + 2f, (float)num2, rect.height - 4f);
                //if (somethingIsForced)
                //{

                if (Widgets.ButtonText(rect3, "ClearForcedApparel".Translate(), true, true, true))
                {
                    pawn.outfits.forcedHandler.Reset();
                }

                if (Mouse.IsOver(rect3))
                {
                    TooltipHandler.TipRegion(rect3, new TipSignal(delegate ()
                    {
                        string text = "ForcedApparel".Translate() + ":\n";
                        foreach (Apparel apparel in pawn.outfits.forcedHandler.ForcedApparel)
                        {
                            text = text + "\n   " + apparel.LabelCap;
                        }
                        return text;
                    }, pawn.GetHashCode() * 612));
                }
                num3 += (float)num2;
                num3 += 4f;

                Rect save = new Rect(num3, rect.y + 2f, (float)(num2 / 2) - 4f, rect.height - 4f);

                if (Widgets.ButtonText(save, "OBR.Save".Translate(), true, true, true))
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

                if (Mouse.IsOver(save))
                {
                    TooltipHandler.TipRegion(save, new TipSignal(delegate ()
                    {
                        string text = "Left click: Save outfit (overwrites)\nRight click: Update outfit";
                        return text;
                    }, pawn.GetHashCode() * 612));
                }
                num3 += save.width;
                num3 += 4f;
                //}

                Rect rect4 = new Rect(num3, rect.y + 2f, (float)(num2 / 2), rect.height - 4f);
                if (!pawn.IsQuestLodger() && Widgets.ButtonText(rect4, "OBR.AssignTabEdit".Translate(), true, true, true))
                {
                    Find.WindowStack.Add(new Dialog_ManageOutfits(pawn.outfits.CurrentOutfit));
                }
                num3 += (float)num2;

                return false;
            }

            return true;
        }
    }

}
//class PawnColumnWorker_Outfit_Transpiler
//{
//    [HarmonyTranspiler]
//    static IEnumerable<CodeInstruction> PawnColumnWorker_Outfit_Transpile(IEnumerable<CodeInstruction> instructions)
//    {
//        var code = new List<CodeInstruction>(instructions);

//        int insertionIndex = -1;

//        for (int i = 0; i < code.Count - 1; i++)
//        {
//            if (code[i].opcode == OpCodes.Brfalse_S && code[i + 1].opcode == OpCodes.Ldloca_S && code[i + 2].opcode == OpCodes.Dup)
//            {
//                insertionIndex = i + 10;
//                break;
//            }
//        }
//        //&& (int)code[i + 1].operand == 4

//        var instructionsToInsert = new List<CodeInstruction>();

//        instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldloca_S, (byte)4));
//        instructionsToInsert.Add(new CodeInstruction(OpCodes.Dup));
//        instructionsToInsert.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Rect), "get_width")));
//        instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldc_R4, (float)2));
//        instructionsToInsert.Add(new CodeInstruction(OpCodes.Div));
//        instructionsToInsert.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Rect), "set_width", new Type[] { typeof(float) })));

//        //instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldc_I4_0));
//        //instructionsToInsert.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Text), "set_Font", new Type[] { typeof(GameFont) })));
//        //instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldc_I4_7));
//        //instructionsToInsert.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Text), "set_Anchor", new Type[] { typeof(TextAnchor) })));
//        //instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldloc_3));
//        //instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldc_R4, (float)-8));
//        //instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldc_R4, (float)165));
//        //instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldc_R4, (float)10));
//        //instructionsToInsert.Add(new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Rect), new Type[] { typeof(float), typeof(float), typeof(float), typeof(float) })));
//        //instructionsToInsert.Add(new CodeInstruction(OpCodes.Ldstr, "OBR.AssignButton"));
//        //instructionsToInsert.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Translator), "Translate", new Type[] { typeof(string) })));
//        //instructionsToInsert.Add(new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Widgets), "Label", new Type[] { typeof(Rect), typeof(TaggedString) })));

//        if (insertionIndex != -1)
//        {
//            code.InsertRange(insertionIndex, instructionsToInsert);
//        }


//        return code;
//    }

//}
