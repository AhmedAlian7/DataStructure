using System;
using System.Collections.Generic;

namespace AdgacencyMatrix_Representation
{
    public class Graph<T>
    {
        public enum enType { Direct, Indirect};

        private int[,] _vertecesMatrix;
        private Dictionary<T, int> _vertecesIndex;
        private int _numberOfVerteces;
        private enType _type;

        public Graph(List<T> verteces, enType type)
        {
            _type = type;
            _numberOfVerteces = verteces.Count;
            _vertecesMatrix = new int[_numberOfVerteces, _numberOfVerteces];
            _vertecesIndex = new Dictionary<T, int>();
            for (int i = 0; i < _numberOfVerteces; i++)
            {
                _vertecesIndex.Add(verteces[i], i);
            }
        }

        public void AddEdge(T sourse, T destination, int cost = 1)
        {
            if (!_vertecesIndex.ContainsKey(sourse) || !_vertecesIndex.ContainsKey(destination))
            {
                throw new EntryPointNotFoundException("Invaliad vetreces");
            }

            _vertecesMatrix[_vertecesIndex[sourse], _vertecesIndex[destination]] = cost;
            if (_type == enType.Indirect)
                _vertecesMatrix[_vertecesIndex[destination], _vertecesIndex[sourse]] = cost;
        }
        public void RemoveEdge(T sourse, T destination)
        {
            if (!_vertecesIndex.ContainsKey(sourse) || !_vertecesIndex.ContainsKey(destination))
            {
                throw new EntryPointNotFoundException("Invaliad vetreces");
            }

            _vertecesMatrix[_vertecesIndex[sourse], _vertecesIndex[destination]] = 0;
            if (_type == enType.Indirect)
                _vertecesMatrix[_vertecesIndex[destination], _vertecesIndex[sourse]] = 0;
        }
        public void Display(string Message = "Matrix Adjacency representation:")
        {
            Console.WriteLine($"\n{Message}\n");

            Console.Write("  ");
            foreach (var vetrex in _vertecesIndex.Keys)
            {
                Console.Write(vetrex + " ");
            }
            Console.WriteLine();

            foreach (var item in _vertecesIndex)
            {
                Console.Write(item.Key + " ");
                for (int i = 0; i < _numberOfVerteces; i++)
                {
                    Console.Write(_vertecesMatrix[item.Value, i] + " ");
                }
                Console.WriteLine();
            }
        }

        public bool IsEdgeBetween(T sourse, T destination)
        {
            if (!_vertecesIndex.ContainsKey(sourse) || !_vertecesIndex.ContainsKey(destination))
            {
                throw new EntryPointNotFoundException("Invaliad vetreces");
            }

            return (_vertecesMatrix[_vertecesIndex[sourse], _vertecesIndex[destination]] != 0);
        }
        public int Indegrees(T vetrex) // coloumns
        {
            int count = 0;
            for (int i = 0; i < _numberOfVerteces; i++)
            {
                if (_vertecesMatrix[i, _vertecesIndex[vetrex]] != 0)
                    count++;
            }
            return count;
        }
        public int Outdegrees(T vetrex) // coloumns
        {
            int count = 0;
            for (int i = 0; i < _numberOfVerteces; i++)
            {
                if (_vertecesMatrix[_vertecesIndex[vetrex], i] != 0)
                    count++;
            }
            return count;
        }
    }
}
