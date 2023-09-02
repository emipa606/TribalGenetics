using System;
using System.Collections.Generic;
using RimWorld;
using Verse;

namespace TribalGenetics;

internal class ThingSetMaker_StartingItemsTribalGenetics : ThingSetMaker
{
    protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
    {
        var item = ThingMaker.MakeThing(InternalDefOf.SanctTissueGrinder);
        var item2 = ThingMaker.MakeThing(InternalDefOf.SanctGeneticsTable);
        outThings.Add(item);
        outThings.Add(item2);
    }

    protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
    {
        throw new NotImplementedException();
    }
}