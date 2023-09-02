using RimWorld;
using Verse;

namespace TribalGenetics;

[DefOf]
public static class InternalDefOf
{
    public static JobDef GrindBody;

    public static RitualAttachableOutcomeEffectDef DiscoverGeneLab;

    public static ThingDef SanctTissueGrinder;

    public static ThingDef SanctGeneticsTable;

    public static ThingDef GR_GenoframeAwful;

    public static ThingDef GR_GenoframePoor;

    public static ThingDef GR_GenoframeNormal;

    public static ThingDef GR_GenoframeGood;

    public static ThingDef GR_GenoframeExcellent;

    public static ThingDef GR_GenoframeMasterwork;

    public static ThingDef GR_GenoframeLegendary;

    static InternalDefOf()
    {
        DefOfHelper.EnsureInitializedInCtor(typeof(InternalDefOf));
    }
}