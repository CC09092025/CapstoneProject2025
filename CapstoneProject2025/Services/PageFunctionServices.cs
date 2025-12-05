using System;
using CapstoneProject2025.Models;

namespace CapstoneProject2025.Services
{
    public class PageFunctionServices
    {
        public string GetRelativeDate(DateTime date)
        {
            var diff = DateTime.Now - date;

            if (diff.TotalDays < 1)
            {
                if (diff.TotalHours < 1)
                    return $"{Math.Max((int)diff.TotalMinutes, 1)} minutes ago";
                return $"{Math.Max((int)diff.TotalHours, 1)} hours ago";
            }
            if (diff.TotalDays < 30)
                return $"{Math.Max((int)diff.TotalDays, 1)} days ago";
            if (diff.TotalDays < 365)
                return $"{Math.Max((int)(diff.TotalDays / 30), 1)} months ago";
            return $"{Math.Max((int)(diff.TotalDays / 365), 1)} years ago";
        }

        public string GetRelativeFutureDate(DateTime date)
        {
            var diff = date - DateTime.Now;

            if (diff.TotalDays < 0)
            {
                // Past date
                if (diff.TotalHours > -1)
                    return $"{Math.Max((int)(-diff.TotalMinutes), 1)} minutes ago";
                return $"{Math.Max((int)(-diff.TotalHours), 1)} hours ago";
            }

            if (diff.TotalDays < 1)
            {
                if (diff.TotalHours < 1)
                    return $"in {Math.Max((int)diff.TotalMinutes, 1)} minutes";
                return $"in {Math.Max((int)diff.TotalHours, 1)} hours";
            }
            if (diff.TotalDays < 30)
                return $"in {Math.Max((int)diff.TotalDays, 1)} days";
            if (diff.TotalDays < 365)
                return $"in {Math.Max((int)(diff.TotalDays / 30), 1)} months";
            return $"in {Math.Max((int)(diff.TotalDays / 365), 1)} years";
        }

        public int GetStatusOrder(string status)
        {
            return status switch
            {
                "Expired" => 1,
                "Bad" => 2,
                "Expiring Soon" => 3,
                "Going Bad" => 4,
                _ => 5
            };
        }

        // Determine highlight classes for products

        public bool IsExpired(Product product)
        {
            // Only true for Expiration-type products
            return product.GetExpirationStatus() == "Expired";
        }

        public bool IsExpiringSoon(Product product)
        {
            // 10-day window
            var diff = (product.ExpDate.Date - DateTime.Now.Date).TotalDays;
            return diff >= 0 && diff <= 10 && product.GetExpirationStatus() == "Expiring Soon";
        }

        public bool IsBad(Product product)
        {
            return product.GetExpirationStatus() == "Bad";
        }

        public bool IsGoingBad(Product product)
        {
            return product.GetExpirationStatus() == "Going Bad";
        }

        public string GetHighlightClass(Product product)
        {
            if (IsBad(product)) return "bad-item";
            if (IsGoingBad(product)) return "goingbad-item";
            if (IsExpired(product)) return "expired-item";
            if (IsExpiringSoon(product)) return "expiring-item";

            return "";
        }
    }
}
