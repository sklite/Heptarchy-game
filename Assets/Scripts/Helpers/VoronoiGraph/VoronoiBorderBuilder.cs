using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Mth = Assets.Scripts.Helpers.MathCalculator;

namespace Assets.Scripts.Helpers.VoronoiGraph
{
    class VoronoiBorderBuilder
    {
        private readonly Vector3 _notFound = new Vector3(-99999, -99999, -99999);

        public List<VorBorder> BuildClosedShape(List<(Vector3, Vector3)> borders)
        {
            var lines = borders.Where(item => !Mth.EqualVectors(item.Item1, item.Item2, MathCalculator.MinNodeDistance)).ToList();

            var initialBorder = new VorBorder { Line = lines.First() };

            AddCornerNodes(lines);

            var restLines = new List<(Vector3, Vector3)>(lines);
            restLines.Remove(initialBorder.Line);

            BuildForwardBorders(initialBorder, ref restLines, lines.Count);
            BuildBackwardBorders(initialBorder, ref restLines, lines.Count);

            var curBorder = initialBorder;
            var result = new List<VorBorder>();

            while (curBorder.Previous != null)
                curBorder = curBorder.Previous;
            

            result.Add(curBorder);
            while (curBorder.Next != null)
            {
                curBorder = curBorder.Next;
                result.Add(curBorder);
            }

            // If region is rounded
            if (Mth.EqualVectors(result.First().Line.Item1, result.Last().Line.Item2))
                result.RemoveAt(result.Count - 1);

            return result;
        }

        (Vector3, Vector3) FindNextLine(Vector3 targetNode, List<(Vector3, Vector3)> lines)
        {
            foreach (var line in lines)
            {
                if (Mth.EqualVectors(targetNode, line.Item1, MathCalculator.MinNodeDistance) || Mth.EqualVectors(targetNode, line.Item2, MathCalculator.MinNodeDistance))
                {
                    return line;
                }
            }

            return (_notFound, _notFound);
        }

        void BuildForwardBorders(VorBorder curBorder, ref List<(Vector3, Vector3)> restLines, int allLinesCount)
        {
            for (int i = 0; i < allLinesCount; i++)
            {
                var foundNextLine = FindNextLine(curBorder.Line.Item2, restLines);
                if (foundNextLine.Item1 == _notFound)
                    break;

                restLines.Remove(foundNextLine);

                if (Mth.EqualVectors(foundNextLine.Item2, curBorder.Line.Item2, MathCalculator.MinNodeDistance))
                    foundNextLine = (foundNextLine.Item2, foundNextLine.Item1);

                curBorder.Next = new VorBorder
                {
                    Line = foundNextLine,
                    Previous = curBorder
                };

                curBorder = curBorder.Next;
            }
        }

        void BuildBackwardBorders(VorBorder curBorder, ref List<(Vector3, Vector3)> restLines, int allLinesCount)
        {
            for (int i = 0; i < allLinesCount; i++)
            {
                var foundPreviousLine = FindNextLine(curBorder.Line.Item1, restLines);
                if (foundPreviousLine.Item1 == _notFound)
                    break;

                restLines.Remove(foundPreviousLine);
                if (Mth.EqualVectors(foundPreviousLine.Item1, curBorder.Line.Item1, MathCalculator.MinNodeDistance))
                    foundPreviousLine = (foundPreviousLine.Item2, foundPreviousLine.Item1);

                curBorder.Previous = new VorBorder()
                {
                    Line = foundPreviousLine,
                    Next = curBorder
                };
                curBorder = curBorder.Previous;
            }
        }
        
        void AddCornerNodes(List<(Vector3, Vector3)> borders)
        {
            var foundBorders = new Dictionary<AlignedBorder, Vector3>();

            foreach (var vorBorder in borders)
            {
                if (Mth.EqualsFloat(Math.Abs(vorBorder.Item1.x), MapSc.Max.x, Mth.MinNodeDistance))
                {
                    if (vorBorder.Item1.x > 0)
                        foundBorders[AlignedBorder.Right] = vorBorder.Item1;
                    else
                        foundBorders[AlignedBorder.Left] = vorBorder.Item1;
                }

                if (Mth.EqualsFloat(Math.Abs(vorBorder.Item1.y), MapSc.Max.y, Mth.MinNodeDistance))
                {
                    if (vorBorder.Item1.y > 0)
                        foundBorders[AlignedBorder.Top] = vorBorder.Item1;
                    else
                        foundBorders[AlignedBorder.Bottom] = vorBorder.Item1;
                }

                if (Mth.EqualsFloat(Math.Abs(vorBorder.Item2.x), MapSc.Max.x, Mth.MinNodeDistance))
                {
                    if (vorBorder.Item2.x > 0)
                        foundBorders[AlignedBorder.Right] = vorBorder.Item2;
                    else
                        foundBorders[AlignedBorder.Left] = vorBorder.Item2;
                }

                if (Mth.EqualsFloat(Math.Abs(vorBorder.Item2.y), MapSc.Max.y, Mth.MinNodeDistance))
                {
                    if (vorBorder.Item2.y > 0)
                        foundBorders[AlignedBorder.Top] = vorBorder.Item2;
                    else
                        foundBorders[AlignedBorder.Bottom] = vorBorder.Item2;
                }
            }

            if (foundBorders.Count != 2)
                return;
            
            (Vector3, Vector3) newBorder = default;

            if (foundBorders.ContainsKey(AlignedBorder.Left) && foundBorders.ContainsKey(AlignedBorder.Top))
            {
                newBorder = (foundBorders[AlignedBorder.Left], new Vector3(MapSc.Min.x, MapSc.Max.y, foundBorders[AlignedBorder.Left].z));
            }

            if (foundBorders.ContainsKey(AlignedBorder.Top) && foundBorders.ContainsKey(AlignedBorder.Right))
            {
                newBorder = (foundBorders[AlignedBorder.Top], new Vector3(MapSc.Max.x, MapSc.Max.y, foundBorders[AlignedBorder.Top].z));
            }

            if (foundBorders.ContainsKey(AlignedBorder.Right) && foundBorders.ContainsKey(AlignedBorder.Bottom))
            {
                newBorder = (foundBorders[AlignedBorder.Right], new Vector3(MapSc.Max.x, MapSc.Min.y, foundBorders[AlignedBorder.Right].z));
            }

            if (foundBorders.ContainsKey(AlignedBorder.Bottom) && foundBorders.ContainsKey(AlignedBorder.Left))
            {
                newBorder = (foundBorders[AlignedBorder.Bottom], new Vector3(MapSc.Min.x, MapSc.Min.y, foundBorders[AlignedBorder.Bottom].z));
            }
            
            borders.Add(newBorder);
        }

        enum AlignedBorder
        {
            Top,
            Left,
            Right,
            Bottom
        }
    }
}
