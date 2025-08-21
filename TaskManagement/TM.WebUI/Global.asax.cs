using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using TM.Infrastructure;

namespace TM.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            CultureInfo cInfo = new CultureInfo("en-GB");
            cInfo.DateTimeFormat.ShortDatePattern = "dd/MM/yyyy";
            cInfo.DateTimeFormat.DateSeparator = "/";
            Thread.CurrentThread.CurrentCulture = cInfo;
            Thread.CurrentThread.CurrentUICulture = cInfo;
        }


        protected void Application_AcquireRequestState(object sender, EventArgs e)
        {
            HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie != null && !string.IsNullOrEmpty(authCookie.Value))
            {
                try
                {
                    FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authCookie.Value);
                    if (ticket != null)
                    {
                        TMIdentity identity = new TMIdentity(ticket.UserData);
                        var basicTicket = Application["BasicTicket" + identity.Id];
                        var roleTicket = Application["RoleTicket" + identity.Id];

                        if (basicTicket != null && roleTicket != null && basicTicket.ToString() == ticket.UserData)
                        {
                            // Set user roles
                            identity.SetRoles(roleTicket.ToString());
                            TMPrincipal principal = new TMPrincipal(identity);
                            HttpContext.Current.User = principal;
                            Thread.CurrentPrincipal = principal;

                            // Renew the authentication ticket
                            int timeOut = Convert.ToInt32(new AppSettingsReader().GetValue("COOKIE_TIMEOUT", typeof(string)));
                            FormsAuthenticationTicket newTicket = new FormsAuthenticationTicket(
                                1,
                                FormsAuthentication.FormsCookieName,
                                DateTime.Now,
                                DateTime.Now.AddMinutes(timeOut),
                                ticket.IsPersistent,
                                ticket.UserData
                            );

                            string encryptedTicket = FormsAuthentication.Encrypt(newTicket);
                            HttpCookie newAuthCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket)
                            {
                                HttpOnly = true, // Prevent JavaScript access
                                Secure = FormsAuthentication.RequireSSL, // Set based on your SSL config
                                Expires = newTicket.Expiration
                            };

                            HttpContext.Current.Response.Cookies.Add(newAuthCookie);
                            return;
                        }
                        else
                        {
                            // Invalidate cookie and clear app cache
                            InvalidateAuthCookie(authCookie, identity.Name);
                            RedirectToLogin();
                        }
                    }
                    else
                    {
                        // Decrypt failed, possibly tampered or corrupted
                        InvalidateAuthCookie(authCookie, null);
                        RedirectToLogin();
                    }
                }
                catch (CryptographicException)
                {
                    // Catch crypto failure and handle gracefully
                    InvalidateAuthCookie(authCookie, null);
                    RedirectToLogin();
                }
            }

        }
        private void InvalidateAuthCookie(HttpCookie authCookie, string userName)
        {
            if (authCookie != null)
            {
                authCookie.Expires = DateTime.Now.AddDays(-1);
                HttpContext.Current.Response.Cookies.Add(authCookie);
            }

            if (!string.IsNullOrEmpty(userName))
            {
                Application["BasicTicket" + userName] = null;
                Application["RoleTicket" + userName] = null;
            }
        }

        private void RedirectToLogin()
        {
            string returnUrl = HttpContext.Current.Request.Path;
            HttpContext.Current.Response.Redirect("/Login?ReturnUrl=" + HttpUtility.UrlEncode(returnUrl), true);
        }

    }
}
