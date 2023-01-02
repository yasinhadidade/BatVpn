using System;
using System.Collections.Generic;

#nullable disable

namespace BatVpn.Domain.Entities.Common
{
    public partial class AspNetUser
    {
        public AspNetUser()
        {
            AspNetUserClaims = new HashSet<AspNetUserClaim>();
            AspNetUserLogins = new HashSet<AspNetUserLogin>();
            AspNetUserRoles = new HashSet<AspNetUserRole>();
            AspNetUserTokens = new HashSet<AspNetUserToken>();
            BasketItems = new HashSet<BasketItem>();
            Followers = new HashSet<Follower>();
            Orders = new HashSet<Order>();
            PageAccesses = new HashSet<PageAccess>();
            Pages = new HashSet<Page>();
            PagesRatings = new HashSet<PagesRating>();
            ProductsRatings = new HashSet<ProductsRating>();
            Replies = new HashSet<Reply>();
            Transactions = new HashSet<Transaction>();
            UserAddresses = new HashSet<UserAddress>();
            UserConfirmation = new HashSet<UserConfirm>();
            PageEmployees = new HashSet<PageEmployees>();
            UserBankCarts = new HashSet<UserBankCart>();
            BookmarkPosts = new HashSet<BookmarkPost>();
            ReportViolations = new HashSet<ReportViolation>();
            OrderRatings = new HashSet<OrderRating>();
        }

        public string Id { get; set; }
        public DateTime RegisterDate { get; set; }
        public string ProfileName { get; set; }
        public string FullName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NationalCode { get; set; }
        public DateTime BirthDate { get; set; }
        public bool IsActive { get; set; }
        public string UserName { get; set; }
        public string NormalizedUserName { get; set; }
        public string Email { get; set; }
        public string NormalizedEmail { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string ConcurrencyStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTimeOffset? LockoutEnd { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }
        public string IbanNumber { get; set; }
        public string Address { get; set; }
        public decimal? Long { get; set; }
        public decimal? Lat { get; set; }
        public string PostalCode { get; set; }
        public DateTime? ProfileRegisterDate { get; set; }
        public bool HasUserProfile { get; set; }
        public bool HasSellerProfile { get; set; }

        public bool IsVerifiedNationalCode { get; set; }
        public bool IsVerifiedIbanNumber { get; set; }

        public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual ICollection<AspNetUserRole> AspNetUserRoles { get; set; }
        public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; }
        public virtual ICollection<BasketItem> BasketItems { get; set; }
        public virtual ICollection<Follower> Followers { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PageAccess> PageAccesses { get; set; }
        public virtual ICollection<Page> Pages { get; set; }
        public virtual ICollection<PagesRating> PagesRatings { get; set; }
        public virtual ICollection<ProductsRating> ProductsRatings { get; set; }
        public virtual ICollection<Reply> Replies { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
        public virtual ICollection<UserAddress> UserAddresses { get; set; }

        public virtual ICollection<PageEmployees> PageEmployees { get; set; }
        public virtual ICollection<UserConfirm> UserConfirmation { get; set; }
        public virtual ICollection<UserBankCart> UserBankCarts { get; set; }
        public virtual ICollection<BookmarkPost> BookmarkPosts { get; set; }
        public virtual ICollection<ReportViolation> ReportViolations { get; set; }
        public virtual ICollection<OrderRating> OrderRatings { get; set; }


    }
}
