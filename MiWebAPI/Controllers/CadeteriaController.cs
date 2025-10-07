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
        _acceso = new AccesoADatos();
        _cadeteria = _acceso.TraerCadeteriaCVS();
        _cadeteria.Cadetes = _acceso.TraerCadetesCVS();

        _cadeteria.TomarPedido(
            "Mili Romero",
            "San Martín 123",
            "3810000000",
            "Puerta verde",
            "Entregar rápido"
        );
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

    [HttpPost("AgregarPedido")]
    public IActionResult AgregarPedido([FromBody] Pedido pedido)
    {
        if (pedido == null || pedido.cliente == null)
            return BadRequest("El pedido o el cliente no pueden ser nulos.");

        var cliente = pedido.cliente;
        _cadeteria.TomarPedido(cliente.nombre, cliente.direccion, cliente.telefono, cliente.datosReferenciaDireccion, pedido.obs);

        _acceso.GuardarCadeteriaJson(_cadeteria);
        return Created($"api/cadeteria/AgregarPedido/{pedido.nro}", pedido);
    }

    [HttpPut("AsignarPedido")]
    public IActionResult AsignarPedido([FromQuery] int idPedido, [FromQuery] int idCadete)
    {
        // Cargar la catedería desde el JSON antes de modificarla
        _cadeteria = _acceso.TraerCadeteriaJSON();

        if (_cadeteria == null)
            return NotFound("No se pudo cargar la catedería.");

        if (_cadeteria.PedidosAsignar.Find(n => n.nro == idPedido) == null || _cadeteria.Cadetes.Find(n => n.id == idCadete) == null)
            return NotFound("No se encontro cadete/pedido");

        var pedido = _cadeteria.PedidosAsignar.Find(n => n.nro == idPedido);

        var cadete = _cadeteria.Cadetes.Find(n => n.id == idCadete);

        cadete.Pedidos.Add(pedido);

        return Ok($"Pedido {idPedido} asignado al cadete {idCadete} correctamente.");
        
    }

    [HttpPut("CambiarEstadoPedido")]
    public IActionResult CambiarEstadoPedido([FromQuery] int idPedido, [FromQuery] string nuevoEstado)
    {
        var pedidoPendiente = _cadeteria.PedidosAsignar.FirstOrDefault(p => p.nro == idPedido);

        var (cadete, pedidoAsignado) = _cadeteria.BuscarPedidoPorNumero(idPedido);

        if (pedidoAsignado != null)
        {
            pedidoAsignado.CambiarEstado(nuevoEstado);
            _acceso.GuardarCadeteriaJson(_cadeteria);
            return Ok($"El pedido {idPedido} cambió a estado '{nuevoEstado}' (asignado a {cadete?.nombre}).");
        }

        return NotFound($"No se encontró el pedido {idPedido}.");
    }

    [HttpPut("CambiarCadetePedido")]
    public IActionResult CambiarCadetePedido([FromQuery] int idPedido, [FromQuery] int idNuevoCadete)
    {
        var pedidoPendiente = _cadeteria.PedidosAsignar.FirstOrDefault(p => p.nro == idPedido);

        if (pedidoPendiente != null)
        {
            bool asignado = _cadeteria.AsignarPedido(idPedido, idNuevoCadete);

            if (!asignado)
                return BadRequest($"No se pudo asignar el pedido {idPedido} al cadete {idNuevoCadete} (pendiente).");

            _acceso.GuardarCadeteriaJson(_cadeteria);
            return Ok($"El pedido {idPedido} fue asignado correctamente al cadete {idNuevoCadete}.");
        }

        bool resultado = _cadeteria.ReasignarPedido(idPedido, idNuevoCadete);

        if (!resultado)
            return NotFound($"No se pudo reasignar el pedido {idPedido} al cadete {idNuevoCadete}.");

        _acceso.GuardarCadeteriaJson(_cadeteria);
        return Ok($"Pedido {idPedido} reasignado correctamente al cadete {idNuevoCadete}.");
    }
}