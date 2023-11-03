using CollectionLike;
using CollectionLike.Enumerables;
using CollectionLike.Pooled;
using DoubleEngine.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

//using Collections.Pooled;

namespace DoubleEngine
{
    using Registry = LinearRegistyVec2D;

    public partial record IndexedPolyVec2D
    {
        public partial class Subtracter: IDisposable
        {
            /*public struct IndexSliver
            {
                internal List<RegistryIndex> _poly; //Poly is Clockwise
                internal List<List<RegistryIndex>> _holes; // any hole is CounterClockwise
            }
            */
            private Registry _registry;
            private PoolListEdgeRegistered _polyBuffer;
            private bool _haveAtLeastOneCollision;

            private PoolListEdgeVec2D _subtractingEdges;
            private PoolListTuple_edgeIndex_splitterPoint _subtractingEdgesSplitAt;

            internal Subtracter()
            {
                _registry = Registry.Create(1000);
                _polyBuffer = Expendables.CreateList<EdgeRegistered>(100);
                _subtractingEdgesSplitAt = Expendables.CreateList<(int edgeIndex, Vec2D splitterPoint)>(100);
                _haveAtLeastOneCollision = false;
            }
            public void Dispose()
            {
                _polyBuffer?.Dispose();
                _subtractingEdgesSplitAt?.Dispose();
                _registry?.Dispose();

            }

            private static PoolListEdgeRegistered SubtractingEdgesSplitedUnsorted(
                ReadOnlySpan<EdgeVec2D> subtractingEdges,
                IReadOnlyList<(int edgeIndex, Vec2D splitterPoint)> splitAt,
                Registry registry)
            {
                PoolListEdgeRegistered result = Expendables.CreateList<EdgeRegistered>(subtractingEdges.Length*2);

                for (int i = 0; i < subtractingEdges.Length; i++)
                {
                    Vec2D startPoint = subtractingEdges[i].start;
                    Vec2D[] splitPoints = splitAt.Where(x => x.edgeIndex == i).Select(x => x.splitterPoint).OrderBy(x => x.DistanceSqr(startPoint)).ToArray();
                    RegistryIndex regStart = registry.GetOrAdd(startPoint);
                    RegistryIndex regEnd = registry.GetOrAdd(subtractingEdges[i].end);
                    if (splitPoints.Length == 0)
                        result.Add(new EdgeRegistered(regStart, regEnd));
                    else
                    {
                        RegistryIndex prev = registry.GetOrAdd(splitPoints[0]);
                        result.Add(new EdgeRegistered(regStart, prev));
                        for (int k = 1; k < splitPoints.Length; k++)
                        {
                            RegistryIndex current = registry.GetOrAdd(splitPoints[k]);
                            result.Add(new EdgeRegistered(prev, current));
                            prev = current;
                        }
                        result.Add(new EdgeRegistered(prev, regEnd));
                    }
                }
                return result;

            }
            private static EdgeVec2D[] RegEdgesToEdgesVec2D(IReadOnlyList<EdgeRegistered> regEdges, Registry registry)
                => regEdges.Select(x => new EdgeVec2D(registry.GetItem(x.start), registry.GetItem(x.end))).ToArray();
            //private static Vec2D[] RegCornersToCornersVec2D(List<RegistryIndex> regCorners, Registry registry)
            //    => regCorners.Select(x => registry.GetItem(x)).ToArray();

