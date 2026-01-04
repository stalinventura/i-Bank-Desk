

using MahApps.Metro.Controls;
using Payment.Bank.View.Consultas;
using Payment.Bank.View.Formularios;
using Payment.Bank.View.Informes.Reportes;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;


namespace Payment.Bank.Modulos
{
    public class clsItemClick
    {       
        public static void menuItem_Click(Object sender, EventArgs e) 
        {
            try
            {
                clsMenuItemBO menuItems;
                menuItems = (clsMenuItemBO)sender;
                clsVariablesBO.Permiso = menuItems;
                
                switch (menuItems.Name)
                {
                    #region Archivo

                    case "ARC_SOLICITUDES":
                        {
                            frmConsultasSolicitudes Productos = new frmConsultasSolicitudes();
                            Productos.Owner = clsVariablesBO.Host;
                            Productos.ShowDialog();
                        }
                        break;
                    case "ARC_CONTRACTOS":
                        {
                            frmConsultasContratos Productos = new frmConsultasContratos();
                            Productos.Owner = clsVariablesBO.Host;
                            Productos.ShowDialog();
                        }
                        break;
                    case "ARC_RECIBOS":
                        {
                            frmConsultasRecibos Recibos = new frmConsultasRecibos();
                            Recibos.Owner = clsVariablesBO.Host;
                            Recibos.ShowDialog();
                        }
                        break;
                    case "ARC_PAGO_EMPRESAS":
                        {
                            frmPagosEmpresas Recibos = new frmPagosEmpresas();
                            Recibos.Owner = clsVariablesBO.Host;
                            Recibos.ShowDialog();
                        }
                        break;
                    case "ARC_PAGO_RUTAS":
                        {
                            frmPagosRutas Recibos = new frmPagosRutas();
                            Recibos.Owner = clsVariablesBO.Host;
                            Recibos.ShowDialog();
                        }
                        break;

                    //    case "REG_CLIENTES":                        
                    //        {
                    //            frmConsultasClientes Productos = new frmConsultasClientes();
                    //            Productos.ShowDialog();
                    //        }
                    //        break;
                    //    case "REG_SUPLIDORES":
                    //        {
                    //            frmConsultasSuplidores Productos = new frmConsultasSuplidores();
                    //            Productos.ShowDialog();
                    //        }
                    //        break;
                    //    case "REG_COMPRAS":
                    //        {
                    //            frmConsultasCompras Productos = new frmConsultasCompras();
                    //            Productos.ShowDialog();
                    //        }
                    //        break;
                    //    case "REG_FACTURAS":
                    //        {
                    //            frmConsultasFacturas Productos = new frmConsultasFacturas();
                    //            Productos.ShowDialog();
                    //        }
                    //        break;

                    #endregion

                    #region Inmobiliarias

                    case "INM_ALQUILERES":
                       {
                            frmConsultasAlquileres Inmobiliarias = new frmConsultasAlquileres();
                            Inmobiliarias.Owner = clsVariablesBO.Host;
                            Inmobiliarias.ShowDialog();
                        }
                        break;
                    case "INM_APARTAMENTOS":
                        {
                            frmConsultasApartamentos Inmobiliarias = new frmConsultasApartamentos();
                            Inmobiliarias.Owner = clsVariablesBO.Host;
                            Inmobiliarias.ShowDialog();
                        }
                        break;
                    case "INM_PISOS":
                        {
                            frmConsultasPisos Inmobiliarias = new frmConsultasPisos();
                            Inmobiliarias.Owner = clsVariablesBO.Host;
                            Inmobiliarias.ShowDialog();
                        }
                        break;

                    case "INM_EDIFICIOS":
                        {
                            frmConsultasEdificios Inmobiliarias = new frmConsultasEdificios();
                            Inmobiliarias.Owner = clsVariablesBO.Host;
                            Inmobiliarias.ShowDialog();
                        }
                        break;
                    case "ARC_PAGO_RENTA":
                        {
                            frmConsultasPagoRentas Recibos = new frmConsultasPagoRentas();
                            Recibos.Owner = clsVariablesBO.Host;
                            Recibos.ShowDialog();
                        }
                        break;

                    case "INM_RELACION_INGRESOS":
                        {
                            frmRelacionIngresosInmueblesGetByFecha Recibos = new frmRelacionIngresosInmueblesGetByFecha();
                            Recibos.Owner = clsVariablesBO.Host;
                            Recibos.ShowDialog();
                        }
                        break;

                    case "INM_CUENTASXCOBRAR":
                        {
                            frmCuentasPorCobrarInmuebles Recibos = new frmCuentasPorCobrarInmuebles();
                            Recibos.Owner = clsVariablesBO.Host;
                            Recibos.ShowDialog();
                        }
                        break;

                    #endregion

                    #region Inversiones


                    case "INV_CERTIFICADOS":
                        {
                            frmConsultasCertificados Inversiones = new frmConsultasCertificados();
                            Inversiones.Owner = clsVariablesBO.Host;
                            Inversiones.ShowDialog();
                        }
                        break;

                    #endregion  

                    #region Mantenimientos

                    case "HER_CONDITIONS":
                        {
                            frmConsultasCondiciones Pais = new frmConsultasCondiciones();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_PAISES":
                        {
                            frmConsultasPaises Pais = new frmConsultasPaises();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_PROVINCIAS":
                        {
                            frmConsultasProvincias Pais = new frmConsultasProvincias();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_CIUDADES":
                        {
                            frmConsultasCiudades Pais = new frmConsultasCiudades();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_PREGUNTAS":
                        {
                            frmConsultasPreguntas Pais = new frmConsultasPreguntas();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_GRAFICOS":
                        {
                            frmConsultasGraficos Pais = new frmConsultasGraficos();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_OCUPACIONES":
                        {
                            frmConsultasOcupaciones Pais = new frmConsultasOcupaciones();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_OPERADORES":
                        {
                            frmConsultasOperadores Pais = new frmConsultasOperadores();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_HORARIOS":
                        {
                            frmConsultasHorarios Pais = new frmConsultasHorarios();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_INGRESOS":
                        {
                            frmConsultasIngresos Pais = new frmConsultasIngresos();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_TIPOSOLICITUDES":
                        {
                            frmConsultasTipoSolicitudes Pais = new frmConsultasTipoSolicitudes();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_TIPOCONTRATOS":
                        {
                            frmConsultasTipoContratos Pais = new frmConsultasTipoContratos();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_COLORES":
                        {
                            frmConsultasColores Pais = new frmConsultasColores();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_MARCAS":
                        {
                            frmConsultasMarcas Pais = new frmConsultasMarcas();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_MODELOS":
                        {
                            frmConsultasModelos Pais = new frmConsultasModelos();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_TIPOVEHICULOS":
                        {
                            frmConsultasTipoVehiculos Pais = new frmConsultasTipoVehiculos();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;
                    case "MAN_FORMAPAGOS":
                        {
                            frmConsultasFormaPagos FormaPago = new frmConsultasFormaPagos();
                            FormaPago.Owner = clsVariablesBO.Host;
                            FormaPago.ShowDialog();
                        }
                        break;
                    case "MAN_USUARIOS":
                        {
                            frmConsultasUsuarios Usuario = new frmConsultasUsuarios();
                            Usuario.Owner = clsVariablesBO.Host;
                            Usuario.ShowDialog();
                        }
                        break;


                    case "MAN_ROAD":
                        {
                            frmConsultasRutas Pais = new frmConsultasRutas();
                            Pais.Owner = clsVariablesBO.Host;
                            Pais.ShowDialog();
                        }
                        break;

                    //    case "MAN_UBICACIONES":
                    //        {
                    //            frmConsultasUbicaciones Ubicaciones = new frmConsultasUbicaciones();
                    //            Ubicaciones.ShowDialog();
                    //        }
                    //        break;
                    //    case "MAN_PRESENTACIONES":
                    //        {
                    //            frmConsultasPresentaciones Ubicaciones = new frmConsultasPresentaciones();
                    //            Ubicaciones.ShowDialog();
                    //        }
                    //        break;
                    //    case "MAN_FORMA_PAGOS":
                    //        {
                    //            frmConsultasFormaPagos Ubicaciones = new frmConsultasFormaPagos();
                    //            Ubicaciones.ShowDialog();
                    //        }
                    //        break;
                    //    case "MAN_CATEGORIAS":                        
                    //        {
                    //            frmConsultasCategorias Ubicaciones = new frmConsultasCategorias();
                    //            Ubicaciones.ShowDialog();
                    //        }
                    //        break;
                    //    case "MAN_EMPRESAS":
                    //        {
                    //            frmConsultasEmpresas Ubicaciones = new frmConsultasEmpresas();
                    //            Ubicaciones.ShowDialog();
                    //        }
                    //        break;
                    //    case "MAN_COMPROBANTES":
                    //        {
                    //            frmConsultasComprobantes Comprobantes = new frmConsultasComprobantes();
                    //            Comprobantes.ShowDialog();
                    //        }
                    //        break;

                    #endregion

                    #region Nominas

                    case "NOM_NOMINA":
                        {
                            frmConsultasNominas Herramienta = new frmConsultasNominas();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "NOM_PUESTOS":
                        {
                            frmConsultasPuestos Herramienta = new frmConsultasPuestos();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "NOM_EMPLEADOS":
                        {
                            frmConsultasEmpleados Herramienta = new frmConsultasEmpleados();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    #endregion

                    #region Herramientas


                    case "HER_BOX_AUDIT":
                        {
                            frmConsultasArqueos Herramienta = new frmConsultasArqueos();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;
                    case "HER_TURNS":
                        {
                            frmConsultasTurnos Herramienta = new frmConsultasTurnos();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_BOX_CLOSE":
                        {
                            frmCierres Herramienta = new frmCierres();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_BOX":
                        {
                            frmConsultasCajas Herramienta = new frmConsultasCajas();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_GATEWAY":
                        {
                            frmConsultasPasarelas Herramienta = new frmConsultasPasarelas();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_SHERIFF":
                        {
                            frmConsultasAlguaciles Herramienta = new frmConsultasAlguaciles();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_PERMISOS":
                        {
                            frmConsultasRoles Herramienta = new frmConsultasRoles();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_DEVICES":
                        {
                            frmConsultasDispositivos Herramienta = new frmConsultasDispositivos();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_BLACKLIST":
                        {
                            frmConsultasListaNegra Herramienta = new frmConsultasListaNegra();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_PRINTERS":
                        {
                            frmConsultasImpresiones Herramienta = new frmConsultasImpresiones();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_BUSINESS":
                        {
                            frmConsultasEmpresas Herramienta = new frmConsultasEmpresas();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_NOTIFICACION":
                        {
                            frmNotificaciones Herramienta = new frmNotificaciones();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_CARTAS":
                        {
                            frmConsultasCartas Herramienta = new frmConsultasCartas();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_PUBLIC_NOTARY":
                        {
                            frmConsultasNotariosPublicos Herramienta = new frmConsultasNotariosPublicos();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_LAWYERS":
                        {
                            frmConsultasAbogados Herramienta = new frmConsultasAbogados();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_ACCOUNTING_OFFICERS":
                        {
                            frmConsultasOficiales Herramienta = new frmConsultasOficiales();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_ANALYSIS":
                        {
                            frmAnalisisContratos Herramienta = new frmAnalisisContratos();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_CORREOS":
                        {
                            frmConsultasCorreos Usuario = new frmConsultasCorreos();
                            Usuario.Owner = clsVariablesBO.Host;
                            Usuario.ShowDialog();
                        }
                        break;

                    case "HER_TASK":
                        {
                            frmConsultasTareas Herramienta = new frmConsultasTareas();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "HER_BACKUP":
                        {
                            frmConsultasCopias Herramienta = new frmConsultasCopias();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "CON_BRANCHES":
                        {
                            frmConsultasSucursales Herramienta = new frmConsultasSucursales();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    #endregion

                    #region Informes

                    case "INF_LISTADO_KALIFIKA":
                        {
                            frmListadoKalifikaGetByFecha Informes = new frmListadoKalifikaGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();

                        }
                        break;

                    case "INF_RELACION_COMISIONES_INGRESOS":
                        {
                            frmRelacionComisionIngresosGetByFecha Informes = new frmRelacionComisionIngresosGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();

                        }
                        break;

                    case "INF_LISTADO_LLAMADAS":
                        {
                            frmListadoLlamadasGetByFecha Informes = new frmListadoLlamadasGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_LISTADO_SMS":
                        {
                            frmListadoSmsGetByFecha Informes = new frmListadoSmsGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_LISTADO_TRANSACCIONES":
                        {
                            frmDetalleNotasGetByFecha Informes = new frmDetalleNotasGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_RENOVACION_CONTRATOS":
                        {
                            frmRenovacionesGetByCuota Informes = new frmRenovacionesGetByCuota();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_ANALISIS_MOROSIDAD":
                        {
                            frmListadoAnalisisMorosidad Informes = new frmListadoAnalisisMorosidad();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_LETTERS":
                        {
                            frmCartasGetByRutaID Informes = new frmCartasGetByRutaID();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_CUENTAS_POR_COBRAR":                        
                        {
                            frmCuentasPorCobrar Informes = new frmCuentasPorCobrar();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_LISTADO_RUTAS":
                        {
                            frmListadoRutasGetByFecha Informes = new frmListadoRutasGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_RECIBOS_RUTAS":
                        {
                            frmRecibosRutasGetByFecha Informes = new frmRecibosRutasGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_CUENTAS_POR_COBRAR_EMPRESAS":
                        {
                            frmCuentasPorCobrarGetByInstitucionID Informes = new frmCuentasPorCobrarGetByInstitucionID();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_RELACION_INGRESOS":
                        {
                            frmRelacionIngresosGetByFecha Informes = new frmRelacionIngresosGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_CUOTAS_VENCIDAS":
                        {
                            frmCuotasVencidasGetByFecha Informes = new frmCuotasVencidasGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_RELACION_CONTRATOS":
                        {
                            frmRelacionContratosGetByFecha Informes = new frmRelacionContratosGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_LISTADO_DATACREDITO":
                        {
                            frmListadoDataCreditoGetByFecha Informes = new frmListadoDataCreditoGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_LISTADO_TRANSUNION":
                        {
                            frmListadoTransUnionGetByFecha Informes = new frmListadoTransUnionGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_ANTIGUEDAD_SALDO":
                        {
                            frmAntiguedadSaldos Informes = new frmAntiguedadSaldos();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_CONTRATOS_VENCIDOS":
                        {
                            frmContratosVencidosGetByFecha Informes = new frmContratosVencidosGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    case "INF_LISTADO_COMISIONES":
                        {
                            frmListadoComisionesGetByFecha Informes = new frmListadoComisionesGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;
                    case "INF_CUOTAS_A_VENCER":                        
                        {
                            frmCuotasAVencerGetByFecha Informes = new frmCuotasAVencerGetByFecha();
                            Informes.Owner = clsVariablesBO.Host;
                            Informes.ShowDialog();
                        }
                        break;

                    #endregion

                    #region Contabilidad  

                    case "CON_BALANCE_GENERAL":
                        {
                            frmBalanceGeneralGetByFecha Herramienta = new frmBalanceGeneralGetByFecha();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "CON_BALANZA_COMPROBACION":
                        {
                            frmBalanzaComprobacionGetByFecha Herramienta = new frmBalanzaComprobacionGetByFecha();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "CON_ESTADO_RESULTADOS":
                        {
                            frmEstadoResultadosGetByFecha Herramienta = new frmEstadoResultadosGetByFecha();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "CON_EXPENDITURES":
                        {
                            frmConsultasDesembolsos Herramienta = new frmConsultasDesembolsos();
                            Herramienta.Owner = clsVariablesBO.Host;
                            Herramienta.ShowDialog();
                        }
                        break;

                    case "CON_GRUPOS":
                        {
                            frmConsultasGrupos Grupos = new frmConsultasGrupos();
                            Grupos.Owner =  clsVariablesBO.Host;
                            Grupos.ShowDialog();
                        }
                        break;
                    case "CON_SUBGRUPOS":
                        {
                            frmConsultasSubGrupos SubGrupos = new frmConsultasSubGrupos();
                            SubGrupos.Owner = clsVariablesBO.Host;
                            SubGrupos.ShowDialog();
                        }
                        break;
                    case "CON_CUENTASCONTROL":
                        {
                            frmConsultasCuentaControl CuentaControl = new frmConsultasCuentaControl();
                            CuentaControl.Owner = clsVariablesBO.Host;
                            CuentaControl.ShowDialog();
                        }
                        break;
                    case "CON_SUBCUENTASCONTROL":
                        {
                            frmConsultasSubCuentaControl SubCuentaControl = new frmConsultasSubCuentaControl();
                            SubCuentaControl.Owner = clsVariablesBO.Host;
                            SubCuentaControl.ShowDialog();
                        }
                        break;
                    case "CON_AUXILIARES":
                        {
                            frmConsultasAuxiliares Auxiliares = new frmConsultasAuxiliares();
                            Auxiliares.Owner = clsVariablesBO.Host;
                            Auxiliares.ShowDialog();
                        }
                        break;

                    case "CON_ENTRADAS":
                        {
                            frmConsultasEntradas Entradas = new frmConsultasEntradas();
                            Entradas.Owner = clsVariablesBO.Host;
                            Entradas.ShowDialog();
                        }
                        break;
                    #endregion

                    #region Bancos

                    case "BAN_BANK":
                        {
                            frmConsultasBancos Bancos = new frmConsultasBancos();
                            Bancos.Owner = clsVariablesBO.Host;
                            Bancos.ShowDialog();
                        }
                        break;

                    case "BAN_APLICATION_CHECK":
                        {
                            frmConsultasSolicitudCheques Bancos = new frmConsultasSolicitudCheques();
                            Bancos.Owner = clsVariablesBO.Host;
                            Bancos.ShowDialog();
                        }
                        break;

                    case "BAN_CHECK_BANK":
                        {
                            frmConsultasCheques Bancos = new frmConsultasCheques();
                            Bancos.Owner = clsVariablesBO.Host;
                            Bancos.ShowDialog();
                        }
                        break;

                    #endregion

                    default:
                        {
                            clsMessage.ErrorMessage("Este usuario no tiene permisos para ejecutar esta accion.", "Mensaje");
                        } break;
                }

            }
            catch { }


            //clsMessage.ErrorMessage(w.ToString() + " x " + h.ToString()); REG_CONTRATOS
        }
    }
}
