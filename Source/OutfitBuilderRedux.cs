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
            OutfitPolicyGameComponent component = Current.Game.GetComponent<OutfitPolicyGameComponent>();

            ApparelPolicy outfitAssignedToPawn = component.OutfitAssignedToPawn(pawn);
            if (outfitAssignedToPawn.label != pawn.Name.ToStringShort.CapitalizeFirst())
            {
                outfitAssignedToPawn = component.CreateNewOutfit(pawn);
            }

            outfitAssignedToPawn.filter.SetDisallowAll();
            outfitAssignedToPawn.filter.allowedHitPointsPercents = pawn.outfits.CurrentApparelPolicy.filter.allowedHitPointsPercents;
            outfitAssignedToPawn.filter.allowedQualities = pawn.outfits.CurrentApparelPolicy.filter.allowedQualities;
            outfitAssignedToPawn.filter.disallowedSpecialFilters = pawn.outfits.CurrentApparelPolicy.filter.disallowedSpecialFilters;

            foreach (Apparel apparel in pawn.apparel.WornApparel)
            {
                outfitAssignedToPawn.filter.SetAllow(apparel.def, true);
            }

            pawn.outfits.CurrentApparelPolicy = outfitAssignedToPawn;
            pawn.outfits.forcedHandler.Reset();
        }

        public static void OutfitBuilderRedux_UpdateOutfit(Pawn pawn)
        {
            OutfitPolicyGameComponent component = Current.Game.GetComponent<OutfitPolicyGameComponent>();

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
