using System;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.BaseImpl;

namespace IntecoAG.XAFExt.StateMachine {

    public class StateMachinePermissionRequestProcessor : PermissionRequestProcessorBase<StateMachineTransitionPermissionRequest> {

        public override bool IsGranted(StateMachineTransitionPermissionRequest permissionRequest) {

            if (permission is StateMachineTransitionPermission) {
                return permissionRequest.Modifier == ((StateMachineTransitionPermission)permission).Modifier &&
                       permissionRequest.StateCaption == ((StateMachineTransitionPermission)permission).StateCaption &&
                       permissionRequest.StateMachineName == ((StateMachineTransitionPermission)permission).StateMachineName;
            }
            return false;
        }

        protected StateMachineTransitionPermission permission  {
            get {
                throw new NotImplementedException();
            }
        }
        //protected override bool IsRequestFit(StateMachineTransitionPermissionRequest permissionRequest, OperationPermissionBase permission, IRequestSecurityStrategy securityInstance) {
        //    if (permission is StateMachineTransitionPermission) {
        //        return permissionRequest.Modifier == ((StateMachineTransitionPermission)permission).Modifier &&
        //               permissionRequest.StateCaption == ((StateMachineTransitionPermission)permission).StateCaption &&
        //               permissionRequest.StateMachineName == ((StateMachineTransitionPermission)permission).StateMachineName;
        //    }
        //    return false;
        //}


    }
}