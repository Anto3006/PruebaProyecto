using Planetario.Handlers;
using Planetario.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
namespace Planetario.Controllers {
  public class VisitanteController : Controller {
    public VisitanteHandler AccesoMetodosVisitante;
    public ActividadHandler AccesoDatosActividad;
    public VisitanteController() {
      AccesoMetodosVisitante = new VisitanteHandler();
      AccesoDatosActividad = new ActividadHandler();
    }
    public ActionResult InscribirVisitante(String titulo = "", DateTime fecha = new DateTime()) {
      ActividadModel actividad = AccesoDatosActividad.ObtenerActividad(titulo, fecha);
      ViewBag.Actividad = actividad;
      return View();
    }

    [HttpPost]
    public ActionResult InscribirVisitante(VisitanteModel visitante) {
      visitante.Pais = Request.Form["paisSeleccionado"];
      ActividadModel actividad = AccesoDatosActividad.ObtenerActividad(visitante.TituloActividadInscrita, visitante.FechaActividadInscrita);
      ViewBag.Actividad = actividad;
      ViewBag.Exito = false;
      try {
        if (ModelState.IsValid) {
          AccesoMetodosVisitante.AgregarVisitante(visitante);
          AccesoMetodosVisitante.AgregarInscripcion(visitante);
          ViewBag.Message = "La inscripción fue realizada con éxito";
          ModelState.Clear();
          ViewBag.Exito = true;
        }
        return View();
      }
      catch {
        ViewBag.Message = "Algo salió mal y no fue posible realizar la inscripción";
        return View();
      }
    }

    public ActionResult InscribirEnActividad(String titulo = "", DateTime fecha = new DateTime()) {
      ActividadModel actividad = AccesoDatosActividad.ObtenerActividad(titulo, fecha);
      ViewBag.Actividad = actividad;
      return View();
    }

    [HttpPost]
    public ActionResult InscribirEnActividad(VisitanteModel visitante) {
      ActividadModel actividad = AccesoDatosActividad.ObtenerActividad(visitante.TituloActividadInscrita, visitante.FechaActividadInscrita);
      ViewBag.Exito = false;
      ViewBag.Actividad = actividad;
      try {
        if (AccesoMetodosVisitante.VerificarInscripcion(visitante.NumeroIdentificacion)) {
          VisitanteModel visitanteRecuperado = AccesoMetodosVisitante.RecuperarVisitante(visitante.NumeroIdentificacion);
          visitanteRecuperado.FechaActividadInscrita = visitante.FechaActividadInscrita;
          visitanteRecuperado.TituloActividadInscrita = visitante.TituloActividadInscrita;    
          AccesoMetodosVisitante.AgregarInscripcion(visitanteRecuperado);  
          ViewBag.Message = "La inscripción fue realizada con éxito";
          ModelState.Clear();
          ViewBag.Exito = true;
        } else {
          ViewBag.Message = "Algo salió mal y no fue posible realizar la inscripción";
        }
        return View();
      }
      catch {
        ViewBag.Message = "Algo salió mal y no fue posible realizar la inscripción";
        return View();
      }
    }
  }
  
}