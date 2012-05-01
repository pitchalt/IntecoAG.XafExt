;#DropDB IntecoAG.XAFExt.StateMachine.TestsEasyTest

#Application IntecoAG.XAFExt.StateMachine.Tests.Win

*Action Navigation(User)

*ProcessRecord
 User Name = [DomainUserName]
 
*FillForm
 User Name = Test User
 
*Action Cancel

*HandleDialog
 Respond = Yes

*CheckFieldValues
 User Name = [DomainUserName]

