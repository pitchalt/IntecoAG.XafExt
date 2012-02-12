using System;

using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Updating;
using DevExpress.Xpo;
using DevExpress.Data.Filtering;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Security;

namespace IntecoaAG.XAFExt.CDS.Tests.Module.DatabaseUpdate {
    public class Updater : ModuleUpdater {
        public Updater(IObjectSpace objectSpace, Version currentDBVersion) : base(objectSpace, currentDBVersion) { }
        public override void UpdateDatabaseAfterUpdateSchema() {
            base.UpdateDatabaseAfterUpdateSchema();

            IObjectSpace objSpace = this.ObjectSpace;

            #region ��������� ����

            testCountry country1 = ObjectSpace.FindObject<testCountry>(new BinaryOperator("NameShort", "������"));
            if (country1 == null) {
                country1 = ObjectSpace.CreateObject<testCountry>();
                country1.NameShort = "������";
                country1.NameFull = "���������� ���������";
                country1.Comment = "������, ����������� ������";
                country1.Save();
            }

            testCountry country2 = ObjectSpace.FindObject<testCountry>(new BinaryOperator("NameShort", "���"));
            if (country2 == null) {
                country2 = ObjectSpace.CreateObject<testCountry>();
                country2.NameShort = "���";
                country2.NameFull = "���������� ����� �������";
                country2.Comment = "����� ������������";
                country2.Save();
            }

            testCountry country3 = ObjectSpace.FindObject<testCountry>(new BinaryOperator("NameShort", "�������"));
            if (country3 == null) {
                country3 = ObjectSpace.CreateObject<testCountry>();
                country3.NameShort = "�������";
                country3.NameFull = "����������� ����������";
                country3.Comment = "������, ��� ����� ����� � ������";
                country3.Save();
            }

            testCountry country4 = ObjectSpace.FindObject<testCountry>(new BinaryOperator("NameShort", "���"));
            if (country4 == null) {
                country4 = ObjectSpace.CreateObject<testCountry>();
                country4.NameShort = "���";
                country4.NameFull = "��������� �������� ����������";
                country4.Comment = "����� ������";
                country4.Save();
            }

            objSpace.CommitChanges();

            #endregion
        }
    }
}
