/*
 * 图结构定义，使用链接表做基本数据结构
 */
 public struct VNode
{
    public ENode firstEdge;
    public int vertexIndex;

    public VNode(ENode edge,int vertexIndex)
    {
        this.firstEdge = edge;
        this.vertexIndex = vertexIndex;
    }
}

public class ENode
{
    public int wight;
    public ENode nextENode;
    public int vertexIndex;

    public ENode(int vertexIndex,ENode nextENode,int weight=1)
    {
        this.vertexIndex = vertexIndex;
        this.nextENode = nextENode;
        this.wight = weight;
    }
}


public class Graph  {

    public VNode[] vertexs;
    public int row = 0;
    public int col = 0;
	
    public Graph(int[,]map)
    {
        InitGraph(map);
    }

    public void InitGraph(int [,]map)
    {
        row = map.GetLength(0);
        col = map.GetLength(1);
        vertexs = new VNode[row * col];

        #region 顶点节点和边节点依据地图信息初始化
        for (int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                int index = i * col + j;
                vertexs[index] = new VNode(null, index);

                bool hasEdge = false;
                ENode current = null;
                //顶点上方
                if (i - 1 >= 0&&map[i-1,j]!=0)
                {
                    vertexs[index].firstEdge = new ENode(index - col, null);
                    hasEdge = true;
                    current = vertexs[index].firstEdge;
                }

                //顶点下方
                if (i + 1 <= row - 1 && map[i + 1,j] != 0)
                {
                    if (hasEdge)
                    {
                        current.nextENode = new ENode(index + col, null);
                        current = current.nextENode;
                    }
                    else
                    {
                        vertexs[index].firstEdge = new ENode(index + col, null);
                        hasEdge = true;
                        current = vertexs[index].firstEdge;
                    }
                }

                //顶点左边
                if (j - 1 >= 0 && map[i,j - 1] != 0)
                {
                    if (hasEdge)
                    {
                        current.nextENode = new ENode(index - 1, null);
                        current = current.nextENode;
                    }
                    else
                    {
                        vertexs[index].firstEdge = new ENode(index -1, null);
                        hasEdge = true;
                        current = vertexs[index].firstEdge;
                    }
                }

                //顶点右边
                if (j + 1 <= col - 1 && map[i,j +1] != 0)
                {
                    if (hasEdge)
                    {
                        current.nextENode = new ENode(index + 1, null);
                    }
                    else
                    {
                        vertexs[index].firstEdge = new ENode(index +1, null);
                    }
                }

            }
        }
    }
    #endregion

}
