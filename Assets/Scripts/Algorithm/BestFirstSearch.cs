/*
 * BFS，最优路径选择算法
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BestFirstSearch: PathFindingBase<int>
{
    private int[] h;
    private bool[] visited;
    private bool[] searched;
    private int searchedCount;

    private int endx = 0;
    private int endy = 0;

    public BestFirstSearch(Graph g,int start,int end)
    {
        InitData(g, start, end);
    }

    public override void InitData(Graph graph, int start, int end)
    {
        g = graph;
        int size = g.vertexs.Length;
        path = new int[size];
        pre = new int[size];
        queue = new PriorityQueue<int>();
        searchedVetexesShow = new List<int>();
        this.start = start;
        this.end = end;
        this.endy = end / g.col;
        this.endx = end % g.col;

        h = new int[size];
        visited = new bool[size];
        searched = new bool[size];
        searchedCount = 0;

        for (int i = 0; i < size; i++)
        {
            visited[i] = false;
            path[i] = int.MinValue;
            pre[i] = int.MinValue;
            searched[i] = false;
            h[i] = int.MaxValue;
        }
    }

    public override bool Execute()
    {
        if (start == end)
        {
            searchedCount = 1;
            return true;
        }

        h[start] = Heuristic(start);
        pre[start] = -1;

        queue.Enqueue(start, h[start]);
        visited[start] = true; //已经访问获得了h值，因此该数组也表示已经在队列里了

        while (queue.Size() != 0)
        {
            QueueItem<int> v = queue.Dequeue();
            searched[v.key] = true; //已经在获得路径的集合中
            searchedCount++;
            searchedVetexesShow.Add(v.key);
            if (v.key == end) break;
            //计算相邻顶点的h值
            UpdateH(v.key);
        }
        //更新正向路径
        return GetPathOrder();
    }


    //使用汉密尔顿启发式距离函数
    private int Heuristic(int vertexIndex)
    {
        int x = vertexIndex % g.col;
        int y = vertexIndex / g.col;
        return Mathf.Abs(x - endx) + Mathf.Abs(y - endy);
    }

    private void UpdateH(int vIndex)
    {
        ENode e = g.vertexs[vIndex].firstEdge;
        if (e == null) return;
        while (e != null)
        {
            if (searched[e.vertexIndex] == false&&visited[e.vertexIndex]==false)
            {
                h[e.vertexIndex] = Heuristic(e.vertexIndex);
                pre[e.vertexIndex] = vIndex;
                queue.Enqueue(e.vertexIndex, h[e.vertexIndex]);
                visited[e.vertexIndex] = true;
            }
            e = e.nextENode;
        }
    }

}
