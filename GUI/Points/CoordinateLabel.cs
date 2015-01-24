using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MachineLearningNeuralNetwork.GUI.Points
{
    public class CoordinateLabel
    {
        public Rectangle Bounds;

        public CoordinateLabel(Rectangle bounds)
        {
            this.Bounds = bounds;
        }

        public override string ToString()
        {
            return "test";
        }
        public void Draw(Graphics g)
        {
            //Draw(g);
            g.DrawRectangle(new Pen(Color.Red, 1), this.Bounds.X, this.Bounds.Y, 1, 1);
            //g.DrawString(
            //g.DrawRectangle(new Pen(Color.Red, 2),
            //    this.Bounds.X + 10,
            //    this.Bounds.Y + 10,
            //    this.Bounds.Width - 20,
            //    this.Bounds.Height - 20);
        }
        public virtual void Action()
        {
            //this.Draw();
        }
    }
}
