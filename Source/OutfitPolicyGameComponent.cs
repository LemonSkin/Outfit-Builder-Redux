using System.Collections.Generic;
using RimWorld;
using Verse;

namespace LemonSkin.OBR
{
    public class OutfitPolicyGameComponent(Game game) : GameComponent
    {

        //public ApparelPolicy OutfitAssignedToPawn(Pawn pawn)
        //{
        //    return pawn.outfits.CurrentApparelPolicy;
        //}

        public ApparelPolicy GetOutfit(Pawn pawn)
        {
            List<ApparelPolicy> outfits = Current.Game.outfitDatabase.AllOutfits;
            foreach (ApparelPolicy outfit in outfits)
            {
                if (outfit.label == pawn.Name.ToStringShort.CapitalizeFirst())
                {
                    return outfit;
                }
            }

            ApparelPolicy newOutfit = Current.Game.outfitDatabase.MakeNewOutfit();
            newOutfit.label = pawn.Name.ToStringShort.CapitalizeFirst();
            return newOutfit;
        }
    }
}
