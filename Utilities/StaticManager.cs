using System.Collections.Generic;
using System.Linq;

namespace TUA.Utilities
{
    /// <summary>
    /// When using new types, remember to add a flush command to TUA.Unload
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public static class ReflManager<T>
	{
		private static readonly Dictionary<string, T> ITEMS;

		public static int Count => ITEMS.Count;

		static ReflManager()
		{
			ITEMS = new Dictionary<string, T>();
		}

        public static void Clear()
        {
            ITEMS.Clear();
        }

		public static void AddItem(string id, T item)
		{
            if (ContainsItem(id))
                throw new System.Exception("ReflManager already contains value of same name");

			ITEMS.Add(id, item);
		}

		public static bool RemoveItem(string id)
		{
		    return ITEMS.Remove(id);
		}

		public static bool ContainsItem(string id)
		{
			return ITEMS.ContainsKey(id);
		}

		public static T GetItem(string id)
		{
			return ContainsItem(id) ? ITEMS[id] : default;
		}

		public static (string Name, T Item)[] GetItems()
		{
            List<(string Name, T Item)> result = new List<(string Name, T Item)>();
			foreach (var key in ITEMS.Keys)
			{
				result.Add((key, ITEMS[key]));
			}

			return result.ToArray();
		}
	}
}
