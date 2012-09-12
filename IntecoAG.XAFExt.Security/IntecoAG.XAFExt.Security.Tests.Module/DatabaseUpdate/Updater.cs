using System;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;
//
using IntecoAG.XAFExt.Security.Tests.Module.BusinessObjects;

namespace IntecoAG.XAFExt.Security.Tests.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) {
        }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            // Добавление двух объектов
            Book bookDetective = ObjectSpace.FindObject<Book>(new BinaryOperator("Name", "Убей себя ап стену"));
            if (bookDetective == null) {
                bookDetective = ObjectSpace.CreateObject<Book>();
                bookDetective.Description = "Дряное чтиво";
                bookDetective.Name = "Убей себя ап стену";
                bookDetective.Number = 1;
                bookDetective.Status = BookState.Porvana;
                bookDetective.Save();
            }

            Book bookRoman = ObjectSpace.FindObject<Book>(new BinaryOperator("Name", "Он и его блондинки"));
            if (bookRoman == null) {
                bookRoman = ObjectSpace.CreateObject<Book>();
                bookRoman.Description = "Чтиво не затягивает";
                bookRoman.Name = "Он и его блондинки";
                bookRoman.Number = 2;
                bookRoman.Status = BookState.Porvana;
                bookRoman.Save();
            }

            FictionBook bookFiction1 = ObjectSpace.FindObject<FictionBook>(new BinaryOperator("Name", "Разжижение мозгов: что это такое?"));
            if (bookFiction1 == null) {
                bookFiction1 = ObjectSpace.CreateObject<FictionBook>();
                bookFiction1.Description = "По мере чтения разжижаются мозги";
                bookFiction1.Name = "Разжижение мозгов: что это такое?";
                bookFiction1.Number = 3;
                bookFiction1.Date = DateTime.Now;
                bookFiction1.Subject = "О человеческой глупости";
                bookFiction1.Theme = "Мозг и пищеварение";
                bookFiction1.Status = BookState.Obgazhena;
                bookFiction1.Save();
            }

            FictionBook bookFiction2 = ObjectSpace.FindObject<FictionBook>(new BinaryOperator("Name", "Нарастание глупости самок вида Homo Sapiens"));
            if (bookFiction2 == null) {
                bookFiction2 = ObjectSpace.CreateObject<FictionBook>();
                bookFiction2.Description = "Повышает премудрость";
                bookFiction2.Name = "Нарастание глупости самок вида Homo Sapiens";
                bookFiction2.Number = 4;
                bookFiction2.Date = DateTime.Now;
                bookFiction1.Subject = "О человеческой глупости";
                bookFiction1.Theme = "Самцы и самки";
                bookFiction2.Status = BookState.Prochitana;
                bookFiction2.Save();
            }

            // Добавление двух ролей
            CreateAdministratorRole();
            CreateReaderRole();

            ObjectSpace.CommitChanges();
        }

        private SecurityRole CreateAdministratorRole() {
            SecurityRole administratorRole = ObjectSpace.FindObject<SecurityRole>(
                new BinaryOperator("Name", SecurityStrategyComplex.AdministratorRoleName));
            if (administratorRole == null) {
                administratorRole = ObjectSpace.CreateObject<SecurityRole>();
                administratorRole.Name = SecurityStrategyComplex.AdministratorRoleName;
                administratorRole.CanEditModel = true;
                administratorRole.BeginUpdate();
                administratorRole.Permissions.GrantRecursive(typeof(object), SecurityOperations.FullAccess);
                administratorRole.EndUpdate();
                administratorRole.Save();
            }

            SecurityUser user = SecuritySystem.CurrentUser as SecurityUser;
            administratorRole.Users.Add(user);

            return administratorRole;
        }

        private SecurityRole CreateReaderRole() {
            SecurityRole exporterRole = ObjectSpace.FindObject<SecurityRole>(new BinaryOperator("Name", "Reader"));
            if (exporterRole == null) {
                exporterRole = ObjectSpace.CreateObject<SecurityRole>();
                exporterRole.Name = "Reader";
                exporterRole.Save();
            }
            return exporterRole;
        }



    }
}
