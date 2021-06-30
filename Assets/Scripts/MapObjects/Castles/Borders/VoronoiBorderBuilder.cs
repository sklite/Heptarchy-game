using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.MapObjects.Voronoi;
using UnityEngine;
using Mth = Assets.Scripts.Helpers.MathCalculator;

namespace Assets.Scripts.MapObjects.Castles.Borders
{
    class VoronoiBorderBuilder
    {
        private readonly Vector3 _notFound = new Vector3(-99999, -99999, -99999);
        private const float MinNodeDistance = 0.8f;

        public List<VorBorder> BuildClosedShape(List<(Vector3, Vector3)> borders)
        {
            var lines = borders.Where(item => !Mth.EqualVectors(item.Item1, item.Item2, MinNodeDistance)).ToList();


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
                if (Mth.EqualVectors(targetNode, line.Item1, MinNodeDistance) || Mth.EqualVectors(targetNode, line.Item2, MinNodeDistance))
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

                if (Mth.EqualVectors(foundNextLine.Item2, curBorder.Line.Item2, MinNodeDistance))
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
                if (Mth.EqualVectors(foundPreviousLine.Item1, curBorder.Line.Item1, MinNodeDistance))
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
            float? x = null, y = null;
            (Vector3, Vector3) lastNode = default;

            foreach (var vorBorder in borders)
            {
                if (Mth.EqualsFloat(Math.Abs(vorBorder.Item1.x), MapSc.Max.x, MinNodeDistance))
                {
                    x = vorBorder.Item1.x;
                    lastNode = vorBorder;
                    continue;
                }

                if (Mth.EqualsFloat(Math.Abs(vorBorder.Item1.y), MapSc.Max.y, MinNodeDistance))
                {
                    y = vorBorder.Item1.y;
                    lastNode = vorBorder;
                    continue;
                }

                if (Mth.EqualsFloat(Math.Abs(vorBorder.Item2.x), MapSc.Max.x, MinNodeDistance))
                {
                    x = vorBorder.Item2.x;
                    lastNode = vorBorder;
                    continue;
                }

                if (Mth.EqualsFloat(Math.Abs(vorBorder.Item2.y), MapSc.Max.y, MinNodeDistance))
                {
                    y = vorBorder.Item2.y;
                    lastNode = vorBorder;
                    continue;
                }
            }

            if (x == null || y == null)
                return;

            (Vector3, Vector3) newBorder = default;

            if (Mth.EqualsFloat(lastNode.Item1.x, x.Value, MinNodeDistance) || Mth.EqualsFloat(lastNode.Item1.y, y.Value, MinNodeDistance))
            {
                newBorder = (new Vector3(x.Value, y.Value, lastNode.Item1.z), lastNode.Item1);
            }

            if (Mth.EqualsFloat(lastNode.Item2.x, x.Value, MinNodeDistance) || Mth.EqualsFloat(lastNode.Item2.y, y.Value, MinNodeDistance))
            {
                newBorder = (new Vector3(x.Value, y.Value, lastNode.Item2.z), lastNode.Item2);
            }

            borders.Add(newBorder);
        }
    }
}
