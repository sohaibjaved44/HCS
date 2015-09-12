namespace HCS
{
    public class Language
    {
        #region PrivateMembers

        private string language_cde = null;
        private string language_dsc = null;
        #endregion
        #region Public Properties
        public string LANGUAGECDE { get { return language_cde; } set{language_cde=value;} }
        public string LANGUAGEDSC { get{return language_dsc;} set{language_dsc=value;} }

        #endregion
    
        public Language()
        {

            language_cde = string.Empty;
            language_dsc = string.Empty;
        }
    }
}
