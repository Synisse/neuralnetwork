using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MachineLearningNeuralNetwork.GUI.Points
{
    public class PointTile
    {
        public Rectangle Bounds;
        public bool IsTrainingValue { get; set; }

        public PointTile(Rectangle bounds)
        {
            this.Bounds = bounds;
            IsTrainingValue = false;
        }

        public PointTile(Rectangle bounds, Color color)
        {
            this.Bounds = bounds;
            Color = color;
            IsTrainingValue = false;
        }

        public Color Color { get; set; }

        public void Draw(Graphics g, Color color)
        {
            g.DrawRectangle(new Pen(color, 1), this.Bounds.X, this.Bounds.Y, 5, 5);
        }
    }

}
