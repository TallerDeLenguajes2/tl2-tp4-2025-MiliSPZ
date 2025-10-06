/* Cree un Controlador Para la cadetería llamado CadeteriaController, y en el
Implemente un endpoint para cada una de las operaciones ya existentes,
siguiendo la siguiente forma:
● [Get] GetPedidos() => Retorna una lista de Pedidos

● [Get] GetCadetes() => Retorna una lista de Cadetes

● [Get] GetInforme() => Retorna un objeto Informe
● [Post] AgregarPedido(Pedido pedido)
● [Put] AsignarPedido(int idPedido, int idCadete)
● [Put] CambiarEstadoPedido(int idPedido,int NuevoEstado)
● [Put] CambiarCadetePedido(int idPedido,int idNuevoCadete) */

namespace SistemaCatederia;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[Controller]")]

public class ApiController : ControllerBase
{
    private readonly AccesoADatos _acceso = new AccesoADatos();
    private Catederia _cadeteria;

    public ApiController()
    {
        _cadeteria = _acceso.TraerCadeteriaCVS();
        _cadeteria.Cadetes = _acceso.TraerCadetesCVS();
    }

    [HttpGet("GetPedidos")]
    public IActionResult GetPedidos()
    {
        var pedidos = _cadeteria.ObtenerTodosLosPedidos();
        if (!pedidos.Any())
        {
            return NoContent();
        }

        return Ok(pedidos);
    }

    [HttpGet("Getcadetes")]
    public IActionResult GetCadetes()
    {
        if (_cadeteria.Cadetes == null || !_cadeteria.Cadetes.Any())
        {
            return NoContent();
        }

        return Ok(_cadeteria.Cadetes);
    }

    [HttpGet("GetInforme")]
    public IActionResult GetInforme()
    {
        var informeCadetes = _cadeteria.ObtenerInformeCadetes();
        var informeResumen = _cadeteria.ObtenerResumenInforme();

        var informeCompleto = new { totalPedidos = informeResumen.totalPedidos, totalGanado = informeResumen.totalGanado, promedio = informeResumen.promedio, detalleCadetes = informeCadetes };

        return Ok(informeCompleto);
    }

}