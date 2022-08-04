using System.Collections;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(HaltAllArgumentsSequence), SequenceType.Sequence)]
    public class HaltAllArgumentsSequence : Sequence
    {
        public override IEnumerator Invoke()
        {
            SequenceManager.HaltAllCurrentArguments();

            yield break;
        }

        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            return new HaltAllArgumentsSequence();
        }
    }
}
