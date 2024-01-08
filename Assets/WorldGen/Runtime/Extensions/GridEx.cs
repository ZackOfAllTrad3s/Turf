using System;
using System.Threading.Tasks;

public static class GridEx
{
    public static void ForEachParallel(this float[,] grid, Action<int/*x*/, int/*y*/, float/*noise*/> onNoiseSampled)
    {
        int sizeX = grid.GetLength(0);
        int sizeY = grid.GetLength(1);
        Parallel.For(0, sizeX * sizeY, (i) =>
        {
            int x = i / sizeX;
            int y = i % sizeY;
            onNoiseSampled(x, y, grid[x, y]);
        });
    }
}
