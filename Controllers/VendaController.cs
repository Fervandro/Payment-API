using Microsoft.AspNetCore.Mvc;
using tech_test_payment_api.Context;
using tech_test_payment_api.Models;

namespace tech_test_payment_api.Controllers
{
    [ApiController]
    [Route ("")]
    public class VendaController : ControllerBase
    {
        private readonly VendaContext _context;   

        public VendaController(VendaContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Criar(Venda venda)
        {
            if (venda.Itens.Count == 0)
                return BadRequest(new { Erro = "Você deve pelo menos um item na venda." });

            venda.Status = EnumStatusVenda.AguardandoPagamento;
            _context.Vendas.Add(venda);
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new { id = venda.Id }, venda);
        }

         [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            var venda = _context.Vendas.Find(id);

            if (venda == null)
            {
                return NotFound();
            }
            return Ok(venda);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, EnumStatusVenda status)
        {
            var vendaBanco = _context.Vendas.Find(id);

            if (vendaBanco == null)
                return NotFound();

            if (vendaBanco.Status == EnumStatusVenda.AguardandoPagamento )
            {
                if (status == EnumStatusVenda.Cancelada || status == EnumStatusVenda.PagamentoAprovado)
                {
                    return Ok(vendaBanco.Status = status);
                }else
                {
                    return BadRequest(new { Erro = "Mudança de status inválida." });
                }
            }
            if (vendaBanco.Status == EnumStatusVenda.PagamentoAprovado )
            {
                if (status == EnumStatusVenda.Cancelada || status == EnumStatusVenda.EnviadoParaTransportadora)
                {
                    return Ok(vendaBanco.Status = status);
                }else
                {
                    return BadRequest(new { Erro = "Mudança de status inválida." });
                }
            }
            if (vendaBanco.Status == EnumStatusVenda.EnviadoParaTransportadora )
            {
                if (status == EnumStatusVenda.Entregue)
                {
                    return Ok(vendaBanco.Status = status);
                }else
                {
                    return BadRequest(new { Erro = "Mudança de status inválida." });
                }
            }

            _context.Vendas.Update(vendaBanco);
            _context.SaveChanges();

            return Ok();
        }
    }
}