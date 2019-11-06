/*
 * A*寻路算法
 */
using System.Collections.Generic;
using UnityEngine;

public class AStarPathFinding : PathFindingBase<int>
{
    private int[] dis;
    private int[] h;
    private bool[] visited;
    private bool[] searched;
    private int searchedCount;

    private int endx = 0;
    private int endy = 0;

    public AStarPathFinding(Graph g,int start,int end)
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

        dis = new int[size];
        h = new int[size];
        visited = new bool[size];
        searched = new bool[size];
        searchedCount = 0;


        for (int i = 0; i < size; i++)
        {
            dis[i] = int.MaxValue;
            h[i] = int.MaxValue;
            visited[i] = false;
            path[i] = int.MinValue;
            pre[i] = int.MinValue;
            searched[i] = false;
        }
    }

    public override bool Execute()
    {
        if (start == end)
        {
            searchedCount = 1;
            return true;
        }

        dis[start] = 0;
        h[start] = Heuristic(start);
        pre[start] = -1;

        queue.Enqueue(start, dis[start]);
        visited[start] = true;

        while (queue.Size() != 0)
        {
            QueueItem<int> v = queue.Dequeue();
            searched[v.key] = true;
            searchedCount++;
            searchedVetexesShow.Add(v.key);
            if (v.key == end) break;
            UpdatePriority(v.key);
        }
        return GetPathOrder();
    }

    //使用汉密尔顿启发式距离函数，并且增加了系数权重
    private int Heuristic(int vertexIndex)
    {
        int x = vertexIndex % g.col;
        int y = vertexIndex / g.col;
        return (Mathf.Abs(x - endx) + Mathf.Abs(y - endy));
    }

    private void UpdatePriority(int vIndex)
    {
        ENode e = g.vertexs[vIndex].firstEdge;
        if (e == null) return;
        while (e!=null)
        {
            if (searched[e.vertexIndex] == false)
            {
                if (visited[e.vertexIndex] == false)
                {
                    //第一次访问到的顶点，直接计算各种值，直接入队列
                    dis[e.vertexIndex] = dis[vIndex] + e.wight;
                    pre[e.vertexIndex] = vIndex;
                    h[e.vertexIndex] = Heuristic(e.vertexIndex);
                    int priority = dis[e.vertexIndex] + h[e.vertexIndex];
                    queue.Enqueue(e.vertexIndex, priority);
                    visited[e.vertexIndex] = true;
                }
                else
                { //顶点已经被访问过了，已经在队列里了
                    bool relaxed = false;
                    int newDis = dis[vIndex] + e.wight;
                    if (dis[e.vertexIndex] > newDis)
                    {
                        relaxed = true;
                        dis[e.vertexIndex] = newDis;
                    }
                    if (relaxed)
                    {//进行过松弛操作了，更新队列里的优先级
                        int priority = dis[e.vertexIndex] + h[e.vertexIndex];
                        queue.Decrease(e.vertexIndex, priority);
                    }
                }
            }
            e = e.nextENode;
        }
    }
}
