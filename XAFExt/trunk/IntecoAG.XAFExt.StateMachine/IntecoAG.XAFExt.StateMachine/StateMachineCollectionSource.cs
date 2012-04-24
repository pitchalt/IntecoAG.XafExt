using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
//
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.StateMachine;
using DevExpress.XtraEditors;

namespace IntecoAG.XAFExt.StateMachine {

    public class StateMachineCollectionSource  : CollectionSourceBase //, IQueryDataSource
    {
        private ITypeInfo objectTypeInfoCore;

        public override bool? IsObjectFitForCollection(object obj) {
            return false;
        }

        protected override void ApplyCriteriaCore(CriteriaOperator criteria) { }

        public override ITypeInfo ObjectTypeInfo {
            get { return objectTypeInfoCore; }
        }


        private IObjectSpace _objectSpace;
        private IStateMachineRepository _repository;
        private Type _type;

        public StateMachineCollectionSource(IObjectSpace objectSpace, IStateMachineRepository repository, Type type)
            : base(objectSpace) {
            _objectSpace = objectSpace;
            _repository = repository;
            _type = type;
            objectTypeInfoCore = XafTypesInfo.Instance.FindTypeInfo(typeof(IStateMachine));
        }

        protected override object CreateCollection() {
            BindingList<IStateMachine> result = new BindingList<IStateMachine>();
            foreach (var item in _repository.GetStateMachines(_objectSpace, _type)) {
                result.Add(item);
            }
            return result;
        }

        //public IQueryable Query {
        //    get { return queryCore; }
        //    set { queryCore = value; }
        //}

        //public virtual IQueryable<T> GetQuery() {
        //    return null;
        //}

    }
}