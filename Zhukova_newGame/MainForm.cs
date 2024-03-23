using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading; 

namespace Zhukova_newGame
{
    public partial class MainForm : Form 
    {
        Graphics graphics; // инициализация графики 
        System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        System.Windows.Forms.Timer timerPlayer = new System.Windows.Forms.Timer();
        int changeFrame = 60; //смена изоброжения - ускрение шарика 
        int player; // коорината игрока по y 
        int ballx; // координата меча по x
        int bally; // координата меча по y 
        int ballSpeedX = 3; // скорость меча по x
        int ballSpeedY = 3; // скорость меча по y 
        int count = 0;
        public MainForm()
        {
            InitializeComponent();
            button1.Click += Button1_Click;
        } 
        private void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                ballSpeedX = int.Parse(textBox1.Text);
                ballSpeedY = int.Parse(textBox1.Text);
            }
            catch(FormatException)
            {
                MessageBox.Show("Ошибка ввода");
                this.Close();  
            }
            timer.Enabled = true; // запуск таймера
            timer.Interval = 1000 / changeFrame; //интервал обновления графики 
            timer.Tick += new EventHandler(TimerCallback); // время обновиться 
            ballx = this.Width / 2; // первоначальное положение 
            bally = this.Height / 2; // первоначальное положение 
            textBox1.Visible = false;
            button1.Visible = false;
            label2.Visible = false;
            label3.Visible = true;
            Paint += new PaintEventHandler(this.MainFormPaint);
            this.ResumeLayout(false);
            this.PerformLayout();
            Thread playerThread = new Thread(Player);
            playerThread.Start(); 
        }
        void Player()
        {
            KeyPreview = true;
            KeyDown += new KeyEventHandler(MainFormKeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();
            timerPlayer.Enabled = true; // запуск таймера
            timerPlayer.Interval = 1000 / changeFrame; //интервал обновления графики 
            timerPlayer.Tick += new EventHandler(UpdatePlayer); // время обновиться 
        }
        void MainFormPaint(object sender, PaintEventArgs e) //генирируем объекты ракетка и мячик
        {
            graphics = CreateGraphics();
            DrawRectangle(0, player, 20, 130, new SolidBrush(Color.Black));
            DrawBall(ballx, bally, 20, 20, new SolidBrush(Color.Black));
        }
        void DrawRectangle(int x, int y, int w, int h, SolidBrush Color)
        {
            graphics.FillRectangle(Color, new Rectangle(x, y, w, h));
        } 
        void DrawBall(int x, int y, int w, int h, SolidBrush Color)
        {
            graphics.FillEllipse(Color, new Rectangle(x, y, w, h));
        }
        void UpdateBall()
        {
            ballx += ballSpeedX;
            bally += ballSpeedY;

            if (ballx + 40 >= this.Width)
            {
                ballSpeedX = -ballSpeedX;
            }

            if (bally <= 0 || bally + 60 >= this.Height)
            {
                ballSpeedY = -ballSpeedY;
            }

            if (IsCollided())
            {
                ballSpeedX = -(ballSpeedX);
            }

            if (ballx <= 0)
            {
                label1.Visible = true;
                timer.Stop();
                timerPlayer.Stop(); 
            }
        }
        bool IsCollided()
        {
            if (ballx < 25 && ballx > 20 && bally + 25 >= player && bally + 20 <= player + 130 || ballx < 25 && ballx > 20 && bally >= player && bally <= player + 130)
            {
                count++;
                label3.Text = "Счет : " + count;
                return true; 
            }
            else
            {
                return false;
            }
        }
        void UpdatePlayer(object sender, EventArgs e)
        {
            DrawRectangle(0, player, 20, 130, new SolidBrush(Color.Black));
            this.Invalidate(); // перерисовка элементов
            return;
        }
        void TimerCallback(object sender, EventArgs e)
        {
            //DrawRectangle(0, player, 20, 130, new SolidBrush(Color.Black));
            DrawBall(ballx, bally, 20, 20, new SolidBrush(Color.Black));
            UpdateBall();
            this.Invalidate(); // перерисовка элементов
            return; 
        } 
        void MainFormKeyDown(object sender, KeyEventArgs e)
        {
            int key = e.KeyValue;
            //38 - движение вверх
            //40 - движение вниз
            if (key == 38 && player >= 0)
            {
                player -= 5; //5 - скорость игрока
            }

            if (key == 40 && player + 165 <= this.Height )
            {
                player += 5; //5 - скорость игрока 
            }
        }
    }
}