            //WIP //probably should work, need testing
            internal static bool TryAssembleFromCutted(PoolListEdgeRegistered remainder,  IReadOnlyList<EdgeRegistered> subtracting,  Registry registry, out PoolListRegistryIndex corners)
            {
                if (remainder.Count == 0)
                {
                    corners = null;
                    return false;
                }
                corners = Expendables.CreateList<RegistryIndex>(remainder.Count + subtracting.Count);// new List<RegistryIndex>();
                EdgeRegistered starter = remainder.Last();
                //Debug.Log($"starter {starter}");
                EdgeRegistered last = starter;
                using PoolListEdgeRegistered nextEdges = Expendables.CreateList<EdgeRegistered>(remainder.Count+subtracting.Count);
                do
                {
                    nextEdges.Clear();
                    nextEdges.AddAllWhere(remainder, x => x.start == last.end);//.AddRange(remainder.Where(x => x.start == last.end));
                    nextEdges.AddAllWhere(subtracting, x => x.start == last.end);//.AddRange(subtracting.Where(x => x.start == last.end));
                    //Debug.Log($"nextEdges count { nextEdges.Count}");
                    if (nextEdges.Count == 0)
                    {
                        Logger.DebugLog(String.Join(", ", remainder));
                        Logger.DebugLog(String.Join(", ", subtracting));
                        Logger.DebugLog(String.Join(", ", registry.Snapshot().ToArray()));
                        Logger.DebugLog(String.Join(", ", corners));

                        throw new InvalidOperationException($"{last}");
                    }
                    last = RightmostAppropriateEdge(last, nextEdges, registry);

                    //Debug.Log($"last {last}");
                    corners.Add(last.start);
                    remainder.RemoveAll(x=>x==last);
                }
                while (last != starter);
                return true;
            }
            internal static EdgeRegistered RightmostAppropriateEdge(EdgeRegistered previous, IReadOnlyList<EdgeRegistered> choseFrom, Registry registry)
            {
                EdgeVec2D previousEdgeVec2D = previous.ToEdgeVec2D(registry);
                var current = choseFrom[0];
                for(int i = 1; i < choseFrom.Count; i++) 
                    if (EdgeShouldBeReplacedByRightMost(previousEdgeVec2D, current.ToEdgeVec2D(registry), choseFrom[i].ToEdgeVec2D(registry)))
                        current = choseFrom[i];
                return current;
            }
            internal static bool EdgeShouldBeReplacedByRightMost(in EdgeVec2D previous, in EdgeVec2D current, in EdgeVec2D newCurrent)
            {
                bool currentEndIsToTheLeft = previous.RelationToLeft(current.end);
                bool newIsToTheLeft = previous.RelationToLeft(newCurrent.end);
                if (currentEndIsToTheLeft && (!newIsToTheLeft))
                    return true;
                bool currentEndIsToTheRightOrSameDirection = !currentEndIsToTheLeft;
                if (currentEndIsToTheRightOrSameDirection && newIsToTheLeft)
                    return false;
                if (current.RelationToRight(newCurrent.end))
                    return true;
                return false;
            }
            
