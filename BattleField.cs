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
        private bool gameOver = false;
        private bool bulletFired = false;        

        private Spaceship spaceship = null;
        private Bullet bullet = null;
        private Timer mainTimer = null;

        public Battlefield()
        {
            InitializeComponent();
            InitializeBattleField();
            InitializeMainTimer();
        }

        private void InitializeBattleField()
        {
            this.BackColor = Color.Black;
            spaceship = new Spaceship();
            spaceship.Left = ClientRectangle.Width - (ClientRectangle.Width / 2 + spaceship.Width / 2);
            spaceship.Top = ClientRectangle.Height - (spaceship.Height + 20);
            this.Controls.Add(spaceship);
            Activate();
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
            if(moveUp && spaceship.Top + spaceship.Height > 0)
            {
                spaceship.Top -= 2;
            }
        }

        private void FireBullet()
        {
            if(spaceship.EngineStatus == "off")
            {
                bullet = new Bullet(5);
            }
            else if (spaceship.EngineStatus == "on")
            {
                bullet = new Bullet(10);
            }
            bullet.Top = spaceship.Top;
            bullet.Left = spaceship.Left + (spaceship.Width / 2 - bullet.Width / 2);
            this.Controls.Add(bullet);
            bullet.BringToFront();
        }

        private void Battlefield_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Space && !bulletFired)
            {
                FireBullet();
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
            else if(e.KeyCode == Keys.O)
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
                FireBullet();
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
    }
}
