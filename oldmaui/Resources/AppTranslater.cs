using System;
using System.Globalization;
using System.Collections.Generic;

namespace CloudCheckInMaui.Resources
{
    public static class AppTranslater
    {
        private static readonly Dictionary<string, string> Translations = new Dictionary<string, string>
        {
            // General app strings
            { "AppName", "OPTRAX" },
            
            // Authentication
            { "LoginWithFingerOrPin", "Login with Fingerprint or PIN" },
            { "PressHere", "Press Here" },
            { "ReturnToLogin", "Return to Login" },
            
            // Camera
            { "Capture", "Capture" },
            { "Submit", "Submit" },
            { "LookintoCamera", "Look into the camera" },
            { "GetStarted", "Get Started" },
            { "ThankYou_for_Registering", "Thank You for Registering" },
            { "Account_verfied_Admin", "Your account will be verified by Admin" },
            
            // Home
            { "CheckIn", "Check-In" },
            { "CheckOut", "Check-Out" },
            { "TimeSheets", "TimeSheets" },
            
            // Menu
            { "Home", "Home" },
            { "Holiday", "Holiday" },
            { "Logout", "Logout" },
            { "MenuHome", "Home" },
            
            // Holiday
            { "HolidayCalender", "Holiday Calendar" },
            { "ApplyForHoliday", "Apply For Holiday" },
            { "HolidayDateTimeValidatonError", "Start date and time must be earlier than end date and time" },
            
            // Language Selection
            { "SelectLanguage", "Select Language" },
            { "Next", "Next" },
            
            // Settings
            { "Settings", "Settings" },
            { "Language", "Language" },
            { "Notifications", "Notifications" },
            { "Theme", "Theme" },
            { "About", "About" },
            
            // Profile
            { "Profile", "Profile" },
            { "Name", "Name" },
            { "Email", "Email" },
            { "Phone", "Phone" },
            { "Department", "Department" },
            
            // Coming Soon
            { "ComingSoon", "Coming Soon" },
            
            // Timesheet
            { "TimesheetTitle", "Timesheet" },
            { "LoggedInUser", "Logged In User" },
            { "Attendance", "Attendance" },
            { "Site", "Site" },
            { "Date", "Date" },
            { "ClockIn", "Clock In" },
            { "ClockOut", "Clock Out" },
            { "NoTimesheetData", "No timesheet data available" },
            { "LoadingTimesheet", "Loading timesheet..." },
            { "TotalHours", "Total Hours" },
            { "Status", "Status" }
        };

        public static string Translate(string key)
        {
            if (Translations.TryGetValue(key, out string translation))
            {
                return translation;
            }
            
            return key;
        }
    }
} 