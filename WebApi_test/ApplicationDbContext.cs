﻿using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;
using WebApi_test.Entities;

namespace WebApi_test
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext([NotNullAttribute] DbContextOptions options) : base(options)
        {
        }

        public DbSet<Genre> Genres { get; set; }
    }
}
