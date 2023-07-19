﻿using Microsoft.EntityFrameworkCore;
using CaseStudyAPI.DAL.DomainClasses;
namespace CaseStudyAPI.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public virtual DbSet<Product>? Products { get; set; }
        public virtual DbSet<Brand>? Brands { get; set; }

    }
}