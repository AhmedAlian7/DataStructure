using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IList_Implementation
{
    internal class MyCollection<T> : IList<T>
    {
        private T[] _arr;
        private int _count =0;

        public MyCollection()
        {
            _arr = new T[2];
        }

        public T this[int index] { get => _arr[index]; set => _arr[index] = value; }

        public int Count => _count;

        public bool IsReadOnly => false;

        public void Add(T item)
        {

            if (_count == _arr.Length)
            {
                // Double the array size when capacity is reached
                T[] newArr = new T[_arr.Length * 2];
                Array.Copy(_arr, newArr, _count);
                _arr = newArr;
            }
            _arr[_count] = item;
            _count++;   

        }

        public void Clear()
        {
            _count = 0;
            _arr = new T[4];
        }

        public bool Contains(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(_arr[i], item))
                    return true;
            }
            return false;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (arrayIndex < 0 || arrayIndex > array.Length)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));
            if (array.Length - arrayIndex < _count)
                throw new ArgumentException("The destination array is not large enough.");

            Array.Copy(_arr, 0, array, arrayIndex, _count);
        }   

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < _count; i++)
            {
                yield return _arr[i];
            }
        }

        public int IndexOf(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(_arr[i], item))
                    return i;
            }
            return -1;
        }

        public void Insert(int index, T item)
        {
            if (index > _count)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (_count == _arr.Length)
            {
                T[] newArr = new T[_arr.Length * 2];
                Array.Copy(_arr, newArr, _count);
                _arr = newArr;
            }

            if (index < _count)
            {
                Array.Copy(_arr, index, _arr, index + 1, _count - index);
            }

            _arr[index] = item;
            _count++;
        }

        public bool Remove(T item)
        {
            for (int i = 0; i < _count; i++)
            {
                if (EqualityComparer<T>.Default.Equals(_arr[i], item))
                {
                    for (int j = i; j < _count - 1; j++)
                    {
                        _arr[j] = _arr[j + 1];
                    }
                    _arr[_count - 1] = default;
                    _count--;
                    return true;
                }
            }
            return false;
        }

        public void RemoveAt(int index)
        {
            if (index < 0 || index > _arr.Length)
                throw new ArgumentOutOfRangeException(nameof(index));

            T item = _arr[index];
            Remove(item);   
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
