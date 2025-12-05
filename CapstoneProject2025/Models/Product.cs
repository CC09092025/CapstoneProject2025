using SQLite;

namespace CapstoneProject2025.Models
{
    public class Product
    {
        [PrimaryKey]
        public string ItemId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime ExpDate { get; set; }
        public bool IsExpired { get; set; }
        public DateTime LastModified { get; set; }
        public DateType DateType { get; set; }

        // Parameterless constructor required for SQLite
        public Product()
        {
        }

        public Product(string name, string category, DateTime expDate, DateType dateType)
        {
            ItemId = Guid.NewGuid().ToString();
            Name = name;
            Category = category;
            ExpDate = expDate;
            DateType = dateType;
            DateAdded = DateTime.Now;
            LastModified = DateTime.Now;
            IsExpired = expDate < DateTime.Now;
        }

        public void Update(string name, string category, DateTime expDate, DateType dateType)
        {
            Name = name;
            Category = category;
            ExpDate = expDate;
            DateType = dateType;
            LastModified = DateTime.Now;
            IsExpired = expDate < DateTime.Now;
        }

        public bool IsExpiringSoon()
        {
            var diff = (ExpDate - DateTime.Now).TotalDays;
            return diff >= 0 && diff <= 10;  // or 10 if you prefer, but consistent!
        }


        public bool IsExpiredNow()
        {
            return ExpDate < DateTime.Now;
        }

        public string GetExpirationStatus()
        {
            var diff = (ExpDate - DateTime.Now).TotalDays;

            // EXPIRED / BAD
            if (diff < 0)
                return DateType == DateType.Expiration ? "Expired" : "Bad";

            // EXPIRING SOON / GOING BAD (0–10 days)
            if (diff <= 10)
                return DateType == DateType.Expiration ? "Expiring Soon" : "Going Bad";

            // Otherwise no status
            return string.Empty;
        }


        public string GetCategoryTag()
        {
            var safeCategory = Category
                .ToLower()
                .Replace(" ", "-");

            return $"category-{safeCategory}";
        }

        public string GetStatusTagClass()
        {
            var status = GetExpirationStatus().ToLower().Replace(" ", "-");
            return $"status-tag {status}";
        }
    }

    public enum DateType
    {
        Expiration,
        BestBefore
    }
}