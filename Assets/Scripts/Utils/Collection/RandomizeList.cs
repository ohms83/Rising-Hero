using System.Collections.Generic;
using UnityEngine;

namespace Utils.Collection
{
    public static class RandomizeList<T>
    {
        public static T GetRandomItem(IList<T> source)
        {
            return GetRandomItem(source, (uint)Time.frameCount);
        }
        public static T GetRandomItem(IList<T> source, uint seed)
        {
            var count = source.Count;
            if (count == 0)
                return default;
            
            return source[Random.Range(0, source.Count)];
        }

        public static List<T> GetRandomItems(IList<T> source, int numItem)
        {
            return GetRandomItems(source, numItem, (uint)Time.frameCount);
        }
        
        public static List<T> GetRandomItems(IList<T> source, int numItem, uint seed)
        {
            if (source.Count == 0)
                return default;
            
            var randList = new List<T>(source);
            var result = new List<T>();
            
            for (var i = 0; i < numItem && randList.Count > 0; ++i)
            {
                var index = Random.Range(0, randList.Count);
                result.Add(randList[index]);
                randList.RemoveAt(index);
            }
            
            return result;
        }
    }
}
