﻿using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace IdleClickerEngine
{
    public class HealthBar
    {
            private Rectangle _progressBar;

            public HealthBar(Rectangle progressBar)
            {
                _progressBar = progressBar;
            }

            /// <summary>
            /// Count percent of hp
            /// </summary>
            /// <param name="total"></param>
            /// <param name="current"></param>
            /// <returns></returns>
            public int PercentOfHealth(double total, double current)
            {
                return (int)(current / total * 100);
            }

        /// <summary>
        /// Draw healthbar
        /// </summary>
        /// <param name="e"></param>
        /// <param name="remainingHealth"></param>
        /// <param name="monsterHealth"></param>
        public void DrawHealth(PaintEventArgs e, BigInteger remainingHealth, BigInteger monsterHealth)
            {
                e.Graphics.DrawRectangle(new Pen(Brushes.Red), _progressBar);
                e.Graphics.FillRectangle(Brushes.Red,
                    new Rectangle(_progressBar.X, _progressBar.Y,
                        (_progressBar.Width * PercentOfHealth((double)monsterHealth, (double)remainingHealth)) / 100,
                        _progressBar.Height));
            }
    }
}