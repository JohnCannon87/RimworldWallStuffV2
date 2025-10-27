using System.Collections.Generic;
using RimWorld;
using Verse;

namespace WallStuff
{
    /// <summary>
    /// Allows placing multi-cell buildings that must be attached to a wall.
    /// All occupied cells must be backed by a full wall (supportsWallAttachments = true).
    /// </summary>
    public class Placeworker_AttachedToWallMultiCell : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            // Get occupied cells for this thing
            foreach (IntVec3 cell in GenAdj.OccupiedRect(loc, rot, checkingDef.Size))
            {
                // Check adjacent cell in the direction of the wall (behind the object)
                IntVec3 wallCell = cell + GenAdj.CardinalDirections[rot.AsInt];

                List<Thing> wallCellThings = wallCell.GetThingList(map);
                bool foundSupportingWall = false;

                foreach (Thing wallThing in wallCellThings)
                {
                    ThingDef wallDef = GenConstruct.BuiltDefOf(wallThing.def) as ThingDef;
                    if (wallDef?.building == null)
                        continue;

                    // Is this wall capable of supporting attachments?
                    if (wallDef.building.supportsWallAttachments)
                    {
                        foundSupportingWall = true;
                        break;
                    }
                }

                if (!foundSupportingWall)
                    return "MustPlaceOnWall".Translate();

                // Check that there’s no other attachment already on this same wall face
                List<Thing> thingList = cell.GetThingList(map);
                foreach (Thing thing2 in thingList)
                {
                    ThingDef def2 = GenConstruct.BuiltDefOf(thing2.def) as ThingDef;
                    if (def2?.building == null)
                        continue;

                    if (def2.building.isAttachment && thing2.Rotation == rot)
                        return "SomethingPlacedOnThisWall".Translate();
                }
            }

            return true;
        }
    }
}
