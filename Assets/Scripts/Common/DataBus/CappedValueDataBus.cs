using UnityEngine;

namespace Common.DataBus
{
    public class CappedValueDataBus<T> : ScriptableObject
    {
        public T value;
        public T max;
    }
}
