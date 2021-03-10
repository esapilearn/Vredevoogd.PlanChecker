using ESAPIX.Constraints;
using ESAPIX.Extensions;
using System;
using System.Linq;
using VMS.TPS.Common.Model.API;

namespace ESAPX_StarterUI.ViewModels
{
    public class CTDateConstraint : IConstraint
    {
        public string Name => "CT Date Checker";
        public string FullName => "CT Date < 60 Days";

        public ConstraintResult CanConstrain(PlanningItem pi)
        {
            var pq = new PQAsserter(pi);
            return pq.HasImage().CumulativeResult;
        }

        public ConstraintResult Constrain(PlanningItem pi)
        {
            var diffDays = (DateTime.Now - pi.StructureSet.Image.CreationDateTime).Value.TotalDays;
            return IsCTTooOld(diffDays);
        }

        public ConstraintResult IsCTTooOld(double diffDays)
        {
            var msg = $"CT is {diffDays} days old";

            if (diffDays <= 60)
            {
                return new ConstraintResult(this, ResultType.PASSED, msg);
            }
            else
            {
                return new ConstraintResult(this, ResultType.ACTION_LEVEL_3, msg);
            }
        }
    }
}