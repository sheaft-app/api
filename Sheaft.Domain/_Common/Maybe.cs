namespace Sheaft.Domain;

/// <summary>
/// This code is from CSharpFunctionalExtensions nuget written by vladimir khorikov
/// </summary>
/// <typeparam name="T"></typeparam>
public struct Maybe<T> : IEquatable<Maybe<T>>
{
    private readonly T? _value;
    private bool _isValueSet;

    public T GetValueOrThrow(string? errorMessage = null)
    {
        if (HasNoValue || _value == null)
            throw new InvalidOperationException(errorMessage);
        
        return _value;
    }

    public T Value => this.GetValueOrThrow();

    public static Maybe<T> None => new Maybe<T>();

    public bool HasValue => _isValueSet;

    public bool HasNoValue => !HasValue;

    private Maybe(T value)
    {
        if (value == null)
        {
            _isValueSet = false;
            _value = default;
        }
        else
        {
            _isValueSet = true;
            _value = value;
        }
    }

    public static implicit operator Maybe<T>(T value) => value?.GetType() == typeof (Maybe<T>) ? (Maybe<T>) (object) value : new Maybe<T>(value);

    public static implicit operator Maybe<T>(Maybe value) => Maybe<T>.None;

    public static Maybe<T> From(T obj) => new Maybe<T>(obj);

    public static bool operator ==(Maybe<T> maybe, T value)
    {
        if ((object) value is Maybe<T>)
            return maybe.Equals(value);
        
        return maybe.HasNoValue ? value == null : maybe._value.Equals(value);
    }

    public static bool operator !=(Maybe<T> maybe, T value) => !(maybe == value);

    public static bool operator ==(Maybe<T> first, Maybe<T> second) => first.Equals(second);

    public static bool operator !=(Maybe<T> first, Maybe<T> second) => !(first == second);

    public override bool Equals(object? obj)
    {
        if (obj == null)
            return false;
        
        if (obj.GetType() != typeof (Maybe<T>))
        {
            if (obj is T objT)
                obj = new Maybe<T>(objT);
            
            if (obj is not Maybe<T>)
                return false;
        }
        
        return Equals((Maybe<T>) obj);
    }

    public bool Equals(Maybe<T> other)
    {
        if (HasNoValue && other.HasNoValue)
            return true;
        
        return !HasNoValue && !other.HasNoValue && _value.Equals(other._value);
    }

    public override int GetHashCode() => HasNoValue ? 0 : _value.GetHashCode();

    public override string ToString() => HasNoValue ? "No value" : _value.ToString();
}

public struct Maybe
{
    public static Maybe None => new Maybe();

    /// <summary>
    /// Creates a new <see cref="T:CSharpFunctionalExtensions.Maybe`1" /> from the provided <paramref name="value" />
    /// </summary>
    public static Maybe<T> From<T>(T value) => Maybe<T>.From(value);
}