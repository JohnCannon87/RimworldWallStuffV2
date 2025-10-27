using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace WallStuff
{
    public class PlaceWorker_Repair : PlaceWorker
    {
        public override void DrawGhost(ThingDef def, IntVec3 center, Rot4 rot, Color ghostCol, Thing thing = null)
       {
            var map = Find.CurrentMap;

            GenDraw.DrawFieldEdges(new List<IntVec3>() { center }, Color.green);
            var room = center.GetRoom(map);
            if (room == null || room.UsesOutdoorTemperature)
            {
                return;
            }
            GenDraw.DrawFieldEdges(room.Cells.ToList(), Color.green);
        }
    }
}