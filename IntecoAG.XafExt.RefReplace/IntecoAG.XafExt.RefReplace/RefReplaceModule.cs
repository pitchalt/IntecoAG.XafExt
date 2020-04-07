using System;
using System.Text;
using System.Linq;
using DevExpress.ExpressApp;
using System.ComponentModel;
using DevExpress.ExpressApp.DC;
using System.Collections.Generic;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Updating;
using DevExpress.ExpressApp.Model.Core;
using DevExpress.ExpressApp.Model.DomainLogics;
using DevExpress.ExpressApp.Model.NodeGenerators;
using DevExpress.ExpressApp.Xpo;

namespace IntecoAG.XafExt.RefReplace {
    // For more typical usage scenarios, be sure to check out https://documentation.devexpress.com/eXpressAppFramework/clsDevExpressExpressAppModuleBasetopic.aspx.
    public sealed partial class RefReplaceModule : ModuleBase {
        public RefReplaceModule() {
            InitializeComponent();
			BaseObject.OidInitializationMode = OidInitializationMode.AfterConstruction;
            _ForbiddenTypes = new Dictionary<ITypeInfo, List<ITypeInfo>>();
            _ForbiddenMembers = new Dictionary<ITypeInfo, List<IMemberInfo>>();
        }

        private readonly Dictionary<ITypeInfo, List<ITypeInfo>> _ForbiddenTypes;
        public IReadOnlyDictionary<ITypeInfo, IEnumerable<ITypeInfo>> ForbiddenTypes {
            get { return (IReadOnlyDictionary<ITypeInfo, IEnumerable<ITypeInfo>>)_ForbiddenTypes; }
        }
        public void ForbiddenTypesAdd(ITypeInfo typeInfo, ITypeInfo forbiddenTypeInfo) {
            if (!_ForbiddenTypes.TryGetValue(typeInfo, out List<ITypeInfo> forbidden_list)) {
                _ForbiddenTypes[typeInfo] = new List<ITypeInfo>(16) { forbiddenTypeInfo };
                return;
            }
            forbidden_list.Add(forbiddenTypeInfo);
        }
        public IEnumerable<ITypeInfo> ForbiddenTypesGet(ITypeInfo typeInfo) {
            if (_ForbiddenTypes.TryGetValue(typeInfo, out List<ITypeInfo> forbidden_list))
                return forbidden_list;
            else
                return new List<ITypeInfo>(0);
        }

        private readonly Dictionary<ITypeInfo, List<IMemberInfo>> _ForbiddenMembers;
        public IReadOnlyDictionary<ITypeInfo, IEnumerable<IMemberInfo>> ForbiddenMembers {
            get { return (IReadOnlyDictionary<ITypeInfo, IEnumerable<IMemberInfo>>)_ForbiddenMembers; }
        }
        public void ForbiddenMembersAdd(ITypeInfo typeInfo, IMemberInfo forbiddenMemberInfo) {
            if (!_ForbiddenMembers.TryGetValue(typeInfo, out List<IMemberInfo> forbidden_list)) {
                _ForbiddenMembers[typeInfo] = new List<IMemberInfo>(16) { forbiddenMemberInfo };
                return;
            }
            forbidden_list.Add(forbiddenMemberInfo);
        }
        public IEnumerable<IMemberInfo> ForbiddenMembersGet(ITypeInfo typeInfo) {
            if (_ForbiddenMembers.TryGetValue(typeInfo, out List<IMemberInfo> forbidden_list))
                return forbidden_list;
            else
                return new List<IMemberInfo>(0);
        }
        public override IEnumerable<ModuleUpdater> GetModuleUpdaters(IObjectSpace objectSpace, Version versionFromDB) {
            ModuleUpdater updater = new DatabaseUpdate.Updater(objectSpace, versionFromDB);
            return new ModuleUpdater[] { updater };
        }
        public override void Setup(XafApplication application) {
            base.Setup(application);
            // Manage various aspects of the application UI and behavior at the module level.
        }
        public override void CustomizeTypesInfo(ITypesInfo typesInfo) {
            base.CustomizeTypesInfo(typesInfo);
            CalculatedPersistentAliasHelper.CustomizeTypesInfo(typesInfo);
        }
    }
}
