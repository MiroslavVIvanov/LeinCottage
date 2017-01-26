namespace LeinCottage.Web.CustomAttributes
{
    using System;
    using System.Web.Mvc;

    public class BasicAuthenticationAttribute : ActionFilterAttribute
    {
        public BasicAuthenticationAttribute(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        public string BasicRealm { get; set; }

        protected string Username { get; set; }

        protected string Password { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            var req = filterContext.HttpContext.Request;
            var auth = req.Headers["Authorization"];
            if (!string.IsNullOrEmpty(auth))
            {
                var credentials = System.Text.Encoding.ASCII.GetString(Convert.FromBase64String(auth.Substring(6))).Split(':');
                var user = new
                {
                    Name = credentials[0],
                    Pass = credentials[1]
                };

                if (user.Name == this.Username && user.Pass == this.Password)
                {
                    return;
                }
            }

            filterContext
                .HttpContext
                .Response
                .AddHeader("WWW-Authenticate", string.Format("Basic realm=\"{0}\"", this.BasicRealm ?? "Ryadel"));
            filterContext.Result = new HttpUnauthorizedResult();
        }
    }
}