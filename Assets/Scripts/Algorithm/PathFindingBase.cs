/*
 * 路径搜索算法的基类
 */
using System.Collections.Generic;

public abstract  class PathFindingBase<T> {

    protected int[] path=null;
    protected int[] pre = null;
    protected PriorityQueue<T> queue = new PriorityQueue<T>();
    protected List<int> searchedVetexesShow = null;
    protected Graph g = null;
    protected int start = 0;
    protected int end = 0;


    public abstract void InitData(Graph g,int start, int end);

    public abstract bool Execute();

    public virtual int[] ShowPath() { return path; }

    public virtual List<int> GetCountOfVertexsSearched() { return searchedVetexesShow; }
   
    protected bool GetPathOrder()
    {
        Stack<int> order = new Stack<int>();
        order.Push(end);
        int i = pre[end];
        if (i == int.MinValue)//end终点没有前驱，该终点无路径
            return false;
        while (i != -1)
        {
            order.Push(i);
            i = pre[i];
        }
        i = 0;
        while (order.Count > 0)
        {
            path[i] = order.Pop();
            i++;
        }
        return true;
    }

    public virtual int GetPathLength()
    {
        int count = 0;
        while (path[count] != int.MinValue)
            count++;
        return count;
    }

}
