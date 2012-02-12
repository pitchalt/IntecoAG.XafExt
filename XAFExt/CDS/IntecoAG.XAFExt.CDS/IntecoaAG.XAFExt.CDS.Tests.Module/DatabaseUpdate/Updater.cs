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

            #region Заполняем базу

            testCountry country1 = ObjectSpace.FindObject<testCountry>(new BinaryOperator("NameShort", "Россия"));
            if (country1 == null) {
                country1 = ObjectSpace.CreateObject<testCountry>();
                country1.NameShort = "Россия";
                country1.NameFull = "Российская Федерация";
                country1.Comment = "Страна, захваченная жульём";
                country1.Save();
            }

            testCountry country2 = ObjectSpace.FindObject<testCountry>(new BinaryOperator("NameShort", "США"));
            if (country2 == null) {
                country2 = ObjectSpace.CreateObject<testCountry>();
                country2.NameShort = "США";
                country2.NameFull = "Соединённые Штаты Америки";
                country2.Comment = "Скоро разъединятся";
                country2.Save();
            }

            testCountry country3 = ObjectSpace.FindObject<testCountry>(new BinaryOperator("NameShort", "Франция"));
            if (country3 == null) {
                country3 = ObjectSpace.CreateObject<testCountry>();
                country3.NameShort = "Франция";
                country3.NameFull = "Французская Республика";
                country3.Comment = "Страна, где любят везде и всегда";
                country3.Save();
            }

            testCountry country4 = ObjectSpace.FindObject<testCountry>(new BinaryOperator("NameShort", "КНР"));
            if (country4 == null) {
                country4 = ObjectSpace.CreateObject<testCountry>();
                country4.NameShort = "КНР";
                country4.NameFull = "Китайская Народная Республика";
                country4.Comment = "Много народа";
                country4.Save();
            }

            objSpace.CommitChanges();

            #endregion
        }
    }
}
