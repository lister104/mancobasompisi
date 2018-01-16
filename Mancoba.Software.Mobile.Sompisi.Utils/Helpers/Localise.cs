using System;
using System.Globalization;
using System.Resources;

namespace Mancoba.Sompisi.Utils.Helpers
{
    public static class Localise
    {
        private const string STRINGS_ROOT = "Mancoba.Sompisi.Utils.AppResources";

        /// <summary>
        /// Gets the string.
        /// </summary>
        /// <param name="sID">The s identifier.</param>
        /// <returns></returns>
        public static string GetString(string sID)
        {
            string sLocale = CultureInfo.CurrentCulture.Name;

            sLocale = sLocale.Replace("-", "_");
            string sResource = STRINGS_ROOT + "_" + sLocale;
            Type type = Type.GetType(sResource);
            if (type == null)
            {
                sLocale = CultureInfo.CurrentCulture.Parent.Name;

                sLocale = sLocale.Replace("-", "_");
                sResource = STRINGS_ROOT + "_" + sLocale;
                type = Type.GetType(sResource);
            }
            if (type == null)
            {
                sResource = STRINGS_ROOT;
                type = Type.GetType(sResource);
                if (type == null)
                {
                    System.Diagnostics.Debug.WriteLine("No strings resource file when looking for " + sID + " in " + sLocale);
                    return null; // This shouldn't ever happen in theory
                }
            }

            ResourceManager resman = new ResourceManager(type);

            return resman.GetString(sID);

        }
    }
}

