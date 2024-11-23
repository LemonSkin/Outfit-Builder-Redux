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

        public static void OutfitBuilderRedux_Do(Pawn pawn, bool overwrite)
        {

            OutfitPolicyGameComponent component = Current.Game.GetComponent<OutfitPolicyGameComponent>();

            ApparelPolicy outfitAssignedToPawn = component.OutfitAssignedToPawn(pawn);

            if (outfitAssignedToPawn == null)
            {
                outfitAssignedToPawn = component.CreateNewOutfit(pawn);
            }

            if (overwrite)
            {
                outfitAssignedToPawn.filter.SetDisallowAll();
            }else
            {
                if (pawn.outfits.CurrentApparelPolicy.label != pawn.Name.ToStringShort.CapitalizeFirst())
                {
                    outfitAssignedToPawn.filter.CopyAllowancesFrom(pawn.outfits.CurrentApparelPolicy.filter);
                }
            }

            foreach (Apparel apparel in pawn.apparel.WornApparel)
            {
                outfitAssignedToPawn.filter.SetAllow(apparel.def, true);
            }

            pawn.outfits.CurrentApparelPolicy = outfitAssignedToPawn;

            pawn.outfits.forcedHandler.Reset();
        }
    }
}
