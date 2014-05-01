namespace org.niffty.ui {
  partial class FormMain {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing) {
      if (disposing && (components != null)) {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
      this.musicControl = new org.niffty.ui.MusicControl();
      this.SuspendLayout();
      // 
      // musicControl
      // 
      this.musicControl.Dock = System.Windows.Forms.DockStyle.Fill;
      this.musicControl.Location = new System.Drawing.Point(0, 0);
      this.musicControl.Name = "musicControl";
      this.musicControl.PageIndex = -1;
      this.musicControl.Score = null;
      this.musicControl.Size = new System.Drawing.Size(284, 262);
      this.musicControl.TabIndex = 0;
      // 
      // FormMain
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(284, 262);
      this.Controls.Add(this.musicControl);
      this.Name = "FormMain";
      this.Text = "Form1";
      this.ResumeLayout(false);

    }

    #endregion

    private MusicControl musicControl;
  }
}

