REM set ORMFile=%~dp0ImpliedEqualityConstraint\ImpliedEqualityConstraint1.orm
REM set TestFile=%~dp0ImpliedEqualityConstraint\ImpliedEqualityConstraint1.MoreCode.cs
REM call RunTest "EqualityIsImpliedByMandatoryError Scenarios (More code: Favor AccessibleObject.Select and DTE methods instead of clicks and REM keyboard)"

REM set TestFile=%~dp0ImpliedEqualityConstraint\ImpliedEqualityConstraint1.MoreClicks.cs
REM call RunTest "EqualityIsImpliedByMandatoryError Scenarios (More clicks: Favor ClickAccessibleObject and SendKeys instead of REM AccessibleObject.Select and DTE.ExecuteCommand)"

set AutomationSuiteFile=%~dp0TestSuite.xml
echo "TestSuite.xml Configuration set. Running Automation Scenarios"
start /w %LaunchDevenv%.exe /resetaddin %TESTADDIN%.Connect /command %TESTADDIN%.Connect.%TESTADDIN% /RootSuffix %RootSuffix% 