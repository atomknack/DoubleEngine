using System.Collections;
using System.Collections.Generic;

namespace CollectionLike.Comparers.SetsWithCustomComparer;

public sealed class Vec3DCloseEnoughSet : HashSet<Vec3D>
{
    //private static readonly Vec3DComparer_2em5 _comparerInstance = new Vec3DComparer_2em5();
    public static Vec3DCloseEnoughSet NewHashSet() => new Vec3DCloseEnoughSet(Vec3DComparer_2em5.ComparerInstance);//_comparerInstance);
    private Vec3DCloseEnoughSet(Vec3DComparer_2em5 comparer) : base(comparer) { }
}
public sealed class Vec2DCloseEnoughSet : HashSet<Vec2D>
{
    //private static readonly Vec2DComparer_2em5 _comparerInstance = new Vec2DComparer_2em5();
    public static Vec2DCloseEnoughSet NewHashSet() => new Vec2DCloseEnoughSet(Vec2DComparer_2em5.ComparerInstance);
    private Vec2DCloseEnoughSet(Vec2DComparer_2em5 comparer) : base(comparer) { }
}
/*
public sealed class Vector2CloseEnoughSet : HashSet<Vector2>
{
private static readonly Vector2Comparer_1em5 _comparerInstance = new Vector2Comparer_1em5();
public static Vector2CloseEnoughSet NewHashSet() => new Vector2CloseEnoughSet(_comparerInstance);
private Vector2CloseEnoughSet(Vector2Comparer_1em5 comparer): base(comparer) { }
}
*/