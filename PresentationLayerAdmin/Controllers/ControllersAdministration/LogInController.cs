using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using BusinessLogicLayer;
using EntityLayer;

namespace PresentationLayerAdmin.Controllers
{
    public partial class LogInController : Controller
    {
        private IConnectionBridge logicLayer = new ConnectionBridge(); // INYECCIÓN DE DEPENDENCIA
    }

    public partial class LogInController : Controller
    {
        // GET: LogIn
        public ActionResult LogIn() // INGRESAR 
        {
            FormsAuthentication.SignOut();
            return View();
        }

        public ActionResult UpdatePassword() // CAMBIAR DE CONTRASEÑA
        {
            return View();
        }

        public ActionResult ResetPassword() // REGENERAR CONTRASEÑA 
        {
            return View();
        }

        public ActionResult RegisterAccount() // REGISTRAR
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> LogIn(string email, string password)
        {
            // DATOS
            List<Admin>admins = new List<Admin>();
            List<Client>clients = new List<Client>();
            Admin admin = new Admin();
            Client client = new Client();

            // OPERACIONES
            admins = await logicLayer.listAdminsReturn();
            admin = admins.Where(a => a.emailAdmin == email && a.passwordAdmin == ConnectionBridge.encriptationSHA256Security(password)).FirstOrDefault();
            if (admin != null)
            {
                // TempData["Id"] = admin.Id Esta es una variable que se puede usar para pasar datos entre vistas
                FormsAuthentication.SetAuthCookie(admin.emailAdmin, false); // SE PONE FALSE PARA QUE NO SE GUARDE ESTE INICIO DE SESIÓN
                Session["Admin"] = admin;
                ViewBag.Error = null;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                clients = await logicLayer.listClientsReturn();
                client = clients.Where(a => a.email == email && a.password == ConnectionBridge.encriptationSHA256Security(password)).FirstOrDefault();
                string aux = ConnectionBridge.encriptationSHA256Security(password);
                if (client != null)
                {
                    if(await logicLayer.validateCredentialsClientRequest(client))
                    {
                        TempData["Email"] = client.email; //Esta es una variable que se puede usar para pasar datos entre vistas
                        FormsAuthentication.SetAuthCookie(client.email, false); // SE PONE FALSE PARA QUE NO SE GUARDE ESTE INICIO DE SESIÓN

                        Session["Client"] = client;
                        ViewBag.Error = null;
                        return RedirectToAction("SuperMarketMaui", "Business");
                    }
                    return View();
                }
                else
                {
                    ViewBag.Error = "CORREO O CONTRASEÑA INCORRECTOS";
                    return View();
                }
                
            }
         }

        [HttpPost]
        public async Task<ActionResult> ResetPassword(string email)
        {
            // DATOS 
            List<Admin> admins = new List<Admin>();
            string message = string.Empty;
            Admin admin = new Admin();

            // OPERACIONES
            admins = await logicLayer.listAdminsReturn();
            admin = admins.Where(b => b.emailAdmin == email).FirstOrDefault();

            if (admin != null)
            {
                logicLayer.resetPasswordReturnAdmin(admin.Id, email, out message);
                return RedirectToAction("LogIn", "LogIn");
            }
            else
            {
                List<Client> clients = new List<Client>();
                Client client = new Client();
                clients = await logicLayer.listClientsReturn();
                client = clients.Where(c => c.email == email).FirstOrDefault();
                if (client != null)
                {
                    await logicLayer.resetPasswordReturnClient(client.ID, email, message);
                    return RedirectToAction("LogIn", "LogIn");
                }
                else
                {
                    ViewBag.Error = "EL CORREO NO ESTÁ REGISTRADO";
                    return View();
                }
            }
        }

        [HttpPost]
        public async Task<ActionResult> RegisterAccount(string name, string email, string password,string confirmedPassword)
        {
            
            if(password == confirmedPassword)
            {
                string message = String.Empty;
                Client client = new Client()
                {
                    name = name,
                    email = email,
                    password = password,
                    resetPassword = true
                };
                bool aux = await logicLayer.createClientReturn(client, message);
                
                if (aux)
                {
                    ViewBag.Error = null;
                    return RedirectToAction("LogIn", "LogIn");
                }
                else
                {
                    ViewBag.Error = message;
                    return View();
                }
            }
            else
            {
                ViewBag.Error = "LAS CONTRASEÑAS DEBEN SER IGUALES";
                return View();
            }
            
        }

        public ActionResult SignOutSystem()
        {
            Session["Client"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("SuperMarketMaui", "Business");
        }
    }
}