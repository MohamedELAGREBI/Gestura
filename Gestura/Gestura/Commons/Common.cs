namespace Gestura.Commons
{
    public static class Common
    {
        public static List<string> GetImportMethodList()
        {
            return new List<string>
            {
                ImportMethodEnum.None.EnumToString(),
                ImportMethodEnum.LocalStorage.EnumToString(),
                ImportMethodEnum.UrlWeb.EnumToString()
            };
        }
    }
}
