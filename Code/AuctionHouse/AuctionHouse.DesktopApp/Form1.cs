using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.Requester;
using System.Net;
using System.Text;
using System.Text.Json;

namespace AuctionHouse.DesktopApp
{
    public partial class Form1 : Form
    {

        private readonly APIRequester _apiRequester;

        public Form1(APIRequester apiRequester)
        {
            InitializeComponent();
            _apiRequester = apiRequester;

        }

        private async void GetAllUsersbtn_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Prevent double-clicks while we’re busy
                GetAllUsersbtn.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;

                // 2. Call the API asynchronously
                string json = await _apiRequester.Get("api/user")
                                                 .ConfigureAwait(false);

                // 3. Switch back to the UI thread to update controls
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() => PopulateList(json)));
                }
                else
                {
                    PopulateList(json);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load users:\n{ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                GetAllUsersbtn.Enabled = true;
            }
        }

        private void PopulateList(string json)
        {
            // ───── 1. Deserialize (case-insensitive) ─────
            var users = JsonSerializer.Deserialize<List<User>>(json,
                         new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            // ───── 2. (Re)bind the ListBox ─────
            Userlistbox.DataSource = null;
            Userlistbox.DataSource = users ?? new List<User>();

            // If you have FirstName + LastName, expose a FullName property
            //   - OR - override ToString() and just set DisplayMember = null
            Userlistbox.SelectedIndex = -1;                      // no pre-selection

            // ───── 3. Wire the handler exactly once ─────
            Userlistbox.SelectedIndexChanged -= Userlistbox_SelectedIndexChanged;
            Userlistbox.SelectedIndexChanged += Userlistbox_SelectedIndexChanged;

        }

        private void Userlistbox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Userlistbox.SelectedItem is not User user)            // nothing selected
            {
                ClearUserFields();
                return;
            }

            // ── Push properties into the inputs ──
            FirstNametxtbox.Text = user.FirstName;
            LastNametxtbox.Text = user.LastName;
            Usernametxtbox.Text = user.UserName;      // or user.Login if that’s your property
            Emailtxtbox.Text = user.Email;
            PhoneNumbertxtbox.Text = user.PhoneNumber;
            Addresstxtbox.Text = user.Address;
            Passwordtxtbox.Text = user.Password;      // only if you really show it

            CantBuycheckbox.Checked = user.CantBuy;
            CantSellcheckbox.Checked = user.CantSell;
        }


        private void ClearUserFields()
        {
            foreach (Control ctl in new Control[]
            {
        FirstNametxtbox, LastNametxtbox, Usernametxtbox,
        Emailtxtbox, PhoneNumbertxtbox, Addresstxtbox, Passwordtxtbox
            })
                ctl.Text = string.Empty;

            CantBuycheckbox.Checked = false;
            CantSellcheckbox.Checked = false;
        }


        private async void Savebtn_Click(object sender, EventArgs e)
        {
            // 0. Make sure someone is highlighted
            if (Userlistbox.SelectedItem is not User user)
            {
                MessageBox.Show("Select a user first.", "Nothing to save",
                                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
                return;
            }

            try
            {
                Savebtn.Enabled = false;
                Cursor.Current = Cursors.WaitCursor;

                // 1. Copy UI → object
                user.FirstName = FirstNametxtbox.Text.Trim();
                user.LastName = LastNametxtbox.Text.Trim();
                user.UserName = Usernametxtbox.Text.Trim();
                user.Email = Emailtxtbox.Text.Trim();
                user.PhoneNumber = PhoneNumbertxtbox.Text.Trim();
                user.Address = Addresstxtbox.Text.Trim();
                user.CantBuy = CantBuycheckbox.Checked;
                user.CantSell = CantSellcheckbox.Checked;

                // 2. Serialize & ship with PUT
                var json = JsonSerializer.Serialize(user,
                                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var resp = await _apiRequester.Put($"api/user/{user.UserId}", user)
                                                             .ConfigureAwait(false);   // see note ⬇

                //if (!resp.IsSuccessStatusCode)
                //{
                //    MessageBox.Show($"Server returned {(int)resp.StatusCode} {resp.ReasonPhrase}",
                //                    "Save failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    return;
                //}

                // 3. Refresh the row’s caption so the ListBox shows new data
                Userlistbox.Refresh();           // simple; or re-data-bind if you prefer
                MessageBox.Show("User saved 👍", "Success",
                                MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to save user:\n{ex.Message}",
                                "Unexpected error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1, MessageBoxOptions.ServiceNotification);
            }
            finally
            {
                Cursor.Current = Cursors.Default;
                Savebtn.Enabled = true;
            }
        }

        
    }
}
