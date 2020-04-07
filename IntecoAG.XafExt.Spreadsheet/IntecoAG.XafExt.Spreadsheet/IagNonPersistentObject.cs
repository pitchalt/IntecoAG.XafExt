using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Validation;

namespace IntecoAG.XafExt {
    [DomainComponent]
    //[DefaultClassOptions]
    //[ImageName("BO_Unknown")]
    //[DefaultProperty("SampleProperty")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public abstract class IagNonPersistentObject : IXafEntityObject, IObjectSpaceLink, INotifyPropertyChanged {

        [Browsable(false)]  // Hide the entity identifier from UI.
        public int ID { get; set; }

        private IObjectSpace _ObjectSpace;
        protected IObjectSpace ObjectSpace {
            get { return _ObjectSpace; }
        }
        IObjectSpace IObjectSpaceLink.ObjectSpace {
            get { return _ObjectSpace; }
            set { _ObjectSpace = value; }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(String property_name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));
        }

        protected virtual void OnCreated() { }
        void IXafEntityObject.OnCreated() {
            OnCreated();
            // Place the entity initialization code here.
            // You can initialize reference properties using Object Space methods; e.g.:
            // this.Address = objectSpace.CreateObject<Address>();
        }
        protected virtual void OnLoaded() { }
        void IXafEntityObject.OnLoaded() {
            OnLoaded();
            // Place the code that is executed each time the entity is loaded here.
        }
        protected virtual void OnSaving() { }
        void IXafEntityObject.OnSaving() {
            OnSaving();
            // Place the code that is executed each time the entity is saved here.
        }

        protected IagNonPersistentObject() {
        }
        // Add this property as the key member in the CustomizeTypesInfo event

        //private string sampleProperty;
        //[XafDisplayName("My display name"), ToolTip("My hint message")]
        //[ModelDefault("EditMask", "(000)-00"), VisibleInListView(false)]
        //[RuleRequiredField(DefaultContexts.Save)]
        //public string SampleProperty
        //{
        //    get { return sampleProperty; }
        //    set
        //    {
        //        if (sampleProperty != value)
        //        {
        //            sampleProperty = value;
        //            OnPropertyChanged("SampleProperty");
        //        }
        //    }
        //}

        //[Action(Caption = "My UI Action", ConfirmationMessage = "Are you sure?", ImageName = "Attention", AutoCommit = true)]
        //public void ActionMethod() {
        //    // Trigger custom business logic for the current record in the UI (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112619.aspx).
        //    this.SampleProperty = "Paid";
        //}

        //#region IXafEntityObject members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIXafEntityObjecttopic.aspx)
        //#endregion

        //#region IObjectSpaceLink members (see https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppIObjectSpaceLinktopic.aspx)
        //// Use the Object Space to access other entities from IXafEntityObject methods (see https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113707.aspx).
        //#endregion

        //#region INotifyPropertyChanged members (see http://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged(v=vs.110).aspx)
        //#endregion
    }
}