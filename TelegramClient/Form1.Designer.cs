
namespace TelegramClient;

partial class Form1
{
private void InitializeComponent()
{
    this._lstChats = new ListView();
    this._txtMessage = new TextBox();
    this._btnSend = new Button();
    this._btnSendFile = new Button();
    this._txtHistory = new TextBox();
    this._notifyIcon = new System.Windows.Forms.NotifyIcon();
    this._progressBar = new ProgressBar();

    // Стили
    var telegramBlue = Color.FromArgb(42, 171, 238);
    var telegramGray = Color.FromArgb(245, 245, 245);
    var font = new Font("Segoe UI", 10F);
    
    
    // Подключаем drag-and-drop
    this.AllowDrop = true;
    this.DragEnter += Form1_DragEnter;
    this.DragDrop += Form1_DragDrop;
    
    
    //Индикатор процесса загрузки файла
    
    this._progressBar.Style = ProgressBarStyle.Marquee;
    this._progressBar.Visible = false;
    this._progressBar.Dock = DockStyle.Bottom;
    this.Controls.Add(_progressBar);

    // _notifyIcon
    this._notifyIcon.Icon = SystemIcons.Information;
    this._notifyIcon.Visible = true;
    this._notifyIcon.Text = "TelegramClient";

    // Form1
    this.AutoScaleMode = AutoScaleMode.Font;
    this.ClientSize = new Size(800, 450);
    this.Text = "Telegram Client";
    this.BackColor = telegramGray;
    this.Font = font;
    this.FormBorderStyle = FormBorderStyle.FixedSingle;
    this.MaximizeBox = false;
    
    // _lstChats
    this._lstChats.Location = new Point(20, 20);
    this._lstChats.Size = new Size(160, 395);
    this._lstChats.BorderStyle = BorderStyle.FixedSingle;
    this._lstChats.View = View.Details;
    this._lstChats.Columns.Clear();
    this._lstChats.Columns.Add("Chat", this._lstChats.Width - 5);
    this._lstChats.FullRowSelect = true;
    this._lstChats.Scrollable = true;
   

    // _txtHistory
    this._txtHistory.Location = new Point(190, 20);
    this._txtHistory.Size = new Size(600, 360);
    this._txtHistory.Multiline = true;
    this._txtHistory.ReadOnly = true;
    this._txtHistory.ScrollBars = ScrollBars.Vertical;
    this._txtHistory.BackColor = Color.White;
    this._txtHistory.BorderStyle = BorderStyle.FixedSingle;

    // _txtMessage
    this._txtMessage.Location = new Point(190, 390);
    this._txtMessage.Size = new Size(380, 30);
    this._txtMessage.BorderStyle = BorderStyle.FixedSingle;

    // _btnSend
    this._btnSend.Location = new Point(580, 390);
    this._btnSend.Size = new Size(90, 30);
    this._btnSend.Text = "Отправить";
    this._btnSend.BackColor = telegramBlue;
    this._btnSend.ForeColor = Color.White;
    this._btnSend.FlatStyle = FlatStyle.Flat;
    this._btnSend.Click += new EventHandler(this.btnSend_Click);
    
    // _btnSendFile
    this._btnSendFile.Location = new Point(670, 390);
    this._btnSendFile.Size = new Size(90, 30);
    this._btnSendFile.Text = "файл";
    this._btnSendFile.BackColor = telegramBlue;
    this._btnSendFile.ForeColor = Color.White;
    this._btnSendFile.FlatStyle = FlatStyle.Flat;
    this._btnSendFile.Click += new EventHandler(this.btnSendFile_Click);

    // Добавление контролов

    this.Controls.Add(this._lstChats);
    this.Controls.Add(this._txtHistory);
    this.Controls.Add(this._txtMessage);
    this.Controls.Add(this._btnSend);
    this.Controls.Add(this._btnSendFile);
}



   
}