using System;


namespace DoubleEngine.Atom;


public sealed partial class Grid6SidesCached
{
    private static byte[][] s_transformedByID;
    public static byte TransformedBy(byte whoIndex, byte byIndex)
    {
        return s_transformedByID[whoIndex][byIndex];
    }
    
    //[Obsolete("Need urgent testing!!!!!")]
    private static void CalcTable_s_transformedByID()
    {
        //Debug.Log("re5");

        Vec3D v1 = new Vec3D(1,2,3);
        Vec3D v2 = new Vec3D(-4,7,9);
        Vec3D[] v1s = new Vec3D[allPositions.Count];
        Vec3D[] v2s = new Vec3D[allPositions.Count];
        double[] transScale = new double[allPositions.Count];
        Vec3D[] Scales = new Vec3D[allPositions.Count];
        //Vec3D[] v2s = new Vec3D[allPositions.Count];
        //bool[] inversedYs = new bool[allPositions.Count];
        for (int i = 0; i < v1s.Length; i++)
        {
            var matrix = MatrixD4x4.FromRotation(allPositions[i]._rotation) * MatrixD4x4.FromScale(allPositions[i].Scale);
            v1s[i] = matrix.MultiplyPoint(v1);//example.Rotated(allPositions[i]._rotation);
            v2s[i] = matrix.MultiplyPoint(v2);
            //var tscale = matrix.MultiplyPoint(allPositions[i].Scale);
            var tscale = allPositions[i].Scale;
            transScale[i] = tscale.x*tscale.y*tscale.z;
            Scales[i] = allPositions[i].Scale;
        }
            

        int length = allPositions.Count;
        s_transformedByID = new byte[length][];
        for (int iWho = 0; iWho < length; ++iWho)
        {
            var who = allPositions[iWho];
            var whoQuaternion = who._rotation;
            var whoMatrix = MatrixD4x4.FromRotation(whoQuaternion) * MatrixD4x4.FromScale(who.Scale);
            s_transformedByID[iWho] = new byte[length];
            var byArr = s_transformedByID[iWho];
            for (int jBy = 0; jBy < length; ++jBy)
            {
                var by = allPositions[jBy];
                var byQuaternion = by._rotation;
                var byMatrix = MatrixD4x4.FromRotation(byQuaternion) * MatrixD4x4.FromScale(by.Scale);
                //var transformedQuaternion = QuaternionD.HamiltonProduct(whoQuaternion, byQuaternion); // if result, incorrect try switch places
                var transformation = byMatrix * whoMatrix;
                //byArr[jBy] = FindIndexByTransformedVector(example.Rotated(transformedQuaternion), transformedVectors);
                var newScale = Vec3D.MultiplyVect(who.Scale, by.Scale);// transformation.MultiplyPoint3x4(who.Scale);
                byArr[jBy] = FindIndexByTransformedVector(
                    transformation.MultiplyPoint3x4(v1), v1s//, 
                    //, transformation.ScaleMaybe(), Scales
                    //transformation.MultiplyPoint3x4(v2), v2s,
                    //newScale.x*newScale.y*newScale.z, transScale
                    );
            }
        }
    }

    private static byte FindIndexByTransformedVector(
        Vec3D v1, Vec3D[] v1s//, 
        //,Vec3D scale, Vec3D[] scales
        //Vec3D v2, Vec3D[] v2s, 
        //double s, double[] ss
        )
    {
        //var s = scale.x * scale.y * scale.z;
        for (int i = 0; i < v1s.Length; ++i)
        {
            //var ss = scales[i].x*scales[i].y*scales[i].z;
            if (//Math.Abs(s - ss[i]) < 0.0000001d &&
                v1.CloseByEach(v1s[i], 0.0000001d) //&&
                //&& Math.Abs(s - ss) < 0.0000001d
                //v2.CloseByEach(v2s[i], 0.0000001d)
                )
                return (byte)i;
        }
        throw new Exception("not found, this should newer happen"); //if this happens rewrite using bigger epsilon, or seach in allRotationsInversionsTable
    }
    
    /*
    [Obsolete("Need urgent testing!!!!!")]
    private static void CalcTable_s_transformedByID()
    {
        Debug.Log("re1");
        int length = allPositions.Count;
        s_transformedByID = new byte[length][];
        for (int iWho = 0; iWho < length; ++iWho)
        {
            var who = allPositions[iWho];
            var whoQuaternion = who._rotation;
            s_transformedByID[iWho] = new byte[length];
            for(int jBy = 0; jBy < length; ++jBy)
            {
                var by = allPositions[jBy];
                var byQuaternion = by._rotation;
                var transformedQuaternion = QuaternionD.HamiltonProduct(whoQuaternion, byQuaternion); // if result, incorrect try switch places
                s_transformedByID[iWho][jBy] = FindIndexByQuaternionD(transformedQuaternion, Vec3D.MultiplyVect(who.Scale,by.Scale));
            }
        }
    }
    //[Obsolete("Not tested at all, may be incorrect, Need urgent testing!!!!!")]
    private static byte FindIndexByQuaternionD(QuaternionD quaternion, Vec3D scale)
    {
        for (int i = 0; i < allPositions.Count; ++i)
        {
            if (allPositions[i].Scale.CloseByEach(scale, 0.000000001d) &&
                QuaternionD.IsEqualOrFlippedEqual(allPositions[i]._rotation, quaternion, 0.000000001d))
                return (byte)i;
        }
        throw new Exception("not found, this should newer happen"); //if this happens rewrite using bigger epsilon, or seach in allRotationsInversionsTable
    }
    */

}
