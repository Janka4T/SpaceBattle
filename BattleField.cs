using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpaceBattle
{     
    public partial class Battlefield : Form
    {
        private bool moveLeft = false;
        private bool moveRight = false;
        private bool moveUp = false;
        private bool moveDown = false;
        //private bool gameOver = false;
        private bool bulletFired = false;

        private Point point;
        private Spaceship spaceship = null;
        //private Bullet bullet = null;
        private Timer mainTimer = null;
        private Timer enemyTimer = null;
        private Random rand = new Random();
        
        public Battlefield()
        {
            WindowState = FormWindowState.Maximized;
            InitializeComponent();
            InitializeBattleField();
            InitializeMainTimer();
            InitializeEnemyTimer();
        }

        private void InitializeBattleField()
        {
            if (WindowState == FormWindowState.Maximized)
            {
            this.BackColor = Color.Black;
            spaceship = new Spaceship();
            spaceship.Left = ClientRectangle.Width + (ClientRectangle.Width / 2 + spaceship.Width / 2);
            spaceship.Top = ClientRectangle.Height + (ClientRectangle.Y / 2 + spaceship.Height / 2);
            this.Controls.Add(spaceship);
            }                                               
        }
        private void InitializeMainTimer()
        {
            mainTimer = new Timer();
            mainTimer.Tick += new EventHandler(MainTimer_Tick);
            mainTimer.Interval = 10;
            mainTimer.Start();
        }

        private void MainTimer_Tick(object sender, EventArgs e)
        {
            if (moveLeft && spaceship.Left > 0)
            {
                spaceship.Left -= 2;
            }
            if (moveRight && spaceship.Left + spaceship.Width < ClientRectangle.Width)
            {
                spaceship.Left += 2;
            }
            if(moveDown && spaceship.Top + spaceship.Height < ClientRectangle.Height)
            {
                spaceship.Top += 2;
            }
            if(moveUp && spaceship.Top > 0)
            {
                spaceship.Top -= 2;
            }
            EnemyBulletCollision();
            EnemySpaceShipCollision();
        }

        private void InitializeEnemyTimer()
        {
            enemyTimer = new Timer();
            enemyTimer.Tick += new EventHandler(EnemyTimer_Tick);
            enemyTimer.Interval = 2000;
            enemyTimer.Start();
        }

        private void EnemyTimer_Tick(object sender, EventArgs e)
        {
            Enemy enemy = new Enemy(rand.Next(1, 6), this);
            enemy.Left = rand.Next(20, this.ClientRectangle.Width - enemy.Width);
            this.Controls.Add(enemy);
        }

        private void EnemyBulletCollision()
        {
            foreach(Control c in this.Controls)
            {
                if((string)c.Tag == "enemy")
                {
                    foreach(Control b in this.Controls)
                    {
                        if((string)b.Tag == "bullet")
                        {
                            if (c.Bounds.IntersectsWith(b.Bounds))
                            {
                                c.Dispose();
                                b.Dispose();
                            }
                        }
                    }
                }
            }
        }

        private void EnemySpaceShipCollision()
        {
            foreach(Control c  in this.Controls)
            {
                if((string)c.Tag == "enemy")
                {
                    if (c.Bounds.IntersectsWith(spaceship.Bounds))
                    {
                        c.Dispose();
                        spaceship.Dispose();
                        GameOver();
                    }
                }
            }           
        }

        private void GameOver()
        {
            mainTimer.Stop();
            MessageBox.Show("Game Over");
        }

        private void Battlefield_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && !bulletFired)
            {
                spaceship.Fire(this);
                bulletFired = true;
            }
            else if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                moveLeft = true;
            }
            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                moveRight = true;
            }
            else if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                moveDown = true;
            }
            else if(e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            {
                moveUp = true;
            }
            else if(e.KeyCode == Keys.E)
            {
                if (spaceship.EngineStatus == "off")
                {
                    spaceship.EngineOn();
                }
                else if (spaceship.EngineStatus == "on")
                {
                    spaceship.EngineOff();
                }                
            }
        }

        private void Battlefield_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
               spaceship.Fire(this);
            }
        }

        private void Battlefield_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space)
            {
                bulletFired = false;
            }
            else if (e.KeyCode == Keys.A || e.KeyCode == Keys.Left)
            {
                moveLeft = false;
            }
            else if (e.KeyCode == Keys.D || e.KeyCode == Keys.Right)
            {
                moveRight = false;

            }
            else if (e.KeyCode == Keys.S || e.KeyCode == Keys.Down)
            {
                moveDown = false;
            }
            else if (e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            {
                moveUp = false;
            }
        }

        public void Battlefield_MouseMove(object sender, MouseEventArgs e)
        {
            double angle;
            int x2; x2 = point.X;
            int y2; y2 = point.Y;
            int y1; y1 = spaceship.Location.Y;
            int x1; x1 = spaceship.Location.X;
            //point = this.PointToClient(Cursor.Position);
            //angle = Math.Atan2(x1 - x2 , y1 - y2);
            angle = Math.Pow(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2), 0.5);
        }
    }
}
