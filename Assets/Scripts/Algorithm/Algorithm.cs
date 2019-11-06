/* 
 * 路径搜索算法的封装类，供客户端调用
 */

using System.Collections.Generic;
using System.Diagnostics;

public class Algorithm {

    private PathFindingBase<int> algorithm = null;
    private long timer = 0;
    public Method method;

    public Algorithm(Method method,int[,] map,int start,int end)
    {
        SetAlgorithm(method, map, start, end);
    }

    public bool Execute()
    {
        Stopwatch sw = new Stopwatch();
        sw.Start();
        bool res=algorithm.Execute();
        sw.Stop();
        timer = sw.ElapsedTicks;
        return res;
    }

    public int[] ShowPath() { return algorithm.ShowPath(); }

    public List<int> GetCountOfVertexsSearched() { return algorithm.GetCountOfVertexsSearched(); }

    public void SetAlgorithm(Method method, int[,] map, int start, int end)
    {
        this.method = method;
        switch (method)
        {
            case Method.Dijkstra:
                algorithm = new Dijkstra(new Graph(map), start, end);
                break;
            case Method.BFS:
                algorithm = new BestFirstSearch(new Graph(map),start,end);
                break;
            case Method.AStar:
                algorithm = new AStarPathFinding(new Graph(map), start, end);
                break;
            default :
                algorithm = new Dijkstra(new Graph(map), start, end);
                break;
        }

        //算法执行计时器
        timer = 0;
    }

    public long GetExcuteTime() { return timer; }

    public int GetPathLength() { return algorithm.GetPathLength(); }
}

public enum Method
{
    Dijkstra,
    BFS,
    AStar
}
