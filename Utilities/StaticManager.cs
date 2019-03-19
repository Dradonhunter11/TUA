using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TUA.Utilities
{
	public static class StaticManager<T>
	{
		private static readonly Dictionary<string, T> ITEMS;
		private static Dictionary<string, string> ITEM_ID_TO_NAME_MAP;

		private static readonly T DEFAULT;

		public static int Count => ITEMS.Count;

		static StaticManager()
		{
			ITEMS = new Dictionary<string, T>();
			ITEM_ID_TO_NAME_MAP = new Dictionary<string, string>();
			DEFAULT = default(T);
		}

		public static void AddItem(string idname, T item)
		{
			AddItem(idname, idname, item);
		}

		public static void AddItem(string id, string name, T item)
		{
			if (ContainsItem(id))
				return;

			ITEM_ID_TO_NAME_MAP.Add(id, name);
			ITEMS.Add(id, item);
		}

		public static void RemoveItem(string id)
		{
			if (ContainsItem(id))
			{
				ITEMS.Remove(id);
				ITEM_ID_TO_NAME_MAP.Remove(id);
			}
		}

		public static bool ContainsItem(string id)
		{
			return ITEMS.ContainsKey(id) && ITEM_ID_TO_NAME_MAP.ContainsKey(id);
		}

		public static string GetItemID(int index)
		{
			return ITEMS.Keys.ToArray()[index];
		}

		public static string GetItemName(string id)
		{
			return ContainsItem(id) ? ITEM_ID_TO_NAME_MAP[id] : null;
		}

		public static string GetItemName(int index)
		{
			return GetItemName(GetItemID(index));
		}

		public static T GetItem(string id)
		{
			return ContainsItem(id) ? ITEMS[id] : DEFAULT;
		}

		public static T GetItem(int index)
		{
			return GetItem(GetItemID(index));
		}

		public static Tuple<string, string, T>[] GetItems()
		{
			List<Tuple<string, string, T>> result = new List<Tuple<string, string, T>>();
			foreach (var key in ITEMS.Keys)
			{
				result.Add(new Tuple<string, string, T>(key, ITEM_ID_TO_NAME_MAP[key], ITEMS[key]));
			}

			return result.ToArray();
		}
	}
}
