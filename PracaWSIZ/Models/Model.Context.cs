﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PracaWSIZ.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class SamochodyPDEntities : DbContext
    {
        public SamochodyPDEntities()
            : base("name=SamochodyPDEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AwariaBlacharska> AwariaBlacharska { get; set; }
        public virtual DbSet<AwariaMechaniczna> AwariaMechaniczna { get; set; }
        public virtual DbSet<CzynnosciAutoCoDotyczy> CzynnosciAutoCoDotyczy { get; set; }
        public virtual DbSet<CzynnosciWykonane> CzynnosciWykonane { get; set; }
        public virtual DbSet<Samochody> Samochody { get; set; }
        public virtual DbSet<Sprzedane> Sprzedane { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<Tankowanie> Tankowanie { get; set; }
        public virtual DbSet<Uzytkownik> Uzytkownik { get; set; }
        public virtual DbSet<Zdarzenie> Zdarzenie { get; set; }
    }
}