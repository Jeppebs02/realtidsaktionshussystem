namespace AuctionHouse.DesktopApp
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            CantBuycheckbox = new CheckBox();
            CantSellcheckbox = new CheckBox();
            Userlistbox = new ListBox();
            GetAllUsersbtn = new Button();
            Savebtn = new Button();
            Searchtxtbox = new TextBox();
            label1 = new Label();
            Searchbtn = new Button();
            DeleteUserbtn = new Button();
            Emailtxtbox = new TextBox();
            Addresstxtbox = new TextBox();
            Passwordtxtbox = new TextBox();
            Usernametxtbox = new TextBox();
            LastNametxtbox = new TextBox();
            FirstNametxtbox = new TextBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            PhoneNumbertxtbox = new TextBox();
            SuspendLayout();
            // 
            // CantBuycheckbox
            // 
            CantBuycheckbox.AutoSize = true;
            CantBuycheckbox.Location = new Point(65, 189);
            CantBuycheckbox.Name = "CantBuycheckbox";
            CantBuycheckbox.Size = new Size(108, 29);
            CantBuycheckbox.TabIndex = 1;
            CantBuycheckbox.Text = "Cant Buy";
            CantBuycheckbox.UseVisualStyleBackColor = true;
            // 
            // CantSellcheckbox
            // 
            CantSellcheckbox.AutoSize = true;
            CantSellcheckbox.Location = new Point(65, 224);
            CantSellcheckbox.Name = "CantSellcheckbox";
            CantSellcheckbox.Size = new Size(106, 29);
            CantSellcheckbox.TabIndex = 2;
            CantSellcheckbox.Text = "Cant Sell";
            CantSellcheckbox.UseVisualStyleBackColor = true;
            // 
            // Userlistbox
            // 
            Userlistbox.FormattingEnabled = true;
            Userlistbox.ItemHeight = 25;
            Userlistbox.Location = new Point(349, 319);
            Userlistbox.Name = "Userlistbox";
            Userlistbox.Size = new Size(786, 304);
            Userlistbox.TabIndex = 3;
            Userlistbox.SelectedIndexChanged += Userlistbox_SelectedIndexChanged;
            // 
            // GetAllUsersbtn
            // 
            GetAllUsersbtn.Location = new Point(65, 319);
            GetAllUsersbtn.Name = "GetAllUsersbtn";
            GetAllUsersbtn.Size = new Size(267, 64);
            GetAllUsersbtn.TabIndex = 4;
            GetAllUsersbtn.Text = "Get All Users";
            GetAllUsersbtn.UseVisualStyleBackColor = true;
            GetAllUsersbtn.Click += GetAllUsersbtn_Click;
            // 
            // Savebtn
            // 
            Savebtn.Location = new Point(65, 559);
            Savebtn.Name = "Savebtn";
            Savebtn.Size = new Size(267, 64);
            Savebtn.TabIndex = 5;
            Savebtn.Text = "Save";
            Savebtn.UseVisualStyleBackColor = true;
            Savebtn.Click += Savebtn_Click;
            // 
            // Searchtxtbox
            // 
            Searchtxtbox.Location = new Point(65, 81);
            Searchtxtbox.Name = "Searchtxtbox";
            Searchtxtbox.Size = new Size(200, 31);
            Searchtxtbox.TabIndex = 6;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(65, 41);
            label1.Name = "label1";
            label1.Size = new Size(104, 25);
            label1.TabIndex = 7;
            label1.Text = "Search User";
            // 
            // Searchbtn
            // 
            Searchbtn.Location = new Point(285, 81);
            Searchbtn.Name = "Searchbtn";
            Searchbtn.Size = new Size(78, 31);
            Searchbtn.TabIndex = 8;
            Searchbtn.Text = "Search";
            Searchbtn.UseVisualStyleBackColor = true;
            // 
            // DeleteUserbtn
            // 
            DeleteUserbtn.Location = new Point(65, 489);
            DeleteUserbtn.Name = "DeleteUserbtn";
            DeleteUserbtn.Size = new Size(267, 64);
            DeleteUserbtn.TabIndex = 9;
            DeleteUserbtn.Text = "Delete User";
            DeleteUserbtn.UseVisualStyleBackColor = true;
            // 
            // Emailtxtbox
            // 
            Emailtxtbox.Location = new Point(349, 241);
            Emailtxtbox.Name = "Emailtxtbox";
            Emailtxtbox.Size = new Size(200, 31);
            Emailtxtbox.TabIndex = 10;
            // 
            // Addresstxtbox
            // 
            Addresstxtbox.Location = new Point(649, 241);
            Addresstxtbox.Name = "Addresstxtbox";
            Addresstxtbox.Size = new Size(200, 31);
            Addresstxtbox.TabIndex = 11;
            // 
            // Passwordtxtbox
            // 
            Passwordtxtbox.Location = new Point(935, 241);
            Passwordtxtbox.Name = "Passwordtxtbox";
            Passwordtxtbox.Size = new Size(200, 31);
            Passwordtxtbox.TabIndex = 12;
            // 
            // Usernametxtbox
            // 
            Usernametxtbox.Location = new Point(935, 152);
            Usernametxtbox.Name = "Usernametxtbox";
            Usernametxtbox.Size = new Size(200, 31);
            Usernametxtbox.TabIndex = 15;
            // 
            // LastNametxtbox
            // 
            LastNametxtbox.Location = new Point(649, 152);
            LastNametxtbox.Name = "LastNametxtbox";
            LastNametxtbox.Size = new Size(200, 31);
            LastNametxtbox.TabIndex = 14;
            // 
            // FirstNametxtbox
            // 
            FirstNametxtbox.Location = new Point(649, 69);
            FirstNametxtbox.Name = "FirstNametxtbox";
            FirstNametxtbox.Size = new Size(200, 31);
            FirstNametxtbox.TabIndex = 13;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(349, 213);
            label2.Name = "label2";
            label2.Size = new Size(54, 25);
            label2.TabIndex = 16;
            label2.Text = "Email";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(649, 213);
            label3.Name = "label3";
            label3.Size = new Size(77, 25);
            label3.TabIndex = 17;
            label3.Text = "Address";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(935, 213);
            label4.Name = "label4";
            label4.Size = new Size(87, 25);
            label4.TabIndex = 18;
            label4.Text = "Password";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(935, 124);
            label5.Name = "label5";
            label5.Size = new Size(91, 25);
            label5.TabIndex = 21;
            label5.Text = "Username";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(649, 124);
            label6.Name = "label6";
            label6.Size = new Size(95, 25);
            label6.TabIndex = 20;
            label6.Text = "Last Name";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(649, 41);
            label7.Name = "label7";
            label7.Size = new Size(97, 25);
            label7.TabIndex = 19;
            label7.Text = "First Name";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(349, 124);
            label8.Name = "label8";
            label8.Size = new Size(132, 25);
            label8.TabIndex = 23;
            label8.Text = "Phone Number";
            // 
            // PhoneNumbertxtbox
            // 
            PhoneNumbertxtbox.Location = new Point(349, 152);
            PhoneNumbertxtbox.Name = "PhoneNumbertxtbox";
            PhoneNumbertxtbox.Size = new Size(200, 31);
            PhoneNumbertxtbox.TabIndex = 22;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(10F, 25F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1185, 683);
            Controls.Add(label8);
            Controls.Add(PhoneNumbertxtbox);
            Controls.Add(label5);
            Controls.Add(label6);
            Controls.Add(label7);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(Usernametxtbox);
            Controls.Add(LastNametxtbox);
            Controls.Add(FirstNametxtbox);
            Controls.Add(Passwordtxtbox);
            Controls.Add(Addresstxtbox);
            Controls.Add(Emailtxtbox);
            Controls.Add(DeleteUserbtn);
            Controls.Add(Searchbtn);
            Controls.Add(label1);
            Controls.Add(Searchtxtbox);
            Controls.Add(Savebtn);
            Controls.Add(GetAllUsersbtn);
            Controls.Add(Userlistbox);
            Controls.Add(CantSellcheckbox);
            Controls.Add(CantBuycheckbox);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private CheckBox CantBuycheckbox;
        private CheckBox CantSellcheckbox;
        private ListBox Userlistbox;
        private Button GetAllUsersbtn;
        private Button Savebtn;
        private TextBox Searchtxtbox;
        private Label label1;
        private Button Searchbtn;
        private Button DeleteUserbtn;
        private TextBox Emailtxtbox;
        private TextBox Addresstxtbox;
        private TextBox Passwordtxtbox;
        private TextBox Usernametxtbox;
        private TextBox LastNametxtbox;
        private TextBox FirstNametxtbox;
        private Label label2;
        private Label label3;
        private Label label4;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private TextBox PhoneNumbertxtbox;
    }
}
