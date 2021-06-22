using System;
using System.Collections.Generic;
using UnityEngine;

namespace RobertStan.ColliderUpdater
{
    public class Vertices : List<Vector2>
    {
        public Vertices(int capacity) : base(capacity) { }

        public Vertices(IEnumerable<Vector2> vertices)
        {
            AddRange(vertices);
        }
    }

    public sealed class TextureConverter
    {
        private const int closePixelsLength = 8;
        private static int[,] closePixels = new int[closePixelsLength, 2] { { -1, -1 }, { 0, -1 }, { 1, -1 }, { 1, 0 }, { 1, 1 }, { 0, 1 }, { -1, 1 }, { -1, 0 } };
        private const float hullTolerance = 0.9f;

        private byte[] solids;
        private int solidsLength;
        private int width;
        private int height;

        public List<Vertices> DetectVertices(UnityEngine.Color[] colors, int width, int alphaTolerance)
        {
            this.width = width;
            this.height = colors.Length / width;
            int n, s;

            solids = new byte[colors.Length];

            for (int i = 0; i < solids.Length; i++)
            {
                n = alphaTolerance - (int)(colors[i].a * 255.0f);
                s = ((int)((n & 0x80000000) >> 31)) - 1;
                n = n * s * s;
                solids[i] = (byte)n;
            }

            solidsLength = colors.Length;

            List<Vertices> detectedVerticesList = DetectVertices();
            List<Vertices> result = new List<Vertices>();

            for (int i = 0; i < detectedVerticesList.Count; i++)
            {
                result.Add(detectedVerticesList[i]);
            }

            return result;
        }

