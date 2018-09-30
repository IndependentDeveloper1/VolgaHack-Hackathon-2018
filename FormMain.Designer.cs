namespace VoiceControl
{
    partial class FormMain
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.richTextBox_messages = new System.Windows.Forms.RichTextBox();
            this.pictureBox_load = new System.Windows.Forms.PictureBox();
            this.pictureBox_background = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_load)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_background)).BeginInit();
            this.SuspendLayout();
            // 
            // richTextBox_messages
            // 
            this.richTextBox_messages.BackColor = System.Drawing.SystemColors.MenuText;
            this.richTextBox_messages.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBox_messages.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox_messages.ForeColor = System.Drawing.SystemColors.Menu;
            this.richTextBox_messages.Location = new System.Drawing.Point(295, 306);
            this.richTextBox_messages.Name = "richTextBox_messages";
            this.richTextBox_messages.ReadOnly = true;
            this.richTextBox_messages.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.richTextBox_messages.Size = new System.Drawing.Size(175, 182);
            this.richTextBox_messages.TabIndex = 1;
            this.richTextBox_messages.Text = "";
            // 
            // pictureBox_load
            // 
            this.pictureBox_load.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox_load.Image = global::VoiceControl.Properties.Resources.rokid_wave_sound_fantasy;
            this.pictureBox_load.Location = new System.Drawing.Point(183, 56);
            this.pictureBox_load.Name = "pictureBox_load";
            this.pictureBox_load.Size = new System.Drawing.Size(382, 286);
            this.pictureBox_load.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_load.TabIndex = 2;
            this.pictureBox_load.TabStop = false;
            // 
            // pictureBox_background
            // 
            this.pictureBox_background.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox_background.Image = global::VoiceControl.Properties.Resources.giphy;
            this.pictureBox_background.Location = new System.Drawing.Point(0, 0);
            this.pictureBox_background.Name = "pictureBox_background";
            this.pictureBox_background.Size = new System.Drawing.Size(735, 500);
            this.pictureBox_background.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox_background.TabIndex = 3;
            this.pictureBox_background.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(735, 500);
            this.Controls.Add(this.richTextBox_messages);
            this.Controls.Add(this.pictureBox_load);
            this.Controls.Add(this.pictureBox_background);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Voice Control Vasya";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_load)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_background)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.RichTextBox richTextBox_messages;
        private System.Windows.Forms.PictureBox pictureBox_load;
        private System.Windows.Forms.PictureBox pictureBox_background;
    }
}

