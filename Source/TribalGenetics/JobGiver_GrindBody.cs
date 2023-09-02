using Verse;
using Verse.AI;

namespace TribalGenetics;

public class JobGiver_GrindBody : ThinkNode_JobGiver
{
    public override ThinkNode DeepCopy(bool resolve = true)
    {
        return (JobGiver_GrindBody)base.DeepCopy(resolve);
    }

    protected override Job TryGiveJob(Pawn pawn)
    {
        var pawn2 = pawn.mindState.duty.focusSecond.Pawn;
        return pawn.CanReserveAndReach(pawn2, PathEndMode.ClosestTouch, Danger.None)
            ? JobMaker.MakeJob(InternalDefOf.GrindBody, pawn2, pawn.mindState.duty.focus)
            : null;
    }
}