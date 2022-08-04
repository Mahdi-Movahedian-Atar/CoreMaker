using System.Collections;

namespace CM.SequenceManager
{
    [SequenceManagerElement(typeof(StartAllArgumentsSequence), SequenceType.Sequence)]
    public class StartAllArgumentsSequence : Sequence
    {
        public override IEnumerator Invoke()
        {
            SequenceManager.StartAllCurrentArguments();

            yield break;
        }

        public override Sequence MakeSequence(SequenceManagerElementData elementData)
        {
            return new StartAllArgumentsSequence();
        }
    }
}