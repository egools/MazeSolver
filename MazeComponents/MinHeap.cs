using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeComponents
{
    public class MinHeap<T> where T : IComparable
    {
        private T[] _arr;
        public int Size => _arr.Length;
        public bool IsEmpty => _arr.Length == 0;
        public double Height => Math.Floor(Math.Log(Size, 2));

        public MinHeap()
        {
            _arr = new T[0];
        }

        public MinHeap(T[] arr)
        {
            _arr = new T[0];
            Heapify(arr);
        }

        public T ExtractMin()
        {
            T element = _arr[0];
            DeleteMin();
            return element;
        }

        public void DeleteMin()
        {
            _arr[0] = _arr[Size - 1];
            Array.Resize(ref _arr, Size - 1);
            SiftDown(0);
        }

        public void Add(T element)
        {
            Array.Resize(ref _arr, Size + 1);
            _arr[Size - 1] = element;
            SiftUp(Size - 1);
        }

        public void Heapify(T[] arr)
        {
            _arr = new T[0];
            Meld(arr);
        }

        public void Meld(T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                Add(arr[i]);
            }
        }

        private void SiftDown(int index)
        {
            var pow = 2 * index + 1;
            if (pow >= Size) return;

            var tmp = _arr[index];
            var leftChild = _arr[pow];
            int indexToSwap = -1;

            if (leftChild.CompareTo(tmp) < 0)
                indexToSwap = pow;
            if (pow + 1 < Size && _arr[pow + 1].CompareTo(leftChild) < 0)
                indexToSwap = pow + 1;

            if (indexToSwap > 0)
            {
                _arr[index] = _arr[indexToSwap];
                _arr[indexToSwap] = tmp;
                SiftDown(indexToSwap);
            }
        }

        private void SiftUp(int index)
        {
            var parentIndex = (int)Math.Floor((index - 1) / 2f);
            if (parentIndex >= 0)
            {
                if (_arr[parentIndex].CompareTo(_arr[index]) >= 0)
                {
                    var tmp = _arr[parentIndex];
                    _arr[parentIndex] = _arr[index];
                    _arr[index] = tmp;
                    SiftUp(parentIndex);
                }
            }
        }

        public void Clear()
        {
            _arr = new T[0];
        }

        public bool Contains(T element)
        {
            return _arr.Contains(element);
        }
    }
}
