using Import.Resources;
using Newtonsoft.Json;
using StocksAPI.Models;
using System.Net.Http;
using System.Windows;

namespace StockHTML
{
    /// <summary>
    /// interaction logic for login
    /// </summary>
    public partial class Login : Window
    {
        /// <summary>
        /// constructor for the login window
        /// </summary>
        public Login()
        {
            InitializeComponent();
        }

        /// <summary>
        /// event handler for the login button
        /// </summary>
        /// <param name="_sEND">sender</param>
        /// <param name="_EVArgs">event args</param>
        /// <exception cref="Exception">exception</exception>
        private async void ValidateLOGIN(object _sEND, RoutedEventArgs _EVArgs)
        {
            HttpClient cLIENT = new HttpClient();
            string eMAIL = EmailBox.Text;
            string pASS = PasswordBox.Password;

            string aPIURL = $"http://localhost:5001/api/user/{eMAIL}";
            HttpResponseMessage rESP = cLIENT.GetAsync(aPIURL).Result;

            if (!rESP.IsSuccessStatusCode)
                throw new Exception($"{Labels.LoginFailed}: {eMAIL}");

            string _jSON = await rESP.Content.ReadAsStringAsync();
            Check(_jSON, eMAIL, pASS);
        }

        /// <summary>
        /// checks the credentials for the user. if credentials are valid it opens another window
        /// </summary>
        /// <param name="_jSON">json data</param>
        /// <param name="_eMAIL">email</param>
        /// <param name="_pASS">poassword</param>
        public void Check(string _jSON, string _eMAIL, string _pASS)
        {
            User uSER = JsonConvert.DeserializeObject<User>(_jSON);

            if (uSER.Email == _eMAIL && uSER.Password == _pASS)
            {
                Dashboard dASH = new Dashboard(_eMAIL);
                Close();
                dASH.Show();
                dASH.DataGridView.ItemsSource = dASH.DataTable.DefaultView;
            }
        }
    }
}
