using System.Collections.Generic;
using System.Linq;
using GeneticRim;
using RimWorld;
using Verse;

namespace TribalGenetics;

public class RitualOutcomeEffectWorker_GrinderSacrifice : RitualOutcomeEffectWorker_FromQuality
{
    public RitualOutcomeEffectWorker_GrinderSacrifice()
    {
    }

    public RitualOutcomeEffectWorker_GrinderSacrifice(RitualOutcomeEffectDef def)
        : base(def)
    {
    }

    public override bool SupportsAttachableOutcomeEffect => false;

    public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
    {
        var quality = GetQuality(jobRitual, progress);
        var outcome = GetOutcome(quality, jobRitual);
        LookTargets letterLookTargets = jobRitual.selectedTarget;
        var positivityIndex = outcome.positivityIndex;
        ThingDef thingDef = null;
        if (positivityIndex == -2)
        {
            thingDef = InternalDefOf.GR_GenoframeAwful;
        }

        if (positivityIndex == -1)
        {
            thingDef = InternalDefOf.GR_GenoframePoor;
        }

        if (positivityIndex == 1)
        {
            thingDef = InternalDefOf.GR_GenoframeNormal;
        }

        if (positivityIndex == 2)
        {
            thingDef = InternalDefOf.GR_GenoframeGood;
        }

        var selectedTarget = jobRitual.selectedTarget;
        var thing = ThingMaker.MakeThing(thingDef);
        GenPlace.TryPlaceThing(thing, selectedTarget.Thing.Position, selectedTarget.Thing.Map, ThingPlaceMode.Near);
        ApplyExtraOutcome(totalPresence, jobRitual, outcome, out var extraOutcomeDesc, ref letterLookTargets);
        string extraLetterText = null;
        if (jobRitual.Ritual != null)
        {
            ApplyAttachableOutcome(totalPresence, jobRitual, outcome, out extraLetterText, ref letterLookTargets);
        }

        string text = outcome.description.Formatted(jobRitual.Ritual?.Label).CapitalizeFirst();
        var text2 = def.OutcomeMoodBreakdown(outcome);
        if (!text2.NullOrEmpty())
        {
            text = $"{text}\n\n{text2}";
        }

        if (extraOutcomeDesc != null)
        {
            text = $"{text}\n\n{extraOutcomeDesc}";
        }

        if (extraLetterText != null)
        {
            text = $"{text}\n\n{extraLetterText}";
        }

        text = $"{text}\n\n{OutcomeQualityBreakdownDesc(quality, progress, jobRitual)}";
        ApplyDevelopmentPoints(jobRitual.Ritual, outcome, out var extraOutcomeDesc2);
        if (extraOutcomeDesc2 != null)
        {
            text = $"{text}\n\n{extraOutcomeDesc2}";
        }

        if (jobRitual.Ritual != null)
        {
            Find.LetterStack.ReceiveLetter(
                "OutcomeLetterLabel".Translate(outcome.label.Named("OUTCOMELABEL"),
                    jobRitual.Ritual.Label.Named("RITUALLABEL")), text,
                outcome.Positive ? LetterDefOf.RitualOutcomePositive : LetterDefOf.RitualOutcomeNegative,
                letterLookTargets);
        }
    }

    protected override void ApplyExtraOutcome(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual,
        RitualOutcomePossibility outcome, out string extraOutcomeDesc, ref LookTargets letterLookTargets)
    {
        extraOutcomeDesc = null;
        ThingDef thingDef = null;
        var hashSet = DefDatabase<ExtractableAnimalsList>.AllDefsListForReading.ToHashSet();
        foreach (var item in hashSet)
        {
            if (item.tier != "1")
            {
                continue;
            }

            var corpse = jobRitual.PawnWithRole("animal").Corpse;
            if (item.needsHumanLike && corpse.InnerPawn.def.race.Humanlike ||
                (item.extractableAnimals?.Contains(corpse.InnerPawn.def) ?? false))
            {
                thingDef = item.itemProduced;
            }
        }

        jobRitual.PawnWithRole("animal").Corpse.Destroy();
        var selectedTarget = jobRitual.selectedTarget;
        if (!outcome.Positive || !Rand.Chance(0.5f) || thingDef == null)
        {
            return;
        }

        var thing = ThingMaker.MakeThing(thingDef);
        GenPlace.TryPlaceThing(thing, selectedTarget.Thing.Position, selectedTarget.Thing.Map, ThingPlaceMode.Near);
    }
}