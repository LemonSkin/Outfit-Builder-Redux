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

        public static void OutfitBuilderRedux_CreateNewOutfit(Pawn pawn)
        {
            OutfitPolicyGameComponent component =
                Current.Game.GetComponent<OutfitPolicyGameComponent>();

            ApparelPolicy outfitAssignedToPawn = component.OutfitAssignedToPawn(pawn);
            if (outfitAssignedToPawn.label != pawn.Name.ToStringShort.CapitalizeFirst())
            {
                outfitAssignedToPawn = component.CreateOutfit(pawn);
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
            ];

            Dictionary<SpecialThingFilterDef, bool> specialFilterStates = specialThingFilterNames
                .Select(filterName => SpecialThingFilterDef.Named(filterName))
                .ToDictionary(filterDef => filterDef, filterDef => pawn.outfits.CurrentApparelPolicy.filter.Allows(filterDef));

            outfitAssignedToPawn.filter.SetDisallowAll();
            foreach (var i in specialFilterStates)
            {
                outfitAssignedToPawn.filter.SetAllow(i.Key, i.Value);
            }

            outfitAssignedToPawn.filter.AllowedHitPointsPercents = pawn.outfits
                .CurrentApparelPolicy
                .filter
                .AllowedHitPointsPercents;
            outfitAssignedToPawn.filter.AllowedQualityLevels = pawn.outfits
                .CurrentApparelPolicy
                .filter
                .AllowedQualityLevels;

            foreach (Apparel apparel in pawn.apparel.WornApparel)
            {
                outfitAssignedToPawn.filter.SetAllow(apparel.def, true);
            }

            pawn.outfits.CurrentApparelPolicy = outfitAssignedToPawn;
            pawn.outfits.forcedHandler.Reset();
        }

        public static void OutfitBuilderRedux_UpdateOutfit(Pawn pawn)
        {
            OutfitPolicyGameComponent component =
                Current.Game.GetComponent<OutfitPolicyGameComponent>();

            ApparelPolicy outfitAssignedToPawn = component.OutfitAssignedToPawn(pawn);

            foreach (Apparel apparel in pawn.apparel.WornApparel)
            {
                outfitAssignedToPawn.filter.SetAllow(apparel.def, true);
            }
            pawn.outfits.CurrentApparelPolicy = outfitAssignedToPawn;
            pawn.outfits.forcedHandler.Reset();
        }
    }
}
