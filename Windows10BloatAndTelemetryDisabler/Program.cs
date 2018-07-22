﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //powershell -command "& {Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force}" is essential for running some of the powershell commands
            String[] removeScript =
                {
                    "@rem *** Disable Some Service found at: https://www.hwinfo.com/misc/RemoveW10Bloat.bat.txt ***",
                    "powershell -command \"& {Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force}\"",
                    "sc stop DiagTrack",
                    "sc stop diagnosticshub.standardcollector.service",
                    "sc stop dmwappushservice",
                    "sc stop WMPNetworkSvc",
                    "sc stop WSearch",
                    "sc config DiagTrack start = disabled",
                    "sc config diagnosticshub.standardcollector.service start = disabled",
                    "sc config dmwappushservice start = disabled",
                    "REM sc config RemoteRegistry start = disabled",
                    "REM sc config TrkWks start = disabled",
                    "sc config WMPNetworkSvc start = disabled",
                    "sc config WSearch start = disabled",
                    "REM sc config SysMain start = disabled",
                    "REM*** SCHEDULED TASKS tweaks ***",
                    "REM schtasks / Change / TN \"Microsoft\\Windows\\AppID\\SmartScreenSpecific\" /Disable",
                    "schtasks / Change / TN \"Microsoft\\Windows\\Application Experience\\Microsoft Compatibility Appraiser\" /Disable",
                    "schtasks / Change / TN \"Microsoft\\Windows\\Application Experience\\ProgramDataUpdater\" /Disable",
                    "schtasks / Change / TN \"Microsoft\\Windows\\Application Experience\\StartupAppTask\" /Disable",
                    "schtasks / Change / TN \"Microsoft\\Windows\\Customer Experience Improvement Program\\Consolidator\" /Disable",
                    "schtasks / Change / TN \"Microsoft\\Windows\\Customer Experience Improvement Program\\KernelCeipTask\" /Disable",
                    "schtasks / Change / TN \"Microsoft\\Windows\\Customer Experience Improvement Program\\UsbCeip\" /Disable",
                    "schtasks / Change / TN \"Microsoft\\Windows\\Customer Experience Improvement Program\\Uploader\" /Disable",
                    "schtasks / Change / TN \"Microsoft\\Windows\\Shell\\FamilySafetyUpload\" /Disable",
                    "schtasks / Change / TN \"Microsoft\\Office\\OfficeTelemetryAgentLogOn\" /Disable",
                    "schtasks / Change / TN \"Microsoft\\Office\\OfficeTelemetryAgentFallBack\" /Disable",
                    "schtasks / Change / TN \"Microsoft\\Office\\Office 15 Subscription Heartbeat\" /Disable",
                    "REM schtasks / Change / TN \"Microsoft\\Windows\\Autochk\\Proxy\" /Disable",
                    "REM schtasks / Change / TN \"Microsoft\\Windows\\CloudExperienceHost\\CreateObjectTask\" /Disable",
                    "REM schtasks / Change / TN \"Microsoft\\Windows\\DiskDiagnostic\\Microsoft-Windows-DiskDiagnosticDataCollector\" /Disable",
                    "REM schtasks / Change / TN \"Microsoft\\Windows\\DiskFootprint\\Diagnostics\" /Disable *** Not sure if should be disabled, maybe related to S.M.A.R.T.",
                    "REM schtasks / Change / TN \"Microsoft\\Windows\\FileHistory\\File History (maintenance mode)\" /Disable",
                    "REM schtasks / Change / TN \"Microsoft\\Windows\\Maintenance\\WinSAT\" /Disable",
                    "REM schtasks / Change / TN \"Microsoft\\Windows\\NetTrace\\GatherNetworkInfo\" /Disable",
                    "REM schtasks / Change / TN \"Microsoft\\Windows\\PI\\Sqm-Tasks\" /Disable",
                    "REM The stubborn task Microsoft\\Windows\\SettingSync\\BackgroundUploadTask can be Disabled using a simple bit change. I use a REG file for that(attached to this post).",
                    "REM schtasks / Change / TN \"Microsoft\\Windows\\Time Synchronization\\ForceSynchronizeTime\" /Disable",
                    "REM schtasks / Change / TN \"Microsoft\\Windows\\Time Synchronization\\SynchronizeTime\" /Disable",
                    "REM schtasks / Change / TN \"Microsoft\\Windows\\Windows Error Reporting\\QueueReporting\" /Disable",
                    "REM schtasks / Change / TN \"Microsoft\\Windows\\WindowsUpdate\\Automatic App Update\" /Disable",
                    "REM * **Remove Cortana * **",
                    "REM Currently MS doesn't allow to uninstall Cortana using the above step claiming it's a required OS component(hah!)",
                    "REM We will have to rename the Cortana App folder(add \".bak\" to its name), but this can be done only if Cortana is not running.",
                    "REM The issue is that when Cortana process(SearchUI) is killed, it respawns very quickly",
                    "REM So the following code needs to be quick(and it is) so we can manage to rename the folder",
                    "REM",
                    "REM Disabling Cortana this way on Version 1703(RS2) will render all items in the Start Menu unavailable.",
                    "REM So this is commented out for now until a better solution is found.",
                    "REM taskkill / F / IM SearchUI.exe",
                    "REM move \"%windir%\\SystemApps\\Microsoft.Windows.Cortana_cw5n1h2txyewy\" \"%windir%\\SystemApps\\Microsoft.Windows.Cortana_cw5n1h2txyewy.bak\"",
                    "@rem * **Remove Telemetry & Data Collection * **",
                    "reg add \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Device Metadata\" /v PreventDeviceMetadataFromNetwork /t REG_DWORD /d 1 /f",
                    "reg add \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Policies\\DataCollection\" /v \"AllowTelemetry\" /t REG_DWORD /d 0 /f",
                    "reg add \"HKLM\\SOFTWARE\\Policies\\Microsoft\\MRT\" /v DontOfferThroughWUAU /t REG_DWORD /d 1 /f",
                    "reg add \"HKLM\\SOFTWARE\\Policies\\Microsoft\\SQMClient\\Windows\" /v \"CEIPEnable\" /t REG_DWORD /d 0 /f",
                    "reg add \"HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat\" /v \"AITEnable\" /t REG_DWORD /d 0 /f",
                    "reg add \"HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\AppCompat\" /v \"DisableUAR\" /t REG_DWORD /d 1 /f",
                    "reg add \"HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\DataCollection\" /v \"AllowTelemetry\" /t REG_DWORD /d 0 /f",
                    "reg add \"HKLM\\SYSTEM\\CurrentControlSet\\Control\\WMI\\AutoLogger\\AutoLogger-Diagtrack-Listener\" /v \"Start\" /t REG_DWORD /d 0 /f",
                    "reg add \"HKLM\\SYSTEM\\CurrentControlSet\\Control\\WMI\\AutoLogger\\SQMLogger\" /v \"Start\" /t REG_DWORD /d 0 /f",
                    "@REM Settings->Privacy->General->Let apps use my advertising ID...",
                    "reg add \"HKCU\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\AdvertisingInfo\" /v Enabled /t REG_DWORD /d 0 /f",
                    "REM - SmartScreen Filter for Store Apps: Disable",
                    "reg add \"HKCU\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\AppHost\" /v EnableWebContentEvaluation /t REG_DWORD /d 0 /f",
                    "REM - Let websites provide locally...",
                    "reg add \"HKCU\\Control Panel\\International\\User Profile\" /v HttpAcceptLanguageOptOut /t REG_DWORD /d 1 /f",
                    "@REM WiFi Sense: HotSpot Sharing: Disable",
                    "reg add \"HKLM\\Software\\Microsoft\\PolicyManager\\default\\WiFi\\AllowWiFiHotSpotReporting\" /v value /t REG_DWORD /d 0 /f",
                    "@REM WiFi Sense: Shared HotSpot Auto - Connect: Disable",
                    "reg add \"HKLM\\Software\\Microsoft\\PolicyManager\\default\\WiFi\\AllowAutoConnectToWiFiSenseHotspots\" /v value /t REG_DWORD /d 0 /f",
                    "@REM Change Windows Updates to \"Notify to schedule restart\"",
                    "reg add \"HKLM\\SOFTWARE\\Microsoft\\WindowsUpdate\\UX\\Settings\" /v UxOption /t REG_DWORD /d 1 /f",
                    "@REM Disable P2P Update downlods outside of local network",
                    "reg add \"HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\DeliveryOptimization\\Config\" /v DODownloadMode /t REG_DWORD /d 0 /f",
                    "@REM * **Disable Cortana & Telemetry * **",
                    "reg add \"HKLM\\SOFTWARE\\Policies\\Microsoft\\Windows\\Windows Search\" /v \"AllowCortana\" /t REG_DWORD /d 0",
                    "REM * **Hide the search box from taskbar.You can still search by pressing the Win key and start typing what you're looking for ***",
                    "REM 0 = hide completely, 1 = show only icon, 2 = show long search box",
                    "reg add \"HKCU\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Search\" /v \"SearchboxTaskbarMode\" /t REG_DWORD /d 0 /f",
                    "REM * **Disable MRU lists(jump lists) of XAML apps in Start Menu * **",
                    "reg add \"HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced\" /v \"Start_TrackDocs\" /t REG_DWORD /d 0 /f",
                    "REM * **Set Windows Explorer to start on This PC instead of Quick Access * **",
                    "REM 1 = This PC, 2 = Quick access",
                    "REM reg add \"HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced\" /v \"LaunchTo\" /t REG_DWORD /d 1 /f",
                    "REM * **Disable Suggestions in the Start Menu * **",
                    "reg add \"HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\ContentDeliveryManager\" /v \"SystemPaneSuggestionsEnabled\" /t REG_DWORD /d 0 /f ",
                    "@rem Remove Apps",
                    "PowerShell -Command \"Get-AppxPackage *3DBuilder* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *Cortana* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *Getstarted* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *WindowsAlarms* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *WindowsCamera* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *bing* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *MicrosoftOfficeHub* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *OneNote* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *people* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *WindowsPhone* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *photos* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *SkypeApp* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *solit* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *WindowsSoundRecorder* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *xbox* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *windowscommunicationsapps* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *zune* | Remove-AppxPackage\"",
                    "REM PowerShell -Command \"Get-AppxPackage *WindowsCalculator* | Remove-AppxPackage\"",
                    "REM PowerShell -Command \"Get-AppxPackage *WindowsMaps* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *Sway* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *CommsPhone* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *ConnectivityStore* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *Microsoft.Messaging* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *ContentDeliveryManager* | Remove-AppxPackage\"",
                    "PowerShell -Command \"Get-AppxPackage *Microsoft.WindowsStore* | Remove-AppxPackage\"",
                    "@rem NOW JUST SOME TWEAKS",
                    "REM * **Show hidden files in Explorer * **",
                    "reg add \"HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced\" /v \"Hidden\" /t REG_DWORD /d 1 /f",
                    "REM * **Show super hidden system files in Explorer * **",
                    "reg add \"HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced\" /v \"ShowSuperHidden\" /t REG_DWORD /d 1 /f",
                    "REM * **Show file extensions in Explorer * **",
                    "reg add \"HKEY_CURRENT_USER\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Explorer\\Advanced\" /v \"HideFileExt\" /t  REG_DWORD /d 0 /f",
                    "REM * **Uninstall OneDrive * **",
                    "start / wait \"\" \"%SYSTEMROOT%\\SYSWOW64\\ONEDRIVESETUP.EXE\" /UNINSTALL",
                    "rd C:\\OneDriveTemp / Q / S > NUL 2 > &1",
                    "rd \"%USERPROFILE%\\OneDrive\" /Q /S >NUL 2>&1",
                    "rd \"%LOCALAPPDATA%\\Microsoft\\OneDrive\" /Q /S >NUL 2>&1",
                    "rd \"%PROGRAMDATA%\\Microsoft OneDrive\" /Q /S >NUL 2>&1",
                    "reg add \"HKEY_CLASSES_ROOT\\CLSID\\{018D5C66-4533-4307-9B53-224DE2ED1FE6}\\ShellFolder\" /f /v Attributes /t REG_DWORD /d 0 >NUL 2>&1",
                    "reg add \"HKEY_CLASSES_ROOT\\Wow6432Node\\CLSID\\{018D5C66-4533-4307-9B53-224DE2ED1FE6}\\ShellFolder\" /f /v Attributes /t REG_DWORD /d 0 >NUL 2>&1",
                    "echo OneDrive has been removed.Windows Explorer needs to be restarted.",
                    "pause",
                    "start / wait TASKKILL / F / IM explorer.exe",
                    "start explorer.exe",
};
            String[] restoreScript =
                {

                    @"@rem *** Enable Services Back found at: https://www.hwinfo.com/misc/PutBackW10Bloat.bat.txt ***",
                    "powershell -command \"& {Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force}\"",
                    @"sc config DiagTrack start = auto",
                    @"sc config diagnosticshub.standardcollector.service start = auto",
                    @"sc config dmwappushservice start = auto",
                    @"REM sc config RemoteRegistry start = auto",
                    @"REM sc config TrkWks start = auto",
                    @"sc config WMPNetworkSvc start = auto",
                    @"sc config WSearch start = auto",
                    @"REM sc config SysMain start = auto",
                    @"sc start DiagTrack",
                    @"sc start diagnosticshub.standardcollector.service",
                    @"sc start dmwappushservice",
                    @"sc start WMPNetworkSvc",
                    @"sc start WSearch",
                    @"REM*** SCHEDULED TASKS tweaks ***",
                    @"REM schtasks / Change / TN ""Microsoft\Windows\AppID\SmartScreenSpecific"" / Enable",
                    @"schtasks / Change / TN ""Microsoft\Windows\Application Experience\Microsoft Compatibility Appraiser"" / Enable",
                    @"schtasks / Change / TN ""Microsoft\Windows\Application Experience\ProgramDataUpdater"" / Enable",
                    @"schtasks / Change / TN ""Microsoft\Windows\Application Experience\StartupAppTask"" / Enable",
                    @"schtasks / Change / TN ""Microsoft\Windows\Customer Experience Improvement Program\Consolidator"" / Enable",
                    @"schtasks / Change / TN ""Microsoft\Windows\Customer Experience Improvement Program\KernelCeipTask"" / Enable",
                    @"schtasks / Change / TN ""Microsoft\Windows\Customer Experience Improvement Program\UsbCeip"" / Enable",
                    @"schtasks / Change / TN ""Microsoft\Windows\Customer Experience Improvement Program\Uploader"" / Enable",
                    @"schtasks / Change / TN ""Microsoft\Windows\Shell\FamilySafetyUpload"" / Enable",
                    @"schtasks / Change / TN ""Microsoft\Office\OfficeTelemetryAgentLogOn"" / Enable",
                    @"schtasks / Change / TN ""Microsoft\Office\OfficeTelemetryAgentFallBack"" / Enable",
                    @"schtasks / Change / TN ""Microsoft\Office\Office 15 Subscription Heartbeat"" / Enable",
                    @"REM schtasks / Change / TN ""Microsoft\Windows\Autochk\Proxy"" / Enable",
                    @"REM schtasks / Change / TN ""Microsoft\Windows\CloudExperienceHost\CreateObjectTask"" / Enable",
                    @"REM schtasks / Change / TN ""Microsoft\Windows\DiskDiagnostic\Microsoft-Windows-DiskDiagnosticDataCollector"" / Enable",
                    @"REM schtasks / Change / TN ""Microsoft\Windows\DiskFootprint\Diagnostics"" / Enable * **Not sure if should be disabled, maybe related to S.M.A.R.T.",
                    @"REM schtasks / Change / TN ""Microsoft\Windows\FileHistory\File History (maintenance mode)"" / Enable",
                    @"REM schtasks / Change / TN ""Microsoft\Windows\Maintenance\WinSAT"" / Enable",
                    @"REM schtasks / Change / TN ""Microsoft\Windows\NetTrace\GatherNetworkInfo"" / Enable",
                    @"REM schtasks / Change / TN ""Microsoft\Windows\PI\Sqm-Tasks"" / Enable",
                    @"REM The stubborn task Microsoft\Windows\SettingSync\BackgroundUploadTask can be Disabled using a simple bit change. I use a REG file for that(attached to this post).",
                    @"REM schtasks / Change / TN ""Microsoft\Windows\Time Synchronization\ForceSynchronizeTime"" / Enable",
                    @"REM schtasks / Change / TN ""Microsoft\Windows\Time Synchronization\SynchronizeTime"" / Enable",
                    @"REM schtasks / Change / TN ""Microsoft\Windows\Windows Error Reporting\QueueReporting"" / Enable",
                    @"REM schtasks / Change / TN ""Microsoft\Windows\WindowsUpdate\Automatic App Update"" / Enable",
                    @"REM * **Put Cortana back * **",
                    @"move ""%windir%\SystemApps\Microsoft.Windows.Cortana_cw5n1h2txyewy.bak"" ""%windir%\SystemApps\Microsoft.Windows.Cortana_cw5n1h2txyewy"",
                    @""%windir%\SystemApps\Microsoft.Windows.Cortana_cw5n1h2txyewy\SearchUI.exe""",
                    @"@rem * **Remove Telemetry & Data Collection * **",
                    @"reg add ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Device Metadata"" / v PreventDeviceMetadataFromNetwork / t REG_DWORD / d 0 / f",
                    @"reg add ""HKLM\SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\DataCollection"" / v ""AllowTelemetry"" / t REG_DWORD / d 1 / f",
                    @"reg add ""HKLM\SOFTWARE\Policies\Microsoft\MRT"" / v DontOfferThroughWUAU / t REG_DWORD / d 0 / f",
                    @"reg add ""HKLM\SOFTWARE\Policies\Microsoft\SQMClient\Windows"" / v ""CEIPEnable"" / t REG_DWORD / d 1 / f",
                    @"reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\AppCompat"" / v ""AITEnable"" / t REG_DWORD / d 1 / f",
                    @"reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\AppCompat"" / v ""DisableUAR"" / t REG_DWORD / d 0 / f",
                    @"reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\DataCollection"" / v ""AllowTelemetry"" / t REG_DWORD / d 1 / f",
                    @"reg add ""HKLM\SYSTEM\CurrentControlSet\Control\WMI\AutoLogger\AutoLogger-Diagtrack-Listener"" / v ""Start"" / t REG_DWORD / d 1 / f",
                    @"reg add ""HKLM\SYSTEM\CurrentControlSet\Control\WMI\AutoLogger\SQMLogger"" / v ""Start"" / t REG_DWORD / d 1 / f",
                    @"@REM * **Enable Cortana & Telemetry * **",
                    @"reg add ""HKLM\SOFTWARE\Policies\Microsoft\Windows\Windows Search"" / v ""AllowCortana"" / t REG_DWORD / d 1",
                    @"REM * **Hide the search box from taskbar.You can still search by pressing the Win key and start typing what you're looking for ***",
                    @"REM 0 = hide completely, 1 = show only icon, 2 = show long search box",
                    @"rem reg add ""HKCU\SOFTWARE\Microsoft\Windows\CurrentVersion\Search"" / v ""SearchboxTaskbarMode"" / t REG_DWORD / d 0 / f",
                    @"REM * **Enable MRU lists(jump lists) of XAML apps in Start Menu * **",
                    @"reg add ""HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"" / v ""Start_TrackDocs"" / t REG_DWORD / d 1 / f",
                    @"REM * **Set Quick Access to start on This PC instead of Windows Explorer * **",
                    @"REM 1 = This PC, 2 = Quick access",
                    @"REM reg add ""HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"" / v ""LaunchTo"" / t REG_DWORD / d 2 / f",
                    @"REM * **Enable Suggestions in the Start Menu * **",
                    @"reg add ""HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\ContentDeliveryManager"" / v ""SystemPaneSuggestionsEnabled"" / t REG_DWORD / d 1 / f",
                    @"@rem Put Built -in Apps Back",
                    @"PowerShell - Command ""Get-AppxPackage -AllUsers | Foreach {Add-AppxPackage -DisableDevelopmentMode -Register ?$($_.InstallLocation)\AppXManifest.xml?}"",
                    @""@rem NOW JUST SOME TWEAKS"",
                    @""REM * **Do not show hidden files in Explorer * **",
                    @"reg add ""HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"" / v ""Hidden"" / t REG_DWORD / d 0 / f",
                    @"REM * **Hide super hidden system files in Explorer * **",
                    @"reg add ""HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"" / v ""ShowSuperHidden"" / t REG_DWORD / d 0 / f",
                    @"REM * **Hide file extensions in Explorer * **",
                    @"reg add ""HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced"" / v ""HideFileExt"" / t  REG_DWORD / d 1 / f",
                    @"REM * **Install OneDrive * **",
                    @"start / wait "" ""%SYSTEMROOT%\SYSWOW64\ONEDRIVESETUP.EXE""",
                };
            String input = "z";
            //User input and input validation
            Console.BackgroundColor = ConsoleColor.Blue;
            Console.ForegroundColor = ConsoleColor.White;
            while ((input[0] != 'a') && (input[0] != 'b'))
            {
                Console.Clear();
                Console.WriteLine("To remove bloat and telemetry enter: a. \nTo restore telemetry and bloat enter: b.");
                input = Console.ReadLine();
            }
            Console.Clear();
            Console.WriteLine("Please make sure to run as administrator as the program executes commands to CMD. Further user input may be requested.");
            Console.WriteLine("Press any key to start...");
            Console.ReadKey();
            if (input[0] == 'a')
            {
                RunRemoveScript(removeScript);
            }
            else if (input[0] == 'b')
            {
                RunRestoreScript(restoreScript);
            }
            else
            {
                Console.Clear();
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("ERROR: Invalid input! \nPress any key to close...");
                Console.ReadKey();
                return;
            }
            Console.WriteLine("Bloat and Telemetry disabler/enabler complete.");
            Console.WriteLine("Press any key to close...");
            Console.ReadKey();
        }
        static void RunRemoveScript(String[] batchSource)
        {
            BatchProcessingUnit bpu = new BatchProcessingUnit();
            bpu.BatchExecuter(bpu.BatchBuilder(batchSource, "C:\\Users\\Zach\\Desktop\\test.bat"));
        }
        static void RunRestoreScript(String[] batchSource)
        {
            BatchProcessingUnit bpu = new BatchProcessingUnit();
            bpu.BatchExecuter(bpu.BatchBuilder(batchSource, "C:\\Users\\Zach\\Desktop\\test.bat"));
        }
    }
}
