
namespace TelegramClient;

partial class Form1
{
private void InitializeComponent()
{
    this.txtPhone = new TextBox();
    this.btnStartAuth = new Button();
    this.txtCode = new TextBox();
    this.btnFinishAuth = new Button();
    this.txtPassword = new TextBox();
    this.btnPassword = new Button();
    this.lstChats = new ListView();
    this.txtMessage = new TextBox();
    this.btnSend = new Button();
    this.txtHistory = new TextBox();
    this.notifyIcon = new System.Windows.Forms.NotifyIcon();

    var telegramBlue = Color.FromArgb(42, 171, 238);
    var telegramGray = Color.FromArgb(245, 245, 245);
    var font = new Font("Segoe UI", 10F);

    // notifyIcon
    this.notifyIcon.Icon = SystemIcons.Information;
    this.notifyIcon.Visible = true;
    this.notifyIcon.Text = "TelegramClient";

    // Form1
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(700, 450);
    this.Text = "Telegram Client";
    this.BackColor = telegramGray;
    this.Font = font;
    this.FormBorderStyle = FormBorderStyle.FixedSingle;
    this.MaximizeBox = false;

    // txtPhone
    this.txtPhone.Location = new Point(20, 20);
    this.txtPhone.Size = new Size(220, 30);
    this.txtPhone.PlaceholderText = "Номер телефона";
    this.txtPhone.BorderStyle = BorderStyle.FixedSingle;

    // btnStartAuth
    this.btnStartAuth.Location = new Point(250, 20);
    this.btnStartAuth.Size = new Size(180, 30);
    this.btnStartAuth.Text = "Получить код";
    this.btnStartAuth.BackColor = telegramBlue;
    this.btnStartAuth.ForeColor = Color.White;
    this.btnStartAuth.FlatStyle = FlatStyle.Flat;
    this.btnStartAuth.Click += new EventHandler(this.btnStartAuth_Click);

    // txtCode
    this.txtCode.Location = new Point(20, 60);
    this.txtCode.Size = new Size(220, 30);
    this.txtCode.PlaceholderText = "Код подтверждения";
    this.txtCode.BorderStyle = BorderStyle.FixedSingle;

    // btnFinishAuth
    this.btnFinishAuth.Location = new Point(250, 60);
    this.btnFinishAuth.Size = new Size(180, 30);
    this.btnFinishAuth.Text = "Подтвердить код";
    this.btnFinishAuth.BackColor = telegramBlue;
    this.btnFinishAuth.ForeColor = Color.White;
    this.btnFinishAuth.FlatStyle = FlatStyle.Flat;
    this.btnFinishAuth.Click += new EventHandler(this.btnFinishAuth_Click);

    // txtPassword
    this.txtPassword.Location = new Point(20, 100);
    this.txtPassword.Size = new Size(220, 30);
    this.txtPassword.PlaceholderText = "Пароль (если есть)";
    this.txtPassword.UseSystemPasswordChar = true;
    this.txtPassword.BorderStyle = BorderStyle.FixedSingle;
    this.txtPassword.Visible = false;

    // btnPassword
    this.btnPassword.Location = new Point(250, 100);
    this.btnPassword.Size = new Size(180, 30);
    this.btnPassword.Text = "Отправить пароль";
    this.btnPassword.BackColor = telegramBlue;
    this.btnPassword.ForeColor = Color.White;
    this.btnPassword.FlatStyle = FlatStyle.Flat;
    this.btnPassword.Click += new EventHandler(this.btnPassword_Click);
    this.btnPassword.Visible = false;

    // lstChats
    this.lstChats.Location = new Point(20, 140);
    this.lstChats.Size = new Size(160, 250);
    this.lstChats.BorderStyle = BorderStyle.FixedSingle;

    // txtHistory
    this.txtHistory.Location = new Point(190, 140);
    this.txtHistory.Size = new Size(480, 160);
    this.txtHistory.Multiline = true;
    this.txtHistory.ReadOnly = true;
    this.txtHistory.ScrollBars = ScrollBars.Vertical;
    this.txtHistory.BackColor = Color.White;
    this.txtHistory.BorderStyle = BorderStyle.FixedSingle;

    // txtMessage
    this.txtMessage.Location = new Point(190, 310);
    this.txtMessage.Size = new Size(380, 30);
    this.txtMessage.BorderStyle = BorderStyle.FixedSingle;

    // btnSend
    this.btnSend.Location = new Point(580, 310);
    this.btnSend.Size = new Size(90, 30);
    this.btnSend.Text = "Отправить";
    this.btnSend.BackColor = telegramBlue;
    this.btnSend.ForeColor = Color.White;
    this.btnSend.FlatStyle = FlatStyle.Flat;
    this.btnSend.Click += new EventHandler(this.btnSend_Click);

    // Добавление контролов
    this.Controls.Add(this.txtPhone);
    this.Controls.Add(this.btnStartAuth);
    this.Controls.Add(this.txtCode);
    this.Controls.Add(this.btnFinishAuth);
    this.Controls.Add(this.txtPassword);
    this.Controls.Add(this.btnPassword);
    this.Controls.Add(this.lstChats);
    this.Controls.Add(this.txtHistory);
    this.Controls.Add(this.txtMessage);
    this.Controls.Add(this.btnSend);
}



   
}