            public static bool CutOutIfOverlapsPoly(ROSpanVec2D poly, ROSpanVec2D subtracting, out Vec2D[] newVertices, out PoolListEdgeIndexed singleEdges)
            {
                using var subtracter = new Subtracter();

                using PoolListRegistryIndex regPoly = AddCorners(stackalloc int[poly.Length].FillAsRange(), poly, subtracter._registry);
                using PoolListRegistryIndex regSubtracting = AddCorners(stackalloc int[subtracting.Length].FillAsRange(), subtracting, subtracter._registry);

                subtracter.AddPolyEdgesThatNotContactWithSubtracting(regPoly.Span, regSubtracting.Span);
                if (subtracter._haveAtLeastOneCollision == false && CollisionDiscrete2D.PointInsidePolygon(poly, subtracting[0]) == false)
                {
                    newVertices = null;
                    singleEdges = null;
                    return false;
                }

                RemapperRegistryIndex remapper;

                if(subtracter._haveAtLeastOneCollision == false)
                {
                    remapper = new RemapperRegistryIndex(regPoly.Count + regSubtracting.Count);
                    remapper.AddMany(regPoly.Span);
                    regSubtracting.Reverse();
                    remapper.AddMany(regSubtracting.Span);
                    newVertices = remapper.BuildRemappedFromRegister<Vec2D>(subtracter._registry);

                    singleEdges = Expendables.CreateList<EdgeIndexed>(regPoly.Count + regSubtracting.Count);
                    remapper.RemappedToBuffer(singleEdges, regPoly.RegisteredEdgesEnumerableFromCorners());
                    remapper.RemappedToBuffer(singleEdges, regSubtracting.RegisteredEdgesEnumerableFromCorners());
                    return true;
                }
                using var subtractSplited = SubtractingEdgesSplitedUnsorted(subtracter._subtractingEdges.Span, subtracter._subtractingEdgesSplitAt, subtracter._registry);
                EdgeRegistered.ReverseEdgesDirectionInplace(subtractSplited.Span);

                using PoolListEdgeRegistered registeredSingleEdges = 
                    Expendables.CreateList<EdgeRegistered>(subtracter._polyBuffer.Capacity + subtractSplited.Count);

                bool canAssemble = true;
                while (canAssemble)
                {
                    PoolListRegistryIndex assembledPoly;
                    canAssemble = TryAssembleFromCutted(subtracter._polyBuffer, subtractSplited, subtracter._registry, out assembledPoly);
                    using (assembledPoly)
                        if (canAssemble)
                        {

                            registeredSingleEdges.AddRange(assembledPoly.RegisteredEdgesEnumerableFromCorners());
                        }
                }
                remapper = new RemapperRegistryIndex(registeredSingleEdges.Count*2);
                remapper.AddMany(registeredSingleEdges.Span);
                newVertices = remapper.BuildRemappedFromRegister<Vec2D>(subtracter._registry);

                singleEdges = Expendables.CreateList<EdgeIndexed>(subtracter._polyBuffer.Capacity + subtractSplited.Count);
                remapper.RemappedToBuffer(singleEdges, registeredSingleEdges.Span);
                return true;
            }
            /*
            public static (EdgeVec2D[] cutOut, EdgeVec2D[] splitedSubtracting, List<Vec2D[]> polys) DebugTestCutOutFromPoly(ReadOnlySpan<Vec2D> poly, ReadOnlySpan<Vec2D> subtracting)
            {
                using var subtracter = new Subtracter();
                return subtracter.Debug_TestCutOutFromPoly(poly, subtracting);
            }

            internal (EdgeVec2D[] cutOut, EdgeVec2D[] splitedSubtracting, List<Vec2D[]> polys) Debug_TestCutOutFromPoly(ROSpanVec2D poly, ROSpanVec2D subtracting)
            {
                using PoolListRegistryIndex regPoly = AddCorners(stackalloc int[poly.Length].FillAsRange(), poly, _registry);
                using PoolListRegistryIndex regSubtracting = AddCorners(stackalloc int[subtracting.Length].FillAsRange(), subtracting, _registry);
                AddPolyEdgesThatNotContactWithSubtracting(regPoly.Span, regSubtracting.Span);
                using var cuttedCopy = _polyBuffer.Clone();
                using var subtractSplited = SubtractingEdgesSplitedUnsorted(_subtractingEdges, _subtractingEdgesSplitAt, _registry);
                EdgeRegistered.ReverseEdgesDirectionInplace(subtractSplited.Span);

                List<Vec2D[]> polys = new List<Vec2D[]>();

                bool canAssemble = true;
                while (canAssemble)
                {
                    PoolListRegistryIndex assembledPoly;
                    canAssemble = TryAssembleFromCutted(_polyBuffer, subtractSplited, _registry, out assembledPoly);
                    using (assembledPoly)
                        if (canAssemble)
                        {
                            //Debug.Log($"Found assembled poly {assempledPoly.Count}");
                            //Debug.Log(String.Join("_", assempledPoly.Select(x=> x.Value)));
                            polys.Add(_registry.AssembleIndices(assembledPoly));
                        }

                } 
                //assempledPoly?.Dispose();

                //DebugSubtracter();

                return (
                    RegEdgesToEdgesVec2D(cuttedCopy, _registry),
                    RegEdgesToEdgesVec2D(subtractSplited, _registry),
                    polys
                    );
            }
            */
            private void AddPolyEdgesThatNotContactWithSubtracting(ReadOnlySpan<RegistryIndex> fromPoly, ReadOnlySpan<RegistryIndex> subtractingRegPoly)
            {
                using PoolListVec2D subtractingPoly = _registry.PoolListAssembleIndices(subtractingRegPoly);
                _subtractingEdges = subtractingPoly.Span.PoolListEdgesVec2DFromCornersVec2D();// EdgesVec2DFromCornersVec2D();
                //Debug.Log(subtractingPoly);
                //Debug.Log()
                AddPolyToBuffer(fromPoly, _subtractingEdges.Span, subtractingPoly.Span);
            }

