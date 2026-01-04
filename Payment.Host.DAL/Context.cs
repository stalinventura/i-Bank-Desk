

using Payment.Host.Entity;
using System.Data.Entity;
using System.Runtime.InteropServices;

namespace Payment.Host.DAL
{
    [DbConfigurationType(typeof(MySql.Data.MySqlClient.MySqlConfiguration))]
    public class Context : DbContext
    {
        public Context() : base("server=softarch.ddns.net;User Id=admin; Persist Security Info=True;database=i-host;password=$sqladmin;charset=utf8;convertzerodatetime=true;")
        {            
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Payment.Host.DAL.Migrations.Configuration>());
           
        }

        public virtual DbSet<clsEmpresasBE> clsEmpresasBE { get; set; }
        public virtual DbSet<clsPersonasBE> clsPersonasBE { get; set; }
        public virtual DbSet<clsPaisesBE> clsPaisesBE { get; set; }
        public virtual DbSet<clsProvinciasBE> clsProvinciasBE { get; set; }
        public virtual DbSet<clsCiudadesBE> clsCiudadesBE { get; set; }
        public virtual DbSet<clsEstadosCivilesBE> clsEstadosCivilesBE { get; set; }
        public virtual DbSet<clsOperadoresBE> clsOperadoresBE { get; set; }
        public virtual DbSet<clsSexosBE> clsSexosBE { get; set; }
        public virtual DbSet<clsMonedasBE> clsMonedasBE { get; set; }
        public virtual DbSet<clsFotografiasBE> clsFotografiasBE { get; set; }
        public virtual DbSet<clsReferenciasBE> clsReferenciasBE { get; set; }
        public virtual DbSet<clsTipoReferenciasBE> clsTipoReferenciasBE { get; set; }
        public virtual DbSet<clsUsuariosBE> clsUsuariosBE { get; set; }
        public virtual DbSet<clsPreguntasBE> clsPreguntasBE { get; set; }
        public virtual DbSet<clsHistorialBE> clsHistorialBE { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {


            //Paises | Provincias
            modelBuilder.Entity<clsPaisesBE>()
           .HasMany(e => e.Provincias)
           .WithRequired(e => e.Paises)
           .WillCascadeOnDelete(false);

            //Provincias | Ciudades
            modelBuilder.Entity<clsProvinciasBE>()
           .HasMany(e => e.Ciudades)
           .WithRequired(e => e.Provincias)
           .WillCascadeOnDelete(false);

            //Ciudades | Personas
            modelBuilder.Entity<clsCiudadesBE>()
           .HasMany(e => e.Personas)
           .WithRequired(e => e.Ciudades)
           .WillCascadeOnDelete(false);

            //Sexos | Personas
            modelBuilder.Entity<clsSexosBE>()
           .HasMany(e => e.Personas)
           .WithRequired(e => e.Sexos)
           .WillCascadeOnDelete(false);

            //Estados Civiles | Personas
            modelBuilder.Entity<clsEstadosCivilesBE>()
           .HasMany(e => e.Personas)
           .WithRequired(e => e.EstadosCiviles)
           .WillCascadeOnDelete(false);

            //Operadores | Personas
            modelBuilder.Entity<clsOperadoresBE>()
           .HasMany(e => e.Personas)
           .WithRequired(e => e.Operadores)
           .WillCascadeOnDelete(false);

            //Personas | Fotografias
            modelBuilder.Entity<clsPersonasBE>()
            .HasOptional(e => e.Fotografias)
            .WithRequired(e => e.Personas)
            .WillCascadeOnDelete(false);

            //Personas | Referencias Laborales
            modelBuilder.Entity<clsPersonasBE>()
           .HasMany(e => e.Referencias)
           .WithRequired(e => e.Personas)
           .WillCascadeOnDelete(false);

            //TipoReferencias | Referencias
            modelBuilder.Entity<clsTipoReferenciasBE>()
           .HasMany(e => e.Referencias)
           .WithRequired(e => e.TipoReferencias)
           .WillCascadeOnDelete(false);

            //Paises | Monedas
            modelBuilder.Entity<clsPaisesBE>()
           .HasMany(e => e.Monedas)
           .WithRequired(e => e.Paises)
           .WillCascadeOnDelete(false);

            //Preguntas | Usuarios
            modelBuilder.Entity<clsPreguntasBE>()
           .HasMany(e => e.Usuarios)
           .WithRequired(e => e.Preguntas)
           .WillCascadeOnDelete(false);

            //Personas | Usuarios
            modelBuilder.Entity<clsPersonasBE>()
            .HasOptional(e => e.Usuarios)
            .WithRequired(e => e.Personas)
            .WillCascadeOnDelete(false);


            //Personas | Historial
            modelBuilder.Entity<clsPersonasBE>()
           .HasMany(e => e.Historial)
           .WithRequired(e => e.Personas)
           .WillCascadeOnDelete(false);

            //Empresas | Historial
            modelBuilder.Entity<clsEmpresasBE>()
           .HasMany(e => e.Historial)
           .WithRequired(e => e.Empresas)
           .WillCascadeOnDelete(false);

        }
    }
}
