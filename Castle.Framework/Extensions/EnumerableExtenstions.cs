using System;
using System.Collections.Generic;
using System.Linq;

public static class EnumerableExtenstions
{
    #region Recursive Join
    public static IEnumerable<TResult> RecursiveJoin<TSource, TKey, TResult>(this IEnumerable<TSource> source,
        Func<TSource, TKey> parentKeySelector,
        Func<TSource, TKey> childKeySelector,
        Func<TSource, IEnumerable<TResult>, TResult> resultSelector)
    {
        return RecursiveJoin(source, parentKeySelector, childKeySelector,
           resultSelector, Comparer<TKey>.Default);
    }

    public static IEnumerable<TResult> RecursiveJoin<TSource, TKey, TResult>(this IEnumerable<TSource> source,
        Func<TSource, TKey> parentKeySelector,
        Func<TSource, TKey> childKeySelector,
        Func<TSource, int, IEnumerable<TResult>, TResult> resultSelector)
    {
        return RecursiveJoin(source, parentKeySelector, childKeySelector,
           (TSource element, int depth, int index, IEnumerable<TResult> children)
              => resultSelector(element, index, children));
    }

    public static IEnumerable<TResult> RecursiveJoin<TSource, TKey, TResult>(this IEnumerable<TSource> source,
        Func<TSource, TKey> parentKeySelector,
        Func<TSource, TKey> childKeySelector,
        Func<TSource, IEnumerable<TResult>, TResult> resultSelector,
        IComparer<TKey> comparer)
    {
        return RecursiveJoin(source, parentKeySelector, childKeySelector,
           (TSource element, int depth, int index, IEnumerable<TResult> children)
              => resultSelector(element, children), comparer);
    }

    public static IEnumerable<TResult> RecursiveJoin<TSource, TKey, TResult>(this IEnumerable<TSource> source,
        Func<TSource, TKey> parentKeySelector,
        Func<TSource, TKey> childKeySelector,
        Func<TSource, int, IEnumerable<TResult>, TResult> resultSelector,
        IComparer<TKey> comparer)
    {
        return RecursiveJoin(source, parentKeySelector, childKeySelector,
           (TSource element, int depth, int index, IEnumerable<TResult> children)
              => resultSelector(element, index, children), comparer);
    }

    public static IEnumerable<TResult> RecursiveJoin<TSource, TKey, TResult>(this IEnumerable<TSource> source,
        Func<TSource, TKey> parentKeySelector,
        Func<TSource, TKey> childKeySelector,
        Func<TSource, int, int, IEnumerable<TResult>, TResult> resultSelector)
    {
        return RecursiveJoin(source, parentKeySelector, childKeySelector,
           resultSelector, Comparer<TKey>.Default);
    }

    public static IEnumerable<TResult> RecursiveJoin<TSource, TKey, TResult>(this IEnumerable<TSource> source,
        Func<TSource, TKey> parentKeySelector,
        Func<TSource, TKey> childKeySelector,
        Func<TSource, int, int, IEnumerable<TResult>, TResult> resultSelector,
        IComparer<TKey> comparer)
    {
        // prevent source being enumerated more than once per RecursiveJoin call
        source = new LinkedList<TSource>(source);

        // fast binary search lookup
        SortedDictionary<TKey, TSource> parents = new SortedDictionary<TKey, TSource>(comparer);
        SortedDictionary<TKey, LinkedList<TSource>> children
           = new SortedDictionary<TKey, LinkedList<TSource>>(comparer);

        foreach (TSource element in source)
        {
            parents[parentKeySelector(element)] = element;

            LinkedList<TSource> list;

            TKey childKey = childKeySelector(element);

            if (!children.TryGetValue(childKey, out list))
            {
                children[childKey] = list = new LinkedList<TSource>();
            }

            list.AddLast(element);
        }

        // initialize to null otherwise compiler complains at single line assignment
        Func<TSource, int, IEnumerable<TResult>> childSelector = null;

        childSelector = (TSource parent, int depth) =>
        {
            LinkedList<TSource> innerChildren = null;

            if (children.TryGetValue(parentKeySelector(parent), out innerChildren))
            {
                return innerChildren.Select((child, index)
                   => resultSelector(child, index, depth, childSelector(child, depth + 1)));
            }

            return Enumerable.Empty<TResult>();
        };

        return source.Where(element => !parents.ContainsKey(childKeySelector(element)))
           .Select((element, index) => resultSelector(element, index, 0, childSelector(element, 1)));
    }
    #endregion


    #region Recursive Select
    public static IEnumerable<TSource> RecursiveSelect<TSource>(this IEnumerable<TSource> source,
        Func<TSource, IEnumerable<TSource>> childSelector)
    {
        return RecursiveSelect(source, childSelector, element => element);
    }

    public static IEnumerable<TResult> RecursiveSelect<TSource, TResult>(this IEnumerable<TSource> source,
        Func<TSource, IEnumerable<TSource>> childSelector, Func<TSource, TResult> selector)
    {
        return RecursiveSelect(source, childSelector, (element, index, depth) => selector(element));
    }

    public static IEnumerable<TResult> RecursiveSelect<TSource, TResult>(this IEnumerable<TSource> source,
        Func<TSource, IEnumerable<TSource>> childSelector, Func<TSource, int, TResult> selector)
    {
        return RecursiveSelect(source, childSelector, (element, index, depth) => selector(element, index));
    }

    public static IEnumerable<TResult> RecursiveSelect<TSource, TResult>(this IEnumerable<TSource> source,
        Func<TSource, IEnumerable<TSource>> childSelector, Func<TSource, int, int, TResult> selector)
    {
        return RecursiveSelect(source, childSelector, selector, 0);
    }

    private static IEnumerable<TResult> RecursiveSelect<TSource, TResult>(this IEnumerable<TSource> source,
        Func<TSource, IEnumerable<TSource>> childSelector, Func<TSource, int, int, TResult> selector, int depth)
    {
        return source.SelectMany((element, index) => Enumerable.Repeat(selector(element, index, depth), 1)
           .Concat(RecursiveSelect(childSelector(element) ?? Enumerable.Empty<TSource>(),
              childSelector, selector, depth + 1)));
    }
    #endregion


    public static IEnumerable<T> Union<T>(this IEnumerable<T> source, T item)
    {
        return source.Union(Enumerable.Repeat(item, 1));
    }


    public static bool IsEmpty<T>(this IEnumerable<T> source)
    {
        return source.Count() == 0;
    }


    public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
    {
        for (var i = 0; i < (float)array.Length / size; i++)
        {
            yield return array.Skip(i * size).Take(size);
        }
    }
}