            private void AddPolyToBuffer(ReadOnlySpan<RegistryIndex> poly, ReadOnlySpan<EdgeVec2D> subtractingEdges, ReadOnlySpan<Vec2D> subtractingPoly)
            {
                SubdivideIfNeededAndAddToBuffer(_registry.GetItem(poly.Last()), _registry.GetItem(poly[0]), subtractingEdges, subtractingPoly);
                int indexOfLastElement = poly.IndexOfLast();
                for (int i = 0; i < indexOfLastElement; ++i)
                    SubdivideIfNeededAndAddToBuffer(_registry.GetItem(poly[i]), _registry.GetItem(poly[i + 1]), subtractingEdges, subtractingPoly);
            }
            private void SubdivideIfNeededAndAddToBuffer(Vec2D edgeStart, Vec2D edgeEnd,
                ReadOnlySpan<EdgeVec2D> subtractingPolyEdges,
                ReadOnlySpan<Vec2D> subtractingPoly)
            {
                bool startOutside = false;
                bool endOutside = false;
                bool haveCollision = false;
                RegistryIndex regEdgeStart = _registry.GetOrAdd(edgeStart);
                RegistryIndex regEdgeEnd = _registry.GetOrAdd(edgeEnd);

                if (regEdgeStart == regEdgeEnd)
                    return;
                EdgeVec2D edge = new EdgeVec2D(edgeStart, edgeEnd);

                //  = new EdgeVec2D(edgeStartVec2D, edgeEndVec2D);
                for (int i = 0; i < subtractingPolyEdges.Length; ++i)
                {
                    var subtractingEdge = subtractingPolyEdges[i];
                    if (CoincidentIntersector2D.Intersection(edge, subtractingEdge, out Vec2D collisionStart, out Vec2D? collisionEndNullable))
                    {
                        AddSubtractingEdgesSplit(subtractingPolyEdges, i, collisionStart);
                        haveCollision = true;
                        _haveAtLeastOneCollision = true;
                        //Debug.Log($"Intersection at {collisionStart}, main edge start {edge.start}, end {edge.end}");
                        RegistryIndex regCollisionStart = _registry.GetOrAdd(collisionStart);
                        //Vec2D collisionStart = _registry.GetItem(regCollisionStart);
                        if (CoincidentIntersector2D.EdgesParallel(edge, subtractingEdge))
                        {
                            //Debug.Log($"edge parrallel");
                            if (collisionEndNullable == null || regCollisionStart == _registry.GetOrAdd(collisionEndNullable.Value) )
                            {
                                if( ! subtractingEdge.PointBelongsToEdge(edgeStart) )
                                    startOutside = true;
                                if( ! subtractingEdge.PointBelongsToEdge(edgeEnd) )
                                    endOutside = true;
                                //continue;
                            }
                            else
                            {
                                Vec2D collisionEnd = collisionEndNullable.Value;
                                RegistryIndex regCollisionEnd = _registry.GetOrAdd(collisionEnd);
                                //if (regCollisionEnd == regCollisionStart)
                                //    throw new Exception($"Collision detection end equals start this should newer happen - check collision detection or epsilons");
                                AddSubtractingEdgesSplit(subtractingPolyEdges, i, collisionEnd);

                                if (subtractingEdge.PointBelongsToEdge(edge.start) && subtractingEdge.PointBelongsToEdge(edge.end))
                                    return;
                                //if (_polyBuffer.Contains(new EdgeRegistered(regEdgeStart, regEdgeEnd)))
                                //    return;
                                EdgeVec2D collisionEdge = new EdgeVec2D(collisionStart, collisionEnd);

                                if ( !collisionEdge.PointBelongsToEdge(edgeStart))
                                {
                                    if (edgeStart.DistanceSqr(collisionStart)<edgeStart.DistanceSqr(collisionEnd))
                                        SubdivideIfNeededAndAddToBuffer(edgeStart, collisionStart, subtractingPolyEdges, subtractingPoly);
                                    else
                                        SubdivideIfNeededAndAddToBuffer(edgeStart, collisionEnd, subtractingPolyEdges, subtractingPoly);
                                }

                                if (!collisionEdge.PointBelongsToEdge(edgeEnd))
                                {
                                    if (edgeEnd.DistanceSqr(collisionStart) < edgeEnd.DistanceSqr(collisionEnd))
                                        SubdivideIfNeededAndAddToBuffer(collisionStart, edgeEnd, subtractingPolyEdges, subtractingPoly);
                                    else
                                        SubdivideIfNeededAndAddToBuffer(collisionEnd, edgeEnd, subtractingPolyEdges, subtractingPoly);
                                }
                                //if (edgeStart.DistanceSqr(collisionStart) < edgeEnd.DistanceSqr(collisionEnd))


                                //Vec2D collisionEnd = _registry.GetItem(regCollisionEnd);
                                /*if (regEdgeStart != regCollisionStart && regEdgeStart != regCollisionEnd)
                                {
                                    if (edge.start.DistanceSqr(collisionStart) < edge.end.DistanceSqr(collisionEnd))
                                        SubdivideIfNeededAndAddToBuffer(edge.start, collisionStart, subtractingPolyEdges, subtractingPoly);
                                    else
                                        SubdivideIfNeededAndAddToBuffer(edge.start, collisionEnd, subtractingPolyEdges, subtractingPoly);
                                }
                                if (regEdgeEnd != regCollisionStart && regEdgeEnd != regCollisionEnd)
                                {
                                    if (edge.end.DistanceSqr(collisionStart) < edge.end.DistanceSqr(collisionEnd))
                                        SubdivideIfNeededAndAddToBuffer(collisionStart, edge.end, subtractingPolyEdges, subtractingPoly);
                                    else
                                        SubdivideIfNeededAndAddToBuffer(collisionEnd, edge.end, subtractingPolyEdges, subtractingPoly);
                                }*/
                                return;
                            }
                        }
                        else
                        {
                            if (regEdgeStart != regCollisionStart && regEdgeEnd != regCollisionStart)
                            {
                                SubdivideIfNeededAndAddToBuffer(edge.start, collisionStart, subtractingPolyEdges, subtractingPoly);
                                SubdivideIfNeededAndAddToBuffer(collisionStart, edge.end, subtractingPolyEdges, subtractingPoly);
                                return;
                            }

                            if (regEdgeStart == regCollisionStart && subtractingEdge.RelationToLeft(edge.end))
                                endOutside = true;
                            if (regEdgeEnd == regCollisionStart && subtractingEdge.RelationToLeft(edge.start))
                                startOutside = true;

                        }
                    }
                }

                if (startOutside || endOutside)
                {
                    AddPolyEdgeToBuffer(regEdgeStart, regEdgeEnd);
                    return;
                }


                if (haveCollision == false &&
                        CollisionDiscrete2D.PointInsidePolygon(subtractingPoly, edge.start) == false &&
                        CollisionDiscrete2D.PointInsidePolygon(subtractingPoly, edge.end) == false)
                    AddPolyEdgeToBuffer(regEdgeStart, regEdgeEnd);
            }
            private void AddPolyEdgeToBuffer(RegistryIndex regEdgeStart, RegistryIndex regEdgeEnd)
            {
                if (regEdgeStart != regEdgeEnd)
                    _polyBuffer.Add(new EdgeRegistered(regEdgeStart, regEdgeEnd));
            }
            private void AddSubtractingEdgesSplit(ReadOnlySpan<EdgeVec2D> subtractingEdges, int subtractingEdgeIndex, Vec2D splitPoint)
            {
                if (Registry.ItemsEqual(subtractingEdges[subtractingEdgeIndex].start, splitPoint))
                    return;
                if (Registry.ItemsEqual(subtractingEdges[subtractingEdgeIndex].end, splitPoint))
                    return;
                if (_subtractingEdgesSplitAt.Where(x => x.edgeIndex == subtractingEdgeIndex && Registry.ItemsEqual(x.splitterPoint, splitPoint)).Count() == 0)
                    _subtractingEdgesSplitAt.Add((subtractingEdgeIndex, splitPoint));
            }

            //need testing
            internal static PoolListRegistryIndex AddCorners(ReadOnlySpan<int> corners, ReadOnlySpan<Vec2D> vertices, IRegistry<Vec2D> registry)
            {

                PoolListRegistryIndex result = Expendables.CreateList<RegistryIndex>(corners.Length, sizeToCapacity: true);
                for (int i = 0; i < corners.Length; ++i)
                    result[i] = registry.GetOrAdd(vertices[corners[i]]);
                return result;
            }

        }
    }
}
