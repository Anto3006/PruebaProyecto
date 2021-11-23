using Planetario.Models;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
namespace Planetario.Handlers {
  public class VisitanteHandler : BaseDeDatosHandler{

    public void AgregarVisitante(VisitanteModel visitante) {
      String consulta = "INSERT INTO Visitante VALUES(@nombre,@primerApellido,@segundoApellido,@pais,@correo," +
        "@numeroIdentificacionPK,@genero,@fechaDeNacimiento,@nivelEducativo)";
      ConexionPlanetario.Open();
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@nombre", visitante.Nombre);
      comandoParaConsulta.Parameters.AddWithValue("@primerApellido", visitante.PrimerApellido);
      if(visitante.SegundoApellido == null) {
        comandoParaConsulta.Parameters.AddWithValue("@segundoApellido", "  ");
      } else {
        comandoParaConsulta.Parameters.AddWithValue("@segundoApellido", visitante.SegundoApellido);
      }
      comandoParaConsulta.Parameters.AddWithValue("@pais", visitante.Pais);
      comandoParaConsulta.Parameters.AddWithValue("@correo", visitante.Correo);
      comandoParaConsulta.Parameters.AddWithValue("@numeroIdentificacionPK", visitante.NumeroIdentificacion);
      comandoParaConsulta.Parameters.AddWithValue("@genero", visitante.Genero);
      comandoParaConsulta.Parameters.AddWithValue("@fechaDeNacimiento", visitante.FechaDeNacimiento);
      comandoParaConsulta.Parameters.AddWithValue("@nivelEducativo", visitante.NivelEducativo);
      int exitoCambiarFilas= comandoParaConsulta.ExecuteNonQuery();
      ComprobarCambiosEnTabla(exitoCambiarFilas);
      ConexionPlanetario.Close();

    }

    public void AgregarInscripcion(VisitanteModel visitante) {
      String consulta = "INSERT INTO EstaInscritoEn VALUES(@tituloActividadFK,@fechaActividadFK,@numeroIdentificacionVisitanteFK)";
      ConexionPlanetario.Open();
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      comandoParaConsulta.Parameters.AddWithValue("@tituloActividadFK", visitante.TituloActividadInscrita);
      comandoParaConsulta.Parameters.AddWithValue("@fechaActividadFK", visitante.FechaActividadInscrita);
      comandoParaConsulta.Parameters.AddWithValue("@numeroIdentificacionVisitanteFK", visitante.NumeroIdentificacion);
      int exitoCambiarFilas = comandoParaConsulta.ExecuteNonQuery();
      ComprobarCambiosEnTabla(exitoCambiarFilas);
      ConexionPlanetario.Close();
    }

    public VisitanteModel RecuperarVisitante(long numeroIdentificacion) {
      String consulta = "SELECT * FROM Visitante WHERE numeroIdentificacionPK =" + numeroIdentificacion;
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable tablaConsultada = CrearTablaConsulta(comandoParaConsulta);
      VisitanteModel visitante = new VisitanteModel();
      foreach (DataRow filaVisitante in tablaConsultada.Rows) {
          visitante.NumeroIdentificacion = Convert.ToInt64(filaVisitante["numeroIdentificacionPK"]);
          visitante.Nombre = Convert.ToString(filaVisitante["nombre"]);
          visitante.PrimerApellido = Convert.ToString(filaVisitante["primerApellido"]);
          visitante.SegundoApellido = Convert.ToString(filaVisitante["segundoApellido"]);
          visitante.Pais = Convert.ToString(filaVisitante["pais"]);
          visitante.Correo = Convert.ToString(filaVisitante["correo"]);
          visitante.Genero = Convert.ToString(filaVisitante["genero"]);
          visitante.FechaDeNacimiento = Convert.ToDateTime(filaVisitante["fechaDeNacimiento"]);
          visitante.NivelEducativo = Convert.ToString(filaVisitante["nivelEducativo"]);
      }
      return visitante;
    }

    public bool VerificarInscripcion(long numeroIdentificacion) {
      bool estaInscrito = false;
      String consulta = "SELECT numeroIdentificacionVisitanteFK FROM EstaInscritoEn";
      SqlCommand comandoParaConsulta = new SqlCommand(consulta, ConexionPlanetario);
      DataTable tablaConsultada = CrearTablaConsulta(comandoParaConsulta);
      foreach (DataRow filaVisitante in tablaConsultada.Rows) {
        if (Convert.ToInt64(filaVisitante["numeroIdentificacionVisitanteFK"]) == numeroIdentificacion) {
          estaInscrito = true;
          return estaInscrito;
        }
      }
      return estaInscrito;
    }

  }
}