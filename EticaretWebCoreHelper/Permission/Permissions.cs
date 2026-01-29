using System.Collections.Generic;

namespace EticaretWebCoreHelper
{
    public static class Permissions
    {
        public static List<string> GeneratePermissionsForModule(string module)
        {
            return new List<string>()
        {
            $"Permissions.{module}.Create",
            $"Permissions.{module}.View",
            $"Permissions.{module}.Edit",
            $"Permissions.{module}.Delete",
        };
        }

        public static class Uyeler
        {
            public const string View = "Permissions.Uyeler.View";
            public const string Create = "Permissions.Uyeler.Create";
            public const string Edit = "Permissions.Uyeler.Edit";
            public const string Delete = "Permissions.Uyeler.Delete";

            public static class Uyeler_Uyeler
            {
                public const string View = "Permissions.Uyeler.Uyeler.View";
                public const string Create = "Permissions.Uyeler.Uyeler.Create";
                public const string Edit = "Permissions.Uyeler.Uyeler.Edit";
                public const string Delete = "Permissions.Uyeler.Uyeler.Delete";
            }

            public static class Uyeler_Roller
            {
                public const string View = "Permissions.Uyeler.Roller.View";
                public const string Create = "Permissions.Uyeler.Roller.Create";
                public const string Edit = "Permissions.Uyeler.Roller.Edit";
                public const string Delete = "Permissions.Uyeler.Roller.Delete";
            }
        }

        public static class Sayfalar
        {
            public const string View = "Permissions.Sayfalar.View";
            public const string Create = "Permissions.Sayfalar.Create";
            public const string Edit = "Permissions.Sayfalar.Edit";
            public const string Delete = "Permissions.Sayfalar.Delete";

            public static class Sayfalar_Sayfalar
            {
                public const string View = "Permissions.Sayfalar.Sayfalar.View";
                public const string Create = "Permissions.Sayfalar.Sayfalar.Create";
                public const string Edit = "Permissions.Sayfalar.Sayfalar.Edit";
                public const string Delete = "Permissions.Sayfalar.Sayfalar.Delete";
            }

            public static class Sayfalar_Roller
            {
                public const string View = "Permissions.Sayfalar.Roller.View";
                public const string Create = "Permissions.Sayfalar.Roller.Create";
                public const string Edit = "Permissions.Sayfalar.Roller.Edit";
                public const string Delete = "Permissions.Sayfalar.Roller.Delete";
            }
        }

        public static class Home
        {
            public const string View = "Permissions.Home.View";
            public const string Create = "Permissions.Home.Create";
            public const string Edit = "Permissions.Home.Edit";
            public const string Delete = "Permissions.Home.Delete";

            public static class Home_Home
            {
                public const string View = "Permissions.Home.Home.View";
                public const string Create = "Permissions.Home.Home.Create";
                public const string Edit = "Permissions.Home.Home.Edit";
                public const string Delete = "Permissions.Home.Home.Delete";
            }

        }
    }
}