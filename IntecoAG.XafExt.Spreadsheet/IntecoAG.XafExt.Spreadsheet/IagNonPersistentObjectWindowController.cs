using System;
using DevExpress.ExpressApp;
using DevExpress.Persistent.BaseImpl;
using IntecoAG.XpoExt;
// ... 
namespace IntecoAG.XafExt {

    public class IagNonPersistentObjectWindowController : WindowController {
        public IagNonPersistentObjectWindowController()
            : base() {
            //        TargetWindowType = WindowType.Main;
        }
        private IObjectSpace additionalObjectSpace;
        protected override void OnActivated() {
            base.OnActivated();
            Application.ObjectSpaceCreated += Application_ObjectSpaceCreated;
            additionalObjectSpace = Application.CreateObjectSpace(typeof(IagBaseObject));
        }

        protected override void OnDeactivated() {
            base.OnDeactivated();
            Application.ObjectSpaceCreated -= Application_ObjectSpaceCreated;
            if (additionalObjectSpace != null) {
                additionalObjectSpace.Dispose();
                additionalObjectSpace = null;
            }
        }
        private void Application_ObjectSpaceCreated(Object sender, ObjectSpaceCreatedEventArgs e) {
            if (e.ObjectSpace is NonPersistentObjectSpace) {
                ((NonPersistentObjectSpace)e.ObjectSpace).AdditionalObjectSpaces.Add(additionalObjectSpace);
            }
        }
    }

}

