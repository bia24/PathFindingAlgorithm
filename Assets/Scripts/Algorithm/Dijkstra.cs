/*
 * Dijkstra最短路径搜索算法
 */
using System;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Dijkstra : PathFindingBase<int>
{
    private int[] dis;
    private bool[] visited;
    private bool[] searched;
    private int searchedCount;


    public Dijkstra(Graph g,int start,int end)
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

        dis = new int[size];
        visited = new bool[size];
        searched = new bool[size];
        searchedCount = 0;


        for (int i = 0; i < size; i++)
        {
            dis[i] = int.MaxValue;
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
        pre[start] = -1;

        queue.Enqueue(start, dis[start]);
        visited[start] = true;

        while (queue.Size() != 0)
        {
            QueueItem<int> v = queue.Dequeue();
            searched[v.key] = true;   //已经在最短路径集合中的节点
            searchedCount++;
            searchedVetexesShow.Add(v.key);    //将找到的节点入栈，用以展示可视化用
            if (v.key == end) break; //找到了指定的点
            //遍历该顶点所连接的所有边，进行松弛
            UpdateDis(v.key);
        }
        //路径计算完成，更新正向路径
        return GetPathOrder();
    }

    private void UpdateDis(int vIndex)
    {
        ENode e = g.vertexs[vIndex].firstEdge;
        if (e== null) return;
        while (e != null)
        {
            if (searched[e.vertexIndex] == false)
            {
                if (visited[e.vertexIndex] == false)
                {
                    //第一次访问到，还没有入队列
                    dis[e.vertexIndex] = dis[vIndex] + e.wight;
                    pre[e.vertexIndex] = vIndex;
                    queue.Enqueue(e.vertexIndex, dis[e.vertexIndex]);
                    visited[e.vertexIndex] = true;
                }
                else
                { //顶点已经被访问过了，已经在队列里了
                    int newDis = dis[vIndex] + e.wight;
                    if (dis[e.vertexIndex] > newDis)
                    {
                        dis[e.vertexIndex] = newDis;
                        pre[e.vertexIndex] = vIndex;
                        queue.Decrease(e.vertexIndex, dis[e.vertexIndex]);
                    }  
                }
            }
            e = e.nextENode;
        }
    }
    
   

}
