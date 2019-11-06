/* 
 * 客户端类
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour {

    private static readonly float X_OFFSET = 1F / 16;
    private static readonly float ASPECT = 1.5f;
    private static readonly int BOARD_PIXEL = 1;
    private static readonly int SHOW_NUM_RATIO = 5;
    private static readonly int SHOW_TIME_GAP = 2;
    private static readonly int SHOW_PATH_NUM_RATIO = 10;

    private int[,] map = null;
    private int row = 0;
    private int col = 0;
    public int obstacle = 1200;
    private int ox = 0;
    private int oy = 0;
    private int edge = 0;
    private Square[,] squares = null;

    private Algorithm algorithm = null;

    #region 终点起点设置开关
    private bool CanSetStartAndEnd = false;
    private int startEndCount = 0;
    private int lastStart = int.MinValue;
    private int lastEnd = int.MinValue;
    #endregion

    private string errorMsg = "";

    private void Start()
    {
        //生成随机地图
        map = GetRandomMap(obstacle);
        //为地图重置数据
        ResetStateForMap();
        //重置地图的起点和终点
        ResetStartAndEnd();
    }
    
    private void Update()
    {
        if (CanSetStartAndEnd)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector2 pos = new Vector2(Input.mousePosition.x,Screen.height-Input.mousePosition.y);
                if (pos.x - ox > 0 && (pos.x < squares[0, col - 1].pos.x + edge + BOARD_PIXEL))
                {
                    if (pos.y > oy && (pos.y < squares[row - 1, 0].pos.y + edge + BOARD_PIXEL))
                    {
                        int j = (int)(pos.x - ox ) / (edge + BOARD_PIXEL);
                        int i=(int)(pos.y- oy) / (edge + BOARD_PIXEL);
                        if (squares[i, j].color != Color.grey)
                        {
                            squares[i, j].color = Color.red;
                            startEndCount++;
                            if (startEndCount == 1)
                            {
                                lastStart = i * col + j;
                            }
                            if (startEndCount == 2)
                            {
                                CanSetStartAndEnd = false;
                                lastEnd = i * col + j;
                                startEndCount = 0;
                            }
                        }
                    }
                }
            }
        }
    }

    private void OnGUI()
    {
        //更换随机地图
        if(GUI.Button(new Rect(0, 0, 100, 80), "New Map"))
        {
            StopAllCoroutines();
            map = GetRandomMap(obstacle);
            ResetStateForMap();
            ResetStartAndEnd();
        }
        //设置起点和终点
        if (GUI.Button(new Rect(130, 0, 100, 80), "Set Start&End"))
        {
            StopAllCoroutines();
            ResetStateForMap();
            ResetStartAndEnd();
            CanSetStartAndEnd = true;
            startEndCount = 0;
        }
        //选择使用的算法
        if (GUI.Button(new Rect(300, 0, 100, 40), "Dijkstra")&&lastStart!=int.MinValue&&lastEnd!=int.MinValue)
        {
            StopAllCoroutines();
            ResetStateForMap();
            algorithm = new Algorithm(Method.Dijkstra, map, lastStart, lastEnd);
            if (!algorithm.Execute()) { errorMsg = "Can not find end！！"; return; }
            StartCoroutine("ChangeSquaresColor", algorithm);
        }
        if (GUI.Button(new Rect(420, 0, 100, 40), "BFS") && lastStart != int.MinValue && lastEnd != int.MinValue)
        {
            StopAllCoroutines();
            ResetStateForMap();
            algorithm = new Algorithm(Method.BFS, map, lastStart, lastEnd);
            if (!algorithm.Execute()) { errorMsg = "Can not find end！！"; return; }
            StartCoroutine("ChangeSquaresColor", algorithm);
        }
        if (GUI.Button(new Rect(540, 0, 100, 40), "A Star") && lastStart != int.MinValue && lastEnd != int.MinValue)
        {
            StopAllCoroutines();
            ResetStateForMap();
            algorithm = new Algorithm(Method.AStar, map, lastStart, lastEnd);
            if (!algorithm.Execute()) { errorMsg = "Can not find end！！"; return; }
            StartCoroutine("ChangeSquaresColor", algorithm);
        }

        DrawSquares();

        DrawMessage(errorMsg);
    }

    private void ResetStateForMap()
    {
        //绘制网格所需信息计算
        row = map.GetLength(0);
        col = map.GetLength(1);
        ox = Mathf.FloorToInt(Screen.width * X_OFFSET);
        float w = Screen.width * (1-X_OFFSET*2);
        float h = w / ASPECT;
        oy = Mathf.FloorToInt((Screen.height - h) / 2);
        edge = Mathf.FloorToInt((w / (col - 1))) - BOARD_PIXEL;
 

        //产生地图对应的UI网格数组
        squares = new Square[row, col];
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                Color color = (map[i, j] == 1 ? Color.white : Color.grey);
                squares[i, j] = new Square(ox+ j * (edge + BOARD_PIXEL), oy + i * (edge + BOARD_PIXEL), edge, edge,color);
            }
        }

        //控制开关：起点和终点是否可设定；
        CanSetStartAndEnd = false;

        //错误信息复位
        errorMsg = "";

    }

    //重置起点和终点的开关
    private void ResetStartAndEnd()
    {
        lastStart = lastEnd = int.MinValue;
    }

    private int[,] GetRandomMap(int obstacle)
    {
       return Map.GetRandomMap(obstacle);
    }

    private void DrawSquares()
    {
        for(int i = 0; i < row; i++)
        {
            for(int j = 0; j < col; j++)
            {
                GUI.DrawTexture(squares[i, j].pos, squares[i, j].texture,ScaleMode.ScaleToFit,false,0,squares[i,j].color,0,0);
            }
        }
    }

    private IEnumerator ChangeSquaresColor(Algorithm algorithm)
    {
        List<int> searched = algorithm.GetCountOfVertexsSearched();
        int[] path = algorithm.ShowPath();

        if (searched.Count > 0)
        {
            int index = searched[0];
            squares[index / col, index % col].color = Color.red;
            index = searched[searched.Count - 1];
            squares[index / col, index % col].color = Color.red;
            yield return new WaitForSeconds(SHOW_TIME_GAP/2);
        }

        for(int i = 1; i < searched.Count-1; i++)
        {
            int index = searched[i];
            squares[index / col, index % col].color = Color.green;
            if(i% (searched.Count/ SHOW_NUM_RATIO) == 0)
                yield return new WaitForSeconds(SHOW_TIME_GAP);
        }

        yield return new WaitForSeconds(SHOW_TIME_GAP);

        //起点红色
        squares[path[0] / col, path[0] % col].color = Color.red;

        for (int i = 1; i < path.Length - 1; i++)
        {
            if (path[i] != int.MinValue && path[i + 1] != int.MinValue)
            {
                squares[path[i] / col, path[i] % col].color = Color.blue;
                squares[path[i + 1] / col, path[i + 1] % col].color = Color.blue;
            }
            else
            {
                //终点红色
                if(path[i]!=int.MinValue)
                    squares[path[i] / col, path[i] % col].color = Color.red;
                break;
            }

            if (i % (searched.Count/ SHOW_PATH_NUM_RATIO) == 0)
            {
                yield return new WaitForSeconds(SHOW_TIME_GAP);
            }
        }

        yield return null;
    }

    private void DrawMessage(string msg)
    {
        if (algorithm != null)
        {
            if (msg.Equals(""))
            {
                GUI.Box(new Rect(Screen.width - 300, 0, 140, 70),
                    "Algorithm : " + algorithm.method.ToString()+
                    "\nTime : " + algorithm.GetExcuteTime() +
                    "\nSearced Vetexs : " + algorithm.GetCountOfVertexsSearched().Count +
                    "\n Path Length : " + algorithm.GetPathLength()
                     );
            }
            else
            {
                GUI.Box(new Rect(Screen.width - 300, 0, 140, 70),msg);
            }
        }
    }

}

