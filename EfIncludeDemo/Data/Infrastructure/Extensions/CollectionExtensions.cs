using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EfIncludeDemo.Data.Infrastructure
{
	public static class CollectionExtensions
	{
		public static bool IsEmpty<T>(this IEnumerable<T> collection)
		{
			if (collection != null)
				return !collection.Any<T>();
			return true;
		}

		public static IList<T> SafeAdd<T>(this IList<T> collection, T data) where T : class
		{
			if ((object) data != null && collection != null)
				collection.Add(data);
			return collection;
		}

		public static IList<T> SafeAdd<T>(this IList<T> collection, T? data) where T : struct
		{
			if (data.HasValue && collection != null)
				collection.Add(data.Value);
			return collection;
		}

		public static IList<T> SafeAdd<T>(this IList<T> list, IList<T> otherList)
		{
			if (list == null)
				return otherList;
			if (otherList == null)
				return list;
			foreach (T other in (IEnumerable<T>) otherList)
				list.Add(other);
			return list;
		}

		public static Dictionary<TId, T> SafeAdd<TId, T>(this Dictionary<TId, T> collection, TId key, T data) where T : class
		{
			if (collection == null)
				collection = new Dictionary<TId, T>();
			if ((object) data != null && (object) key != null)
			{
				if (collection.ContainsKey(key))
					collection[key] = data;
				else
					collection.Add(key, data);
			}

			return collection;
		}

		public static Dictionary<TId, T> SafeAdd<TId, T>(this Dictionary<TId, T> collection, Dictionary<TId, T> other) where T : class
		{
			if (collection == null)
				return other;
			if (other == null)
				return collection;
			foreach (KeyValuePair<TId, T> keyValuePair in other)
				collection.SafeAdd<TId, T>(keyValuePair.Key, keyValuePair.Value);
			return collection;
		}

		public static T SafeGet<T>(this List<T> list, int idx)
		{
			if (list.IsEmpty<T>())
				return default(T);
			if (idx >= 0 && idx < list.Count)
				return list[idx];
			return default(T);
		}

		public static string MakeString<T>(this IEnumerable<T> seq, string delimiter = "")
		{
			IEnumerable<T> seq1 = seq;
			string delimiter1 = delimiter;
			return seq1.MakeString<T>((Func<T, string>) (x => x.ToString()), delimiter1);
		}

		public static string MakeString<T>(this IEnumerable<T> seq, Func<T, string> formatter, string delimiter = "")
		{
			if (seq == null || !seq.Any<T>())
				return "";
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(formatter(seq.First<T>()));
			foreach (T obj in seq.Skip<T>(1))
				stringBuilder.Append(delimiter).Append(formatter(obj) ?? "");
			return stringBuilder.ToString();
		}

		public static TValue ValueOrProvide<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, Func<TValue> defaultValueProvider)
		{
			if ((object) key == null || !collection.ContainsKey(key))
				return (defaultValueProvider ?? (Func<TValue>) (() => default(TValue)))();
			return collection[key];
		}

		public static TValue ValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, TValue defaultValue)
		{
			if ((object) key == null || !collection.ContainsKey(key))
				return defaultValue;
			return collection[key];
		}

		public static TValue ValueOrProvide<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, Func<TValue> defaultValueProvider,
			Action<TKey, TValue> process)
		{
			TValue obj = collection.ValueOrProvide<TKey, TValue>(key, defaultValueProvider);
			process(key, obj);
			return obj;
		}

		public static TValue? NValueOrProvide<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, Func<TValue?> defaultValueProvider)
			where TValue : struct
		{
			if ((object) key != null && collection.ContainsKey(key))
				return new TValue?(collection[key]);
			if (defaultValueProvider == null)
				return new TValue?();
			return defaultValueProvider();
		}

		public static TValue? NValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, TValue? defaultValue = null) where TValue : struct
		{
			if ((object) key == null || !collection.ContainsKey(key))
				return defaultValue;
			return new TValue?(collection[key]);
		}

		public static TValue? NValueOrProvide<TKey, TValue>(this IDictionary<TKey, TValue> collection, TKey key, Func<TValue?> defaultValueProvider,
			Action<TKey, TValue?> process) where TValue : struct
		{
			TValue? nullable = collection.NValueOrProvide<TKey, TValue>(key, defaultValueProvider);
			process(key, nullable);
			return nullable;
		}

		public static IEnumerable<TValue> ValueOrDefault<TKey, TValue>(this ILookup<TKey, TValue> collection, TKey key, IEnumerable<TValue> defaultValue = null)
		{
			if (!collection.Contains(key))
				return defaultValue;
			return collection[key];
		}

		public static IEnumerable<TValue> ValueOrProvide<TKey, TValue>(this ILookup<TKey, TValue> collection, TKey key,
			Func<IEnumerable<TValue>> defaultValueProvider)
		{
			if (!collection.Contains(key))
				return defaultValueProvider();
			return collection[key];
		}

		public static IEnumerable<TValue> ValueOrProvide<TKey, TValue>(this ILookup<TKey, TValue> collection, TKey key,
			Func<IEnumerable<TValue>> defaultValueProvider, Action<TKey, IEnumerable<TValue>> process)
		{
			IEnumerable<TValue> objs = collection.ValueOrProvide<TKey, TValue>(key, defaultValueProvider);
			process(key, objs);
			return objs;
		}
	}
}