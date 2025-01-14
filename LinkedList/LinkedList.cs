﻿using System;
using System.Collections;
using System.Collections.Generic;

namespace LinkedList
{
    class LinkedList<Type> : IList<Type>
    {
        class Node
        {
            public Type Value { get; set; }

            public Node Next { get; set; }
        }

        private Node _first;

        private Node _last;

        private int _count;

        private bool _isReadOnly;

        private bool _isFixesSize;

        public Type First => _first == null ? default : _first.Value;

        public Type Last => _last == null ? default : _last.Value;

        public bool IsReadOnly => _isReadOnly;

        public bool IsFixedSize => _isFixesSize;

        public int Count => _count;

        public bool IsEmpty => _count == 0;

        public LinkedList(bool isReadOnly = false)
        {
            _isReadOnly = isReadOnly;
        }

        public LinkedList(int size, bool isReadOnly = false) : this(isReadOnly: isReadOnly)
        {
            _isFixesSize = true;
            if (size <= 0)
            {
                return;
            }

            _first = new Node();
            var element = _first;
            for (int index = 0; index < size; index++)
            {
                element.Next = new Node();
                element = element.Next;
                _count++;
            }

            _last = element;
        }

        public LinkedList(List<Type> list, bool isReadOnly = false) : this(isReadOnly: isReadOnly)
        {
            list.ForEach(Add);
        }

        public void Add(Type value)
        {
            if (_isFixesSize || _isReadOnly)
            {
                return;
            }

            var newElement = new Node() { Value = value };
            _last = newElement;

            if (IsEmpty)
            {
                _first = newElement;
                _count++;
                return;
            }

            GetNode(_count - 1).Next = newElement;
            _count++;
            return;
        }

        public void Insert(int index, Type value)
        {
            if (_isFixesSize || _isReadOnly)
            {
                return;
            }

            if (index == 0)
            {
                _first = new Node() { Value = value, Next = _first };
            }
            else
            {
                var node = GetNode(index - 1);
                node.Next = new Node() { Value = value, Next = node.Next };
            }

            _count++;
        }

        public void RemoveAt(int index)
        {
            if (_isFixesSize || _isReadOnly)
            {
                return;
            }

            if (index == 0)
            {
                _first = _first.Next;
            }
            else
            {
                var node = GetNode(index - 1);
                var nextNode = node.Next != null ? node.Next.Next : null;

                if (index == _count - 1)
                {
                    _last = node;
                }

                node.Next = nextNode;
            }

            _count--;
        }

        public bool Remove(Type value)
        {
            int index = IndexOf(value);

            if (index == -1)
            {
                return false;
            }

            RemoveAt(index);
            return true;
        }

        public Type this[int index]
        {
            get
            {
                return GetNode(index).Value;
            }

            set
            {
                if (_isReadOnly)
                {
                    return;
                }

                var element = GetNode(index);
                element.Value = value;
            }
        }

        public int IndexOf(Type value)
        {
            if (IsEmpty)
            {
                return -1;
            }

            var element = _first;
            var index = 0;
            while (element != null)
            {
                if (element.Value.Equals(value))
                {
                    return index;
                }

                element = element.Next;
                index++;
            }

            return -1;
        }

        public bool Contains(Type value)
        {
            return IndexOf(value) != -1;
        }

        public void Clear()
        {
            _first = _last = null;
            _count = 0;
            _isFixesSize = false;
            _isReadOnly = false;
        }

        public List<Type> ToArray()
        {
            var array = new List<Type>();
            var element = _first;

            while (element != null)
            {
                array.Add(element.Value);
                element = element.Next;
            }

            return array;

        }

        public override string ToString()
        {
            string arrayAsString = string.Join(", ", ToArray());
            return $"[{arrayAsString}]";
        }

        public void ForEach(Action<Type> callback)
        {
            if (IsEmpty)
            {
                return;
            }

            var element = _first;
            while (element != null)
            {
                callback(element.Value);
                element = element.Next;
            }
        }

        public void CopyTo(Type[] list, int start)
        {
            for (int index = 0; index < _count; index++)
            {
                list[start+index] = this[index];
            }
        }

        private Node GetNode(int index)
        {
            if (IsEmpty || index >= _count || index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            var element = _first;
            for (int i = 0; i < index; i++)
            {
                element = element.Next;
            }

            return element;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this).GetEnumerator();
        }

        IEnumerator<Type> IEnumerable<Type>.GetEnumerator()
        {
            var element = _first;
            while (element != null)
            {
                yield return element.Value;
                element = element.Next;
            }
        }
    }
}
