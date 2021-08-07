using RimWorld;
using Verse;

namespace LemonSkin.OBR
{
    [StaticConstructorOnStartup]
    public static class OutfitBuilderRedux
    {
        static OutfitBuilderRedux()
        {
            Log.Message("OBR Loaded!");
        }

        public static void OutfitBuilderRedux_Do(Pawn pawn, bool reset)
        {

            OutfitPolicyGameComponent component = Current.Game.GetComponent<OutfitPolicyGameComponent>();

            Outfit outfitAssignedToPawn = component.OutfitAssignedToPawn(pawn);

            if (outfitAssignedToPawn == null)
            {
                outfitAssignedToPawn = component.CreateNewOutfit(pawn);
            }

            if (reset)
            {
                outfitAssignedToPawn.filter.SetDisallowAll();
            }

            foreach (Apparel apparel in pawn.apparel.WornApparel)
            {
                outfitAssignedToPawn.filter.SetAllow(apparel.def, true);
            }

            pawn.outfits.CurrentOutfit = outfitAssignedToPawn;

            pawn.outfits.forcedHandler.Reset();
        }
    }
}
