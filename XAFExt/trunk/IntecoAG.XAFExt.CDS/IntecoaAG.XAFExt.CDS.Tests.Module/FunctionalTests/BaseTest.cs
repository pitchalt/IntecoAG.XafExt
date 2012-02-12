using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Data;
using System.Diagnostics;

using DevExpress.Xpo;
using DevExpress.Xpo.DB;
using DevExpress.Xpo.DB.Helpers;
using DevExpress.Data.Filtering;

using DevExpress.ExpressApp;
using DevExpress.Persistent.Validation;

using NUnit.Framework;

namespace IntecoaAG.XAFExt.CDS.Tests.FunctionalTests
{
    public class BaseTest
    {
        protected RuleSet ruleSet;
        protected RuleSetValidationResult ruleResult;

        [TestFixtureSetUp]
        public virtual void Init() {
            // Метод выполняется один раз до любых тестов
            Common.PrepareDB();

            // Прочистка БД
            if (Common.dataStore != null) ((IDataStoreForTests)Common.dataStore).ClearDatabase();
        }

        [SetUp]
        public virtual void TestSetup() {
            // Метод выполняется перед каждым ("элементарным") тестом.
            Trace.WriteLine("Test started at " + DateTime.Now);
        }

        [TearDown]
        public virtual void TestDispose() {
            // Метод выполняется после каждого ("элементарного") теста.
            Trace.WriteLine("Test finished at " + DateTime.Now);
        }

        [TestFixtureTearDown]
        public virtual void Cleanup() {
            // Метод выполняется после завершения всех тестов.
            Trace.WriteLine("Completed at " + DateTime.Now);
        }

        protected virtual void FillDatabase(Session ssn) {
            testCountry country0 = Prepare_testCountry(ssn, "0");
            testCountry country1 = Prepare_testCountry(ssn, "1");
            testCountry country2 = Prepare_testCountry(ssn, "2");
            testCountry country3 = Prepare_testCountry(ssn, "3");
            testCountry country4 = Prepare_testCountry(ssn, "4");
            testCountry country5 = Prepare_testCountry(ssn, "5");
            testCountry country6 = Prepare_testCountry(ssn, "6");
            testCountry country7 = Prepare_testCountry(ssn, "7");
            testCountry country8 = Prepare_testCountry(ssn, "8");
            testCountry country9 = Prepare_testCountry(ssn, "9");
        }


        #region Заполнение базы

        protected virtual testCountry Prepare_testCountry(Session ssn, string modificator) {
            testCountry country = new testCountry(ssn);
            country.NameShort = "Страна " + modificator;
            country.NameFull = "Полное название страны " + modificator;
            country.Comment = "Комментарий к стране " + modificator;
            return country;
        }

        #endregion

    }
}
