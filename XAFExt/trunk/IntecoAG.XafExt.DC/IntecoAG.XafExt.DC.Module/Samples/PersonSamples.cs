using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Validation;

namespace IntecoAG.XafExt.DC.Module.Samples {

    [DomainComponent, NavigationItem, ImageName("BO_Person")]
    public interface IPerson {
        [RuleRequiredField]
        String LastName { get; set; }
        String FirstName { get; set; }
        [NonPersistentDc]
        String FullName { get; }
        DateTime Birthday { get; set; }
        Boolean Married { get; set; }
        String SpouseName { get; set; }
    }

    [DomainLogic(typeof(IPerson))]
    public class PersonLogic {
        public static string Get_FullName(IPerson person) {
            return string.Format("{0} {1}", person.FirstName, person.LastName);
        }
        public void AfterChange_Married(IPerson person) {
            if (!person.Married) person.SpouseName = "";
        }
    }

    [DomainComponent]
    public interface IOrganization {
        string Name { get; set; }
        IList<IPerson> Staff { get; }
    }

    [DomainComponent]
    public interface IAccount {
        [FieldSize(8)]
        string Login { get; set; }
        [FieldSize(8)]
        string Password { get; set; }
        [BackReferenceProperty("AccountOne")]
        IList<IContact> ContactA { get; }
        [BackReferenceProperty("AccountTwo")]
        IList<IContact> ContactB { get; }
        IList<IContact> ContactC { get; }
    }
    [DomainLogic(typeof(IAccount))]
    public class AccountLogic {
        public static string GeneratePassword() {
            byte[] randomBytes = new byte[5];
            new RNGCryptoServiceProvider().GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }
        public static void AfterConstruction(IAccount account) {
            account.Password = GeneratePassword();
        }
    }

    [DomainComponent, NavigationItem, ImageName("BO_Contact")]
    public interface ICustomer : IOrganization, IAccount {
    }

    [DomainComponent]
    public interface IContact {
        string Name { get; set; }
        IAccount AccountOne { get; set; }
        IAccount AccountTwo { get; set; }
        IAccount AccountThree { get; set; }
    }

}
