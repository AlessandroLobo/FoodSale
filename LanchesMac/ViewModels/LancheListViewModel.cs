using LanchesMac.Models;
using Newtonsoft.Json.Linq;

namespace LanchesMac.ViewModels
{
    public class LancheListViewModel
    {
        public IEnumerable<Lanche> Lanches { get; set; }
        public string CategoriaAtual { get; set; }
    }
}
