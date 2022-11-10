using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LanchesMac.Controllers
{
    public class ContatoController : Controller
    {
        [Authorize]
        public IActionResult Index()
        {
            // Essa implementação da acesso ou não a pagina de contatos, verificando se voc esta ou não logado
            //if (User.Identity.IsAuthenticated)
            //{
            return View();
            // }

        }
    }
}
