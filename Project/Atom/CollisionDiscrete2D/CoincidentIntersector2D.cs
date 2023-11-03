using System.Collections;
using System.Collections.Generic;
using System;
using DoubleEngine.Atom;

//P modified from: https://stackoverflow.com/a/2255848 of https://stackoverflow.com/questions/2255842/detecting-coincident-subset-of-two-coincident-line-segments

// port of this JavaScript code with some changes:
//   http://www.kevlindev.com/gui/math/intersection/Intersection.js
// found here:
//   http://stackoverflow.com/questions/563198/how-do-you-detect-where-two-line-segments-intersect/563240#563240
namespace DoubleEngine;

public class CoincidentIntersector2D
{
    public static readonly double MyEpsilon = 0.0000_0001;//0.0000001;//0.00001;
    //public static readonly float MyEpsilonFloat = (float)MyEpsilon;

    public static bool EdgesParallel(EdgeVec2D a, EdgeVec2D b) => Math.Abs(Vec2D.Cross(a.segmentVector, b.segmentVector)) < MyEpsilon;
    // this is the general case. Really really general
    public static bool Intersection(EdgeVec2D a, EdgeVec2D b, out Vec2D resultStart, out Vec2D? resultEnd)
    {
        // no code for degenerate cases where one or 2 segments can be a Point (EdgeVec3D does not allow it)
        // at this point we know both a and b are actual segments
        Vec2D startsDifference = a.start - b.start;

        double ua_t = Vec2D.Cross(b.segmentVector, startsDifference);
        double ub_t = Vec2D.Cross(a.segmentVector, startsDifference);
        double u_b = Vec2D.Cross(a.segmentVector, b.segmentVector); 

        // Infinite lines intersect somewhere
        if (!(-MyEpsilon < u_b && u_b < MyEpsilon))   // e.g. u_b != 0.0
        {
            double ua = ua_t / u_b;
            double ub = ub_t / u_b;
            //without epsilon //if (0.0d <= ua && ua <= 1.0d && 0.0d <= ub && ub <= 1.0d)
            if ( -MyEpsilon <= ua && ua <= (1.0d+MyEpsilon) && -MyEpsilon <= ub && ub <= (1.0d+MyEpsilon) )
            {
                // intersect at point
                resultStart = new Vec2D(a.start.x + ua * (a.end.x - a.start.x),
                    a.start.y + ua * (a.end.y - a.start.y));
                resultEnd = null;
                return true;
            }
            else
            {
                // don't intersect
                resultStart = Vec2D.zero;
                resultEnd = null;
                return false;
            }
        }
        // 
        // coincident: lines (not just segments) are same or parallel
        // we need to find the common overlapping section of the lines
        // first find the distance (squared) from one point (a.start) to each point
        if ((-MyEpsilon < ua_t && ua_t < MyEpsilon)
           || (-MyEpsilon < ub_t && ub_t < MyEpsilon))
        {
            /// TODO refactor next lines - a.start.Equals(a.end) is should be allways false because of EdgeVec3D type
            if (a.start.Equals(a.end)) // danger!
                return OneD_Intersection(b, a, out resultStart, out resultEnd);
            else // safe
                return OneD_Intersection(a, b, out resultStart, out resultEnd);
        }

        // Parallel
        //return new Vec3D[] { };
        resultStart = Vec2D.zero;
        resultEnd = null;
        return false;

    }



    // IMPORTANT: a.start and a.end should not be same, e.g. a.start--a.end is a true segment, not a point
    // b1/b2 may be the same (b1--b2 is a point)
    private static bool OneD_Intersection(EdgeVec2D a, EdgeVec2D b, out Vec2D resultStart, out Vec2D? resultEnd)
    {
        //float ua.start = 0.0f; // by definition
        //float ua.end = 1.0f; // by definition
        double ub1, ub2;

        double denomx = a.end.x - a.start.x;
        double denomy = a.end.y - a.start.y;

        if (Math.Abs(denomx) > Math.Abs(denomy))
        {
            ub1 = (b.start.x - a.start.x) / denomx;
            ub2 = (b.end.x - a.start.x) / denomx;
        }
        else
        {
            ub1 = (b.start.y - a.start.y) / denomy;
            ub2 = (b.end.y - a.start.y) / denomy;
        }
        if (MathU.IntervalsOverlap(ub1, ub2, out var resultStartDouble, out var resultEndDouble))
        {
            resultStart = PointFromInterval(resultStartDouble);
            resultEnd = resultEndDouble.HasValue ? PointFromInterval(resultEndDouble.Value) : null;
            return true;
        }
        resultStart = Vec2D.zero;
        resultEnd = null;
        return false;

        Vec2D PointFromInterval(double f) => new Vec2D(a.end.x * f + a.start.x * (1.0f - f), a.end.y * f + a.start.y * (1.0f - f));
    }

    private static bool PointOnLine(Vec2D p, EdgeVec2D a)
    {
        double dummyU = 0.0f;
        double d = DistFromSeg(p, a, MyEpsilon, ref dummyU);
        return d < MyEpsilon;
    }

    private static double DistFromSeg(Vec2D p, EdgeVec2D q, double radius, ref double u)
    {
        // formula here:
        //http://mathworld.wolfram.com/Point-LineDistance2-Dimensional.html
        // where x0,y0 = p
        //       x1,y1 = q.start
        //       x2,y2 = q.end
        double dx21 = q.end.x - q.start.x;
        double dy21 = q.end.y - q.start.y;
        double dx10 = q.start.x - p.x;
        double dy10 = q.start.y - p.y;
        double segLength = Math.Sqrt(dx21 * dx21 + dy21 * dy21);
        if (segLength < MyEpsilon)
            throw new Exception("Expected line segment, not point.");
        double num = Math.Abs(dx21 * dy10 - dx10 * dy21);
        double d = num / segLength;
        return d;
    }
}


