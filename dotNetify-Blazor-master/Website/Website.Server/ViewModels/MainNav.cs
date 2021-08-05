using DotNetify;
using DotNetify.Elements;

namespace Website.Server
{
   public class MainNav : BaseVM
   {
      public const string PATH_BASE = "";

      public MainNav()
      {
         AddProperty("NavMenu", new NavMenu(
            new NavMenuItem[]
            {
               
               new NavRoute("Admin Dashboard", PATH_BASE + "/dashboard"),

            })
         );
      }
   }
}