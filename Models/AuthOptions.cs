using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace scrubsAPI
{
    public class AuthOptions
    {
        public const string ISSUER = "Hits_tsu_api_clean_architecture_best_practice";
        public const string AUDIENCE = "Vl_sof_eng_dependeny_inversion_principal";
        const string KEY = "JoJo'sBizarreAdventureStardustCrusaidersS2Ep11Timing16:51"; 
        public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
    }
}
