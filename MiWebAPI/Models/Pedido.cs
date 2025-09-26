namespace SistemaCatederia;

public class Pedido
{
    /* Relaciones */
    // Composicion
    private Cliente ClienteParticular;
    // Agregacion
    private List<Cadete> Cadetes;

    /* Campos */
    public int nro { get; set; }
    public string? obs { get; set; }
    public Cliente? cliente { get; set; }
    public string? estado { get; set; }
    public List<Cadete> cadetes { get; set; }

    /* Constructores */
    public Pedido(int nro, string? obs, Cliente? cliente, string? estado)
    {
        this.nro = nro;
        this.obs = obs;
        this.cliente = cliente;
        this.estado = estado ?? "Pendiente";
    }

    public Pedido(){}

    /* Métodos */
    public string VerDireccionCliente()
    {
        return cliente != null ? $"$La dirección del cliente {cliente.nombre} es {cliente.direccion}" : "Cliente no disponible";
    }

    public string VerDatosCliente()
    {
        if (cliente == null) return "Cliente no disopnible";

        return $"--------- Datos del Cliente {cliente.id} ---------\n" + $"Nombre: {cliente.nombre}\n" + $"Dirección: {cliente.direccion}\n" + $"Teléfono: {cliente.telefono}\n" + $"Referencia dirección: {cliente.datosReferenciaDireccion}\n"; 
    }

    public void CambiarEstado(string nuevoEstado)
    {
        estado = nuevoEstado;
    }
}
