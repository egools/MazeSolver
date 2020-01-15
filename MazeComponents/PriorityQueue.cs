using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeComponents
{
    public class PriorityQueue<T> where T : IComparable
    {
        private readonly MinHeap<T> _minHeap;
        public int Count => _minHeap.Size;
        public bool IsEmpty => _minHeap.IsEmpty;

        public PriorityQueue()
        {
            _minHeap = new MinHeap<T>();
        }

        public PriorityQueue(T[] arr)
        {
            _minHeap = new MinHeap<T>(arr);
        }

        public void Enqueue(T element)
        {
            _minHeap.Add(element);
        }

        public T Dequeue()
        {
            if (!_minHeap.IsEmpty)
                return _minHeap.ExtractMin();
            else
                throw new InvalidOperationException("PriorityQueue is empty");
        }

        public T Peek()
        {
            if (!_minHeap.IsEmpty)
            {
                var element = _minHeap.ExtractMin();
                _minHeap.Add(element);
                return element;
            }
            else
            {
                throw new InvalidOperationException("PriorityQueue is empty");
            }
        }

        public bool Contains(T element)
        {
            return _minHeap.Contains(element);
        }

        public void Clear()
        {
            _minHeap.Clear();
        }
    }
}
