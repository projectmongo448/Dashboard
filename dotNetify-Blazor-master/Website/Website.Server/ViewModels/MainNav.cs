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

               new NavGroup
               {
                  Label = "ALLTrainMescla5D",
                  Routes = new NavRoute[]
                  {
                       new NavRoute("Overview", PATH_BASE + "/"),
                       new NavRoute("Dashboard", PATH_BASE + "/dashboard"),
                  },
                  IsExpanded = false
               },

            })
         );
      }
   }
}