using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse;

namespace LemonSkin.OBR
{
    [StaticConstructorOnStartup]
    public static class OutfitBuilderRedux
    {
        static OutfitBuilderRedux()
        {
            Log.Message("Outfit Builder Redux Loaded!");
        }

        public static void OutfitBuilderRedux_SaveOutfit(Pawn pawn)
        {
            OutfitPolicyGameComponent component =
                Current.Game.GetComponent<OutfitPolicyGameComponent>();

            ApparelPolicy outfit = pawn.outfits.CurrentApparelPolicy;
            if (outfit.label != pawn.Name.ToStringShort.CapitalizeFirst())
            {
                outfit = component.GetOrMakeNewOutfit(pawn);
            }

            string[] specialThingFilterNames =
            [
                "AllowSmeltableApparel",
                "AllowNonSmeltableApparel",
                "AllowBurnableApparel",
                "AllowNonBurnableApparel",
                "AllowBiocodedApparel",
                "AllowNonBiocodedApparel",
                "AllowDeadmansApparel",
                "AllowNonDeadmansApparel",
            ];

            Dictionary<SpecialThingFilterDef, bool> specialFilterStates = specialThingFilterNames
                .Select(filterName => SpecialThingFilterDef.Named(filterName))
                .ToDictionary(
                    filterDef => filterDef,
                    filterDef => pawn.outfits.CurrentApparelPolicy.filter.Allows(filterDef)
                );

            FloatRange allowedHitPointsPercets = pawn.outfits
                .CurrentApparelPolicy
                .filter
                .AllowedHitPointsPercents;
            QualityRange allowedQualityLevels = pawn.outfits
                .CurrentApparelPolicy
                .filter
                .AllowedQualityLevels;

            outfit.filter.SetDisallowAll();
            foreach (var i in specialFilterStates)
            {
                outfit.filter.SetAllow(i.Key, i.Value);
            }

            outfit.filter.AllowedHitPointsPercents = allowedHitPointsPercets;
            outfit.filter.AllowedQualityLevels = allowedQualityLevels;

            foreach (Apparel apparel in pawn.apparel.WornApparel)
            {
                outfit.filter.SetAllow(apparel.def, true);
            }

            //IEnumerable<ThingDef> apparelDefs = pawn.apparel.WornApparel.Select(apparel =>
            //    apparel.def
            //);

            //string[] specialThingFilterNames =
            //[
            //    "AllowSmeltableApparel",
            //    "AllowNonSmeltableApparel",
            //    "AllowBurnableApparel",
            //    "AllowNonBurnableApparel",
            //    "AllowBiocodedApparel",
            //    "AllowNonBiocodedApparel",
            //    "AllowDeadmansApparel",
            //    "AllowNonDeadmansApparel",
            //];

            //IEnumerable<SpecialThingFilterDef> specialThingFilterDefs = specialThingFilterNames
            //    .Select(filterName => SpecialThingFilterDef.Named(filterName))
            //    .Where(filterDef => pawn.outfits.CurrentApparelPolicy.filter.Allows(filterDef));
            //foreach (SpecialThingFilterDef filterDef in specialThingFilterDefs)
            //{
            //    Log.Message(filterDef.label + ": ");
            //}

            //outfit.filter.SetDisallowAll(apparelDefs, specialThingFilterDefs);

            //outfit.filter.AllowedHitPointsPercents = pawn.outfits
            //    .CurrentApparelPolicy
            //    .filter
            //    .AllowedHitPointsPercents;
            //outfit.filter.AllowedQualityLevels = pawn.outfits
            //    .CurrentApparelPolicy
            //    .filter
            //    .AllowedQualityLevels;

            pawn.outfits.CurrentApparelPolicy = outfit;
            pawn.outfits.forcedHandler.Reset();
        }

        public static void OutfitBuilderRedux_SaveOutfitAs(Pawn pawn, string name)
        {
            Log.Message(name);
        }

        public static void OutfitBuilderRedux_UpdateOutfit(Pawn pawn)
        {
            OutfitPolicyGameComponent component =
                Current.Game.GetComponent<OutfitPolicyGameComponent>();

            //ApparelPolicy outfitAssignedToPawn = component.OutfitAssignedToPawn(pawn);
            ApparelPolicy outfit = pawn.outfits.CurrentApparelPolicy;

            foreach (Apparel apparel in pawn.apparel.WornApparel)
            {
                outfit.filter.SetAllow(apparel.def, true);
            }
            pawn.outfits.CurrentApparelPolicy = outfit;
            pawn.outfits.forcedHandler.Reset();
        }
    }
}
