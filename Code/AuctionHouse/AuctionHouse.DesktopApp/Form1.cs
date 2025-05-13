using AuctionHouse.ClassLibrary.Model;
using AuctionHouse.Requester;
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
    }
}
