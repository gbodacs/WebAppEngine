
using SimpleWebApp;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading;
using SimpleWebApp.Pages;

public class AsyncServer
{
    HttpListener mListener = null;

    public AsyncServer()
    {
        mListener = new HttpListener();
        mListener.Prefixes.Add("http://*:8082/");
        mListener.Prefixes.Add("http://localhost:8082/");
        mListener.Prefixes.Add("http://127.0.0.1:8082/");
    }

    public void StartListen()
    {
        mListener.Start();

        while (true)
        {
            try
            {
                HttpListenerContext context = mListener.GetContext();
                ThreadPool.QueueUserWorkItem(o => HandleRequest(context));
            }
            catch (Exception)
            {
                // Ignored for this example
            }
        }
    }
    private string AdminResponse()
    {
        SimpleWebApp.Pages.AdminPage page = new AdminPage();
        return page.Render();
    }

    private string HomeResponse()
    {
        SimpleWebApp.Pages.HomePage page = new HomePage();
        return page.Render();
    }

    private string LoginResponse()
    {
        SimpleWebApp.Pages.LoginPage page = new LoginPage();
        return page.Render();
    }

    private string LogoutResponse()
    {
        SimpleWebApp.Pages.LogoutPage page = new LogoutPage();
        return page.Render();
    }

    private void HandleRequest(HttpListenerContext context)
    {
        try
        {
            string Parameters = context.Request.RawUrl;
            string FullURL = context.Request.Url.ToString();
            string Response = "";

            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.ContentType = "text/html";
            NameValueCollection Params = context.Request.Headers;

            if (Params.Get("poke") == "exit")
            {
                int i = 0;
            }

            if (Parameters.StartsWith("/main"))
            {
                if (Params.Get("poke") == "exit")
                {
                    Response = MainResponse_Exit();
                }
                else
                {
                    Response = MainResponse();
                }

                context.Response.StatusCode = 200;
                context.Response.SendChunked = true;

                var bytes = Encoding.UTF8.GetBytes(Response);
                context.Response.ContentLength64 = bytes.Length;
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                context.Response.Close();
            }
            else if (Parameters.StartsWith("/about"))
            {
                Response = AboutResponse();

                context.Response.StatusCode = 200;
                context.Response.SendChunked = true;

                var bytes = Encoding.UTF8.GetBytes(Response);
                context.Response.ContentLength64 = bytes.Length;
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                context.Response.Close();
            }
            else if (Parameters.StartsWith("/favicon.ico"))
            {
                context.Response.StatusCode = 200;
                context.Response.SendChunked = true;
                context.Response.ContentType = "image/ico";

                var bytes = Encoding.UTF8.GetBytes("pokijhuuashfdahsfahsflkahsfdshdf86s5v8f76as58fv76awv87gwrufz2g3kjh42tlkj3h4o2hjr345634563456345456hgpokijhuuashfdahsfahsflkahsfdshdf86s5v8f76as58fv76awv87gwrufz2g3kjh42tlkj3h4o2hjr345634563456345456hgpokijhuuashfdahsfahsflkahsfdshdf86s5v8f76as58fv76awv87gy");
                context.Response.ContentLength64 = bytes.Length;
                context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                context.Response.Close();
            }
            else
            {
                Redirect(context, "main/");
            }

            
        }
        catch (Exception)
        {
            // Client disconnected or some other error - ignored for this example
        }
    }

    public void Redirect(HttpListenerContext context, string url)
    {
        url = url.Replace('\\', '/');
        HttpListenerRequest request = context.Request;
        HttpListenerResponse response = context.Response;

        string redirectUrl = "";

        if ( (request.Url.Port == 80) || (request.Url.Port == 443) ) //Don't need the port number into the uri
        {
            redirectUrl = request.Url.Scheme + "://" + request.Url.Host + "/" + url;
        }
        else
        {
            redirectUrl = request.Url.Scheme + "://" + request.Url.Host + ":" + request.Url.Port.ToString() + "/" + url;
        }

        response.Headers.Set("poke", "exit");

        response.StatusCode = (int)HttpStatusCode.Redirect;
        response.Redirect(redirectUrl);
        response.Close();
    }
}