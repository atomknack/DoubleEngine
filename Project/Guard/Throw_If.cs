using System;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CollectionLike.Pooled;
using CollectionLike.Enumerables;

namespace DoubleEngine.Guard;

internal static partial class Throw
{
    #region CA1062
    // to solve CA1062 https://github.com/dotnet/roslyn-analyzers/issues/3451#issuecomment-606690452
    [AttributeUsage(AttributeTargets.Parameter)]
    private sealed class ValidatedNotNullAttribute : Attribute { }
    #endregion

    public static void IfNull<T>([ValidatedNotNull] T obj, string paramName)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(paramName);
        }
    }

    public static void IfNull<T>([ValidatedNotNull] T obj, string paramName, string message)
    {
        if (obj is null)
        {
            throw new ArgumentNullException(paramName, message);
        }
    }
    public static void IfEmpty<T>(IEnumerable<T> values, string paramName, string message)
    {
        if (!values.Any())
        {
            throw new ArgumentException(message, paramName);
        }
    }
    public static void IfNullOrEmpty<T>([ValidatedNotNull] IEnumerable<T> values, string paramName)
    {
        if (values.IsNullOrEmpty())
        {
            throw new ArgumentNullException(paramName);
        }
    }

    public static void IfNullOrEmpty([ValidatedNotNull] string str, string paramName)
    {
        if (string.IsNullOrEmpty(str))
        {
            throw new ArgumentNullException(paramName);
        }
    }

    public static void IfNullOrEmpty([ValidatedNotNull] string str, string paramName, string message)
    {
        if (string.IsNullOrEmpty(str))
        {
            throw new ArgumentNullException(paramName, message);
        }
    }

    public static void IfContainsAnyNull<T>(IEnumerable<T> values, string paramName)
    {
        if (values.Any(t => t is null))
        {
            throw new ArgumentNullException(paramName, "Collection contains a null value");
        }
    }
    public static void IfEnumValueNotDefined<T>(T value, string paramName)
        where T : Enum
    {
        if (!Enum.IsDefined(typeof(T), value))
        {
            throw new ArgumentOutOfRangeException(paramName);
        }
    }
    public static void IfArgumentInNotRange([DoesNotReturnIf(false)] bool rangeCondition, string message, string paramName)
    {
        if (!rangeCondition)
            throw new ArgumentOutOfRangeException(paramName, message);
    }
    public static void IfArgumentNotValid([DoesNotReturnIf(false)] bool condition, string message, string paramName)
    {
        if (!condition)
            throw new ArgumentException(message, paramName);
    }

}
