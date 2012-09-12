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

            // ���������� ���� ��������
            Book bookDetective = ObjectSpace.FindObject<Book>(new BinaryOperator("Name", "���� ���� �� �����"));
            if (bookDetective == null) {
                bookDetective = ObjectSpace.CreateObject<Book>();
                bookDetective.Description = "������ �����";
                bookDetective.Name = "���� ���� �� �����";
                bookDetective.Number = 1;
                bookDetective.Status = BookState.Porvana;
                bookDetective.Save();
            }

            Book bookRoman = ObjectSpace.FindObject<Book>(new BinaryOperator("Name", "�� � ��� ���������"));
            if (bookRoman == null) {
                bookRoman = ObjectSpace.CreateObject<Book>();
                bookRoman.Description = "����� �� ����������";
                bookRoman.Name = "�� � ��� ���������";
                bookRoman.Number = 2;
                bookRoman.Status = BookState.Porvana;
                bookRoman.Save();
            }

            FictionBook bookFiction1 = ObjectSpace.FindObject<FictionBook>(new BinaryOperator("Name", "���������� ������: ��� ��� �����?"));
            if (bookFiction1 == null) {
                bookFiction1 = ObjectSpace.CreateObject<FictionBook>();
                bookFiction1.Description = "�� ���� ������ ����������� �����";
                bookFiction1.Name = "���������� ������: ��� ��� �����?";
                bookFiction1.Number = 3;
                bookFiction1.Date = DateTime.Now;
                bookFiction1.Subject = "� ������������ ��������";
                bookFiction1.Theme = "���� � �����������";
                bookFiction1.Status = BookState.Obgazhena;
                bookFiction1.Save();
            }

            FictionBook bookFiction2 = ObjectSpace.FindObject<FictionBook>(new BinaryOperator("Name", "���������� �������� ����� ���� Homo Sapiens"));
            if (bookFiction2 == null) {
                bookFiction2 = ObjectSpace.CreateObject<FictionBook>();
                bookFiction2.Description = "�������� �����������";
                bookFiction2.Name = "���������� �������� ����� ���� Homo Sapiens";
                bookFiction2.Number = 4;
                bookFiction2.Date = DateTime.Now;
                bookFiction1.Subject = "� ������������ ��������";
                bookFiction1.Theme = "����� � �����";
                bookFiction2.Status = BookState.Prochitana;
                bookFiction2.Save();
            }

            // ���������� ���� �����
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
