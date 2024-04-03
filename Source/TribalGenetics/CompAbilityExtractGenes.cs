using System.Linq;
using GeneticRim;
using RimWorld;
using Verse;
using Verse.Sound;

namespace TribalGenetics;

public class CompAbilityExtractGenes : CompAbilityEffect
{
    public ThingDef thingToSpawn;

    public new CompProperties_AbilityExtractGenes Props => (CompProperties_AbilityExtractGenes)props;

    public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
    {
        thingToSpawn = null;
        var brainExtracted = false;
        if (target.Thing is Corpse corpse)
        {
            var firstHediffOfDef =
                corpse.InnerPawn.health.hediffSet.GetFirstHediffOfDef(GeneticRim.InternalDefOf.GR_ExtractedBrain);
            if (firstHediffOfDef != null)
            {
                brainExtracted = true;
            }
        }
        else
        {
            Messages.Message("GR_NoValidExtraction".Translate(), MessageTypeDefOf.RejectInput);
            parent.StartCooldown(30);
        }

        if (brainExtracted)
        {
            Messages.Message("GR_AlreadyExtracted".Translate(), MessageTypeDefOf.RejectInput);
            parent.StartCooldown(30);
        }
        else
        {
            var hashSet = DefDatabase<ExtractableAnimalsList>.AllDefsListForReading.ToHashSet();
            foreach (var item in hashSet)
            {
                if (item.tier != "1")
                {
                    continue;
                }

                if (target.Thing is Corpse corpse2)
                {
                    if (item.needsHumanLike && corpse2.InnerPawn.def.race.Humanlike ||
                        (item.extractableAnimals?.Contains(corpse2.InnerPawn.def) ?? false))
                    {
                        thingToSpawn = item.itemProduced;
                    }
                }
                else
                {
                    Messages.Message("GR_NoValidExtraction".Translate(), MessageTypeDefOf.RejectInput);
                    parent.StartCooldown(30);
                }
            }

            if (thingToSpawn != null)
            {
                var thing = ThingMaker.MakeThing(thingToSpawn);
                GenPlace.TryPlaceThing(thing, target.Thing.Position, target.Thing.Map, ThingPlaceMode.Near);
                GeneticRim.InternalDefOf.GR_Pop.PlayOneShot(new TargetInfo(target.Thing.Position, target.Thing.Map));
                if (target.Thing is Corpse corpse3)
                {
                    popBlood(null, corpse3);
                    corpse3.InnerPawn.health.AddHediff(GeneticRim.InternalDefOf.GR_ExtractedBrain);
                }
                else
                {
                    Messages.Message("GR_NoValidExtraction".Translate(), MessageTypeDefOf.RejectInput);
                    parent.StartCooldown(30);
                }
            }
            else
            {
                Messages.Message("GR_NoValidExtraction".Translate(), MessageTypeDefOf.RejectInput);
                parent.StartCooldown(30);
            }
        }

        base.Apply(target, dest);
    }

    public void popBlood(Pawn pawn, Corpse corpse)
    {
        var root = IntVec3.Zero;
        Map map = null;
        var thingDef = ThingDefOf.Filth_Blood;
        if (pawn != null)
        {
            root = pawn.Position;
            map = pawn.Map;
            if (pawn.def.race.Insect)
            {
                thingDef = ThingDef.Named("Filth_BloodInsect");
            }
        }

        if (corpse != null)
        {
            root = corpse.Position;
            map = corpse.Map;
            if (corpse.InnerPawn.def.race.Insect)
            {
                thingDef = ThingDef.Named("Filth_BloodInsect");
            }
        }

        for (var i = 0; i < 20; i++)
        {
            CellFinder.TryFindRandomReachableCellNearPosition(root, root, map, 2f,
                TraverseParms.For(TraverseMode.NoPassClosedDoors), null, null, out var result);
            FilthMaker.TryMakeFilth(result, map, thingDef);
        }
    }
}