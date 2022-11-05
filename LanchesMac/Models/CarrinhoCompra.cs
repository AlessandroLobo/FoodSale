using LanchesMac.Context;

namespace LanchesMac.Models
{
    public class CarrinhoCompra
    {
        private readonly AppDbContext _context;

        public CarrinhoCompra(AppDbContext context)
        {
            _context = context;
        }

        public string CarrinhoCompraId { get; set; }
        public List<CarrinhoCompraItem> CarrinhoCompraItems { get; set; }

        public static CarrinhoCompra GetCarrinho(IServiceProvider service)
        {
            // Define uma sessão
            ISession session = service.GetRequiredService<IHttpContextAccessor>()?.HttpContext.Session;

            // Obtém um serviço do tipo do nosso contexto
            var context = service.GetService<AppDbContext>();

            // Atribui o id do carrinho na sessão
            string carrinhoId = session.GetString("CarrinhoId") ?? Guid.NewGuid().ToString();

            //Atribui o id do carrinho na sessão
            session.SetString("CarrinhoId", carrinhoId);

            // Retorna o carrinho com o contexto e o id atributo ou obtido
            return new CarrinhoCompra(context)
            {
                CarrinhoCompraId = carrinhoId
            };
        }

    }
}
