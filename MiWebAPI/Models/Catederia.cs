namespace SistemaCatederia;

public class Catederia
{
    /* Relaciones */
    /* - */

    /* Campos */
    public string? nombre { get; set; }
    public string? telefono { get; set; }
    public List<Cadete>? Cadetes { get; set; }
    public List<Pedido> PedidosAsignar { get; set; } = new List<Pedido>();


    /* Constructores */
    public Catederia(string? nombre, string? telefono, List<Cadete>? cadetes)
    {
        this.nombre = nombre;
        this.telefono = telefono;
        Cadetes = cadetes ?? new List<Cadete>();
    }

    public Catederia() {}

    /* Métodos */
    public Pedido TomarPedido(string nombreCliente, string direccion, string telefono, string referencia, string obs)
    {
        int nro = PedidosAsignar.Count > 0 ? PedidosAsignar.Max(p => p.nro) + 1 : 1;
        Cliente cliente = new Cliente(nombreCliente, direccion, telefono, referencia);
        Pedido pedido = new Pedido(nro, obs, cliente, "Pendiente");

        PedidosAsignar.Add(pedido);
        return pedido;
    }

    //Asignar un pedido (recibe nroPedido e idCadete)
    public bool AsignarPedido(int nroPedido, int idCadete)
    {
        var pedidoSeleccionado = PedidosAsignar.FirstOrDefault(p => p.nro == nroPedido);
        var cadeteSeleccionado = Cadetes.FirstOrDefault(c => c.id == idCadete);

        if (pedidoSeleccionado == null || cadeteSeleccionado == null)
            return false;

        cadeteSeleccionado.RecibirPedido(pedidoSeleccionado.nro, pedidoSeleccionado.obs, pedidoSeleccionado.cliente, "Asignado");

        pedidoSeleccionado.CambiarEstado("Asignado");
        PedidosAsignar.Remove(pedidoSeleccionado);

        return true;
    }

    //Buscar pedido en cadetes
    public (Cadete?, Pedido?) BuscarPedidoPorNumero(int nroPedido)
    {
        foreach (var cadete in Cadetes)
        {
            foreach (var pedido in cadete.Pedidos)
            {
                if (pedido.nro == nroPedido)
                    return (cadete, pedido);
            }
        }
        return (null, null);
    }

    //Cambiar estado de un pedido
    public bool CambiarEstadoPedido(int nroPedido, string nuevoEstado)
    {
        var (cadete, pedido) = BuscarPedidoPorNumero(nroPedido);
        if (pedido == null) return false;

        pedido.CambiarEstado(nuevoEstado);
        return true;
    }

    //Reasignar pedido a otro cadete
    public bool ReasignarPedido(int nroPedido, int nuevoCadeteId)
    {
        var (cadeteActual, pedido) = BuscarPedidoPorNumero(nroPedido);
        var nuevoCadete = Cadetes.FirstOrDefault(c => c.id == nuevoCadeteId);

        if (pedido == null || cadeteActual == null || nuevoCadete == null)
            return false;

        if (!cadeteActual.EliminarPedido(pedido.nro))
            return false;

        nuevoCadete.RecibirPedido(pedido.nro, pedido.obs, pedido.cliente, pedido.estado ?? "Asignado");

        return true;
    }

    //Pedidos pendientes 
    public List<Pedido> ObtenerPedidosPendientes()
    {
        return PedidosAsignar;
    }

    //Pedidos asignados a cadetes
    public List<(int nro, string estado, string cliente, string cadete)> ObtenerPedidosAsignados()
    {
        var lista = new List<(int, string, string, string)>();

        foreach (var cadete in Cadetes)
        {
            foreach (var pedido in cadete.Pedidos)
            {
                lista.Add((pedido.nro, pedido.estado ?? "", pedido.cliente?.nombre ?? "", cadete.nombre ?? ""));
            }
        }

        return lista;
    }

    //Informe de cadetes
    public List<(string cadete, int entregados, int jornal)> ObtenerInformeCadetes()
    {
        var informe = new List<(string, int, int)>();

        foreach (var cadete in Cadetes)
        {
            int cantPedidos = cadete.CantidadPedidosEntregados();
            int jornal = cadete.JornalACobrar();
            informe.Add((cadete.nombre ?? "", cantPedidos, jornal));
        }

        return informe;
    }

    public (int totalPedidos, int totalGanado, double promedio) ObtenerResumenInforme()
    {
        int totalPedidos = Cadetes.Sum(c => c.CantidadPedidosEntregados());
        int totalGanado = Cadetes.Sum(c => c.JornalACobrar());
        double promedio = Cadetes.Count > 0 ? (double)totalPedidos / Cadetes.Count : 0;

        return (totalPedidos, totalGanado, promedio);
    }
}
