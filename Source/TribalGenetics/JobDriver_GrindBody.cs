using System.Collections.Generic;
using RimWorld;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace TribalGenetics;

public class JobDriver_GrindBody : JobDriver
{
    private const TargetIndex VictimIndex = TargetIndex.A;

    private const TargetIndex StandingIndex = TargetIndex.B;

    protected Pawn Victim => (Pawn)job.GetTarget(TargetIndex.A).Thing;

    public override bool TryMakePreToilReservations(bool errorOnFailed)
    {
        return pawn.Reserve(Victim, job, 1, -1, null, errorOnFailed);
    }

    protected override IEnumerable<Toil> MakeNewToils()
    {
        if (!ModLister.CheckIdeology("Sacrifice"))
        {
            yield break;
        }

        var victim = Victim;
        yield return Toils_Goto.GotoThing(TargetIndex.B, PathEndMode.OnCell);
        yield return Toils_General.Wait(35);
        var grindBody = new Toil
        {
            initAction = delegate
            {
                var lord = pawn.GetLord();
                if (lord is { LordJob: LordJob_Ritual lordJob_Ritual })
                {
                    lordJob_Ritual.pawnsDeathIgnored.Add(victim);
                }

                ExecutionUtility.DoExecutionByCut(pawn, victim, 0, false);
                ThoughtUtility.GiveThoughtsForPawnExecuted(victim, pawn, PawnExecutionKind.GenericBrutal);
                TaleRecorder.RecordTale(TaleDefOf.ExecutedPrisoner, pawn, victim);
                victim.Corpse.DeSpawn();
            },
            defaultCompleteMode = ToilCompleteMode.Instant
        };
        yield return Toils_Reserve.Release(TargetIndex.A);
        yield return grindBody;
    }
}