using System.Collections;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(NullSequence),SequenceType.Sequence)]
    public class NullSequence : Sequence
    {
        public override IEnumerator Invoke()
        {
            yield break;
        }

        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            return new NullSequence();
        }
    }

}