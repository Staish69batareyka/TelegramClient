
namespace TelegramClient;

partial class Form1
{
private void InitializeComponent()
{
    this.lstChats = new ListView();
    this.txtMessage = new TextBox();
    this.btnSend = new Button();
    this.txtHistory = new TextBox();
    this.notifyIcon = new System.Windows.Forms.NotifyIcon();
    this.progressBar = new ProgressBar();

    // Стили
    var telegramBlue = Color.FromArgb(42, 171, 238);
    var telegramGray = Color.FromArgb(245, 245, 245);
    var font = new Font("Segoe UI", 10F);
    
    
    // Подключаем drag-and-drop
    this.AllowDrop = true;
    this.DragEnter += Form1_DragEnter;
    this.DragDrop += Form1_DragDrop;
    
    
    //Индикатор процесса загрузки файла
    
    this.progressBar.Style = ProgressBarStyle.Marquee;
    this.progressBar.Visible = false;
    this.progressBar.Dock = DockStyle.Bottom;
    this.Controls.Add(progressBar);

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
    
    // lstChats
    this.lstChats.Location = new Point(20, 20);
    this.lstChats.Size = new Size(160, 395);
    this.lstChats.BorderStyle = BorderStyle.FixedSingle;
    this.lstChats.View = View.List;
    this.lstChats.FullRowSelect = true;
    this.lstChats.Scrollable = true;

    // txtHistory
    this.txtHistory.Location = new Point(190, 20);
    this.txtHistory.Size = new Size(480, 360);
    this.txtHistory.Multiline = true;
    this.txtHistory.ReadOnly = true;
    this.txtHistory.ScrollBars = ScrollBars.Vertical;
    this.txtHistory.BackColor = Color.White;
    this.txtHistory.BorderStyle = BorderStyle.FixedSingle;

    // txtMessage
    this.txtMessage.Location = new Point(190, 390);
    this.txtMessage.Size = new Size(380, 30);
    this.txtMessage.BorderStyle = BorderStyle.FixedSingle;

    // btnSend
    this.btnSend.Location = new Point(580, 390);
    this.btnSend.Size = new Size(90, 30);
    this.btnSend.Text = "Отправить";
    this.btnSend.BackColor = telegramBlue;
    this.btnSend.ForeColor = Color.White;
    this.btnSend.FlatStyle = FlatStyle.Flat;
    this.btnSend.Click += new EventHandler(this.btnSend_Click);

    // Добавление контролов

    this.Controls.Add(this.lstChats);
    this.Controls.Add(this.txtHistory);
    this.Controls.Add(this.txtMessage);
    this.Controls.Add(this.btnSend);
}



   
}