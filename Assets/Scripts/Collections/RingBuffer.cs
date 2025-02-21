using System;
using System.Collections.Generic;
using System.Linq;

public class RingBuffer<T>
{
    T[] data;
    public IReadOnlyCollection<T> Data => data;

    public T NextValue => data[tail];
    public int Count { get; private set; }
    int head;
    int tail;
    public int Capacity {get; private set;}


    public RingBuffer(int capacity)
    {
        this.data = new T[capacity];
        this.Capacity = capacity;
        this.head = 0;
        this.tail = 0;
        this.Count = 0;
        for (int i = 0; i < capacity; i++)
            data[i] = default;
    }

    public T Pop()
    {
        if (Count == 0) throw new InvalidOperationException("Ringbuffer contains no elements!");
        var index = data[tail];
        tail++;
        if (tail >= Capacity) tail -= Capacity;
        Count--;
        return index;
    }

    public void Push(T value, bool overwrite = false)
    {
        if (Count >= Capacity)
            if (!overwrite)
                throw new Exception("Ringbuffer full!");
            else if (Capacity > 0) Pop();
            else return;
        data[head] = value;
        head++;
        if (head >= Capacity) head -= Capacity;
        Count++;
    }

    public void Reset()
    {
        for (int i = 0; i < Capacity; i++) data[i] = default;
        head = 0;
        tail = 0;
        Count = 0;
    }

    public bool Contains(T value) => data.Contains(value);

    public void Resize(int capacity)
    {
        var newData = new T[capacity];
        var oldData = new T[Count];
        for (int i = 0; i < Count; i++)
            oldData[i] = Pop();

        this.data = newData;
        this.Capacity = capacity;
        this.head = 0;
        this.tail = 0;
        this.Count = 0;
        for (int i = 0; i < oldData.Length; i++)
            Push(oldData[i], true);
    }
}