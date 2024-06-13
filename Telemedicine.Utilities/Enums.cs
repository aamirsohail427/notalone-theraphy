using System;
using System.Collections.Generic;
using System.Text;

namespace Telemedicine.Utilities
{
    public static class StripeConfigurations
    {
        public static readonly string PUBLISHABLE_KEY = "pk_test_HwN6cveYPGXKoxqLweTuigjM";
        public static readonly string SECRET_KEY = "sk_test_2ZQfISfE41PcupMsJn4FZxnB";
    }

    public static class TokboxConfigurations
    {
        public static readonly int API_KEY = 46801984;
        public static readonly string SECRET = "be2d635219a8290feea6f2fd7014f0a694c2d404";
        public static readonly string GLOBAL_SESSION_ID = "2_MX40NjgwMTk4NH5-MTU5MjY0NTgxMTk0N34xKzhnRXZKN2V4UktIWE1uY2J1ZmdYWVJ-fg";
    }
    public static class AzureConfigurations
    {
        public static readonly string AZURE_STORAGE_CONNECTION_STRING = "DefaultEndpointsProtocol=https;AccountName=fsoptstorage;AccountKey=cUS5AnAxM4PzxeQu++2a4c3qL4/WpNqlWbDt9j6/x0wLRD8yBQxeARitGMQDcnXBAw0ESAbW5uPq8JG7VdCuAA==;EndpointSuffix=core.windows.net";
        public static readonly string AZURE_BLOB_CONTAINER = "fsopt";
        public static readonly string AZURE_BLOB_BASE_URL = "https://fsoptstorage.blob.core.windows.net/fsopt/";
    }
    public static class Configurations
    {
        public static readonly string ApplicationName = "IClinicNow";
        public static readonly string BaseUrl = "";
        public static readonly string AppointmentFee = "0";
    }
    public enum PaymentTypes
    {
        Stripe = 14
    }

    public static class PaymentTypeStrings
    {
        public static readonly string Stripe = "Stripe";
    }

    public enum UserRoleTypes
    {
        Admin = 1,
        Therapist = 2,
        Client = 3
    }

    public enum NotificationTypes
    {
        Appointment = 10,
        Message = 11,
        Payment = 12,
        SOAPNote = 13
    }

    public static class RoleStrings
    {
        public const string AdminOnly = "Admin";
        public const string TherapistOnly = "Therapist";
        public const string ClientOnly = "Client";
        public const string AdminTherapistOnly = "Admin,Therapist";
        public const string AdminClientOnly = "Admin,Client";
        public const string ClientTherapistOnly = "Client,Therapist";
    }
    public static class UserRoleTypeStrings
    {
        public const string Admin = "Admin";
        public const string Therapist = "Therapist";
        public const string Client = "Client";
    }
    public static class LookupTypeStrings
    {
        public static readonly string SalutationType = "SalutationType";
        public static readonly string GenderType = "GenderType";
        public static readonly string PreferredLanguageType = "PreferredLanguageType";
        public static readonly string EducationType = "EducationType";
        public static readonly string AppointmentType = "AppointmentType";
    }
    public static class SessionStrings
    {
        public static readonly string LoginId = "LoginId";
        public static readonly string UserId = "UserId";
        public static readonly string UserRoleId = "UserRoleId";
        public static readonly string TimeZoneId = "TimeZoneId";
        public static readonly string Username = "Username";
        public static readonly string ProfilePicture = "ProfilePicture";
        public static readonly string TokenId = "TokenId";
    }
    public static class ResponseStrings
    {
        public static readonly string NotFound = "Not Found";
        public static readonly string Success = "Success";
        public static readonly string Unauthorized = "You are currently blocked. Please try to contact customer support.";
        public static readonly string InvalidCredentials = "Invalid username or password";
        public static readonly string InvalidPassword = "Invalid current password";
    }
}
