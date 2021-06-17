using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using UnityEngine;

namespace Assets.Scripts.MapObjects.Voronoi
{
    class VoronoiBorderBuilder
    {
        private readonly Vector3 _notFound = new Vector3(-99999, -99999, -99999);

        public List<VorBorder> BuildClosedShape(List<(Vector3, Vector3)> borders)
        {
            //  points.Distinct()
            var lines = borders.Where(item => item.Item1 != item.Item2).ToList();

            var initialBorder = new VorBorder
            {
                Line = lines.First()
            };

            var restLines = new List<(Vector3, Vector3)>(lines);
            restLines.Remove(initialBorder.Line);


            var curBorder = initialBorder;
            for (int i = 0; i < lines.Count; i++)
            {

                var foundNextLine = FindNextLine(curBorder.Line.Item2, restLines);
                if (foundNextLine.Item1 == _notFound)
                    break;

                restLines.Remove(foundNextLine);

                if (foundNextLine.Item2 == curBorder.Line.Item2)
                    foundNextLine = (foundNextLine.Item2, foundNextLine.Item1);

                curBorder.Next = new VorBorder
                {
                    Line = foundNextLine,
                    Previous = curBorder
                };

                curBorder = curBorder.Next;
            }

            curBorder = initialBorder;
            //restLines = new List<(Vector3, Vector3)>(lines);
            //restLines.Remove(initialBorder.Line);

            for (int i = 0; i < lines.Count; i++)
            {
                var foundPreviousLine = FindNextLine(curBorder.Line.Item1, restLines);
                if (foundPreviousLine.Item1 == _notFound)
                    break;

                restLines.Remove(foundPreviousLine);
                if (foundPreviousLine.Item1 == curBorder.Line.Item1)
                    foundPreviousLine = (foundPreviousLine.Item2, foundPreviousLine.Item1);

                curBorder.Previous = new VorBorder()
                {
                    Line = foundPreviousLine,
                    Next = curBorder
                };
                curBorder = curBorder.Previous;
            }

            var result = new List<VorBorder>();
            while (curBorder.Previous != null)
            {
                curBorder = curBorder.Previous;
            }

            result.Add(curBorder);
            while (curBorder.Next != null)
            {
                curBorder = curBorder.Next;
                result.Add(curBorder);
            }

            if (result.First().Line.Item1 == result.Last().Line.Item2)
                result.RemoveAt(result.Count - 1);

            return result;
        }




        (Vector3, Vector3) FindNextLine(Vector3 targetNode, List<(Vector3, Vector3)> lines)
        {
            foreach (var line in lines)
            {
                if (targetNode == line.Item1 || targetNode == line.Item2)
                {
                    return line;
                }
            }

            return (_notFound, _notFound);

        }

        //bool IsCLosedShape(List<(Vector3, Vector3)> lines)
        //{
        //    var connectedLines = new LinkedList<Vector3>();

        //    connectedLines.AddFirst(lines.First().Item1);
        //    connectedLines.AddLast(lines.First().Item2);

        //    var activeLines = lines.Select(line => (Active: true, line.Item1, line.Item2));

        //    for (int i = 0; i < lines.Count; i++)
        //    {
        //        var lastConnectedPoint = connectedLines.Last;

        //        foreach (var activeLine in activeLines.Where(line => line.Active))
        //        {
                    
        //        }

        //    }

        //    foreach (var line in lines)
        //    {
                
                



        //    }
        //}

       // (Vector3, Vector)
    }
}
