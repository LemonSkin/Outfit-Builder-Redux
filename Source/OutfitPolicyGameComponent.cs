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

        public ApparelPolicy GetOrMakeNewOutfit(Pawn pawn)
        {
            string pawnName = pawn.Name.ToStringShort.CapitalizeFirst();

            ApparelPolicy existingOutfit = game.outfitDatabase.AllOutfits.FirstOrDefault(outfit =>
                outfit.label == pawnName
            );

            if (existingOutfit != null)
            {
                return existingOutfit;
            }

            ApparelPolicy newOutfit = game.outfitDatabase.MakeNewOutfit();
            newOutfit.label = pawnName;
            return newOutfit;
        }
    }
}
