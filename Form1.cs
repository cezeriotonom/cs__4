using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace cs__4;

public partial class Form1 : Form
{
    private Timer gameTimer;
    private Rectangle paddle;
    private Rectangle ball;
    private List<Rectangle> bricks;
    private int ballDx = 4;
    private int ballDy = -4;
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

        // Initialize paddle
        paddle = new Rectangle(this.ClientSize.Width / 2 - 50, this.ClientSize.Height - 40, 100, 20);

        // Initialize ball
        ball = new Rectangle(this.ClientSize.Width / 2 - 10, this.ClientSize.Height / 2 - 10, 20, 20);

        // Initialize bricks
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

        // Initialize and start game timer
        gameTimer = new Timer();
        gameTimer.Interval = 16; // Approx 60 FPS
        gameTimer.Tick += GameTimer_Tick;
        gameTimer.Start();

        // Hook up events
        this.Paint += Form1_Paint;
        this.MouseMove += Form1_MouseMove;
    }

    private void GameTimer_Tick(object sender, EventArgs e)
    {
        // Move the ball
        ball.X += ballDx;
        ball.Y += ballDy;

        // Wall collision (left/right)
        if (ball.Left <= 0 || ball.Right >= this.ClientSize.Width)
        {
            ballDx = -ballDx;
        }

        // Wall collision (top)
        if (ball.Top <= 0)
        {
            ballDy = -ballDy;
        }

        // Paddle collision
        if (ball.IntersectsWith(paddle))
        {
            ballDy = -ballDy;
            // Ensure ball is placed above the paddle to avoid getting stuck
            ball.Y = paddle.Top - ball.Height;
        }

        // Brick collision
        for (int i = bricks.Count - 1; i >= 0; i--)
        {
            if (ball.IntersectsWith(bricks[i]))
            {
                bricks.RemoveAt(i);
                ballDy = -ballDy;
                score += 10;
                break; // Exit loop after breaking one brick
            }
        }

        // Game Over condition
        if (ball.Top > this.ClientSize.Height)
        {
            gameTimer.Stop();
            MessageBox.Show("Game Over! Your score: " + score, "Game Over");
            this.Close();
        }

        // Win condition
        if (bricks.Count == 0)
        {
            gameTimer.Stop();
            MessageBox.Show("You Win! Your score: " + score, "Congratulations!");
            this.Close();
        }

        // Redraw the form
        this.Invalidate();
    }

    private void Form1_MouseMove(object sender, MouseEventArgs e)
    {
        // Move paddle with the mouse
        int newX = e.X - paddle.Width / 2;
        if (newX < 0) newX = 0;
        if (newX > this.ClientSize.Width - paddle.Width) newX = this.ClientSize.Width - paddle.Width;
        paddle.X = newX;
    }

    private void Form1_Paint(object sender, PaintEventArgs e)
    {
        Graphics g = e.Graphics;

        // Draw paddle
        g.FillRectangle(Brushes.White, paddle);

        // Draw ball
        g.FillEllipse(Brushes.Cyan, ball);

        // Draw bricks
        foreach (var brick in bricks)
        {
            g.FillRectangle(Brushes.CornflowerBlue, brick);
        }

        // Draw score
        g.DrawString("Score: " + score, new Font("Arial", 16), Brushes.White, 10, 10);
    }
}