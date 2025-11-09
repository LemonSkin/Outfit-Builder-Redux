using System.Collections.Generic;
using RimWorld;
using Verse;

namespace LemonSkin.OBR
{
    public class OutfitPolicyGameComponent : GameComponent
    {
        public OutfitPolicyGameComponent(Game game) { }

        public ApparelPolicy OutfitAssignedToPawn(Pawn pawn)
        {
            return pawn.outfits.CurrentApparelPolicy;
        }

        public ApparelPolicy CreateNewOutfit(Pawn pawn)
        {
            ApparelPolicy newOutfit = Current.Game.outfitDatabase.MakeNewOutfit();
            newOutfit.label = pawn.Name.ToStringShort.CapitalizeFirst();
            return newOutfit;
        }
    }
}
