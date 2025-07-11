using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace CloudCheckInMaui.ConstantHelper
{
    public static class ValidationHelpers
    {
        public static Regex EmailRegex = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        public static Regex PasswordRegex = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+-=]).*$");
        public static Regex NameRegex = new Regex(@"^[A-Za-z][a-zA-Z]*$");
        public static Regex Number = new Regex(@"^[0-9]+$");
        public static Regex AlphaNumber = new Regex(@"^[A-Z0-9a-z][a-zA-Z0-9]*$");
    }
} 