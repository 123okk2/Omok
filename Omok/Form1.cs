using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Omok
{
    //S
    public partial class Form1 : Form
    {
        int[,] pan = new int[19,19];
        int order = 0; // 검은 돌
        SolidBrush black = new SolidBrush(Color.Black);
        SolidBrush white = new SolidBrush(Color.White);
        int hw, vw;

        Boolean isGame = false;

        public Form1()
        {
            InitializeComponent();
            중지ToolStripMenuItem.Enabled = false;
            재개ToolStripMenuItem.Enabled = false;

            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++) pan[i, j] = 2;
            }
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            hw = pictureBox1.Width / 20;
            vw = pictureBox1.Height / 20;
            Pen p = new Pen(Color.Black);
            SolidBrush b = new SolidBrush(Color.Black);

            for (int i = 1; i <= 19; i++)
            {
                g.DrawLine(p, i * hw, vw, i * hw, pictureBox1.Height - vw);
                g.DrawLine(p, hw, i * vw, pictureBox1.Width - hw, i * vw);
            }

            for (int j = 4; j <= 19; j = j + 6)
            {
                for (int i = 4; i <= 19; i = i + 6) g.FillEllipse(b, i * hw - 4, j * vw - 4, 8, 8);
            }
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (isGame)
            {
                Graphics g = pictureBox1.CreateGraphics();
                int qx = e.X / hw;
                int rx = e.X % hw;
                int qy = e.Y / vw;
                int ry = e.Y % vw;

                if (rx > hw / 2) qx = qx + 1;
                if (ry > vw / 2) qy = qy + 1;

                if (pan[qx - 1, qy - 1] == 2)
                {
                    if (order == 0)
                    {
                        g.FillEllipse(black, qx * hw - 8, qy * vw - 8, 16, 16);
                        pan[qx - 1, qy - 1] = order;
                        order = 1;
                    }
                    else
                    {
                        g.FillEllipse(white, qx * hw - 8, qy * vw - 8, 16, 16);
                        pan[qx - 1, qy - 1] = order;
                        order = 0;
                    }
                }
                if (checkWinningCondition(qx - 1, qy - 1, order))
                {
                    MessageBox.Show("승리하였습니다!");
                    재개ToolStripMenuItem.Enabled = false;
                    중지ToolStripMenuItem.Enabled = false;
                    종료ToolStripMenuItem.Enabled = true;
                    isGame = false;
                }
                g.Dispose();
            }
        }

        private void 시작ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            재개ToolStripMenuItem.Enabled = false;
            중지ToolStripMenuItem.Enabled = true;
            isGame = true;


            for (int i = 0; i < 19; i++)
            {
                for (int j = 0; j < 19; j++) pan[i,j] = 2;
            }

            Graphics g = pictureBox1.CreateGraphics();
            Refresh();
        }

        private void 중지ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            재개ToolStripMenuItem.Enabled = true;
            중지ToolStripMenuItem.Enabled = false;
            isGame = false;
        }
         
        private void 종료ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void 재개ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            재개ToolStripMenuItem.Enabled = false;
            중지ToolStripMenuItem.Enabled = true;
            isGame = true;
        }

        private bool checkWinningCondition(int x, int y, int o)
        {
            int cnt = 1;

            // 오른쪽 방향
            for (int i = x + 1; i <= 18; i++)
            {
                if (pan[i, y] == pan[x, y]) cnt++;
                else break;
            }
            // 왼쪽방향
            for (int i = x - 1; i >= 0; i--)
            {
                if (pan[i, y] == pan[x, y]) cnt++;
                else break;
            }
            if (cnt >= 5) return true;

            cnt = 1;

            // 아래 방향
            for (int i = y + 1; i <= 18; i++)
            {
                if (pan[x, i] == pan[x, y]) cnt++;
                else break;
            }
            // 위 방향
            for (int i = y - 1; i >= 0; i--)
            { 
                if (pan[x, i] == pan[x, y]) cnt++;
                else break;
            }

            if (cnt >= 5) return true;

            cnt = 1;

            // 대각선 오른쪽 위방향
            for (int i = x + 1, j = y - 1; i <= 18 && j >= 0; i++, j--)
            {
                if (pan[i, j] == pan[x, y]) cnt++;
                else break;
            }

            // 대각선 왼쪽 아래 방향
            for (int i = x - 1, j = y + 1; i >= 0 && j <= 18; i--, j++)
            {
                if (pan[i, j] == pan[x, y]) cnt++;
                else break;
            }

            if (cnt >= 5) return true;

            cnt = 1;

            // 대각선 왼쪽 위방향
            for (int i = x - 1, j = y - 1; i >= 0 && j >= 0; i--, j--)
            {
                if (pan[i, j] == pan[x, y]) cnt++;
                else break;
            }

            // 대각선 오른쪽 아래 방향 
            for (int i = x + 1, j = y + 1; i <= 18 && j <= 18; i++, j++)
            {
                if (pan[i, j] == pan[x, y]) cnt++;
                else break;
            }

            if (cnt >= 5) return true;

            return false;
        }
    }
}