        public List<Vertices> DetectVertices()
        {
            List<Vertices> detectedPolygons = new List<Vertices>();
            Vector2? holeEntrance = null;
            Vector2? polygonEntrance = null;
            List<Vector2> blackList = new List<Vector2>();

            bool searchOn;
            do
            {
                Vertices polygon;
                if (detectedPolygons.Count == 0)
                {
                    polygon = new Vertices(CreateSimplePolygon(Vector2.zero, Vector2.zero));
                    if (polygon.Count > 2)
                    {
                        polygonEntrance = GetTopMostVertex(polygon);
                    }
                }
                else if (polygonEntrance.HasValue)
                {
                    polygon = new Vertices(CreateSimplePolygon(polygonEntrance.Value, new Vector2(polygonEntrance.Value.x - 1f, polygonEntrance.Value.y)));
                }
                else
                {
                    break;
                }
                searchOn = false;

                if (polygon.Count > 2)
                {
                    do
                    {
                        holeEntrance = SearchHoleEntrance(polygon, holeEntrance);

                        if (holeEntrance.HasValue)
                        {
                            if (!blackList.Contains(holeEntrance.Value))
                            {
                                blackList.Add(holeEntrance.Value);
                                Vertices holePolygon = CreateSimplePolygon(holeEntrance.Value, new Vector2(holeEntrance.Value.x + 1, holeEntrance.Value.y));
                                if (holePolygon != null && holePolygon.Count > 2)
                                {
                                    holePolygon.Add(holePolygon[0]);
                                    int vertex1Index, vertex2Index;
                                    if (SplitPolygonEdge(polygon, holeEntrance.Value, out vertex1Index, out vertex2Index))
                                    {
                                        polygon.InsertRange(vertex2Index, holePolygon);
                                    }
                                    break;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            break;
                        }
                    }
                    while (true);

                    detectedPolygons.Add(polygon);
                }

                if (polygonEntrance.HasValue && SearchNextHullEntrance(detectedPolygons, polygonEntrance.Value, out polygonEntrance))
                {
                    searchOn = true;
                }
            }
            while (searchOn);

            if (detectedPolygons == null || (detectedPolygons != null && detectedPolygons.Count == 0))
            {
                throw new Exception("Couldn't detect any vertices.");
            }

            return detectedPolygons;
        }

        public bool IsSolid(ref Vector2 v)
        {
            return IsSolid((int)v.x + ((int)v.y * width));
        }

        public bool IsSolid(int x, int y)
        {
            return IsSolid(x + (y * width));
        }

        public bool IsSolid(int index)
        {
            return (index >= 0 && index < solids.Length && solids[index] == 0);
        }

        public bool InBounds(ref Vector2 coord)
        {
            return (coord.x >= 0f && coord.x < width && coord.y >= 0f && coord.y < height);
        }

        private Vector2? SearchHoleEntrance(Vertices polygon, Vector2? lastHoleEntrance)
        {
            if (polygon == null)
            {
                throw new ArgumentNullException("'polygon' can't be null.");
            }
            else if (polygon.Count < 3)
            {
                throw new ArgumentException("'polygon.MainPolygon.Count' can't be less then 3.");
            }

            List<float> xCoords;
            Vector2? entrance;

            int startY;
            int endY;

            int lastSolid = 0;
            bool foundSolid;
            bool foundTransparent;

            if (lastHoleEntrance.HasValue)
            {
                startY = (int)lastHoleEntrance.Value.y;
            }
            else
            {
                startY = (int)GetTopMostCoord(polygon);
            }

            endY = (int)GetBottomMostCoord(polygon);

            if (startY >= 0 && startY < height && endY > 0 && endY < height)
            {
                for (int y = startY; y <= endY; y++)
                {
                    xCoords = SearchCrossingEdges(polygon, y);

                    if (xCoords.Count > 1 && xCoords.Count % 2 == 0)
                    {
                        for (int i = 0; i < xCoords.Count; i += 2)
                        {
                            foundSolid = false;
                            foundTransparent = false;

                            for (int x = (int)xCoords[i]; x <= (int)xCoords[i + 1]; x++)
                            {
                                if (IsSolid(x, y))
                                {
                                    if (!foundTransparent)
                                    {
                                        foundSolid = true;
                                        lastSolid = x;
                                    }

                                    if (foundSolid && foundTransparent)
                                    {
                                        entrance = new Vector2(lastSolid, y);

                                        if (DistanceToHullAcceptable(polygon, entrance.Value, true))
                                            return entrance;

                                        entrance = null;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (foundSolid)
                                        foundTransparent = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (xCoords.Count % 2 == 0)
                            System.Diagnostics.Debug.WriteLine("SearchCrossingEdges() % 2 != 0");
                    }
                }
            }

            return null;
        }

        private bool DistanceToHullAcceptableHoles(Vertices polygon, Vector2 point, bool higherDetail)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "'polygon' can't be null.");

            if (polygon.Count < 3)
                throw new ArgumentException("'polygon.MainPolygon.Count' can't be less then 3.");

            if (DistanceToHullAcceptable(polygon, point, higherDetail))
            {
                return true;
            }

            return false;
        }

        private bool DistanceToHullAcceptable(Vertices polygon, Vector2 point, bool higherDetail)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "'polygon' can't be null.");

            if (polygon.Count < 3)
                throw new ArgumentException("'polygon.Count' can't be less then 3.");


            Vector2 edgeVertex2 = polygon[polygon.Count - 1];
            Vector2 edgeVertex1;

            if (higherDetail)
            {
                for (int i = 0; i < polygon.Count; i++)
                {
                    edgeVertex1 = polygon[i];

                    if (LineTools.DistanceBetweenPointAndLineSegment(ref point, ref edgeVertex1, ref edgeVertex2) <= hullTolerance || Vector2.Distance(point, edgeVertex1) <= hullTolerance)
                        return false;

                    edgeVertex2 = polygon[i];
                }

                return true;
            }
            else
            {
                for (int i = 0; i < polygon.Count; i++)
                {
                    edgeVertex1 = polygon[i];

                    if (LineTools.DistanceBetweenPointAndLineSegment(ref point, ref edgeVertex1, ref edgeVertex2) <= hullTolerance)
                        return false;

                    edgeVertex2 = polygon[i];
                }

                return true;
            }
        }

        private bool InPolygon(Vertices polygon, Vector2 point)
        {
            bool inPolygon = !DistanceToHullAcceptableHoles(polygon, point, true);

            if (!inPolygon)
            {
                List<float> xCoords = SearchCrossingEdgesHoles(polygon, (int)point.y);

                if (xCoords.Count > 0 && xCoords.Count % 2 == 0)
                {
                    for (int i = 0; i < xCoords.Count; i += 2)
                    {
                        if (xCoords[i] <= point.x && xCoords[i + 1] >= point.x)
                            return true;
                    }
                }

                return false;
            }

            return true;
        }

        private Vector2? GetTopMostVertex(Vertices vertices)
        {
            float topMostValue = float.MaxValue;
            Vector2? topMost = null;

            for (int i = 0; i < vertices.Count; i++)
            {
                if (topMostValue > vertices[i].y)
                {
                    topMostValue = vertices[i].y;
                    topMost = vertices[i];
                }
            }

            return topMost;
        }

        private float GetTopMostCoord(Vertices vertices)
        {
            float returnValue = float.MaxValue;

            for (int i = 0; i < vertices.Count; i++)
            {
                if (returnValue > vertices[i].y)
                {
                    returnValue = vertices[i].y;
                }
            }

            return returnValue;
        }

        private float GetBottomMostCoord(Vertices vertices)
        {
            float returnValue = float.MinValue;

            for (int i = 0; i < vertices.Count; i++)
            {
                if (returnValue < vertices[i].y)
                {
                    returnValue = vertices[i].y;
                }
            }

            return returnValue;
        }

        private List<float> SearchCrossingEdgesHoles(Vertices polygon, int y)
        {
            if (polygon == null)
                throw new ArgumentNullException("polygon", "'polygon' can't be null.");

            if (polygon.Count < 3)
                throw new ArgumentException("'polygon.MainPolygon.Count' can't be less then 3.");

            List<float> result = SearchCrossingEdges(polygon, y);

            result.Sort();
            return result;
        }

        private List<float> SearchCrossingEdges(Vertices polygon, int y)
        {
            List<float> edges = new List<float>();

            Vector2 slope;
            Vector2 vertex1;    // i
            Vector2 vertex2;    // i - 1

            Vector2 nextSlope;
            Vector2 nextVertex; // i + 1

            bool addFind;

            if (polygon.Count > 2)
            {
                vertex2 = polygon[polygon.Count - 1];

                for (int i = 0; i < polygon.Count; i++)
                {
                    vertex1 = polygon[i];

                    if ((vertex1.y >= y && vertex2.y <= y) ||
                        (vertex1.y <= y && vertex2.y >= y))
                    {
                        if (vertex1.y != vertex2.y)
                        {
                            addFind = true;
                            slope = vertex2 - vertex1;

                            if (vertex1.y == y)
                            {
                                nextVertex = polygon[(i + 1) % polygon.Count];
                                nextSlope = vertex1 - nextVertex;

                                if (slope.y > 0)
                                    addFind = (nextSlope.y <= 0);
                                else
                                    addFind = (nextSlope.y >= 0);
                            }

                            if (addFind)
                                edges.Add((y - vertex1.y) / slope.y * slope.x + vertex1.x);
                        }
                    }

                    vertex2 = vertex1;
                }
            }

            edges.Sort();
            return edges;
        }

        private bool SplitPolygonEdge(Vertices polygon, Vector2 coordInsideThePolygon, out int vertex1Index, out int vertex2Index)
        {
            Vector2 slope;
            int nearestEdgeVertex1Index = 0;
            int nearestEdgeVertex2Index = 0;
            bool edgeFound = false;

            float shortestDistance = float.MaxValue;

            bool edgeCoordFound = false;
            Vector2 foundEdgeCoord = Vector2.zero;

            List<float> xCoords = SearchCrossingEdges(polygon, (int)coordInsideThePolygon.y);

            vertex1Index = 0;
            vertex2Index = 0;

            foundEdgeCoord.y = coordInsideThePolygon.y;

            if (xCoords != null && xCoords.Count > 1 && xCoords.Count % 2 == 0)
            {
                float distance;
                for (int i = 0; i < xCoords.Count; i++)
                {
                    if (xCoords[i] < coordInsideThePolygon.x)
                    {
                        distance = coordInsideThePolygon.x - xCoords[i];

                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;
                            foundEdgeCoord.x = xCoords[i];

                            edgeCoordFound = true;
                        }
                    }
                }

                if (edgeCoordFound)
                {
                    shortestDistance = float.MaxValue;

                    int edgeVertex2Index = polygon.Count - 1;

                    int edgeVertex1Index;
                    for (edgeVertex1Index = 0; edgeVertex1Index < polygon.Count; edgeVertex1Index++)
                    {
                        Vector2 tempVector1 = polygon[edgeVertex1Index];
                        Vector2 tempVector2 = polygon[edgeVertex2Index];
                        distance = LineTools.DistanceBetweenPointAndLineSegment(ref foundEdgeCoord,
                            ref tempVector1, ref tempVector2);
                        if (distance < shortestDistance)
                        {
                            shortestDistance = distance;

                            nearestEdgeVertex1Index = edgeVertex1Index;
                            nearestEdgeVertex2Index = edgeVertex2Index;

                            edgeFound = true;
                        }

                        edgeVertex2Index = edgeVertex1Index;
                    }

                    if (edgeFound)
                    {
                        slope = polygon[nearestEdgeVertex2Index] - polygon[nearestEdgeVertex1Index];
                        slope.Normalize();

                        Vector2 tempVector = polygon[nearestEdgeVertex1Index];
                        distance = Vector2.Distance(tempVector, foundEdgeCoord);

                        vertex1Index = nearestEdgeVertex1Index;
                        vertex2Index = nearestEdgeVertex1Index + 1;

                        polygon.Insert(nearestEdgeVertex1Index, distance * slope + polygon[vertex1Index]);
                        polygon.Insert(nearestEdgeVertex1Index, distance * slope + polygon[vertex2Index]);

                        return true;
                    }
                }
            }

            return false;
        }

        private Vertices CreateSimplePolygon(Vector2 entrance, Vector2 last)
        {
            bool entranceFound = false;
            bool endOfHull = false;

            Vertices polygon = new Vertices(32);
            Vertices hullArea = new Vertices(32);
            Vertices endOfHullArea = new Vertices(32);

            Vector2 current = Vector2.zero;

            #region Entrance check

            if (entrance == Vector2.zero || !InBounds(ref entrance))
            {
                entranceFound = SearchHullEntrance(out entrance);
                if (entranceFound)
                {
                    current = new Vector2(entrance.x - 1f, entrance.y);
                }
            }
            else
            {
                if (IsSolid(ref entrance))
                {
                    if (IsNearPixel(ref entrance, ref last))
                    {
                        current = last;
                        entranceFound = true;
                    }
                    else
                    {
                        Vector2 temp;
                        if (SearchNearPixels(false, ref entrance, out temp))
                        {
                            current = temp;
                            entranceFound = true;
                        }
                        else
                        {
                            entranceFound = false;
                        }
                    }
                }
            }

            #endregion

            if (entranceFound)
            {
                polygon.Add(entrance);
                hullArea.Add(entrance);

                Vector2 next = entrance;

                do
                {
                    Vector2 outstanding;
                    if (SearchForOutstandingVertex(hullArea, out outstanding))
                    {
                        if (endOfHull)
                        {
                            if (endOfHullArea.Contains(outstanding))
                            {
                                polygon.Add(outstanding);
                            }

                            break;
                        }

                        polygon.Add(outstanding);
                        hullArea.RemoveRange(0, hullArea.IndexOf(outstanding));
                    }

                    last = current;
                    current = next;

                    if (GetNextHullPoint(ref last, ref current, out next))
                    {
                        hullArea.Add(next);
                    }
                    else
                    {
                        break;
                    }

                    if (next == entrance && !endOfHull)
                    {
                        endOfHull = true;
                        endOfHullArea.AddRange(hullArea);

                        if (endOfHullArea.Contains(entrance))
                            endOfHullArea.Remove(entrance);
                    }

                } while (true);
            }

            return polygon;
        }

        private bool SearchNearPixels(bool searchingForSolidPixel, ref Vector2 current, out Vector2 foundPixel)
        {
            for (int i = 0; i < closePixelsLength; i++)
            {
                int x = (int)current.x + closePixels[i, 0];
                int y = (int)current.y + closePixels[i, 1];

                if (!searchingForSolidPixel ^ IsSolid(x, y))
                {
                    foundPixel = new Vector2(x, y);
                    return true;
                }
            }

            foundPixel = Vector2.zero;
            return false;
        }

        private bool IsNearPixel(ref Vector2 current, ref Vector2 near)
        {
            for (int i = 0; i < closePixelsLength; i++)
            {
                int x = (int)current.x + closePixels[i, 0];
                int y = (int)current.y + closePixels[i, 1];

                if (x >= 0 && x <= width && y >= 0 && y <= height)
                {
                    if (x == (int)near.x && y == (int)near.y)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private bool SearchHullEntrance(out Vector2 entrance)
        {
            for (int y = 0; y <= height; y++)
            {
                for (int x = 0; x <= width; x++)
                {
                    if (IsSolid(x, y))
                    {
                        entrance = new Vector2(x, y);
                        return true;
                    }
                }
            }

            entrance = Vector2.zero;
            return false;
        }

        private bool SearchNextHullEntrance(List<Vertices> detectedPolygons, Vector2 start, out Vector2? entrance)
        {
            int x;

            bool foundTransparent = false;
            bool inPolygon = false;

            for (int i = (int)start.x + (int)start.y * width; i <= solidsLength; i++)
            {
                if (IsSolid(i))
                {
                    if (foundTransparent)
                    {
                        x = i % width;
                        entrance = new Vector2(x, (i - x) / (float)width);

                        inPolygon = false;
                        for (int polygonIdx = 0; polygonIdx < detectedPolygons.Count; polygonIdx++)
                        {
                            if (InPolygon(detectedPolygons[polygonIdx], entrance.Value))
                            {
                                inPolygon = true;
                                break;
                            }
                        }

                        if (inPolygon)
                            foundTransparent = false;
                        else
                            return true;
                    }
                }
                else
                    foundTransparent = true;
            }

            entrance = null;
            return false;
        }

        private bool GetNextHullPoint(ref Vector2 last, ref Vector2 current, out Vector2 next)
        {
            int x;
            int y;

            int indexOfFirstPixelToCheck = GetIndexOfFirstPixelToCheck(ref last, ref current);
            int indexOfPixelToCheck;

            for (int i = 0; i < closePixelsLength; i++)
            {
                indexOfPixelToCheck = (indexOfFirstPixelToCheck + i) % closePixelsLength;

                x = (int)current.x + closePixels[indexOfPixelToCheck, 0];
                y = (int)current.y + closePixels[indexOfPixelToCheck, 1];

                if (x >= 0 && x < width && y >= 0 && y <= height)
                {
                    if (IsSolid(x, y))
                    {
                        next = new Vector2(x, y);
                        return true;
                    }
                }
            }

            next = Vector2.zero;
            return false;
        }

        private bool SearchForOutstandingVertex(Vertices hullArea, out Vector2 outstanding)
        {
            Vector2 outstandingResult = Vector2.zero;
            bool found = false;

            if (hullArea.Count > 2)
            {
                int hullAreaLastPoint = hullArea.Count - 1;

                Vector2 tempVector1;
                Vector2 tempVector2 = hullArea[0];
                Vector2 tempVector3 = hullArea[hullAreaLastPoint];

                for (int i = 1; i < hullAreaLastPoint; i++)
                {
                    tempVector1 = hullArea[i];

                    if (LineTools.DistanceBetweenPointAndLineSegment(ref tempVector1, ref tempVector2, ref tempVector3) >= hullTolerance)
                    {
                        outstandingResult = hullArea[i];
                        found = true;
                        break;
                    }
                }
            }

            outstanding = outstandingResult;
            return found;
        }

        private int GetIndexOfFirstPixelToCheck(ref Vector2 last, ref Vector2 current)
        {
            switch ((int)(current.x - last.x))
            {
                case 1:
                    switch ((int)(current.y - last.y))
                    {
                        case 1:
                            return 1;

                        case 0:
                            return 0;

                        case -1:
                            return 7;
                    }
                    break;

                case 0:
                    switch ((int)(current.y - last.y))
                    {
                        case 1:
                            return 2;

                        case -1:
                            return 6;
                    }
                    break;

                case -1:
                    switch ((int)(current.y - last.y))
                    {
                        case 1:
                            return 3;

                        case 0:
                            return 4;

                        case -1:
                            return 5;
                    }
                    break;
            }

            return 0;
        }
    }

    public static class LineTools
    {
        public static float DistanceBetweenPointAndLineSegment(ref Vector2 point, ref Vector2 start, ref Vector2 end)
        {
            if (start == end)
                return Vector2.Distance(point, start);

            Vector2 v = end - start;
            Vector2 w = point - start;

            float c1 = Vector2.Dot(w, v);
            if (c1 <= 0) return Vector2.Distance(point, start);

            float c2 = Vector2.Dot(v, v);
            if (c2 <= c1) return Vector2.Distance(point, end);

            float b = c1 / c2;
            Vector2 pointOnLine = (start + (v * b));
            return Vector2.Distance(point, pointOnLine);
        }
    }

    public static class SimplifyTools
    {

        public static Vertices DouglasPeuckerSimplify(Vertices vertices, float distanceTolerance)
        {
            if (vertices.Count <= 3)
                return vertices;

            bool[] usePoint = new bool[vertices.Count];

            for (int i = 0; i < vertices.Count; i++)
                usePoint[i] = true;

            SimplifySection(vertices, 0, vertices.Count - 1, usePoint, distanceTolerance);

            Vertices simplified = new Vertices(vertices.Count);

            for (int i = 0; i < vertices.Count; i++)
            {
                if (usePoint[i])
                    simplified.Add(vertices[i]);
            }

            return simplified;
        }

        private static void SimplifySection(Vertices vertices, int i, int j, bool[] usePoint, float distanceTolerance)
        {
            if ((i + 1) == j)
                return;

            Vector2 a = vertices[i];
            Vector2 b = vertices[j];

            double maxDistance = -1.0;
            int maxIndex = i;
            for (int k = i + 1; k < j; k++)
            {
                Vector2 point = vertices[k];

                double distance = LineTools.DistanceBetweenPointAndLineSegment(ref point, ref a, ref b);

                if (distance > maxDistance)
                {
                    maxDistance = distance;
                    maxIndex = k;
                }
            }

            if (maxDistance <= distanceTolerance)
            {
                for (int k = i + 1; k < j; k++)
                {
                    usePoint[k] = false;
                }
            }
            else
            {
                SimplifySection(vertices, i, maxIndex, usePoint, distanceTolerance);
                SimplifySection(vertices, maxIndex, j, usePoint, distanceTolerance);
            }
        }
    }
}