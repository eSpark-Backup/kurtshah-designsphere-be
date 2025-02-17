using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revit.WPFUI.Helpers;
public class TokenHelper
{
    public static string Base64UrlDecode(string base64Url)
    {
        string base64 = base64Url
            .Replace('-', '+') 
            .Replace('_', '/');

        switch (base64.Length % 4)  
        {
            case 2: base64 += "=="; break;
            case 3: base64 += "="; break;
        }

        var bytes = Convert.FromBase64String(base64);
        return Encoding.UTF8.GetString(bytes);
    }

}
