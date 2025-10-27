using HarmonyLib;
using RimWorld;
using Verse;

namespace WallStuff
{
    [HarmonyPatch(typeof(CompRechargeable), "CompTick")]
    public static class Patch_CompRechargeable_CompTick
    {
        public static void Postfix(CompRechargeable __instance)
        {
            var parent = __instance.parent;
            if (parent == null)
                return;

            // Only affect your wall-mounted version
            if (parent.def.defName != "WallMountedNeuralSupercharger")
                return;

            // Get the private field
            var progressBarField = AccessTools.Field(typeof(CompRechargeable), "progressBar");
            var progressBar = progressBarField.GetValue(__instance) as Effecter;

            // If there is an active progress bar, clean it up and null it
            if (progressBar != null)
            {
                progressBar.Cleanup();
                progressBarField.SetValue(__instance, null);
            }

            // The CompRechargeable will still tick/recharge normally,
            // but no visual bar will ever appear.
        }
    }
}
