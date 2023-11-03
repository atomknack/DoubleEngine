namespace DoubleEngine.Atom;

public readonly struct ScaleInversionV3
{
    public readonly bool invertX;
    public readonly bool invertY;
    public readonly bool invertZ;
    public Vec3D ToScaleVec3D() => 
        new Vec3D(invertX ? -1f : 1f, invertY ? -1f : 1f, invertZ ? -1f : 1f);

    public ScaleInversionV3(bool invertX, bool invertY, bool invertZ)
    {
        this.invertX = invertX;
        this.invertY = invertY;
        this.invertZ = invertZ;
    }
}