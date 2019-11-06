/* 
 * 在Unity的GUI中显示的方块单元
 */
using UnityEngine;

public class Square{

    public Rect pos = Rect.zero;
    public Texture2D texture = null;
    public Color color = Color.white;
   
    public Square(int x, int y,int width,int height,Color color)
    {
        pos = new Rect(x, y, width, height);
        texture = Texture2D.whiteTexture;
        this.color = color;
    }
}
