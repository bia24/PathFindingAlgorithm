/* 
 * 使用最小堆实现优先队列
 */
using System;
using System.Collections.Generic;

public class PriorityQueue<T>{

    private static readonly int maxQueueSize = 1000;

    private Dictionary<T, int> pos = null;

    private QueueItem<T>[] q=null;

    private int size=0;

    public PriorityQueue()
    {
        q = new QueueItem<T>[maxQueueSize];
        pos = new Dictionary<T, int>();
        size = 0;
    }

    public void Clear()
    {
        Array.Clear(q, 0, 1000);
        size = 0;
    }

    public int Size()
    {
        return size;
    }

    public void Enqueue(T key,int priority)
    {
        if (size == maxQueueSize)
        {
            throw new Exception("队列已满");
        }

        QueueItem<T> elem = new QueueItem<T>(key, priority);
        q[size++] = elem;
        pos.Add(key, size - 1);
        ShiftUp(size-1);
     
    }

    public QueueItem<T> Dequeue()
    {
        if (size <= 0) throw new Exception("队列为空");
        Swap(0, size - 1);
        size--;
        ShiftDown(0);
        pos.Remove(q[size].key);
        return q[size];
    }

    public void Decrease(T key,int newPriority)
    {
        int p;
        if (!pos.TryGetValue(key, out p)) throw new Exception("找不到队列中的位置");
        q[p].priority = newPriority;
        ShiftUp(p);
    }


    public QueueItem<T> Top()
    {
        if (size > 0) return q[0];
        else   throw new Exception("队列为空");     
    }

    private void ShiftUp(int pos)
    {
        if (pos >= size || pos <= 0) return;
        int parent = (pos - 1) / 2;
        while (parent>=0&&pos>0&&q[parent].priority>q[pos].priority)
        {
            Swap(parent, pos);
            pos = parent;
            parent = (pos - 1) / 2;
        }
    }

    private void ShiftDown(int pos)
    {
        int l = pos * 2 + 1;
        int r = pos * 2 + 2;
        if (l >= size) return;
        int min = pos;
        if (q[min].priority > q[l].priority)
            min = l;
        if (r < size && q[min].priority > q[r].priority)
            min = r;
        if (min != pos)
        {
            Swap(min, pos);
            ShiftDown(min);
        }
    }


    private void Swap(int x, int y)
    {
        pos[q[x].key] = y;
        pos[q[y].key] = x;
        QueueItem<T> temp = q[x];
        q[x] = q[y];
        q[y] = temp;
    }

}

public struct QueueItem<T>
{
    public T key;
    public int priority;

    public QueueItem(T key, int priority)
    {
        this.key = key;
        this.priority = priority;
    }
}