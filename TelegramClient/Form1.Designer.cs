
namespace TelegramClient;

partial class Form1
{
   private void InitializeComponent()
        {
            this.txtPhone = new TextBox();
            this.btnStartAuth = new Button();
            this.txtCode = new TextBox();
            this.btnFinishAuth = new Button();
            this.lstChats = new ListBox();
            this.txtMessage = new TextBox();
            this.btnSend = new Button();
            this.txtHistory = new TextBox();

            // txtPhone
            this.txtPhone.Location = new System.Drawing.Point(10, 10);
            this.txtPhone.Size = new System.Drawing.Size(200, 30);
            this.txtPhone.PlaceholderText = "Введите номер телефона";

            // btnStartAuth
            this.btnStartAuth.Location = new System.Drawing.Point(220, 10);
            this.btnStartAuth.Size = new System.Drawing.Size(150, 30);
            this.btnStartAuth.Text = "Начать авторизацию";
            this.btnStartAuth.Click += new EventHandler(this.btnStartAuth_Click);

            // txtCode
            this.txtCode.Location = new System.Drawing.Point(10, 40);
            this.txtCode.Size = new System.Drawing.Size(200, 30);
            this.txtCode.PlaceholderText = "Код подтверждения";

            // btnFinishAuth
            this.btnFinishAuth.Location = new System.Drawing.Point(220, 40);
            this.btnFinishAuth.Size = new System.Drawing.Size(150, 30);
            this.btnFinishAuth.Text = "Подтвердить код";
            this.btnFinishAuth.Click += new EventHandler(this.btnFinishAuth_Click);

            // lstChats
            this.lstChats.Location = new System.Drawing.Point(10, 70);
            this.lstChats.Size = new System.Drawing.Size(150, 230);

            // txtHistory
            this.txtHistory.Location = new System.Drawing.Point(170, 70);
            this.txtHistory.Size = new System.Drawing.Size(400, 160);
            this.txtHistory.Multiline = true;
            this.txtHistory.ReadOnly = true;
            this.txtHistory.ScrollBars = ScrollBars.Vertical;

            // txtMessage
            this.txtMessage.Location = new System.Drawing.Point(170, 240);
            this.txtMessage.Size = new System.Drawing.Size(300, 30);

            // btnSend
            this.btnSend.Location = new System.Drawing.Point(480, 240);
            this.btnSend.Size = new System.Drawing.Size(100, 30);
            this.btnSend.Text = "Отправить";
            this.btnSend.Click += new EventHandler(this.btnSend_Click);

            // Form1
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 280);
            this.Text = "Telegram Client";

            this.Controls.Add(this.txtPhone);
            this.Controls.Add(this.btnStartAuth);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.btnFinishAuth);
            this.Controls.Add(this.lstChats);
            this.Controls.Add(this.txtHistory);
            this.Controls.Add(this.txtMessage);
            this.Controls.Add(this.btnSend);
        }


   
}