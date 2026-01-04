using System.Data;
using MySql.Data.MySqlClient;
using System.Data.Entity;
using Payment.Host.DAL;
using Payment.Host.Entity;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Globalization;

namespace Payment.Host.Core
{

    public class Manager
    {
        private Context db;

        #region Historial

        public OperationResult HistorialCreate(string EmpresaID, string Documento, int ContratoID, DateTime Fecha, DateTime Vence, float Monto, float Pagado, float Balance, float Cuota, int Cantidad, float Atraso, float? MontoUltimoPago, DateTime? FechaUltimoPago, string Status, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                int HistorialID = 0;

                MySqlConnection cn = new MySqlConnection();
                MySqlDataAdapter da = new MySqlDataAdapter();
                MySqlCommand cmd = new MySqlCommand();

                var row = db.clsHistorialBE.Where(x => x.Documento == Documento && x.EmpresaID == EmpresaID && x.ContratoID == ContratoID).FirstOrDefault();
                if (row == null)
                {
                    cn.ConnectionString = db.Database.Connection.ConnectionString;

                    cn.Open();
                    cmd = new MySqlCommand();
                    cmd.CommandText = "SELECT IFNULL(Max(HistorialID), 100000) +1 as ID from Historial;";
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = cn;

                    MySqlDataReader drPersonas = cmd.ExecuteReader();
                    while (drPersonas.Read())
                    {
                        HistorialID = int.Parse(drPersonas["ID"].ToString());
                    }

                    cn.Close();

                    cn.Open();
                    // Create the InsertCommand.
                    cmd = new MySqlCommand("INSERT INTO Historial (HistorialID, EmpresaID, Documento, ContratoID, Fecha, Vence, Monto, Pagado, Balance, Cuota, Cantidad, Atraso, MontoUltimoPago, FechaUltimoPago, Status, Usuario, ModificadoPor,  FechaModificacion) VALUES (@HistorialID, @EmpresaID, @Documento, @ContratoID, @Fecha, @Vence, @Monto, @Pagado, @Balance, @Cuota, @Cantidad, @Atraso, @MontoUltimoPago, @FechaUltimoPago, @Status, @Usuario, @Usuario, SYSDATE());", cn);
                    cmd.Parameters.Add(new MySqlParameter("@HistorialID", HistorialID));
                    cmd.Parameters.Add(new MySqlParameter("@EmpresaID", EmpresaID));
                    cmd.Parameters.Add(new MySqlParameter("@Documento", Documento));
                    cmd.Parameters.Add(new MySqlParameter("@ContratoID", ContratoID));
                    cmd.Parameters.Add(new MySqlParameter("@Fecha", Fecha));
                    cmd.Parameters.Add(new MySqlParameter("@Vence", Vence));
                    cmd.Parameters.Add(new MySqlParameter("@Monto", Monto));
                    cmd.Parameters.Add(new MySqlParameter("@Pagado", Pagado));
                    cmd.Parameters.Add(new MySqlParameter("@Balance", Balance));
                    cmd.Parameters.Add(new MySqlParameter("@Cuota", Cuota));
                    cmd.Parameters.Add(new MySqlParameter("@Cantidad", Cantidad));
                    cmd.Parameters.Add(new MySqlParameter("@Atraso", Atraso));
                    cmd.Parameters.Add(new MySqlParameter("@MontoUltimoPago", MontoUltimoPago));
                    cmd.Parameters.Add(new MySqlParameter("@FechaUltimoPago", FechaUltimoPago));

                    cmd.Parameters.Add(new MySqlParameter("@Status", Status));
                    cmd.Parameters.Add(new MySqlParameter("@Usuario", Usuario));

                    da.InsertCommand = cmd;
                    cmd.ExecuteNonQuery();
                    cn.Close();

                    result = new OperationResult { ResponseCode = "00", ResponseMessage = "" };
                }
                else
                {
                    result = HistorialUpdate(row.HistorialID, Documento, ContratoID, Fecha, Vence, Monto, Pagado, Balance, Cuota, Cantidad, Atraso, MontoUltimoPago, FechaUltimoPago, Status, Usuario);
                }

                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.InnerException.Message };
                return result;
            }
        }

        public OperationResult HistorialUpdate(int HistorialID, string Documento, int ContratoID, DateTime Fecha, DateTime Vence, float Monto, float Pagado, float Balance, float Cuota, int Cantidad, float Atraso, float? MontoUltimoPago, DateTime? FechaUltimoPago, string Status, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsHistorialBE.Where(x => x.HistorialID == HistorialID).FirstOrDefault();
                row.Fecha = Fecha;
                row.Vence = Vence;
                row.Monto = Monto;
                row.Pagado = Pagado;
                row.Balance = Balance;
                row.Cuota = Cuota;
                row.Cantidad = Cantidad;
                row.Atraso = Atraso;
                row.MontoUltimoPago = MontoUltimoPago;
                row.FechaUltimoPago = FechaUltimoPago;
                row.Status = Status;

                row.FechaModificacion = DateTime.Now;
                row.ModificadoPor = Usuario;
                db.Entry(row).State = EntityState.Modified;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "" };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        //public clsHistoriasBE HistoriasGetByHistoriaID(int HistoriaID)
        //{
        //    try
        //    {
        //        db = new DAL.Context();
        //        return db.clsHistoriasBE.Where(x => x.HistoriaID == HistoriaID).FirstOrDefault();

        //    }
        //    catch
        //    {
        //        return new clsHistoriasBE();
        //    }
        //}

        //public List<clsHistoriasBE> HistoriasGet(string Search)
        //{
        //    try
        //    {
        //        db = new DAL.Context();
        //        if (!string.IsNullOrEmpty(Search))
        //        {
        //            return db.clsHistoriasBE.Where(x => x.Personas.Nombres.Contains(Search) || x.Personas.Apellidos.Contains(Search) || x.Personas.Documento.Contains(Search)).ToList();
        //        }
        //        else
        //        {
        //            return db.clsHistoriasBE.ToList();
        //        }
        //    }
        //    catch
        //    {
        //        return new List<clsHistoriasBE>();
        //    }
        //}

        //public OperationResult HistoriasDelete(int HistoriaID)
        //{
        //    OperationResult result;
        //    try
        //    {
        //        db = new DAL.Context();
        //        var row = db.clsHistoriasBE.Where(x => x.HistoriaID == HistoriaID).FirstOrDefault();
        //        row.Estado = false;
        //        db.Entry(row).State = EntityState.Modified;
        //        db.SaveChanges();
        //        result = new OperationResult { ResponseCode = "00", ResponseMessage = "" };
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        NotificarException(ex);
        //        result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
        //        return result;
        //    }
        //}

        //public clsHistoriasBE HistoriasGetByDocumento(string Documento, int SucursalID)
        //{
        //    try
        //    {
        //        db = new DAL.Context();
        //        return db.clsHistoriasBE.Where(x => x.Documento == Documento && x.SucursalID == SucursalID).FirstOrDefault();
        //    }
        //    catch
        //    {
        //        return new clsHistoriasBE();
        //    }
        //}

        #endregion

        #region Empresas

        public List<clsEmpresasBE> EmpresasGet()
        {
            try
            {
                db = new DAL.Context();
                return db.clsEmpresasBE.ToList();
            }
            catch
            {
                return new List<clsEmpresasBE>();
            }
        }

        #endregion

        #region Sexos

        public List<clsSexosBE> SexosGet(string Search)
        {
            try
            {
                db = new DAL.Context();
                db.Configuration.ProxyCreationEnabled = false;
                if (!string.IsNullOrEmpty(Search))
                {
                    return db.clsSexosBE.Where(x => x.Sexo.Contains(Search)).ToList();
                }
                else
                {
                    return db.clsSexosBE.ToList();
                }
            }
            catch
            {
                return new List<clsSexosBE>();
            }
        }

        #endregion

        #region Estados Civiles

        public List<clsEstadosCivilesBE> EstadosCivilesGet(string Search)
        {
            try
            {
                db = new DAL.Context();
                db.Configuration.ProxyCreationEnabled = false;

                if (!string.IsNullOrEmpty(Search))
                {
                    return db.clsEstadosCivilesBE.Where(x => x.EstadoCivil.Contains(Search)).ToList();
                }
                else
                {
                    return db.clsEstadosCivilesBE.ToList();
                }
            }
            catch
            {
                return new List<clsEstadosCivilesBE>();
            }
        }

        #endregion

        #region Paises

        public OperationResult PaisesCreate(string Pais, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                int ID = 0;
                ID = (db.clsPaisesBE.Count() == 0 ? 100000 : db.clsPaisesBE.Max(x => x.PaisID)) + 1;
                db.clsPaisesBE.Add(new clsPaisesBE { PaisID = ID, Fecha = DateTime.Now, Pais = Pais, Usuario = Usuario, ModificadoPor = Usuario, FechaModificacion = DateTime.Now });
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult PaisesUpdate(int PaisID, string Pais, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsPaisesBE.Where(x => x.PaisID == PaisID).FirstOrDefault();
                row.Pais = Pais;
                row.FechaModificacion = DateTime.Now;
                row.ModificadoPor = Usuario;
                db.Entry(row).State = EntityState.Modified;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult PaisesDelete(int PaisID)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsPaisesBE.Where(x => x.PaisID == PaisID).FirstOrDefault();
                db.Entry(row).State = EntityState.Deleted;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public List<clsPaisesBE> PaisesGet(string Search)
        {
            try
            {
                db = new DAL.Context();
                db.Configuration.ProxyCreationEnabled = false;
                if (!string.IsNullOrEmpty(Search))
                {
                    return db.clsPaisesBE.Where(x => x.Pais.Contains(Search)).ToList();
                }
                else
                {
                    return db.clsPaisesBE.ToList();
                }
            }
            catch
            {
                return new List<clsPaisesBE>();
            }
        }

        #endregion

        #region Provincias

        public OperationResult ProvinciasCreate(string Provincia, int PaisID, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                int ID = 0;
                ID = (db.clsProvinciasBE.Count() == 0 ? 100000 : db.clsProvinciasBE.Max(x => x.ProvinciaID)) + 1;
                db.clsProvinciasBE.Add(new clsProvinciasBE { ProvinciaID = ID, Fecha = DateTime.Now, Provincia = Provincia, PaisID = PaisID, Usuario = Usuario, ModificadoPor = Usuario, FechaModificacion = DateTime.Now });
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult ProvinciasUpdate(int ProvinciaID, string Provincia, int PaisID, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsProvinciasBE.Where(x => x.ProvinciaID == ProvinciaID).FirstOrDefault();
                row.Provincia = Provincia;
                row.PaisID = PaisID;
                row.FechaModificacion = DateTime.Now;
                row.ModificadoPor = Usuario;
                db.Entry(row).State = EntityState.Modified;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult ProvinciasDelete(int ProvinciaID)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsProvinciasBE.Where(x => x.ProvinciaID == ProvinciaID).FirstOrDefault();
                db.Entry(row).State = EntityState.Deleted;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public List<clsProvinciasBE> ProvinciasGet(string Search)
        {
            try
            {
                db = new DAL.Context();
                db.Configuration.ProxyCreationEnabled = false;
                if (!string.IsNullOrEmpty(Search))
                {
                    return db.clsProvinciasBE.Where(x => x.Provincia.Contains(Search) || x.Paises.Pais.Contains(Search)).ToList();
                }
                else
                {
                    return db.clsProvinciasBE.ToList();
                }
            }
            catch
            {
                return new List<clsProvinciasBE>();
            }
        }

        #endregion

        #region Ciudades

        public OperationResult CiudadesCreate(string Ciudad, int ProvinciaID, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                int ID = 0;
                ID = (db.clsCiudadesBE.Count() == 0 ? 100000 : db.clsCiudadesBE.Max(x => x.CiudadID)) + 1;
                db.clsCiudadesBE.Add(new clsCiudadesBE { CiudadID = ID, Fecha = DateTime.Now, Ciudad = Ciudad, ProvinciaID = ProvinciaID, Usuario = Usuario, ModificadoPor = Usuario, FechaModificacion = DateTime.Now });
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult CiudadesUpdate(int CiudadID, string Ciudad, int ProvinciaID, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsCiudadesBE.Where(x => x.CiudadID == CiudadID).FirstOrDefault();
                row.Ciudad = Ciudad;
                row.ProvinciaID = ProvinciaID;
                row.FechaModificacion = DateTime.Now;
                row.ModificadoPor = Usuario;
                db.Entry(row).State = EntityState.Modified;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult CiudadesDelete(int CiudadID)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsCiudadesBE.Where(x => x.CiudadID == CiudadID).FirstOrDefault();
                db.Entry(row).State = EntityState.Deleted;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public List<clsCiudadesBE> CiudadesGet(string Search)
        {
            try
            {
                db = new DAL.Context();
                db.Configuration.ProxyCreationEnabled = false;
                if (!string.IsNullOrEmpty(Search))
                {
                    return db.clsCiudadesBE.Where(x => x.Ciudad.Contains(Search) || x.Provincias.Provincia.Contains(Search)).OrderBy(x => x.Ciudad).ToList();
                }
                else
                {
                    return db.clsCiudadesBE.OrderBy(x => x.Ciudad).ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<clsCiudadesBE>();
            }
        }

        #endregion

        #region Preguntas

        public OperationResult PreguntasCreate(string Pregunta, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                int ID = 0;
                ID = (db.clsPreguntasBE.Count() == 0 ? 100000 : db.clsPreguntasBE.Max(x => x.PreguntaID)) + 1;
                db.clsPreguntasBE.Add(new clsPreguntasBE { PreguntaID = ID, Fecha = DateTime.Now, Pregunta = Pregunta, Usuario = Usuario, ModificadoPor = Usuario, FechaModificacion = DateTime.Now });
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult PreguntasUpdate(int PreguntaID, string Pregunta, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsPreguntasBE.Where(x => x.PreguntaID == PreguntaID).FirstOrDefault();
                row.Pregunta = Pregunta;
                row.FechaModificacion = DateTime.Now;
                row.ModificadoPor = Usuario;
                db.Entry(row).State = EntityState.Modified;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult PreguntasDelete(int PreguntaID)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsPreguntasBE.Where(x => x.PreguntaID == PreguntaID).FirstOrDefault();
                db.Entry(row).State = EntityState.Deleted;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public List<clsPreguntasBE> PreguntasGet(string Search)
        {
            try
            {
                db = new DAL.Context();
                db.Configuration.ProxyCreationEnabled = false;
                if (!string.IsNullOrEmpty(Search))
                {
                    return db.clsPreguntasBE.Where(x => x.Pregunta.Contains(Search)).ToList();
                }
                else
                {
                    return db.clsPreguntasBE.ToList();
                }
            }
            catch (Exception ex)
            {
                return new List<clsPreguntasBE>();
            }
        }

        #endregion

        #region Operadores

        public OperationResult OperadoresCreate(string Operador, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                int ID = 0;
                ID = (db.clsOperadoresBE.Count() == 0 ? 100000 : db.clsOperadoresBE.Max(x => x.OperadorID)) + 1;
                db.clsOperadoresBE.Add(new clsOperadoresBE { OperadorID = ID, Fecha = DateTime.Now, Operador = Operador, Usuario = Usuario, ModificadoPor = Usuario, FechaModificacion = DateTime.Now });
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        private void NotificarException(Exception ex)
        {
            //throw new NotImplementedException();
        }

        public OperationResult OperadoresUpdate(int OperadorID, string Operador, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsOperadoresBE.Where(x => x.OperadorID == OperadorID).FirstOrDefault();
                row.Operador = Operador;
                row.FechaModificacion = DateTime.Now;
                row.ModificadoPor = Usuario;
                db.Entry(row).State = EntityState.Modified;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult OperadoresDelete(int OperadorID)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsOperadoresBE.Where(x => x.OperadorID == OperadorID).FirstOrDefault();
                db.Entry(row).State = EntityState.Deleted;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public List<clsOperadoresBE> OperadoresGet(string Search)
        {
            try
            {
                db = new DAL.Context();
                db.Configuration.ProxyCreationEnabled = false;
                if (!string.IsNullOrEmpty(Search))
                {
                    return db.clsOperadoresBE.Where(x => x.Operador.Contains(Search)).ToList();
                }
                else
                {
                    return db.clsOperadoresBE.ToList();
                }
            }
            catch
            {
                return new List<clsOperadoresBE>();
            }
        }

        #endregion

        #region Personas

        public Entity.OperationResult PersonasCreate(string Documento, DateTime FechaNacimiento, string Nombres, string Apellidos, string Apodo, string Fotografia, int CiudadID, string Direccion, string Correo, string Telefono, int OperadorID, string Celular, int SexoID, int EstadoCivilID, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var existe = db.clsPersonasBE.Where(x => x.Documento == Documento);
                if (existe.Count() == 0)
                {
                    db.clsPersonasBE.Add(new clsPersonasBE { Documento = Documento, Fecha = DateTime.Now, FechaNacimiento = FechaNacimiento, Nombres = Nombres, Apellidos = Apellidos, Apodo = Apodo, CiudadID = CiudadID, Direccion = Direccion, Correo = Correo.Trim(), Telefono = Telefono, OperadorID = OperadorID, Celular = Celular, SexoID = SexoID, EstadoCivilID = EstadoCivilID, Usuario = Usuario, ModificadoPor = Usuario, FechaModificacion = DateTime.Now, IsAsync = true });
                    db.SaveChanges();
                    result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };

                    if (!string.IsNullOrEmpty(Fotografia))
                    {
                        SavePic(Documento, Fotografia, Usuario);
                    }
                }
                else
                {
                    result = PersonasUpdate(Documento, FechaNacimiento, Nombres, Apellidos, Apodo, Fotografia, CiudadID, Direccion, Correo.Replace(" ", ""), Telefono, OperadorID, Celular, SexoID, EstadoCivilID, Usuario);
                }
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.InnerException.Message };
                return result;
            }
        }

        private OperationResult SavePic(string Documento, string fotografia, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var BE = db.clsFotografiasBE.Where(x => x.Documento == Documento).FirstOrDefault();
                if (BE == null)
                {
                    db.clsFotografiasBE.Add(new clsFotografiasBE { Documento = Documento, Fecha = DateTime.Now, Foto = System.IO.File.ReadAllBytes(fotografia), Usuario = Usuario, ModificadoPor = Usuario, FechaModificacion = DateTime.Now });
                }
                else
                {
                    BE.Foto = System.IO.File.ReadAllBytes(fotografia);
                    BE.ModificadoPor = Usuario;
                    BE.FechaModificacion = DateTime.Now;
                    db.Entry(BE).State = EntityState.Modified;
                }
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex) { return new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message }; }
        }

        public OperationResult PersonasUpdate(string Documento, DateTime FechaNacimiento, string Nombres, string Apellidos, string Apodo, string Fotografia, int CiudadID, string Direccion, string Correo, string Telefono, int OperadorID, string Celular, int SexoID, int EstadoCivilID, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var _row = db.clsPersonasBE.Where(x => x.Documento == Documento);
                if (_row.Count() == 0)
                {
                    db.clsPersonasBE.Add(new clsPersonasBE { Documento = Documento, Fecha = DateTime.Now, FechaNacimiento = FechaNacimiento, Nombres = Nombres, Apellidos = Apellidos, Apodo = Apodo, CiudadID = CiudadID, Direccion = Direccion, Correo = Correo.Trim(), Telefono = Telefono, OperadorID = OperadorID, Celular = Celular, SexoID = SexoID, EstadoCivilID = EstadoCivilID, Usuario = Usuario, ModificadoPor = Usuario, FechaModificacion = DateTime.Now, IsAsync = true });
                    db.SaveChanges();
                    result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };

                    if (!string.IsNullOrEmpty(Fotografia))
                    {
                        SavePic(Documento, Fotografia, Usuario);
                    }
                }
                else
                {
                    var row = _row.FirstOrDefault();
                    row.Documento = Documento;
                    row.FechaNacimiento = FechaNacimiento;
                    row.Nombres = Nombres;
                    row.Apellidos = Apellidos;
                    row.Apodo = Apodo;

                    row.CiudadID = CiudadID;
                    row.Direccion = Direccion;
                    row.Correo = Correo.Trim();
                    row.Telefono = Telefono;
                    row.OperadorID = OperadorID;
                    row.Celular = Celular;
                    row.SexoID = SexoID;
                    row.EstadoCivilID = EstadoCivilID;
                    row.ModificadoPor = Usuario;
                    row.FechaModificacion = DateTime.Now;
                    row.IsAsync = false;
                    db.Entry(row).State = EntityState.Modified;
                    db.SaveChanges();
                    result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };

                    if (!string.IsNullOrEmpty(Fotografia))
                    {
                        SavePic(Documento, Fotografia, Usuario);
                    }
                }
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult PersonasUpdateMovil(string Documento, string Direccion, string Correo, string Telefono, string Celular)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsPersonasBE.Where(x => x.Documento == Documento).FirstOrDefault();
                if (row != null)
                {
                    row.Direccion = Direccion;
                    row.Correo = Correo;
                    row.Telefono = Telefono;
                    row.Celular = Celular;
                    db.Entry(row).State = EntityState.Modified;
                    db.SaveChanges();
                }
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.InnerException.Message };
                return result;
            }
        }
        public OperationResult PersonasUpdateAsync(string Documento)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                int ID = 0;
                var row = db.clsPersonasBE.Where(x => x.Documento == Documento).FirstOrDefault();
                if (row != null)
                {
                    row.IsAsync = true;
                    db.Entry(row).State = EntityState.Modified;
                    db.SaveChanges();
                }
                result = new OperationResult { ResponseCode = "00", ResponseMessage = ID.ToString() };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.InnerException.Message };
                return result;
            }
        }

        public clsPersonasBE PersonasGetByDocumento(string Documento)
        {
            clsPersonasBE result = new clsPersonasBE();
            try
            {
                db = new DAL.Context();
                //db.Configuration.ProxyCreationEnabled = false;
                result = db.clsPersonasBE.Where(x => x.Documento == Documento).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public List<clsHistorialBE> DataCreditosGetByDocumento(string Documento)
        {
            List<clsHistorialBE> result = new List<clsHistorialBE>();
            try
            {
                db = new DAL.Context();
                //db.Configuration.ProxyCreationEnabled = false;
                result = db.clsHistorialBE.Where(x => x.Documento == Documento && x.Cantidad>=0).ToList();
                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }


        public List<clsPersonasBE> PersonasGet()
        {
            List<clsPersonasBE> result = new List<clsPersonasBE>();
            try
            {
                db = new DAL.Context();
                result = db.clsPersonasBE.ToList();

                return result;
            }
            catch
            {
                return result;
            }
        }

        public clsFotografiasBE FotografiasGetByDocumento(string Documento)
        {
            try
            {
                db = new DAL.Context();
                return db.clsFotografiasBE.Where(x => x.Documento == Documento).FirstOrDefault();
            }
            catch
            {
                return new clsFotografiasBE();
            }
        }


        #endregion

        #region Welcome
        public String Bienvenida(string Nombres)
        {
            try
            {
                db = new DAL.Context();
                String Saludo = String.Empty;

                DataSet ds = new DataSet();
                MySqlConnection cn = new MySqlConnection();
                cn.ConnectionString = cn.ConnectionString = db.Database.Connection.ConnectionString;
                cn.Open();

                MySqlCommand cm = new MySqlCommand();
                cm.CommandText = "SELECT CASE WHEN CURRENT_TIME() BETWEEN '06:00:00:000' and '11:59:59:999' THEN '!Buenos Dias' WHEN CURRENT_TIME() BETWEEN '12:00:00:000' and '18:59:59:999' THEN '¡Buenas Tardes' ELSE '!Buenas Noches' END as Saludo;";
                cm.CommandType = CommandType.Text;
                cm.Connection = cn;

                MySqlDataReader drPersonas = cm.ExecuteReader();
                while (drPersonas.Read())
                {
                    Saludo = $"{drPersonas["Saludo"].ToString()}, {ToTitleCase(Nombres)}!";
                }

                return Saludo;
            }
            catch (Exception Ex)
            {
                return string.Empty;
            }
        }

        public string ToTitleCase(string str)
        {
            return CultureInfo.CurrentCulture.TextInfo.ToTitleCase(str.ToLower());
        }

        #endregion

        #region Usuarios

        public clsUsuariosBE LogInEmail(string Documento, string Contraseña)
        {
            try
            {
                db = new DAL.Context();
                var ID = db.clsPersonasBE.Where(x => x.Correo == Documento).FirstOrDefault().Documento;
                string pwdEncoded = Common.Security.GetHash(ID + Contraseña, Common.HashType.SHA256);
                pwdEncoded = Common.FingerPrint.GetHash(pwdEncoded);

                var result = db.clsUsuariosBE.Where(x => x.Documento == ID && x.Contraseña == pwdEncoded).FirstOrDefault();
                return result;
            }
            catch
            {
                return new clsUsuariosBE();
            }
        }

        public clsUsuariosBE LogIn(string Documento, string Contraseña)
        {
            try
            {
                db = new DAL.Context();

                string pwdEncoded = Common.Security.GetHash(Documento + Contraseña, Common.HashType.SHA256);
                pwdEncoded = Common.FingerPrint.GetHash(pwdEncoded);
                return db.clsUsuariosBE.Where(x => x.Personas.Documento == Documento && x.Contraseña == pwdEncoded).FirstOrDefault();
            }
            catch
            {
                return new clsUsuariosBE();
            }
        }

        public clsUsuariosBE UsuariosGetByDocumento(string Documento)
        {
            try
            {
                db = new DAL.Context();
                return db.clsUsuariosBE.Where(x => x.Documento == Documento).FirstOrDefault();

            }
            catch
            {
                return new clsUsuariosBE();
            }
        }

        public string PinGetByDocumento(string Documento, string Contraseña)
        {
            db = new DAL.Context();
            var BE = db.clsUsuariosBE.Where(x => x.Documento == Documento).FirstOrDefault();
            string pwdEncoded = Common.Security.GetHash(Documento + Contraseña, Common.HashType.SHA256);
            pwdEncoded = Common.FingerPrint.GetHash(pwdEncoded);
            BE.Contraseña = pwdEncoded;
            BE.ModificadoPor = Documento;
            BE.FechaModificacion = DateTime.Now;
            db.Entry(BE).State = EntityState.Modified;
            db.SaveChanges();
            return Contraseña.ToString();
        }

        public OperationResult UsuariosCreate(string Documento, int Contraseña, int PreguntaID, string Respuesta, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                string pwdEncoded = Common.Security.GetHash(Documento + Contraseña, Common.HashType.SHA256);
                pwdEncoded = Common.FingerPrint.GetHash(pwdEncoded);
                var row = db.clsUsuariosBE.Where(x => x.Documento == Documento).FirstOrDefault();
                if (row == null)
                {
                    db.clsUsuariosBE.Add(new clsUsuariosBE { Documento = Documento, Fecha = DateTime.Now, Contraseña = pwdEncoded, PreguntaID = PreguntaID, Respuesta = Common.Security.Encryption(Respuesta), EstadoID = true, Usuario = Usuario, ModificadoPor = Usuario, FechaModificacion = DateTime.Now });
                    db.SaveChanges();
                    result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                }
                else
                {
                    result = new OperationResult { ResponseCode = "01", ResponseMessage = "Este usuario ya existe." };
                }
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult UsuariosUpdate(string Documento, string Contraseña, int PreguntaID, string Respuesta, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsUsuariosBE.Where(x => x.Documento == Documento).FirstOrDefault();
                row.PreguntaID = PreguntaID;
                row.Respuesta = Common.Security.Encryption(Respuesta);

                row.FechaModificacion = DateTime.Now;
                row.ModificadoPor = Usuario;
                db.Entry(row).State = EntityState.Modified;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult PinUpdateGetByDocumento(string Documento, string Contraseña, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                string pwdEncoded = Common.Security.GetHash(Documento + Contraseña, Common.HashType.SHA256);
                pwdEncoded = Common.FingerPrint.GetHash(pwdEncoded);

                var row = db.clsUsuariosBE.Where(x => x.Documento == Documento).FirstOrDefault();
                row.Contraseña = pwdEncoded;

                row.FechaModificacion = DateTime.Now;
                row.ModificadoPor = Usuario;
                db.Entry(row).State = EntityState.Modified;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "Registro guardado correctamente." };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public List<clsUsuariosBE> UsuariosGet(string Search)
        {
            try
            {
                db = new DAL.Context();
                if (!string.IsNullOrEmpty(Search))
                {
                    return db.clsUsuariosBE.Where(x => x.Personas.Documento.Contains(Search) || x.Personas.Nombres.Contains(Search) || x.Personas.Apellidos.Contains(Search) || x.Personas.Ciudades.Ciudad.Contains(Search) || x.Personas.Correo.Contains(Search) || x.Personas.Telefono.Contains(Search) || x.Personas.Celular.Contains(Search)).ToList();
                }
                else
                {
                    return db.clsUsuariosBE.ToList();
                }
            }
            catch
            {
                return new List<clsUsuariosBE>();
            }
        }

        #endregion

        #region Referencias

        public OperationResult ReferenciasCreate(int TipoReferenciaID, string Documento, string Referencia, string Direccion, String Telefono, string Usuario)
        {
            OperationResult result;
            try
            {
                int ID = 0;
                db = new DAL.Context();
                ID = (db.clsReferenciasBE.Count() == 0 ? 100000 : db.clsReferenciasBE.Max(x => x.ReferenciaID)) + 1;
                db.clsReferenciasBE.Add(new clsReferenciasBE { TipoReferenciaID = TipoReferenciaID, ReferenciaID = ID, Fecha = DateTime.Now, Documento = Documento, Referencia = Referencia, Direccion = Direccion, Telefono = Telefono, Usuario = Usuario, ModificadoPor = Usuario, FechaModificacion = DateTime.Now });
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "" };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult ReferenciasUpdate(int ReferenciaID, int TipoReferenciaID, string Documento, string Referencia, string Direccion, String Telefono, string Usuario)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsReferenciasBE.Where(x => x.ReferenciaID == ReferenciaID).FirstOrDefault();
                row.TipoReferenciaID = TipoReferenciaID;
                row.Referencia = Referencia;
                row.Direccion = Direccion;
                row.Telefono = Telefono;
                row.FechaModificacion = DateTime.Now;
                row.ModificadoPor = Usuario;
                db.Entry(row).State = EntityState.Modified;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "" };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult ReferenciasDeleteGetByDocumento(string Documento)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsReferenciasBE.Where(x => x.Documento == Documento);
                db.clsReferenciasBE.RemoveRange(row);
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "" };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public OperationResult ReferenciasDelete(int ReferenciaID)
        {
            OperationResult result;
            try
            {
                db = new DAL.Context();
                var row = db.clsReferenciasBE.Where(x => x.ReferenciaID == ReferenciaID).FirstOrDefault();
                db.Entry(row).State = EntityState.Deleted;
                db.SaveChanges();
                result = new OperationResult { ResponseCode = "00", ResponseMessage = "" };
                return result;
            }
            catch (Exception ex)
            {
                NotificarException(ex);
                result = new OperationResult { ResponseCode = "01", ResponseMessage = ex.Message };
                return result;
            }
        }

        public List<clsReferenciasBE> ReferenciasGet(string Search)
        {
            try
            {
                db = new DAL.Context();
                if (!string.IsNullOrEmpty(Search))
                {
                    return db.clsReferenciasBE.Where(x => x.Referencia.Contains(Search)).ToList();
                }
                else
                {
                    return db.clsReferenciasBE.ToList();
                }
            }
            catch
            {
                return new List<clsReferenciasBE>();
            }
        }

        public List<clsReferenciasBE> ReferenciasGetByDocumento(string documento)
        {
            try
            {
                db = new DAL.Context();
                return db.clsReferenciasBE.Where(x => x.Documento == documento).ToList();
            }
            catch
            {
                return new List<clsReferenciasBE>();
            }
        }

        #endregion
    }
}
