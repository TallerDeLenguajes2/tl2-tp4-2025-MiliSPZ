namespace SistemaCatederia;

public class Cadete
{

    /* Relaciones */
    /* - */

    /* Campos */
    public int id { get; set; }
    public string? nombre { get; set; }
    public List<Pedido>? Pedidos { get; set; }

    /* Constructores */
    public Cadete(int id, string? nombre, List<Pedido>? pedidos)
    {
        this.id = id;
        this.nombre = nombre;
        Pedidos = new List<Pedido>();
    }

    public Cadete() {}

    /* Métodos */

    // Calcular jornal del empleado :D
    public int JornalACobrar()
    {
        int pedidosEntregados = CantidadPedidosEntregados();
        return pedidosEntregados * 500;
    }


    // Agregar un pedido al cadete
    public void RecibirPedido(int nro, string? obs, Cliente cliente, string estado = "asignado")
    {
        var pedido = new Pedido(nro, obs, cliente, estado);
        Pedidos?.Add(pedido);
    }

    // Eliminar un pedido del cadete
    public bool EliminarPedido(int nroPedido)
    {
        var pedidoEncontrado = new Pedido();
        foreach (var pedido in Pedidos)
        {
            if (pedido.nro == nroPedido)
            {
                pedidoEncontrado = pedido;
            }
        }

        if (pedidoEncontrado != null)
        {
            return Pedidos.Remove(pedidoEncontrado);
        }
        return false;
    }

    // Cantidad de pedidos entregados
    public int CantidadPedidosEntregados()
    {
        int contador = 0;
        foreach (var pedido in Pedidos)
        {
            if (pedido.estado == "Entregado" || pedido.estado == "entregado")
            {
                contador++;
            }
        }
        return contador;
    }
}
