using System.Collections.Generic;
using RimWorld;
using Verse;

namespace TribalGenetics;

internal class RitualOutcomeEffectWorker_GrinderSacrifice_Human : RitualOutcomeEffectWorker_GrinderSacrifice
{
    public RitualOutcomeEffectWorker_GrinderSacrifice_Human()
    {
    }

    public RitualOutcomeEffectWorker_GrinderSacrifice_Human(RitualOutcomeEffectDef def)
        : base(def)
    {
    }

    public override void Apply(float progress, Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual)
    {
        var quality = GetQuality(jobRitual, progress);
        var outcome = GetOutcome(quality, jobRitual);
        LookTargets letterLookTargets = jobRitual.selectedTarget;
        var positivityIndex = outcome.positivityIndex;
        ThingDef thingDef = null;
        if (positivityIndex == -2)
        {
            thingDef = InternalDefOf.GR_GenoframePoor;
        }

        if (positivityIndex == -1)
        {
            thingDef = InternalDefOf.GR_GenoframeNormal;
        }

        if (positivityIndex == 1)
        {
            thingDef = InternalDefOf.GR_GenoframeGood;
        }

        if (positivityIndex == 2)
        {
            thingDef = InternalDefOf.GR_GenoframeExcellent;
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
        var gR_HumanoidGenetic = GeneticRim.InternalDefOf.GR_HumanoidGenetic;
        jobRitual.PawnWithRole("prisoner").Corpse.Destroy();
        var selectedTarget = jobRitual.selectedTarget;
        if (!outcome.Positive || !Rand.Chance(0.5f))
        {
            return;
        }

        var thing = ThingMaker.MakeThing(gR_HumanoidGenetic);
        GenPlace.TryPlaceThing(thing, selectedTarget.Thing.Position, selectedTarget.Thing.Map, ThingPlaceMode.Near);
    }
}