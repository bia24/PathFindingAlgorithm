/* 
 * 生成地图的静态工具类
 */
using System;

public static class Map {

    private static readonly int ROW = 50;
    private static readonly int COLOUM = 75;

    public static int[,] GetRandomMap(int obstacle = 50)
    {

        int[,] map = new int[ROW, COLOUM];

        int obstacleCount = 0;

        Random random = new Random(DateTime.Now.Second);

        for (int i = 0; i < ROW; i++)
        {
            for(int j = 0; j < COLOUM; j++)
            {
                map[i, j] = 1;

                if (obstacleCount < obstacle)
                {
                    if (random.Next(1, ROW * COLOUM) <= obstacle)
                    {
                        map[i, j] = 0;
                        obstacleCount++;
                    }
                }
            }
        }

        return map;
    }

}
