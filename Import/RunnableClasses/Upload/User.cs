using Import.Model;
using System.Globalization;

namespace Import.RunnableClasses.Upload
{
    [Upload(3, "Importiert Benutzerdaten und speichert diese in der User Tabelle")]
    public class User : RunnableClassBase
    {
        public static Dictionary<int, DTOUser> UserMap { get; set; }

        /// <summary>
        /// run method of the runnable classes
        /// </summary>
        public override void Run()
        {
            FileReaderBase rESXFiles = GetRESXReader();
           CreateUserMap(rESXFiles.Stocks);
            SQLInitializer sQLInitializer = new SQLInitializer();
            sQLInitializer.InsertInUser(rESXFiles.RESXFile);
        }

        /// <summary>
        /// creates a dictionary of users
        /// </summary>
        /// <param name="_rESX">resx file</param>
        private void CreateUserMap(List<string> _rESX)
        {
            UserMap = new Dictionary<int, DTOUser>();
            int rESXC = 1;

            _rESX.ForEach(S =>
            {
                string[] rESXCSplit = S.Split(new[] { ' ', '\t' });
                UserMap.Add(rESXC, new DTOUser
                {
                    Username = rESXCSplit[0].ToString(), EMail = rESXCSplit[1].ToString(), Password = rESXCSplit[2].ToString(), PasswordHash = rESXCSplit[3].ToString(), Balance = decimal.Parse(rESXCSplit[4], CultureInfo.InvariantCulture)
                });
                rESXC++;});
        }
    }
}
