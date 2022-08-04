using System.Collections;
using System.Diagnostics;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(ForwardArgument),SequenceType.Argument)]
    public class ForwardArgument : Argument
    {
        public override void EnterArgument()
        {
            
        }

        public override IEnumerator Invoke()
        {
            yield return null;
            if (TargetSequenceName != null && TargetSequenceName.Length != 0)
            {
                SequenceManager.UpdateTable(TableName, TargetSequenceName[0]);
            }
            yield break;
        }

        public override void ExitArgument()
        {

        }

        public override Argument MakeArgument(SequenceManagerElementData elementData)
        {
            return new ForwardArgument();
        }
    }
} 