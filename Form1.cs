using System;
using System.Drawing;
using System.Windows.Forms;

namespace cs__4;

public partial class Form1 : Form
{
    public Form1()
    {
        InitializeComponent();
        this.Text = "Hello SymDev";
        this.MinimumSize = new Size(800, 600);

        Label helloLabel = new Label();
        helloLabel.Text = "Hello SymDev AI";
        helloLabel.Font = new Font(helloLabel.Font.FontFamily, 24, FontStyle.Bold);
        helloLabel.AutoSize = true;
        this.Controls.Add(helloLabel);

        // Center the label
        this.Resize += (sender, e) => CenterLabel(helloLabel);
        CenterLabel(helloLabel); // Initial centering
    }

    private void CenterLabel(Label label)
    {
        label.Left = (this.ClientSize.Width - label.Width) / 2;
        label.Top = (this.ClientSize.Height - label.Height) / 2;
    }
}