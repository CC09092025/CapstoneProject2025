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
            var daysUntilExpiration = (ExpDate - DateTime.Now).Days;
            return daysUntilExpiration <= 10 && daysUntilExpiration >= 0;
        }

        public bool IsExpiredNow()
        {
            return ExpDate < DateTime.Now;
        }

        public string GetExpirationStatus()
        {
            if (IsExpiredNow())
            {
                return DateType == DateType.Expiration ? "Expired" : "Bad";
            }
            else if (IsExpiringSoon())
            {
                return DateType == DateType.Expiration ? "Expiring Soon" : "Going Bad";
            }
            else
            {
                return string.Empty;
            }
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