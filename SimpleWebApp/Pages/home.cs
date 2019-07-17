using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebApp.Pages
{
    class HomePage : iPage
    {
        public override string Render()
        {
            return "<h1>Logged in - Home page<h1>";
        }
    }
}
