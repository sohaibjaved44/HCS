using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace HCS
{
    /// <summary>
    /// Interaction logic for winLogin.xaml
    /// </summary>
    public partial class winLogin : Window
    {

        #region Private Members

        private login dataContext;
        private List< Language> m_language;
        private bool m_canEdit=false;
        #endregion

        #region Public Properties

        public DateTime DATE
        {
            get
            {
                return DateTime.Now;

            }

        }

        public List<Language> LANGUAGE
        {
            get
            {
                return m_language;
            }
            set
            {
                m_language = value;
            }
        }
        
        #endregion
        
        
        
        public winLogin()
        {
            InitializeComponent();

            tbDate.Text = DateTime.Now.ToString("MM/dd/yyyy");
            tbTime.Text = DateTime.Now.ToString("h:mm tt");
        
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {

            dataContext.password = pbPassword.Password;

            if (Validate())
            {

                if (ValidateLogin())
                {
                    string languagecode = ((Language)cmbLanguage.SelectedItem).LANGUAGECDE;
                    MainWindow window = new MainWindow(languagecode,m_canEdit);
                    window.Show();
                    this.Close();


                }
                else
                {
                    tbUserName.Text = "";
                    pbPassword.Password = "";
                    MessageBox.Show("Invalid Username or Password!");

                }


            }

        }

        private void winLogin_Loaded(object sender, RoutedEventArgs e)
        {
            
            dataContext = new login();
            bindLanguages();
            this.grdLogin.DataContext = dataContext;

        }

        private void bindLanguages()
        {
            LANGUAGE = new List<Language>();
            LANGUAGE.Add(new Language() { LANGUAGECDE = "00001", LANGUAGEDSC = "URDU" });
            LANGUAGE.Add(new Language() { LANGUAGECDE = "00002", LANGUAGEDSC = "ENGLISH" });

            this.cmbLanguage.ItemsSource = LANGUAGE;
        }
        private bool Validate()
        {

            if (string.IsNullOrEmpty(dataContext.user_id))
            {
                MessageBox.Show("Please Enter Username!");
                return false;
            }
            else if (string.IsNullOrEmpty(dataContext.password))
            {
                MessageBox.Show("Please Enter Password!");
                return false;
            }
            else if (cmbLanguage.SelectedItem==null)
            {
                MessageBox.Show("Please Select Language");
                return false;
            }

            return true;

        }

        private bool ValidateLogin()
        {
            login login;
            using (var db = new HCSMLEntities1())
            {
                login = db.logins.Where(p => p.user_id.Equals(dataContext.user_id) && p.password.Equals(dataContext.password)).FirstOrDefault();


            }
            if (login == null)
            {
                return false;
            }
            else
            {
                m_canEdit = login.can_edit;

            }
            return true;
        }

    }
}
