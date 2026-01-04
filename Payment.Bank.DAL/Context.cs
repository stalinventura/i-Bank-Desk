using Payment.Bank.Entity;
using System.Data.Entity;

namespace Payment.Bank.DAL
{
    [DbConfigurationType(typeof(MySql.Data.EntityFramework.MySqlEFConfiguration))]
    public class Context : DbContext
    {
        public Context() : base("server=servidor-pc; port= 3306; User Id=admin; Persist Security Info=True;database=i-bank;password=$sqladmin.1234;")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<Context, Payment.Bank.DAL.Migrations.Configuration>());
        }

        public virtual DbSet<clsProcesadoresBE> clsProcesadorBE { get; set; }
        public virtual DbSet<clsPasarelasBE> clsPasarelasBE { get; set; }
        public virtual DbSet<clsLlamadasBE> clsLlamadasBE { get; set; }
        public virtual DbSet<clsParentescosBE> clsParentescosBE { get; set; }
        public virtual DbSet<clsDependientesBE> clsDependientesBE { get; set; }
        public virtual DbSet<clsTipoEmpresasBE> clsTipoEmpresasBE { get; set; }
        public virtual DbSet<clsPlanesBE> clsPlanesBE { get; set; }
        public virtual DbSet<clsPaquetesBE> clsPaquetesBE { get; set; }
        public virtual DbSet<clsDetalleEntradasBE> clsDetalleEntradasBE { get; set; }
        public virtual DbSet<clsListaNegrasBE> clsListaNegrasBE { get; set; }
        public virtual DbSet<clsChequesBE> clsChequesBE { get; set; }
        public virtual DbSet<clsEstadoSolictudChequesBE> clsEstadoSolictudChequesBE { get; set; }
        public virtual DbSet<clsTipoGeolocalizacionesBE> clsTipoGeolocalizacionesBE { get; set; }
        public virtual DbSet<clsGeolocalizacionesBE> clsGeolocalizacionesBE { get; set; }
        public virtual DbSet<clsAlguacilesBE> clsAlguacilesBE { get; set; }
        public virtual DbSet<clsAbogadosBE> clsAbogadosBE { get; set; }
        public virtual DbSet<clsActosBE> clsActosBE { get; set; }
        public virtual DbSet<clsCartasBE> clsCartasBE { get; set; }
        public virtual DbSet<clsPaisesBE> clsPaisesBE { get; set; }
        public virtual DbSet<clsProvinciasBE> clsProvinciasBE { get; set; }
        public virtual DbSet<clsCiudadesBE> clsCiudadesBE { get; set; }
        public virtual DbSet<clsEmpresasBE> clsEmpresasBE { get; set; }
        public virtual DbSet<clsSucursalesBE> clsSucursalesBE { get; set; }
        public virtual DbSet<clsSexosBE> clsSexosBE { get; set; }
        public virtual DbSet<clsEstadosCivilesBE> clsEstadosCivilesBE { get; set; }
        public virtual DbSet<clsOperadoresBE> clsOperadoresBE { get; set; }
        public virtual DbSet<clsPersonasBE> clsPersonasBE { get; set; }
        public virtual DbSet<clsPreguntasBE> clsPreguntasBE { get; set; }
        public virtual DbSet<clsGraficosBE> clsGraficosBE { get; set; }
        public virtual DbSet<clsOcupacionesBE> clsOcupacionesBE { get; set; }
        public virtual DbSet<clsRolesBE> clsRolesBE { get; set; }
        public virtual DbSet<clsProductosBE> clsProductosBE { get; set; }
        public virtual DbSet<clsPermisosBE> clsPermisosBE { get; set; }
        public virtual DbSet<clsPermisosRolesBE> clsPermisosRolesBE { get; set; }
        public virtual DbSet<clsLenguajesBE> clsLenguajesBE { get; set; }
        public virtual DbSet<clsEtiquetasBE> clsEtiquetasBE { get; set; }
        public virtual DbSet<clsUsuariosBE> clsUsuariosBE { get; set; }
        public virtual DbSet<clsGerentesBE> clsGerentesBE { get; set; }
        public virtual DbSet<clsFotografiasBE> clsFotografiasBE { get; set; }
        public virtual DbSet<clsFirmasBE> clsFirmasBE { get; set; }
        public virtual DbSet<clsNotariosPublicoBE> clsNotariosPublicoBE { get; set; }
        public virtual DbSet<clsHorariosBE> clsHorariosBE { get; set; }
        public virtual DbSet<clsIngresosBE> clsIngresosBE { get; set; }
        public virtual DbSet<clsTipoSolicitudesBE> clsTipoSolicitudesBE { get; set; }
        public virtual DbSet<clsCondicionesBE> clsCondicionesBE { get; set; }
        public virtual DbSet<clsTipoGruposBE> clsTipoGruposBE { get; set; }
        public virtual DbSet<clsGruposBE> clsGruposBE { get; set; }
        public virtual DbSet<clsSubGruposBE> clsSubGruposBE { get; set; }
        public virtual DbSet<clsCuentaControlBE> clsCuentaControlBE { get; set; }
        public virtual DbSet<clsSubCuentaControlBE> clsSubCuentaControlBE { get; set; }
        public virtual DbSet<clsAuxiliaresBE> clsAuxiliaresBE { get; set; }
        public virtual DbSet<clsTipoMovimientosBE> clsTipoMovimientosBE { get; set; }
        public virtual DbSet<clsEntradasBE> clsEntradasBE { get; set; }
        public virtual DbSet<clsEstadoSolicitudesBE> clsEstadoSolicitudesBE { get; set; }
        public virtual DbSet<clsClientesBE> clsClientesBE { get; set; }
        public virtual DbSet<clsDatosEconomicosBE> clsDatosEconomicosBE { get; set; }
        public virtual DbSet<clsSolicitudesBE> clsSolicitudesBE { get; set; }
        public virtual DbSet<clsReferenciasBE> clsReferenciasBE { get; set; }
        public virtual DbSet<clsConfiguracionesBE> clsConfiguracionesBE { get; set; }
        public virtual DbSet<clsContabilizacionesBE> clsContabilizacionesBE { get; set; }
        public virtual DbSet<clsTipoContratosBE> clsTipoContratosBE { get; set; }
        public virtual DbSet<clsObjetivosBE> clsObjetivosBE { get; set; }
        public virtual DbSet<clsOficialesBE> clsOficialesBE { get; set; }
        public virtual DbSet<clsColoresBE> clsColoresBE { get; set; }
        public virtual DbSet<clsModelosBE> clsModelosBE { get; set; }
        public virtual DbSet<clsMarcasBE> clsMarcasBE { get; set; }
        public virtual DbSet<clsTipoVehiculosBE> clsTipoVehiculosBE { get; set; }
        public virtual DbSet<clsGarantesBE> clsGarantesBE { get; set; }
        public virtual DbSet<clsGarantiaPersonalBE> clsGarantiaPersonalBE { get; set; }
        public virtual DbSet<clsGarantiaHipotecariaBE> clsGarantiaHipotecariaBE { get; set; }
        public virtual DbSet<clsGarantiaVehiculosBE> clsGarantiaVehiculosBE { get; set; }
        public virtual DbSet<clsGarantiaTarjetasBE> clsGarantiaTarjetasBE { get; set; }
        public virtual DbSet<clsGarantiaComercialesBE> clsGarantiaComercialesBE { get; set; }
        public virtual DbSet<clsComerciosBE> clsComerciosBE { get; set; }
        public virtual DbSet<clsContratosBE> clsContratosBE { get; set; }
        public virtual DbSet<clsEstadoContratosBE> clsEstadoContratosBE { get; set; }
        public virtual DbSet<clsCuotasBE> clsCuotasBE { get; set; }
        public virtual DbSet<clsDetalleNotaDebitosBE> clsDetalleNotaDebitosBE { get; set; }
        public virtual DbSet<clsDetalleNotaCreditosBE> clsDetalleNotaCreditosBE { get; set; }
        public virtual DbSet<clsDetalleRecibosBE> clsDetalleRecibosBE { get; set; }
        public virtual DbSet<clsRecibosBE> clsRecibosBE { get; set; }
        public virtual DbSet<clsFormaPagosBE> clsFormaPagosBE { get; set; }
        public virtual DbSet<clsComprobantesBE> clsComprobantesBE { get; set; }
        public virtual DbSet<clsTipoReferenciasBE> clsTipoReferenciasBE { get; set; }
        public virtual DbSet<clsTipoCertificadosBE> clsTipoCertificadosBE { get; set; }
        public virtual DbSet<clsCertificadosBE> clsCertificadosBE { get; set; }
        public virtual DbSet<clsDetalleCertificadosBE> clsDetalleCertificadosBE { get; set; }
        public virtual DbSet<clsCorreosBE> clsCorreosBE { get; set; }
        public virtual DbSet<clsServidoresBE> clsServidoresBE { get; set; }
        public virtual DbSet<clsTipoTareasBE> clsTipoTareasBE { get; set; }
        public virtual DbSet<clsTareasBE> clsTareasBE { get; set; }
        public virtual DbSet<clsCopiasBE> clsCopiasBE { get; set; }
        public virtual DbSet<clsTipoCartasBE> clsTipoCartasBE { get; set; }
        public virtual DbSet<clsInstitucionesBE> clsInstitucionesBE { get; set; }
        public virtual DbSet<clsRutasBE> clsRutasBE { get; set; }
        public virtual DbSet<clsZonasBE> clsZonasBE { get; set; }
        public virtual DbSet<clsTipoImpresionesBE> clsTipoImpresionesBE { get; set; }
        public virtual DbSet<clsImpresionesBE> clsImpresionesBE { get; set; }
        public virtual DbSet<clsSolicitudChequesBE> clsSolicitudChequesBE { get; set; }
        public virtual DbSet<clsDetalleSolicitudChequesBE> clsDetalleSolicitudChequesBE { get; set; }

        public virtual DbSet<clsTipoDocumentosBE> clsTipoDocumentosBE { get; set; }
        public virtual DbSet<clsBancosBE> clsBancosBE { get; set; }
        public virtual DbSet<clsDesembolsosBE> clsDesembolsosBE { get; set; }
        public virtual DbSet<clsDetalleDesembolsosBE> clsDetalleDesembolsosBE { get; set; }
        public virtual DbSet<clsHistorialCartasBE> clsHistorialCartasBE { get; set; }
        public virtual DbSet<clsSmsBE> clsSmsBE { get; set; }

        public virtual DbSet<clsDiasBE> clsDiasBE { get; set; }
        public virtual DbSet<clsDispositivosBE> clsDispositivosBE { get; set; }
        public virtual DbSet<clsJornadasBE> clsJornadasBE { get; set; }
        public virtual DbSet<clsDetalleJornadasBE> clsDetalleJornadasBE { get; set; }

        public virtual DbSet<clsTipoDenominacionesBE> clsTipoDenominacionesBE { get; set; }
        public virtual DbSet<clsDenominacionesBE> clsDenominacionesBE { get; set; }
        public virtual DbSet<clsCajasBE> clsCajasBE { get; set; }
        public virtual DbSet<clsTurnosBE> clsTurnosBE { get; set; }
        public virtual DbSet<clsArqueosBE> clsArqueosBE { get; set; }
        public virtual DbSet<clsDetalleArqueosBE> clsDetalleArqueosBE { get; set; }
        public virtual DbSet<clsUsuariosRutasBE> clsUsuariosRutasBE { get; set; }


        //Portal Web
        public virtual DbSet<clsServiciosBE> clsServiciosBE { get; set; }

        //Cuentas de Ahorros
        public virtual DbSet<clsTipoCuentasBE> clsTipoCuentasBE { get; set; }
        public virtual DbSet<clsCuentasBE> clsCuentasBE { get; set; }
        public virtual DbSet<clsMonedasBE> clsMonedasBE { get; set; }
        public virtual DbSet<clsTransaccionesBE> clsTransaccionesBE { get; set; }
        public virtual DbSet<clsTipoTransaccionesBE> clsTipoTransaccionesBE { get; set; }

        //Inmoviliaria
        public virtual DbSet<clsEdificiosBE> clsEdificiosBE { get; set; }
        public virtual DbSet<clsPisosBE> clsPisosBE { get; set; }
        public virtual DbSet<clsApartamentosBE> clsApartamentosBE { get; set; }
        public virtual DbSet<clsTipoAlquileresBE> clsTipoAlquileresBE { get; set; }
        public virtual DbSet<clsRentasBE> clsRentasBE { get; set; }
        public virtual DbSet<clsPagoRentasBE> clsPagoRentasBE { get; set; }
        public virtual DbSet<clsDetallePagoRentasBE> clsDetallePagoRentasBE { get; set; }
        public virtual DbSet<clsAlquileresBE> clsAlquileresBE { get; set; }
        public virtual DbSet<clsGarantiaAlquileresBE> clsGarantiaAlquileresBE { get; set; }
        public virtual DbSet<clsTipoApartamentosBE> clsTipoApartamentosBE { get; set; }
        public virtual DbSet<clsTipoEntradasBE> clsTipoEntradasBE { get; set; }

        #region Nominas

        public virtual DbSet<clsPuestosBE> clsPuestosBE { get; set; }
        public virtual DbSet<clsEmpleadosBE> clsEmpleadosBE { get; set; }
        public virtual DbSet<clsNominasBE> clsNominasBE { get; set; }
        public virtual DbSet<clsDetalleNominasBE> clsDetalleNominasBE { get; set; }

        #endregion
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            //Empresas | Sucursales
            modelBuilder.Entity<clsEmpresasBE>()
            .HasMany(e => e.Sucursales)
            .WithRequired(e => e.Empresas)
            .WillCascadeOnDelete(false);

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

            //Ciudades | Sucursales
            modelBuilder.Entity<clsCiudadesBE>()
           .HasMany(e => e.Sucursales)
           .WithRequired(e => e.Ciudades)
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

            //TipoDocumentos| Personas
            modelBuilder.Entity<clsTipoDocumentosBE>()
           .HasMany(e => e.Personas)
           .WithRequired(e => e.TipoDocumentos)
           .WillCascadeOnDelete(false);

            //Personas | Usuarios
            modelBuilder.Entity<clsPersonasBE>()
            .HasOptional(e => e.Usuarios)
            .WithRequired(e => e.Personas)
            .WillCascadeOnDelete(false);

            //Personas | GarantiaAlquilerer
            modelBuilder.Entity<clsPersonasBE>()
            .HasMany(e => e.GarantiaAlquileres)
            .WithRequired(e => e.Personas)
            .WillCascadeOnDelete(false);

            //Notarios | Actos
            modelBuilder.Entity<clsNotariosPublicoBE>()
           .HasMany(e => e.Actos)
           .WithRequired(e => e.NotariosPublico)
           .WillCascadeOnDelete(false);

            //Personas | Fotografias
            modelBuilder.Entity<clsPersonasBE>()
            .HasOptional(e => e.Fotografias)
            .WithRequired(e => e.Personas)
            .WillCascadeOnDelete(false);

            //Personas | Firmas
            modelBuilder.Entity<clsPersonasBE>()
            .HasOptional(e => e.Firmas)
            .WithRequired(e => e.Personas)
            .WillCascadeOnDelete(false);

            //Preguntas | Usuarios
            modelBuilder.Entity<clsPreguntasBE>()
           .HasMany(e => e.Usuarios)
           .WithRequired(e => e.Preguntas)
           .WillCascadeOnDelete(false);

            //Personas | Instituciones
            modelBuilder.Entity<clsPersonasBE>()
           .HasMany(e => e.Instituciones)
           .WithRequired(e => e.Personas)
           .WillCascadeOnDelete(false);

            //Instituciones | DatosLaborales
            modelBuilder.Entity<clsInstitucionesBE>()
           .HasMany(e => e.DatosEconomicos)
           .WithRequired(e => e.Instituciones)
           .WillCascadeOnDelete(false);

            //Lenguajes | Etiquetas
            modelBuilder.Entity<clsLenguajesBE>()
           .HasMany(e => e.Etiquetas)
           .WithRequired(e => e.Lenguajes)
           .WillCascadeOnDelete(false);

            //Lenguajes | Usuarios
            modelBuilder.Entity<clsLenguajesBE>()
           .HasMany(e => e.Usuarios)
           .WithRequired(e => e.Lenguajes)
           .WillCascadeOnDelete(false);

            //Preguntas | Usuarios
            modelBuilder.Entity<clsPreguntasBE>()
           .HasMany(e => e.Usuarios)
           .WithRequired(e => e.Preguntas)
           .WillCascadeOnDelete(false);

            //Graficos | Roles
            modelBuilder.Entity<clsGraficosBE>()
           .HasMany(e => e.Roles)
           .WithRequired(e => e.Graficos)
           .WillCascadeOnDelete(false);

            //Productos | Permisos
            modelBuilder.Entity<clsProductosBE>()
           .HasMany(e => e.Permisos)
           .WithRequired(e => e.Productos)
           .WillCascadeOnDelete(false);

            //Permisos | PermisosRoles
            modelBuilder.Entity<clsPermisosBE>()
           .HasMany(e => e.PermisosRoles)
           .WithRequired(e => e.Permisos)
           .WillCascadeOnDelete(false);

            //Roles | PermisosRoles
            modelBuilder.Entity<clsRolesBE>()
           .HasMany(e => e.PermisosRoles)
           .WithRequired(e => e.Roles)
           .WillCascadeOnDelete(false);

            //Roles | Usuarios
            modelBuilder.Entity<clsRolesBE>()
           .HasMany(e => e.Usuarios)
           .WithRequired(e => e.Roles)
           .WillCascadeOnDelete(false);

            //Personas | Clientes
            modelBuilder.Entity<clsPersonasBE>()
           .HasMany(e => e.Clientes)
           .WithRequired(e => e.Personas)
           .WillCascadeOnDelete(false);

            //Configuraciones | Impresiones
            modelBuilder.Entity<clsConfiguracionesBE>()
           .HasMany(e => e.Impresiones)
           .WithRequired(e => e.Configuraciones)
           .WillCascadeOnDelete(false);

            //Tipo Impresiones | Impresiones
            modelBuilder.Entity<clsTipoImpresionesBE>()
           .HasMany(e => e.Impresiones)
           .WithRequired(e => e.TipoImpresiones)
           .WillCascadeOnDelete(false);

            //Sucursales | Clientes
            modelBuilder.Entity<clsSucursalesBE>()
           .HasMany(e => e.Clientes)
           .WithRequired(e => e.Sucursales)
           .WillCascadeOnDelete(false);

            //Personas | Datos Economicos
            modelBuilder.Entity<clsPersonasBE>()
            .HasOptional(e => e.DatosEconomicos)
            .WithRequired(e => e.Personas)
            .WillCascadeOnDelete(false);

            //Personas | Referencias Laborales
            modelBuilder.Entity<clsPersonasBE>()
           .HasMany(e => e.Referencias)
           .WithRequired(e => e.Personas)
           .WillCascadeOnDelete(false);

            //Horarios | Datos Economicos
            modelBuilder.Entity<clsHorariosBE>()
           .HasMany(e => e.DatosEconomicos)
           .WithRequired(e => e.Horarios)
           .WillCascadeOnDelete(false);

            //Ocupaciones | Datos Economicos
            modelBuilder.Entity<clsOcupacionesBE>()
           .HasMany(e => e.DatosEconomicos)
           .WithRequired(e => e.Ocupaciones)
           .WillCascadeOnDelete(false);

            //Ingresos | Datos Economicos
            modelBuilder.Entity<clsIngresosBE>()
           .HasMany(e => e.DatosEconomicos)
           .WithRequired(e => e.Ingresos)
           .WillCascadeOnDelete(false);

            //Clientes | Solicitudes
            modelBuilder.Entity<clsClientesBE>()
           .HasMany(e => e.Solicitudes)
           .WithRequired(e => e.Clientes)
           .WillCascadeOnDelete(false);

            //Estados | Solicitudes
            modelBuilder.Entity<clsEstadoSolicitudesBE>()
           .HasMany(e => e.Solicitudes)
           .WithRequired(e => e.Estados)
           .WillCascadeOnDelete(false);

            //Condiciones | Solicitudes
            modelBuilder.Entity<clsCondicionesBE>()
           .HasMany(e => e.Solicitudes)
           .WithRequired(e => e.Condiciones)
           .WillCascadeOnDelete(false);

            //TipoSolicitudes | Solicitudes
            modelBuilder.Entity<clsTipoSolicitudesBE>()
           .HasMany(e => e.Solicitudes)
           .WithRequired(e => e.TipoSolicitudes)
           .WillCascadeOnDelete(false);

            //Estados | Solicitud Cheques
            modelBuilder.Entity<clsEstadoSolictudChequesBE>()
           .HasMany(e => e.SolicitudCheques)
           .WithRequired(e => e.Estados)
           .WillCascadeOnDelete(false);

            //TipoEntradas | Detalle Entradas
            modelBuilder.Entity<clsTipoEntradasBE>()
           .HasMany(e => e.DetalleEntradas)
           .WithRequired(e => e.TipoEntradas)
           .WillCascadeOnDelete(false);

            //Sucursales | Entradas
            modelBuilder.Entity<clsSucursalesBE>()
           .HasMany(e => e.Entradas)
           .WithRequired(e => e.Sucursales)
           .WillCascadeOnDelete(false);

            //Monto | Solicitudes
            modelBuilder.Entity<clsSolicitudesBE>()
            .Property(e => e.Monto)
            ;//.HasPrecision(18, 3);


            //MontoMinimo | Configuracion
            modelBuilder.Entity<clsConfiguracionesBE>()
            .Property(e => e.MinimumAmount)
            ;//.HasPrecision(18, 3);


            //TipoContratos | Contratos
            modelBuilder.Entity<clsTipoContratosBE>()
           .HasMany(e => e.Contratos)
           .WithRequired(e => e.TipoContratos)
           .WillCascadeOnDelete(false);

            //Objetivos | Contratos
            modelBuilder.Entity<clsObjetivosBE>()
           .HasMany(e => e.Contratos)
           .WithOptional(e => e.Objetivos)
           .WillCascadeOnDelete(false);

            //Monto | Solicitudes de Cheques
            modelBuilder.Entity<clsSolicitudChequesBE>()
            .Property(e => e.Monto)
            ;//.HasPrecision(18, 3);

            //Solicitud Cheques | Cheques
            modelBuilder.Entity<clsSolicitudChequesBE>()
            .HasOptional(e => e.Cheques)
            .WithRequired(e => e.SolicitudCheques)
            .WillCascadeOnDelete(false);

            //Bancos |  Solicitudes de Cheques
            modelBuilder.Entity<clsBancosBE>()
           .HasMany(e => e.SolicitudCheques)
           .WithRequired(e => e.Bancos)
           .WillCascadeOnDelete(false);

            //Solicitud Cheques |  Detalle Solicitudes de Cheques
            modelBuilder.Entity<clsSolicitudChequesBE>()
           .HasMany(e => e.DetalleSolicitudCheques)
           .WithRequired(e => e.SolicitudCheques)
           .WillCascadeOnDelete(false);

            //Auxiliares |  Bancos
            modelBuilder.Entity<clsAuxiliaresBE>()
           .HasMany(e => e.Bancos)
           .WithRequired(e => e.Auxiliares)
           .WillCascadeOnDelete(false);

            //Estados | Contratos
            modelBuilder.Entity<clsEstadoContratosBE>()
           .HasMany(e => e.Contratos)
           .WithRequired(e => e.Estados)
           .WillCascadeOnDelete(false);

            //Solicitudes | Contratos
            modelBuilder.Entity<clsSolicitudesBE>()
           .HasMany(e => e.Contratos)
           .WithRequired(e => e.Solicitudes)
           .WillCascadeOnDelete(false);

            //Monto | Contratos
            modelBuilder.Entity<clsContratosBE>()
            .Property(e => e.Comision)
            ;//.HasPrecision(18, 3);

            //Monto | Contratos
            modelBuilder.Entity<clsContratosBE>()
            .Property(e => e.Interes)
            ;//.HasPrecision(18, 3);

            //Monto | Contratos
            modelBuilder.Entity<clsContratosBE>()
            .Property(e => e.Mora)
            ;//.HasPrecision(18, 3);

            //Monto | Contratos
            modelBuilder.Entity<clsContratosBE>()
            .Property(e => e.Cuota)
            ;//.HasPrecision(18, 3);

            //Contratos | Cuotas
            modelBuilder.Entity<clsContratosBE>()
           .HasMany(e => e.Cuotas)
           .WithRequired(e => e.Contratos)
           .WillCascadeOnDelete(false);

            //Capital | Cuotas
            modelBuilder.Entity<clsCuotasBE>()
            .Property(e => e.Capital)
            ;//.HasPrecision(18, 3);

            //Comision | Cuotas
            modelBuilder.Entity<clsCuotasBE>()
            .Property(e => e.Comision)
            ;//.HasPrecision(18, 3);

            //Interes | Cuotas
            modelBuilder.Entity<clsCuotasBE>()
            .Property(e => e.Interes)
            ;//.HasPrecision(18, 3);

            //Moras | Cuotas
            modelBuilder.Entity<clsCuotasBE>()
            .Property(e => e.Mora)
            ;//.HasPrecision(18, 3);

            //Legal | Cuotas
            modelBuilder.Entity<clsCuotasBE>()
            .Property(e => e.Legal)
            ;//.HasPrecision(18, 3);

            //Personas | Notarios
            modelBuilder.Entity<clsPersonasBE>()
            .HasMany(e => e.NotariosPublico)
            .WithRequired(e => e.Personas)
            .WillCascadeOnDelete(false);

            //Personas | Gerentes
            modelBuilder.Entity<clsPersonasBE>()
            .HasOptional(e => e.Gerentes)
            .WithRequired(e => e.Personas)
            .WillCascadeOnDelete(false);

            //Gerentes | Sucursales
            modelBuilder.Entity<clsGerentesBE>()
           .HasMany(e => e.Sucursales)
           .WithRequired(e => e.Gerentes)
           .WillCascadeOnDelete(false);

            //Sucursales | Notarios
            modelBuilder.Entity<clsSucursalesBE>()
           .HasMany(e => e.NotariosPublico)
           .WithRequired(e => e.Sucursales)
           .WillCascadeOnDelete(false);

            //TipoGrupos | Grupos
            modelBuilder.Entity<clsTipoGruposBE>()
           .HasMany(e => e.Grupos)
           .WithRequired(e => e.TipoGrupos)
           .WillCascadeOnDelete(false);

            //Grupos | SubGrupos
            modelBuilder.Entity<clsGruposBE>()
           .HasMany(e => e.SubGrupos)
           .WithRequired(e => e.Grupos)
           .WillCascadeOnDelete(false);

            //SubGrupos | Cuentas Control
            modelBuilder.Entity<clsSubGruposBE>()
           .HasMany(e => e.CuentasControl)
           .WithRequired(e => e.SubGrupos)
           .WillCascadeOnDelete(false);

            //Cuentas Control | Sub Cuentas Control
            modelBuilder.Entity<clsCuentaControlBE>()
           .HasMany(e => e.SubCuentasControl)
           .WithRequired(e => e.CuentaControl)
           .WillCascadeOnDelete(false);

            //Sub Cuentas Control | Auxiliares
            modelBuilder.Entity<clsSubCuentaControlBE>()
           .HasMany(e => e.Auxiliares)
           .WithRequired(e => e.SubCuentaControl)
           .WillCascadeOnDelete(false);

           // //Auxiliares | TipoSolicitudes
           // modelBuilder.Entity<clsAuxiliaresBE>()
           //.HasMany(e => e.TipoSolicitudes)
           //.WithRequired(e => e.Auxiliares)
           //.WillCascadeOnDelete(false);

            //Auxiliares | DetalleEntradas
            modelBuilder.Entity<clsAuxiliaresBE>()
           .HasMany(e => e.DetalleEntradas)
           .WithRequired(e => e.Auxiliares)
           .WillCascadeOnDelete(false);

            //TipoMovimientos | Entradas
            modelBuilder.Entity<clsTipoMovimientosBE>()
           .HasMany(e => e.Entradas)
           .WithRequired(e => e.TipoMovimientos)
           .WillCascadeOnDelete(false);

            //Sucursales | Usuarios
            modelBuilder.Entity<clsSucursalesBE>()
            .HasMany(e => e.Usuarios)
            .WithRequired(e => e.Sucursales)
            .WillCascadeOnDelete(false);

            //Sucursales | Configuraciones
            modelBuilder.Entity<clsSucursalesBE>()
            .HasOptional(e => e.Configuraciones)
            .WithRequired(e => e.Sucursales)
            .WillCascadeOnDelete(false);

            //Personas | Agentes
            modelBuilder.Entity<clsPersonasBE>()
           .HasMany(e => e.Oficiales)
           .WithRequired(e => e.Personas)
           .WillCascadeOnDelete(false);

            //Sucursales | Agentes
            modelBuilder.Entity<clsSucursalesBE>()
           .HasMany(e => e.Oficiales)
           .WithRequired(e => e.Sucursales)
           .WillCascadeOnDelete(false);

            //Comercios | GarantiaComerciales
            modelBuilder.Entity<clsComerciosBE>()
           .HasMany(e => e.GarantiaComerciales)
           .WithRequired(e => e.Comercios)
           .WillCascadeOnDelete(false);

            //Solicitudes | Garantia Comerciales
            modelBuilder.Entity<clsSolicitudesBE>()
            .HasOptional(e => e.GarantiaComerciales)
            .WithRequired(e => e.Solicitudes)
            .WillCascadeOnDelete(false);

            //TipoVehiculos | Garantias
            modelBuilder.Entity<clsTipoVehiculosBE>()
           .HasMany(e => e.GarantiaVehiculos)
           .WithRequired(e => e.TipoVehiculos)
           .WillCascadeOnDelete(false);

            //Colores | GarantiaVehiculos
            modelBuilder.Entity<clsColoresBE>()
           .HasMany(e => e.GarantiaVehiculos)
           .WithRequired(e => e.Colores)
           .WillCascadeOnDelete(false);

            //Marcas | Modelos
            modelBuilder.Entity<clsMarcasBE>()
           .HasMany(e => e.Modelos)
           .WithRequired(e => e.Marcas)
           .WillCascadeOnDelete(false);

            //Modelos | Garantias
            modelBuilder.Entity<clsModelosBE>()
           .HasMany(e => e.GarantiaVehiculos)
           .WithRequired(e => e.Modelos)
           .WillCascadeOnDelete(false);

            //Garantes | Garantia Personal
            modelBuilder.Entity<clsGarantesBE>()
           .HasMany(e => e.GarantiaPersonal)
           .WithRequired(e => e.Garantes)
           .WillCascadeOnDelete(false);

            //Solicitudes | Garantia Personal
            modelBuilder.Entity<clsSolicitudesBE>()
            .HasOptional(e => e.GarantiaPersonal)
            .WithRequired(e => e.Solicitudes)
            .WillCascadeOnDelete(false);

            //Solicitudes | Garantia Hipotecaria
            modelBuilder.Entity<clsSolicitudesBE>()
            .HasOptional(e => e.GarantiaHipotecaria)
            .WithRequired(e => e.Solicitudes)
            .WillCascadeOnDelete(false);

            //Solicitudes | Garantia Vehiculos
            modelBuilder.Entity<clsSolicitudesBE>()
            .HasOptional(e => e.GarantiaVehiculos)
            .WithRequired(e => e.Solicitudes)
            .WillCascadeOnDelete(false);

            //Solicitudes | Garantia Tarjetas
            modelBuilder.Entity<clsSolicitudesBE>()
            .HasOptional(e => e.GarantiaTarjetas)
            .WithRequired(e => e.Solicitudes)
            .WillCascadeOnDelete(false);

            //Monto | Clientes
            modelBuilder.Entity<clsClientesBE>()
            .Property(e => e.Monto)
            ;//.HasPrecision(18, 3);

            ////Latitude | Clientes
            //modelBuilder.Entity<clsClientesBE>()
            //.Property(e => e.Latitude)
            //;//.HasPrecision(18, 3);

            ////Longitude | Clientes
            //modelBuilder.Entity<clsClientesBE>()
            //.Property(e => e.Longitude)
            //;//.HasPrecision(18, 3);

            //Monto | Solicitudes
            modelBuilder.Entity<clsSolicitudesBE>()
            .Property(e => e.Monto)
            ;//.HasPrecision(18, 3);

            //Monto | Garantia Hipotecaria
            modelBuilder.Entity<clsGarantiaHipotecariaBE>()
            .Property(e => e.Monto)
            ;//.HasPrecision(18, 3);

            //Agentes | Contratos
            modelBuilder.Entity<clsOficialesBE>()
           .HasMany(e => e.Contratos)
           .WithRequired(e => e.Oficiales)
           .WillCascadeOnDelete(false);

            //Cuotas | DetalleNotaDebitos
            modelBuilder.Entity<clsCuotasBE>()
           .HasMany(e => e.DetalleNotaDebitos)
           .WithRequired(e => e.Cuotas)
           .WillCascadeOnDelete(false);

            //Cuotas | DetalleNotaCreditos
            modelBuilder.Entity<clsCuotasBE>()
           .HasMany(e => e.DetalleNotaCreditos)
           .WithRequired(e => e.Cuotas)
           .WillCascadeOnDelete(false);

            //Contratos | Recibos
            modelBuilder.Entity<clsContratosBE>()
           .HasMany(e => e.Recibos)
           .WithRequired(e => e.Contratos)
           .WillCascadeOnDelete(false);

            //Comprobantes | Recibos
            modelBuilder.Entity<clsComprobantesBE>()
           .HasMany(e => e.Recibos)
           .WithRequired(e => e.Comprobantes)
           .WillCascadeOnDelete(false);

            //Zonas | Contratos
            modelBuilder.Entity<clsZonasBE>()
           .HasMany(e => e.Contratos)
           .WithRequired(e => e.Zonas)
           .WillCascadeOnDelete(false);

            //Rutas | Zonas
            modelBuilder.Entity<clsRutasBE>()
           .HasMany(e => e.Zonas)
           .WithRequired(e => e.Rutas)
           .WillCascadeOnDelete(false);

            // //Rutas | Contratos
            // modelBuilder.Entity<clsRutasBE>()
            //.HasMany(e => e.Clientes)
            //.WithRequired(e => e.Rutas)
            //.WillCascadeOnDelete(false);

            //Forma de Pagos | Recibos
            modelBuilder.Entity<clsFormaPagosBE>()
           .HasMany(e => e.Recibos)
           .WithRequired(e => e.FormaPagos)
           .WillCascadeOnDelete(false);

            //Sucursales | Recibos
            modelBuilder.Entity<clsSucursalesBE>()
           .HasMany(e => e.Recibos)
           .WithRequired(e => e.Sucursales)
           .WillCascadeOnDelete(false);

            //Capital | Notas Debito
            modelBuilder.Entity<clsDetalleNotaDebitosBE>()
            .Property(e => e.Capital)
            ;//.HasPrecision(18, 3);

            //Comision | Notas Debito
            modelBuilder.Entity<clsDetalleNotaDebitosBE>()
            .Property(e => e.Comision)
            ;//.HasPrecision(18, 3);

            //Interes | Notas Debito
            modelBuilder.Entity<clsDetalleNotaDebitosBE>()
            .Property(e => e.Interes)
            ;//.HasPrecision(18, 3);

            //Moras | Notas Debito
            modelBuilder.Entity<clsDetalleNotaDebitosBE>()
            .Property(e => e.Mora)
            ;//.HasPrecision(18, 3);

            //Legal | Notas Debito
            modelBuilder.Entity<clsDetalleNotaDebitosBE>()
            .Property(e => e.Legal)
            ;//.HasPrecision(18, 3);

            //Capital | Notas Credito
            modelBuilder.Entity<clsDetalleNotaCreditosBE>()
            .Property(e => e.Capital)
            ;//.HasPrecision(18, 3);

            //Comision | Notas Credito
            modelBuilder.Entity<clsDetalleNotaCreditosBE>()
            .Property(e => e.Comision)
            ;//.HasPrecision(18, 3);

            //Interes | Notas Credito
            modelBuilder.Entity<clsDetalleNotaCreditosBE>()
            .Property(e => e.Interes)
            ;//.HasPrecision(18, 3);

            //Moras | Notas Credito
            modelBuilder.Entity<clsDetalleNotaCreditosBE>()
            .Property(e => e.Mora)
            ;//.HasPrecision(18, 3);

            //Legal | Notas Credito
            modelBuilder.Entity<clsDetalleNotaCreditosBE>()
            .Property(e => e.Legal)
            ;//.HasPrecision(18, 3);

            //Capital | Detalle Recibos
            modelBuilder.Entity<clsDetalleRecibosBE>()
            .Property(e => e.Capital)
            ;//.HasPrecision(18, 3);

            //Comision | Detalle Recibos
            modelBuilder.Entity<clsDetalleRecibosBE>()
            .Property(e => e.Comision)
            ;//.HasPrecision(18, 3);

            //Interes | Detalle Recibos
            modelBuilder.Entity<clsDetalleRecibosBE>()
            .Property(e => e.Interes)
            ;//.HasPrecision(18, 3);

            //Moras | Detalle Recibos
            modelBuilder.Entity<clsDetalleRecibosBE>()
            .Property(e => e.Mora)
            ;//.HasPrecision(18, 3);

            //Legal | Detalle Recibos
            modelBuilder.Entity<clsDetalleRecibosBE>()
            .Property(e => e.Legal)
            ;//.HasPrecision(18, 3);

            //SubTotal | Detalle Recibos
            modelBuilder.Entity<clsDetalleRecibosBE>()
            .Property(e => e.SubTotal)
            ;//.HasPrecision(18, 3);

            //Cuotas | DetalleRecibos
            modelBuilder.Entity<clsCuotasBE>()
           .HasMany(e => e.DetalleRecibos)
           .WithRequired(e => e.Cuotas)
           .WillCascadeOnDelete(false);

            //Recibos | DetalleRecibos
            modelBuilder.Entity<clsRecibosBE>()
           .HasMany(e => e.DetalleRecibos)
           .WithRequired(e => e.Recibos)
           .WillCascadeOnDelete(false);

            //TipoReferencias | Referencias
            modelBuilder.Entity<clsTipoReferenciasBE>()
           .HasMany(e => e.Referencias)
           .WithRequired(e => e.TipoReferencias)
           .WillCascadeOnDelete(false);

            //Mora | Configuraciones
            modelBuilder.Entity<clsCondicionesBE>()
            .Property(e => e.Mora)
            ;//.HasPrecision(18, 3);

            //Mora | Configuraciones
            modelBuilder.Entity<clsCondicionesBE>()
            .Property(e => e.Interes)
            ;//.HasPrecision(18, 3);

            //Mora | Configuraciones
            modelBuilder.Entity<clsCondicionesBE>()
            .Property(e => e.Comision)
            ;//.HasPrecision(18, 3);
            
            //Monedas | Paises
            modelBuilder.Entity<clsMonedasBE>()
           .HasMany(e => e.Paises)
           .WithRequired(e => e.Monedas)
           .WillCascadeOnDelete(false);
            
            //Monedas | Certificados
            modelBuilder.Entity<clsMonedasBE>()
           .HasMany(e => e.Certificados)
           .WithRequired(e => e.Monedas)
           .WillCascadeOnDelete(false);
            
            //Monto | Certificados
            modelBuilder.Entity<clsCertificadosBE>()
            .Property(e => e.Monto)
            ;//.HasPrecision(18, 3);

            //Interes | Certificados
            modelBuilder.Entity<clsCertificadosBE>()
            .Property(e => e.Interes)
            ;//.HasPrecision(18, 3);

            //Sucursales | Certificados
            modelBuilder.Entity<clsSucursalesBE>()
           .HasMany(e => e.Certificados)
           .WithRequired(e => e.Sucursales)
           .WillCascadeOnDelete(false);

            //Personas | Certificados
            modelBuilder.Entity<clsPersonasBE>()
           .HasMany(e => e.Certificados)
           .WithRequired(e => e.Personas)
           .WillCascadeOnDelete(false);

            //TipoCertificados | Certificados
            modelBuilder.Entity<clsTipoCertificadosBE>()
           .HasMany(e => e.Certificados)
           .WithRequired(e => e.TipoCertificados)
           .WillCascadeOnDelete(false);

           // Sucursales | Bancos
           // modelBuilder.Entity<clsSucursalesBE>()
           //.HasMany(e => e.Bancos)
           //.WithRequired(e => e.Sucursales)
           //.WillCascadeOnDelete(false);


            //Certificados | DetalleCertificados
            modelBuilder.Entity<clsCertificadosBE>()
           .HasMany(e => e.DetalleCertificados)
           .WithRequired(e => e.Certificados)
           .WillCascadeOnDelete(false);

            //Monto | DetalleCertificados
            modelBuilder.Entity<clsDetalleCertificadosBE>()
            .Property(e => e.Monto)
            ;//.HasPrecision(18, 3);

            //Interes | DetalleCertificados
            modelBuilder.Entity<clsDetalleCertificadosBE>()
            .Property(e => e.Interes)
            ;//.HasPrecision(18, 3);

            //Monto | Cartas
            modelBuilder.Entity<clsCartasBE>()
            .Property(e => e.Monto)
            ;//.HasPrecision(18, 3);

            //Empresas | Correos
            modelBuilder.Entity<clsEmpresasBE>()
           .HasMany(e => e.Correos)
           .WithRequired(e => e.Empresas)
           .WillCascadeOnDelete(false);

            //Servidores | Correos
            modelBuilder.Entity<clsServidoresBE>()
           .HasMany(e => e.Correos)
           .WithRequired(e => e.Servidores)
           .WillCascadeOnDelete(false);

            //TipoTareas | Tareas
            modelBuilder.Entity<clsTipoTareasBE>()
           .HasMany(e => e.Tareas)
           .WithRequired(e => e.TipoTareas)
           .WillCascadeOnDelete(false);

            //Empresas | Tareas
            modelBuilder.Entity<clsEmpresasBE>()
           .HasMany(e => e.Tareas)
           .WithRequired(e => e.Empresas)
           .WillCascadeOnDelete(false);

            //Empresas | BackUp
            modelBuilder.Entity<clsEmpresasBE>()
            .HasOptional(e => e.Copias)
            .WithRequired(e => e.Empresas)
            .WillCascadeOnDelete(false);

            //TipoCartas | Cartas
            modelBuilder.Entity<clsTipoCartasBE>()
           .HasMany(e => e.Cartas)
           .WithRequired(e => e.TipoCartas)
           .WillCascadeOnDelete(false);

            //Empresas | Cartas
            modelBuilder.Entity<clsEmpresasBE>()
           .HasMany(e => e.Cartas)
           .WithRequired(e => e.Empresas)
           .WillCascadeOnDelete(false);
            
            //TipoCuentas | Cuentas
            modelBuilder.Entity<clsTipoCuentasBE>()
           .HasMany(e => e.Cuentas)
           .WithRequired(e => e.TipoCuentas)
           .WillCascadeOnDelete(false);

            // Monenas || TipoCuentas
            modelBuilder.Entity<clsMonedasBE>()
           .HasMany(e => e.TipoCuentas)
           .WithRequired(e => e.Monedas)
           .WillCascadeOnDelete(false);

            //Clientes | Cuentas
            modelBuilder.Entity<clsClientesBE>()
           .HasMany(e => e.Cuentas)
           .WithRequired(e => e.Clientes)
           .WillCascadeOnDelete(false);

            //Cuentas | Transacciones
            modelBuilder.Entity<clsCuentasBE>()
           .HasMany(e => e.Transacciones)
           .WithRequired(e => e.Cuentas)
           .WillCascadeOnDelete(false);

            //Sucursales | Transacciones
            modelBuilder.Entity<clsSucursalesBE>()
           .HasMany(e => e.Transacciones)
           .WithRequired(e => e.Sucursales)
           .WillCascadeOnDelete(false);

            //Formas de Pago | Transacciones
            modelBuilder.Entity<clsFormaPagosBE>()
           .HasMany(e => e.Transacciones)
           .WithRequired(e => e.FormaPagos)
           .WillCascadeOnDelete(false);

            //Monto | Transacciones
            modelBuilder.Entity<clsTransaccionesBE>()
            .Property(e => e.Monto)
            ;//.HasPrecision(18, 3);

            //Tipo de Transacciones | Transacciones
            modelBuilder.Entity<clsTipoTransaccionesBE>()
           .HasMany(e => e.Transacciones)
           .WithRequired(e => e.TipoTransacciones)
           .WillCascadeOnDelete(false);

            //Portal Web

            //Empresas | Servicios
            modelBuilder.Entity<clsEmpresasBE>()
           .HasMany(e => e.Servicios)
           .WithRequired(e => e.Empresas)
           .WillCascadeOnDelete(false);


            //Inmoviliaria

            //Edificios | Apartamentos
            modelBuilder.Entity<clsEdificiosBE>()
           .HasMany(e => e.Apartamentos)
           .WithRequired(e => e.Edificios)
           .WillCascadeOnDelete(false);

            //Pisos | Apartamentos
            modelBuilder.Entity<clsPisosBE>()
           .HasMany(e => e.Apartamentos)
           .WithRequired(e => e.Pisos)
           .WillCascadeOnDelete(false);

            //Notarios | Alquileres
            modelBuilder.Entity<clsNotariosPublicoBE>()
           .HasMany(e => e.Alquileres)
           .WithRequired(e => e.NotariosPublico)
           .WillCascadeOnDelete(false);

            //Apartamentos | Alquileres
            modelBuilder.Entity<clsApartamentosBE>()
           .HasMany(e => e.Alquileres)
           .WithRequired(e => e.Apartamentos)
           .WillCascadeOnDelete(false);

            //Alquileres | Garantia Alquiler
            modelBuilder.Entity<clsAlquileresBE>()
            .HasOptional(e => e.GarantiaAlquileres)
            .WithRequired(e => e.Alquileres)
            .WillCascadeOnDelete(false);

            //Personas | Alquileres
            modelBuilder.Entity<clsPersonasBE>()
           .HasMany(e => e.Alquileres)
           .WithRequired(e => e.Personas)
           .WillCascadeOnDelete(false);

            //Monto | Alquileres
            modelBuilder.Entity<clsAlquileresBE>()
            .Property(e => e.Monto)
            ;//.HasPrecision(18, 3);

            //Alquileres | Rentas
            modelBuilder.Entity<clsAlquileresBE>()
           .HasMany(e => e.Rentas)
           .WithRequired(e => e.Alquileres)
           .WillCascadeOnDelete(false);

            //Alquiletes | PagoRentas
            modelBuilder.Entity<clsAlquileresBE>()
           .HasMany(e => e.PagoRentas)
           .WithRequired(e => e.Alquileres)
           .WillCascadeOnDelete(false);

            //PagoRentas | DetallePagoRentas
            modelBuilder.Entity<clsPagoRentasBE>()
           .HasMany(e => e.DetallePagoRentas)
           .WithRequired(e => e.PagoRentas)
           .WillCascadeOnDelete(false);

            //Rentas | DetallePagoRentas
            modelBuilder.Entity<clsRentasBE>()
           .HasMany(e => e.DetallePagoRentas)
           .WithRequired(e => e.Rentas)
           .WillCascadeOnDelete(false);

            //TipoAlquileres | Alquileres
            modelBuilder.Entity<clsTipoAlquileresBE>()
           .HasMany(e => e.Alquileres)
           .WithRequired(e => e.TipoAlquileres)
           .WillCascadeOnDelete(false);

            //Monto | DetallePagoRentas
            modelBuilder.Entity<clsDetallePagoRentasBE>()
            .Property(e => e.Monto)
            ;//.HasPrecision(18, 3);
                       
            //TipoImpresiones | Impresiones
            modelBuilder.Entity<clsTipoImpresionesBE>()
           .HasMany(e => e.Impresiones)
           .WithRequired(e => e.TipoImpresiones)
           .WillCascadeOnDelete(false);

            //Configuraciones | Impresiones
            modelBuilder.Entity<clsConfiguracionesBE>()
           .HasMany(e => e.Impresiones)
           .WithRequired(e => e.Configuraciones)
           .WillCascadeOnDelete(false);

            //Personas | Dependientes
            modelBuilder.Entity<clsPersonasBE>()
           .HasMany(e => e.Dependientes)
           .WithRequired(e => e.Personas)
           .WillCascadeOnDelete(false);

            //Parentescos | Dependientes
            modelBuilder.Entity<clsParentescosBE>()
           .HasMany(e => e.Dependientes)
           .WithRequired(e => e.Parentescos)
           .WillCascadeOnDelete(false);

            //Ancho | Impresiones
            modelBuilder.Entity<clsImpresionesBE>()
            .Property(e => e.Ancho)
            ;//.HasPrecision(18, 3);

            //Alto | Impresiones
            modelBuilder.Entity<clsImpresionesBE>()
            .Property(e => e.Alto)
            ;//.HasPrecision(18, 3);

            //Debito | Detalle Desembolsos
            modelBuilder.Entity<clsDetalleDesembolsosBE>()
            .Property(e => e.Debito)
            ;//.HasPrecision(18, 3);

            //Credito | Detalle Desembolsos
            modelBuilder.Entity<clsDetalleDesembolsosBE>()
            .Property(e => e.Credito)
            ;//.HasPrecision(18, 3);

            //Debito | Detalle Entradas
            modelBuilder.Entity<clsDetalleDesembolsosBE>()
            .Property(e => e.Debito)
            ;//.HasPrecision(18, 3);

            //Credito | Detalle Entradas
            modelBuilder.Entity<clsDetalleDesembolsosBE>()
            .Property(e => e.Credito)
            ;//.HasPrecision(18, 3);

            //Detalle Desembolsos | Desembolsos
            modelBuilder.Entity<clsDesembolsosBE>()
           .HasMany(e => e.DetalleDesembolsos)
           .WithRequired(e => e.Desembolsos)
           .WillCascadeOnDelete(false);

            //Auxiliares | Detalle Desembolsos
            modelBuilder.Entity<clsAuxiliaresBE>()
           .HasMany(e => e.DetalleDesembolsos)
           .WithRequired(e => e.Auxiliares)
           .WillCascadeOnDelete(false);

            //Cartas | HistorialCartas
            modelBuilder.Entity<clsCartasBE>()
           .HasMany(e => e.HistorialCartas)
           .WithRequired(e => e.Cartas)
           .WillCascadeOnDelete(false);

            //Cuotas | HistorialCartas
            modelBuilder.Entity<clsCuotasBE>()
           .HasMany(e => e.HistorialCartas)
           .WithRequired(e => e.Cuotas)
           .WillCascadeOnDelete(false);

            //Contratos | Sms
            modelBuilder.Entity<clsContratosBE>()
           .HasMany(e => e.Sms)
           .WithOptional(e => e.Contratos)
           .WillCascadeOnDelete(false);

           // //Contratos | LLamadas
            modelBuilder.Entity<clsContratosBE>()
           .HasMany(e => e.Llamadas)
           .WithRequired(e => e.Contratos)
           .WillCascadeOnDelete(false);

            //Sucursales | Abogados
            modelBuilder.Entity<clsSucursalesBE>()
            .HasOptional(e => e.Abogados)
            .WithRequired(e => e.Sucursales)
            .WillCascadeOnDelete(false);

            //Personas | Abogados
            modelBuilder.Entity<clsPersonasBE>()
           .HasMany(e => e.Abogados)
           .WithRequired(e => e.Personas)
           .WillCascadeOnDelete(false);

            //Sucursales | Alguaciles
            modelBuilder.Entity<clsSucursalesBE>()
            .HasOptional(e => e.Alguaciles)
            .WithRequired(e => e.Sucursales)
            .WillCascadeOnDelete(false);

            //Personas | Alguaciles
            modelBuilder.Entity<clsPersonasBE>()
           .HasMany(e => e.Alguaciles)
           .WithRequired(e => e.Personas)
           .WillCascadeOnDelete(false);

            //TipoGeolocalizaciones | Geolocalizaciones
            modelBuilder.Entity<clsTipoGeolocalizacionesBE>()
           .HasMany(e => e.Geolocalizaciones)
           .WithRequired(e => e.TipoGeolocalizaciones)
           .WillCascadeOnDelete(false);

            //Personas | Geolocalizaciones
            modelBuilder.Entity<clsPersonasBE>()
           .HasMany(e => e.Geolocalizaciones)
           .WithRequired(e => e.Personas)
           .WillCascadeOnDelete(false);

            //Entradas | DetalleEntradas
            modelBuilder.Entity<clsEntradasBE>()
           .HasMany(e => e.DetalleEntradas)
           .WithRequired(e => e.Entradas)
           .WillCascadeOnDelete(false);

            // //Auxiliares | Formas de Pago
            modelBuilder.Entity<clsAuxiliaresBE>()
           .HasMany(e => e.FormaPagos)
           .WithRequired(e => e.Auxiliares)
           .WillCascadeOnDelete(false);

            //Empresas | Dispositivos
            modelBuilder.Entity<clsEmpresasBE>()
           .HasMany(e => e.Dispositivos)
           .WithRequired(e => e.Empresas)
           .WillCascadeOnDelete(false);

            //Jornadas | DetalleJornadas
            modelBuilder.Entity<clsJornadasBE>()
           .HasMany(e => e.DetalleJornadas)
           .WithRequired(e => e.Jornadas)
           .WillCascadeOnDelete(false);

            //Dias | DetalleJornadas
            modelBuilder.Entity<clsDiasBE>()
           .HasMany(e => e.DetalleJornadas)
           .WithRequired(e => e.Dias)
           .WillCascadeOnDelete(false);

            //Dispositivos | Impresiones
            modelBuilder.Entity<clsDispositivosBE>()
           .HasMany(e => e.Impresiones)
           .WithRequired(e => e.Dispositivos)
           .WillCascadeOnDelete(false);

            //Jornadas | Usuarios
            modelBuilder.Entity<clsJornadasBE>()
           .HasMany(e => e.Usuarios)
           .WithRequired(e => e.Jornadas)
           .WillCascadeOnDelete(false);

            //Configuraciones | Contabilizaciones
            modelBuilder.Entity<clsConfiguracionesBE>()
           .HasMany(e => e.Contabilizaciones)
           .WithRequired(e => e.Configuraciones)
           .WillCascadeOnDelete(false);

            //Empresas | Pasarelas
            modelBuilder.Entity<clsEmpresasBE>()
           .HasMany(e => e.Pasarelas)
           .WithRequired(e => e.Empresas)
           .WillCascadeOnDelete(false);

            //Procesadores | Pasarelas
            modelBuilder.Entity<clsProcesadoresBE>()
           .HasMany(e => e.Pasarelas)
           .WithRequired(e => e.Procesadores)
           .WillCascadeOnDelete(false);


            //TipoDenominaciones | Denominaciones
            modelBuilder.Entity<clsTipoDenominacionesBE>()
           .HasMany(e => e.Denominaciones)
           .WithRequired(e => e.TipoDenominaciones)
           .WillCascadeOnDelete(false);

            //Denominaciones | DetalleArqueos
            modelBuilder.Entity<clsDenominacionesBE>()
           .HasMany(e => e.DetalleArqueos)
           .WithRequired(e => e.Denominaciones)
           .WillCascadeOnDelete(false);

            //Usuarios | Turnos
            modelBuilder.Entity<clsUsuariosBE>()
           .HasMany(e => e.Turnos)
           .WithRequired(e => e.Usuarios)
           .WillCascadeOnDelete(false);

            //Cajas | Turnos
            modelBuilder.Entity<clsCajasBE>()
           .HasMany(e => e.Turnos)
           .WithRequired(e => e.Cajas)
           .WillCascadeOnDelete(false);

            //Turnos | Arqueos
            modelBuilder.Entity<clsTurnosBE>()
           .HasMany(e => e.Arqueos)
           .WithRequired(e => e.Turnos)
           .WillCascadeOnDelete(false);

            //Arqueos | DetalleArqueos
            modelBuilder.Entity<clsArqueosBE>()
           .HasMany(e => e.DetalleArqueos)
           .WithRequired(e => e.Arqueos)
           .WillCascadeOnDelete(false);


            //Usuarios | UsuariosRutas
            modelBuilder.Entity<clsUsuariosBE>()
           .HasMany(e => e.UsuariosRutas)
           .WithRequired(e => e.Usuarios)
           .WillCascadeOnDelete(false);

            //Rutas | UsuariosRutas
            modelBuilder.Entity<clsRutasBE>()
           .HasMany(e => e.UsuariosRutas)
           .WithRequired(e => e.Rutas)
           .WillCascadeOnDelete(false);

            //Paquetes | Empresas
            modelBuilder.Entity<clsPaquetesBE>()
           .HasMany(e => e.Empresas)
           .WithRequired(e => e.Paquetes)
           .WillCascadeOnDelete(false);

            //Planes | Empresas
            modelBuilder.Entity<clsPlanesBE>()
           .HasMany(e => e.Empresas)
           .WithRequired(e => e.Planes)
           .WillCascadeOnDelete(false);

            #region Nominas

            //Personas | Empleados
            modelBuilder.Entity<clsPersonasBE>()
           .HasMany(e => e.Empleados)
           .WithRequired(e => e.Personas)
           .WillCascadeOnDelete(false);

            //Puestos | Empleados
            modelBuilder.Entity<clsPuestosBE>()
           .HasMany(e => e.Empleados)
           .WithRequired(e => e.Puestos)
           .WillCascadeOnDelete(false);

            //Sucursales | Empleados
            modelBuilder.Entity<clsSucursalesBE>()
           .HasMany(e => e.Empleados)
           .WithRequired(e => e.Sucursales)
           .WillCascadeOnDelete(false);

            //Bancos | Nominas
            modelBuilder.Entity<clsBancosBE>()
           .HasMany(e => e.Nominas)
           .WithRequired(e => e.Bancos)
           .WillCascadeOnDelete(false);

            //Bancos | Nominas
            modelBuilder.Entity<clsBancosBE>()
           .HasMany(e => e.Nominas)
           .WithRequired(e => e.Bancos)
           .WillCascadeOnDelete(false);

            //Nominas | DetalleNominas
            modelBuilder.Entity<clsNominasBE>()
           .HasMany(e => e.DetalleNominas)
           .WithRequired(e => e.Nominas)
           .WillCascadeOnDelete(false);

            //Empleados | DetalleNominas
            modelBuilder.Entity<clsEmpleadosBE>()
           .HasMany(e => e.DetalleNominas)
           .WithRequired(e => e.Empleados)
           .WillCascadeOnDelete(false);

            //Sueldo | empleados
            modelBuilder.Entity<clsEmpleadosBE>()
            .Property(e => e.Sueldo);

            //SubTotal | Nomina
            modelBuilder.Entity<clsNominasBE>()
            .Property(e => e.SubTotal);

            //Descuento | Nomina
            modelBuilder.Entity<clsNominasBE>()
            .Property(e => e.Descuento);

            //Monto | Nomina
            modelBuilder.Entity<clsNominasBE>()
            .Property(e => e.Total);

            //ARS | DetalleNomina
            modelBuilder.Entity<clsDetalleNominasBE>()
            .Property(e => e.AFP);

            //CxC | DetalleNomina
            modelBuilder.Entity<clsDetalleNominasBE>()
            .Property(e => e.Comision);

            //Incentivos | DetalleNomina
            modelBuilder.Entity<clsDetalleNominasBE>()
            .Property(e => e.ISR);

            //ISR | DetalleNomina
            modelBuilder.Entity<clsDetalleNominasBE>()
            .Property(e => e.SFS);

            //Otros | DetalleNomina
            modelBuilder.Entity<clsDetalleNominasBE>()
            .Property(e => e.Otros);

            //SubTotal | DetalleNomina
            modelBuilder.Entity<clsDetalleNominasBE>()
            .Property(e => e.SubTotal);

            //Sueldo | DetalleNomina
            modelBuilder.Entity<clsDetalleNominasBE>()
            .Property(e => e.Sueldo);



            #endregion

        }
    }
}
