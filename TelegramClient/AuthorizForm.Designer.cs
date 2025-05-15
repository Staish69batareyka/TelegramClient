using System.ComponentModel;

namespace TelegramClient;

partial class AuthorizForm
{
    private void InitializeComponent()
    {
        this._txtPhone = new TextBox();
        this._btnStartAuth = new Button();
        this._txtCode = new TextBox();
        this._btnFinishAuth = new Button();
        this._txtPassword = new TextBox();
        this._btnPassword = new Button();

        var telegramBlue = Color.FromArgb(42, 171, 238);
        var telegramGray = Color.FromArgb(245, 245, 245);
        var font = new Font("Segoe UI", 10F);

        // txtPhone
        this._txtPhone.Location = new Point(20, 20);
        this._txtPhone.Size = new Size(220, 30);
        this._txtPhone.PlaceholderText = "Номер телефона";
        this._txtPhone.BorderStyle = BorderStyle.FixedSingle;

        // btnStartAuth
        this._btnStartAuth.Location = new Point(250, 20);
        this._btnStartAuth.Size = new Size(180, 30);
        this._btnStartAuth.Text = "Подтвердить номер";
        this._btnStartAuth.BackColor = telegramBlue;
        this._btnStartAuth.ForeColor = Color.White;
        this._btnStartAuth.FlatStyle = FlatStyle.Flat;
        this._btnStartAuth.Click += new EventHandler(this.btnStartAuth_Click);

        // txtCode
        this._txtCode.Location = new Point(20, 60);
        this._txtCode.Size = new Size(220, 30);
        this._txtCode.PlaceholderText = "Код подтверждения";
        this._txtCode.BorderStyle = BorderStyle.FixedSingle;

        // btnFinishAuth
        this._btnFinishAuth.Location = new Point(250, 60);
        this._btnFinishAuth.Size = new Size(180, 30);
        this._btnFinishAuth.Text = "Подтвердить код";
        this._btnFinishAuth.BackColor = telegramBlue;
        this._btnFinishAuth.ForeColor = Color.White;
        this._btnFinishAuth.FlatStyle = FlatStyle.Flat;
        this._btnFinishAuth.Click += new EventHandler(this.btnFinishAuth_Click);

        // txtPassword
        this._txtPassword.Location = new Point(20, 100);
        this._txtPassword.Size = new Size(220, 30);
        this._txtPassword.PlaceholderText = "Пароль (если есть)";
        this._txtPassword.UseSystemPasswordChar = true;
        this._txtPassword.BorderStyle = BorderStyle.FixedSingle;
        this._txtPassword.Visible = false;

        // btnPassword
        this._btnPassword.Location = new Point(250, 100);
        this._btnPassword.Size = new Size(180, 30);
        this._btnPassword.Text = "Подтвердить пароль";
        this._btnPassword.BackColor = telegramBlue;
        this._btnPassword.ForeColor = Color.White;
        this._btnPassword.FlatStyle = FlatStyle.Flat;
        this._btnPassword.Click += new EventHandler(this.btnPassword_Click);
        this._btnPassword.Visible = false;

        // AuthorizForm
        this.AutoScaleMode = AutoScaleMode.Font;
        this.ClientSize = new Size(700, 450);
        this.Text = "Authorization Form";
        this.BackColor = telegramGray;
        this.Font = font;
        this.FormBorderStyle = FormBorderStyle.FixedSingle;
        this.MaximizeBox = false;


        this.Controls.Add(this._txtPhone);
        this.Controls.Add(this._btnStartAuth);
        this.Controls.Add(this._txtCode);
        this.Controls.Add(this._btnFinishAuth);
        this.Controls.Add(this._txtPassword);
        this.Controls.Add(this._btnPassword);
    }
}