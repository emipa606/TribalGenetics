using System.Collections.Generic;
using System.Linq;
using RimWorld;
using RimWorld.QuestGen;
using Verse;

namespace TribalGenetics;

public class RitualAttachableOutcomeEffectWorker_DiscoverGeneLab : RitualAttachableOutcomeEffectWorker
{
    public override void Apply(Dictionary<Pawn, int> totalPresence, LordJob_Ritual jobRitual,
        RitualOutcomePossibility outcome,
        out string extraOutcomeDesc, ref LookTargets letterLookTargets)
    {
        if (!CanApplyNow(jobRitual.Ritual, jobRitual.Map))
        {
            extraOutcomeDesc = null;
            return;
        }

        extraOutcomeDesc = def.letterInfoText;
        var vars = new Slate();
        var quest = QuestUtility.GenerateQuestAndMakeAvailable(GeneticRim.InternalDefOf.GR_OpportunitySite_AbandonedLab,
            vars);
        letterLookTargets =
            new LookTargets((letterLookTargets.targets ?? []).Concat(quest.QuestLookTargets));
    }
}