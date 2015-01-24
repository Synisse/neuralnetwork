using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineLearningNeuralNetwork.GUI.Points
{
    public class CoordinateBox : PictureBox
    {
        private Color _currentColor = Color.LightGray;

        private readonly List<PointTile> tiles = new List<PointTile>();
        public int TileSize = 5;


        public Color CurrentColor
        {
            get { return _currentColor; }
            set { _currentColor = value; }
        }

        public void GenerateTiles(int xCount, int yCount)
        { 
            for(var i = 0; i<xCount;i++)
            {
                for(var j = 0; j<yCount;j++)
                {
                    tiles.Add(new PointTile(new Rectangle(i*TileSize,j*TileSize,TileSize,TileSize),CurrentColor));
                }
            }
        }

        public void SetSizeByTileCount(int xTiles, int yTiles)
        {
            base.Size = new Size(xTiles*TileSize, yTiles*TileSize);
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);

            tiles[(e.X/TileSize*(this.Height/TileSize)) + e.Y/TileSize].Color = CurrentColor;
            tiles[(e.X/TileSize*(this.Height/TileSize)) + e.Y/TileSize].IsTrainingValue = true;
            
            this.Invalidate();

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            for (var i = 0; i < tiles.Count; i++)
                DrawTile(e.Graphics,tiles[i]);
        }

        public void SetTile(Color color, int x, int y)
        {

            tiles[(x/TileSize*(this.Height/TileSize)) + y/TileSize].Color = color;
            //for (int i = 0; i < tiles.Count; i++)
            //{
            //    if (tiles[i].Bounds.Contains(new Point(x, y)))
            //    {
            //        tiles[i].Draw(this.CreateGraphics(), color);
            //    }
            //}
        }

        public List<PointTile> GetNonTrainingSetTiles()
        {
            return tiles.Where(tile => !tile.IsTrainingValue).ToList();
        }

        private void DrawTile(Graphics g, PointTile tile)
        {
            g.DrawRectangle(new Pen(tile.Color, 5), tile.Bounds.X, tile.Bounds.Y, TileSize, TileSize);
        }
    }
}
