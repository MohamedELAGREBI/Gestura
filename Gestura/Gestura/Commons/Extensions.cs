using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gestura.Commons
{
    public static class Extensions
    {
        public static string EnumToString(this ImportMethodEnum method)
        {
            switch (method)
            {
                case ImportMethodEnum.UrlWeb:
                    return Constantes.DEFAULT_FROM_WEBURL_DIRECTORY;
                case ImportMethodEnum.LocalStorage:
                    return Constantes.DEFAULT_FROM_LOCALSTORAGE_DIRECTORY;
                case ImportMethodEnum.None:
                    return Constantes.DEFAULT_NO_METHOD_LABEL;
                default:
                    throw new InvalidOperationException("Méthode d'import non valide.");
            }
        }

        public static ImportMethodEnum ToImportMethodEnum(this string method)
        {
            switch (method)
            {
                case Constantes.DEFAULT_FROM_WEBURL_DIRECTORY:
                    return ImportMethodEnum.UrlWeb;
                case Constantes.DEFAULT_FROM_LOCALSTORAGE_DIRECTORY:
                    return ImportMethodEnum.LocalStorage;
                case Constantes.DEFAULT_NO_METHOD_LABEL:
                default:
                    return ImportMethodEnum.None;
            }
        }
    }
}
