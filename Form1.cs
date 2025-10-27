using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace cs__4;

public partial class Form1 : Form
{
    private System.Windows.Forms.Timer gameTimer = default!;
    private Rectangle paddle;
    private Rectangle ball;
    private List<Rectangle> bricks = default!;
    private int ballDx = 8;
    private int ballDy = -8;
    private int score = 0;

    public Form1()
    {
        InitializeComponent();
        SetupGame();
    }

    private void SetupGame()
    {
        this.Text = "Brick Breaker";
        this.MinimumSize = new Size(800, 600);
        this.BackColor = Color.Black;
        this.DoubleBuffered = true; // Prevents flickering

        paddle = new Rectangle(this.ClientSize.Width / 2 - 50, this.ClientSize.Height - 40, 100, 20);
        ball = new Rectangle(this.ClientSize.Width / 2 - 10, this.ClientSize.Height / 2 - 10, 20, 20);

        bricks = new List<Rectangle>();
        int brickWidth = 75;
        int brickHeight = 20;
        int spacing = 5;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                bricks.Add(new Rectangle(j * (brickWidth + spacing) + 35, i * (brickHeight + spacing) + 40, brickWidth, brickHeight));
            }
        }

        gameTimer = new System.Windows.Forms.Timer();
        gameTimer.Interval = 16; // Approx 60 FPS
        gameTimer.Tick += GameTimer_Tick;
        gameTimer.Start();

        this.Paint += Form1_Paint;
        this.MouseMove += Form1_MouseMove;
    }

    private void GameTimer_Tick(object? sender, EventArgs e)
    {
        ball.X += ballDx;
        ball.Y += ballDy;

        if (ball.Left <= 0 || ball.Right >= this.ClientSize.Width) ballDx = -ballDx;
        if (ball.Top <= 0) ballDy = -ballDy;
        if (ball.IntersectsWith(paddle))
        {
            ballDy = -ballDy;
            ball.Y = paddle.Top - ball.Height;
        }

        for (int i = bricks.Count - 1; i >= 0; i--)
        {
            if (ball.IntersectsWith(bricks[i]))
            {
                bricks.RemoveAt(i);
                ballDy = -ballDy;
                score += 10;
                break;
            }
        }

        if (ball.Top > this.ClientSize.Height)
        {
            gameTimer.Stop();
            MessageBox.Show("Game Over! Your score: " + score, "Game Over");
            this.Close();
        }

        if (bricks.Count == 0)
        {
            gameTimer.Stop();
            MessageBox.Show("You Win! Your score: " + score, "Congratulations!");
            this.Close();
        }

        this.Invalidate();
    }

    private void Form1_MouseMove(object? sender, MouseEventArgs e)
    {
        int newX = e.X - paddle.Width / 2;
        if (newX < 0) newX = 0;
        if (newX > this.ClientSize.Width - paddle.Width) newX = this.ClientSize.Width - paddle.Width;
        paddle.X = newX;
    }

    private void Form1_Paint(object? sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;
        g.FillRectangle(Brushes.White, paddle);
        g.FillEllipse(Brushes.Cyan, ball);
        foreach (var brick in bricks)
        {
            g.FillRectangle(Brushes.CornflowerBlue, brick);
        }
        g.DrawString("Score: " + score, new Font("Arial", 16), Brushes.White, 10, 10);
    }
}