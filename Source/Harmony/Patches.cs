using System.Reflection;
using HarmonyLib;
using Verse;

namespace LemonSkin.OBR
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        static HarmonyPatches()
        {
            var inst = new Harmony("rimworld.LemonSkin.OBR");
            inst.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}
